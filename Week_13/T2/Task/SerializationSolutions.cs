using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Task
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(), true);

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            var categories = dbContext.Categories.ToList();
            foreach (var category in categories)
                objectContext.LoadProperty(category, x => x.Products);

            var c = categories.First();

            tester.SerializeAndDeserialize(categories);
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);
            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            var products = dbContext.Products.ToList();
            foreach (var product in products)
            {
                objectContext.LoadProperty(product, p => p.Category);
                objectContext.LoadProperty(product, p => p.Supplier);
                objectContext.LoadProperty(product, p => p.Order_Details);
            }

            tester.SerializeAndDeserialize(products);
        }


        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            OrderDetailSurrogate orderDetailSurrogate = new OrderDetailSurrogate();
            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(typeof(Order_Detail), new StreamingContext(StreamingContextStates.All), orderDetailSurrogate);

            var serializer = new NetDataContractSerializer
            {
                SurrogateSelector = selector
            };

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(serializer, true);
            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            var orderDetails = dbContext.Order_Details.ToList();
            foreach (var detail in orderDetails)
                objectContext.LoadProperty(detail, d => d.Order);


            tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;
            dbContext.Configuration.LazyLoadingEnabled = true;

            CustomerSurrogate surrogate = new CustomerSurrogate();
            var dataContractSerializer = new DataContractSerializer(
                typeof(IEnumerable<Order>),
                new List<Type>(),
                Int16.MaxValue,
                false,
                true,
                surrogate);
            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(new DataContractSerializer(typeof(Order)), true);
            var orders = dbContext.Orders.ToList();
        
            tester.SerializeAndDeserialize(orders);
        }
    }
}
