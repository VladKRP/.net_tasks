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
            if(type.IsAbstract || type.IsInterface)
                throw new AbstractionResolvingException(type, "Can't add abstract class or interface as resolver");
            else if (!TypeResolvers.Keys.Contains(baseType) && !TypeResolvers.Values.Contains(type))
                TypeResolvers.Add(baseType, type);
        }


        public object CreateInstance(Type type, params object[] args)
        {
            var typeProperties = type.GetProperties().Where(x => x.GetCustomAttribute<ImportAttribute>() != null);
            if (type.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                var constructorParameters = type.GetConstructors().FirstOrDefault().GetParameters();
                if (constructorParameters.Count() > 0 && constructorParameters.Count() == args.Count())
                {
                    var instances = args.Zip(constructorParameters, (x, y) => this.CreateInstance(y.ParameterType));
                    return Activator.CreateInstance(type, instances);
                }
            }
            else if (typeProperties != null)
            {
                var types = typeProperties.Select(x => x.PropertyType).Select(x => CreateInstance(x));


            }

            else if (type.IsAbstract || type.IsInterface)
            {
                if (TypeResolvers.TryGetValue(type, out Type resolver))
                    return Activator.CreateInstance(resolver);
            }



            return Activator.CreateInstance(type);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }
    }
}
