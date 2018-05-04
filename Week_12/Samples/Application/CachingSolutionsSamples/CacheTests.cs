using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Managers;
using CachingSolutionsSamples.CacheEngines;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
        private const string redisHostname = "localhost";

        [TestMethod]
		public void MemoryCache_Categories()
		{
			var manager = new GeneralManager<Category>(new GeneralInMemoryCache<Category>());
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Products()
        {
            var manager = new GeneralManager<Product>(new GeneralInMemoryCache<Product>());
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Orders()
        {
            var manager = new GeneralManager<Order>(new GeneralInMemoryCache<Order>());
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Employee()
        {
            var manager = new GeneralManager<Employee>(new GeneralInMemoryCache<Employee>());
            CacheRunner(manager);
        }

        [TestMethod]
		public void RedisCache_Categories()
		{
			var manager = new GeneralManager<Category>(new GeneralRedisCache<Category>(redisHostname));
            CacheRunner(manager);
		}

        [TestMethod]
        public void RedisCache_Products()
        {
            var manager = new GeneralManager<Product>(new GeneralRedisCache<Product>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Orders()
        {
            var manager = new GeneralManager<Order>(new GeneralRedisCache<Order>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Customers()
        {
            var manager = new GeneralManager<Customer>(new GeneralRedisCache<Customer>(redisHostname));
            CacheRunner(manager);
        }

        private void Redis_DeleteCache()
        {
            //var employeeCache = new GeneralRedisCache<Employee>();
            //employeeCache.Delete("")
            //var manager = new GeneralManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
            //var manager = new GeneralManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
            //var manager = new GeneralManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
        }

        private void CacheRunner<T>(IManager<T> manager)
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(manager.GetAll().Count());
                Thread.Sleep(100);
            }
        }
	}
}
