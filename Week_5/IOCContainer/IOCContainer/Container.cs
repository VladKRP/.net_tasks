using IOCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public class Container
    {
        public IDictionary<Type, Type> TypeResolvers { get; private set; }

        public Container()
        {
            TypeResolvers = new Dictionary<Type, Type>();
        }

        public void AddAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("Can't operate with assembly that equals null");
            else
            {
                foreach (var type in assembly.DefinedTypes)
                {
                    var exportAttribute = type.GetCustomAttribute<ExportAttribute>();
                    if (exportAttribute != null && !TypeResolvers.Keys.Contains(type) && !TypeResolvers.Values.Contains(type))
                    {
                        var baseType = exportAttribute.Contract;
                        if (baseType != null)
                            AddType(baseType, type);
                        else
                            AddType(type);
                    }
                }
            }
        }

        public void AddType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException();
            if (type.IsAbstract || type.IsInterface)
                throw new AbstractionResolvingException(type, "Can't add abstract class or interface as resolver");
            else if (!TypeResolvers.Keys.Contains(type) && !TypeResolvers.Values.Contains(type))
                TypeResolvers.Add(type, type);
        }

        public void AddType(Type baseType, Type type)
        {
            if (type == null || baseType == null)
                throw new ArgumentNullException();
            if (type.IsAbstract || type.IsInterface)
                throw new AbstractionResolvingException(type, "Can't add abstract class or interface as resolver");
            else if (!TypeResolvers.Keys.Contains(baseType) && !TypeResolvers.Values.Contains(type))
                TypeResolvers.Add(baseType, type);
        }


        public object CreateInstance(Type type)
        {
            object instance = null;
            if (type.IsAbstract || type.IsInterface)
                instance = ResolveAbstraction(type);
            else
            {
                var importProperties = type.GetProperties().Where(x => x.GetCustomAttribute<ImportAttribute>() != null);
                var importConstructor = type.GetCustomAttribute<ImportConstructorAttribute>();

                if (importConstructor != null)
                    instance = ResolveConstructorImport(type);
                else if (importProperties?.Count() > 0)
                    instance = ResolvePropertyImport(type, importProperties);
            }
            
            return instance ?? Activator.CreateInstance(type);
        }

        private object ResolveAbstraction(Type type)
        {
            if (TypeResolvers.TryGetValue(type, out Type resolver))
                return Activator.CreateInstance(resolver);
            return resolver;
        }

        private object ResolveConstructorImport(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors().FirstOrDefault();
            object[] parameters = constructor.GetParameters().Select(x => CreateInstance(x.ParameterType)).ToArray();
            return constructor.Invoke(parameters);
        }

        private object ResolvePropertyImport(Type type, IEnumerable<PropertyInfo> properties)
        {
            var instance = Activator.CreateInstance(type);
            foreach (var property in properties)
                type.GetProperty(property.Name).SetValue(instance, CreateInstance(property.PropertyType));
            return instance;
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }
    }
}
