using System;
using System.Collections.Generic;
using System.Reflection;
using IOCContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestAssembly;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace IOCContainerTests
{
    [TestClass]
    public class ContainerTests
    {
        private readonly Container _container;

        public ContainerTests()
        {
            _container = new Container();
            _container.AddAssembly(Assembly.Load("TestAssembly"));        
        }


        [TestMethod]
        public void AddAssembly_PassNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _container.AddAssembly(null));
        }

        [TestMethod]
        public void AddAssembly_PassAssembly_AddTypesToTypeResolversDictionary()
        {
            var container = new Container();
            var initialTypeResolversCount = container.TypeResolvers.Count;

            container.AddAssembly(Assembly.Load("TestAssembly"));

            Assert.AreEqual(0, initialTypeResolversCount);
            Assert.AreEqual(4, container.TypeResolvers.Count);
        }

        [TestMethod]
        public void AddType_OneParameterMethod_PassNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _container.AddType(null));
        }

        [TestMethod]
        public void AddType_OneParameterMethod_PassType_AddTypeToTypeResolversDictionary()
        {
            var container = new Container();

            container.AddType(typeof(Logger));

            Assert.AreEqual(1, container.TypeResolvers.Count);
            Assert.AreEqual(typeof(Logger), container.TypeResolvers.Keys.ElementAt(0));
            Assert.AreEqual(typeof(Logger), container.TypeResolvers.Values.ElementAt(0));
        }

        [TestMethod]
        public void AddType_OneParameterMethod_PassAbstraction_ThrowAbstractionResolvingException()
        {
            var container = new Container();
            Type[] types = { typeof(ICustomerDAL), typeof(IEnumerable<int>), typeof(IDisposable), typeof(FileSystemInfo) };

            types.ToList().ForEach(type =>
            {
                Assert.ThrowsException<AbstractionResolvingException>(() => container.AddType(type));
            });
           
        }

        [TestMethod]
        public void AddType_OneParameterMethod_TypeAlreadyExists_DoNothing()
        {
            var container = new Container();

            container.AddType(typeof(Logger));
            container.AddType(typeof(Logger));

            Assert.AreEqual(1, container.TypeResolvers.Count);
            Assert.AreEqual(typeof(Logger), container.TypeResolvers.Keys.ElementAt(0));
            Assert.AreEqual(typeof(Logger), container.TypeResolvers.Values.ElementAt(0));

        }

        [TestMethod]
        public void AddType_TwoParametersMethod_PassNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _container.AddType(typeof(ICustomerDAL), null));
            Assert.ThrowsException<ArgumentNullException>(() => _container.AddType(null, typeof(Logger)));
            Assert.ThrowsException<ArgumentNullException>(() => _container.AddType(null, null));
        }

        [TestMethod]
        public void AddType_TwoParametersMethod_PassAbstractionToResolvingType_ThrowAbstractionResolvingException()
        {
            var container = new Container();

            Assert.ThrowsException<AbstractionResolvingException>(() => _container.AddType(typeof(ICustomerDAL), typeof(ICustomerDAL)));
            Assert.ThrowsException<AbstractionResolvingException>(() => _container.AddType(typeof(IEnumerable<int>), typeof(ICollection<int>)));
            Assert.ThrowsException<AbstractionResolvingException>(() => _container.AddType(typeof(FileSystemInfo), typeof(FileSystemInfo)));
        }


        [TestMethod]
        public void CreateInstance_PassConcreteClass_CreateConcreteClass()
        {
            var container = new Container();

            var customerDAL = (CustomerDAL)container.CreateInstance(typeof(CustomerDAL));

            Assert.IsNotNull(customerDAL);
            Assert.IsInstanceOfType(customerDAL, typeof(CustomerDAL));
        }

        [TestMethod]
        public void CreateInstance_PassAbstraction_CreateConcreteClass()
        {
            var container = new Container();
            container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));
            container.AddType(typeof(Logger));

            var customerDAL = (CustomerDAL)container.CreateInstance(typeof(ICustomerDAL));

            Assert.IsNotNull(customerDAL);
            Assert.IsInstanceOfType(customerDAL, typeof(CustomerDAL));
        }

        [TestMethod]
        public void CreateInstance_PassInstanceWithConstructorInjection_CreateConcreteClass()
        {
            int fieldsCount = 2;
            var container = new Container();
            container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
            var fields = customerBLL.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(customerBLL);
            Assert.AreEqual(fieldsCount, fields.Length);
            Assert.AreEqual(typeof(CustomerBLL), customerBLL.GetType());
        }

        [TestMethod]
        public void CreateInstance_PassInstanceWithPropertyInjection_CreateConcreteClass()
        {
            var container = new Container();
            container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerBLL2 = (CustomerBLL2)container.CreateInstance(typeof(CustomerBLL2));

            Assert.IsNotNull(customerBLL2);
            Assert.IsNotNull(customerBLL2.CustomerDAL);
            Assert.IsNotNull(customerBLL2.Logger);
            Assert.AreEqual(typeof(CustomerBLL2), customerBLL2.GetType());

        }


        [TestMethod]
        public void CreateInstanceGeneric_PassInstanceWithConstructorInjection_CreateConcreteClass()
        {
            var container = new Container();
            container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerBLL = container.CreateInstance<CustomerBLL>();

            Assert.IsNotNull(customerBLL);
            Assert.AreEqual(typeof(CustomerBLL), customerBLL.GetType());
        }
    }
}
