using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
	public class Container
	{
        private ICollection<Assembly> _assemblies;
        private IDictionary<Type, Type> _typeResolvers;
        private ICollection<Type> _types;

        public Container()
        {
            _assemblies = new List<Assembly>();
            _typeResolvers = new Dictionary<Type, Type>();
            _types = new List<Type>();
        }

		public void AddAssembly(Assembly assembly)
		{
            if (assembly != null && !_assemblies.Contains(assembly))
                _assemblies.Add(assembly);
        }

		public void AddType(Type type)
		{
            if (type != null && !_types.Contains(type))
                _types.Add(type);
        }

		public void AddType(Type type, Type baseType)
		{
            if(type != null && baseType != null)
            {
                _typeResolvers.Add(type, baseType);
            }
        }

		public object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}

		public T CreateInstance<T>()
		{
			return (T)CreateInstance(typeof(T));
		}


		public void Sample()
		{
			var container = new Container();
			container.AddAssembly(Assembly.GetExecutingAssembly());

			var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
			var customerBLL2 = container.CreateInstance<CustomerBLL>();

			container.AddType(typeof(CustomerBLL));
			container.AddType(typeof(Logger));
			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
		}
	}
}
