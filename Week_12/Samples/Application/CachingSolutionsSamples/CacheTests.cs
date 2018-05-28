using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Managers;
using CachingSolutionsSamples.CacheEngines;
using System.Runtime.Caching;
using System.Data.SqlClient;
using TableDependency.SqlClient;
using TableDependency.EventArgs;
using System.Collections.Generic;
using TableDependency.Abstracts;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
        private const string redisHostname = "localhost";
        private const string connectionString = @"Server=;Database=Northwind;User ID=;password=";
        public CacheTests()
        {
            //Redis_CacheCleanup();
        }

        [TestMethod]
		public void MemoryCache_Categories()
		{
            using (var tableDependency = new SqlTableDependency<Category>(connectionString, "Categories", "Northwind"))
            {
                using (var manager = new NorthwindCacheManager<Category>(new GeneralInMemoryCache<Category>(), tableDependency))
                {
                    CacheRunner<Category>(manager);
                }
                    
            }
        }

       

        [TestMethod]
        public void MemoryCache_Products()
        {
            var manager = new NorthwindCacheManager<Product>(new GeneralInMemoryCache<Product>(), DateTime.UtcNow.AddSeconds(10));
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Orders()
        {
            var manager = new NorthwindCacheManager<Order>(new GeneralInMemoryCache<Order>());
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Employee()
        {
            var manager = new NorthwindCacheManager<Employee>(new GeneralInMemoryCache<Employee>());
            CacheRunner(manager);
        }

        [TestMethod]
		public void RedisCache_Categories()
		{
			var manager = new NorthwindCacheManager<Category>(new GeneralRedisCache<Category>(redisHostname), DateTime.UtcNow.AddSeconds(10));
            CacheRunner(manager);
		}

        [TestMethod]
        public void RedisCache_Products()
        {
            var manager = new NorthwindCacheManager<Product>(new GeneralRedisCache<Product>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Orders()
        {
            var manager = new NorthwindCacheManager<Order>(new GeneralRedisCache<Order>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Customers()
        {
            var manager = new NorthwindCacheManager<Customer>(new GeneralRedisCache<Customer>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Employees()
        {
            var manager = new NorthwindCacheManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
            CacheRunner(manager);
        }

        private void Redis_CacheCleanup()
        {
            var employeeManager = new NorthwindCacheManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
            employeeManager.DeleteAll();
            var customerManager = new NorthwindCacheManager<Customer>(new GeneralRedisCache<Customer>(redisHostname));
            customerManager.DeleteAll();
            var orderManager = new NorthwindCacheManager<Order>(new GeneralRedisCache<Order>(redisHostname));
            orderManager.DeleteAll();
            var productManager = new NorthwindCacheManager<Product>(new GeneralRedisCache<Product>(redisHostname));
            productManager.DeleteAll();
            var categoryManager = new NorthwindCacheManager<Category>(new GeneralRedisCache<Category>(redisHostname));
            categoryManager.DeleteAll();
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
