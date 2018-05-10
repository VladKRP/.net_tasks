using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Managers;
using CachingSolutionsSamples.CacheEngines;
using System.Runtime.Caching;
using System.Data.SqlClient;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
        private const string redisHostname = "localhost";
        private const string mssqlConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;
                                                        Integrated Security=True;Encrypt=False;
                                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public CacheTests()
        {
            //Redis_CacheCleanup();
        }

        [TestMethod]
		public void MemoryCache_Categories()
		{
            
            string sqlQuery = "select CategoryID, CategoryName, Description, Picture from dbo.Categories";
         
            using (SqlConnection sqlConnection = new SqlConnection(mssqlConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    sqlCommand.Notification = null;

                    SqlDependency sqlDependency = new SqlDependency(sqlCommand);
                    SqlDependency.Start(mssqlConnectionString);

                    sqlConnection.Open();
                    var executionResult = sqlCommand.ExecuteScalar();

                    SqlChangeMonitor sqlChangeMonitor = new SqlChangeMonitor(sqlDependency);

                    var manager = new NorthwindMemoryCacheManager<Category>(new GeneralInMemoryCache<Category>(), sqlChangeMonitor);
                    CacheRunner<Category>(manager);

                    //SqlDependency.Stop(mssqlConnectionString);
                    //sqlConnection.Close();    

                }   
            }
        }

        [TestMethod]
        public void MemoryCache_Products()
        {
            var manager = new NorthwindMemoryCacheManager<Product>(new GeneralInMemoryCache<Product>(), DateTime.UtcNow.AddSeconds(5));
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Orders()
        {
            var manager = new NorthwindMemoryCacheManager<Order>(new GeneralInMemoryCache<Order>());
            CacheRunner(manager);
        }

        [TestMethod]
        public void MemoryCache_Employee()
        {
            var manager = new NorthwindMemoryCacheManager<Employee>(new GeneralInMemoryCache<Employee>());
            CacheRunner(manager);
        }

        [TestMethod]
		public void RedisCache_Categories()
		{
			var manager = new NorthwindRedisCacheManager<Category>(new GeneralRedisCache<Category>(redisHostname), DateTime.UtcNow.AddSeconds(10));
            CacheRunner(manager);
		}

        [TestMethod]
        public void RedisCache_Products()
        {
            var manager = new NorthwindRedisCacheManager<Product>(new GeneralRedisCache<Product>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Orders()
        {
            var manager = new NorthwindRedisCacheManager<Order>(new GeneralRedisCache<Order>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Customers()
        {
            var manager = new NorthwindRedisCacheManager<Customer>(new GeneralRedisCache<Customer>(redisHostname));
            CacheRunner(manager);
        }

        [TestMethod]
        public void RedisCache_Employees()
        {
            var manager = new NorthwindRedisCacheManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
            CacheRunner(manager);
        }

        private void Redis_CacheCleanup()
        {
            var employeeManager = new NorthwindRedisCacheManager<Employee>(new GeneralRedisCache<Employee>(redisHostname));
            employeeManager.DeleteAll();
            var customerManager = new NorthwindRedisCacheManager<Customer>(new GeneralRedisCache<Customer>(redisHostname));
            customerManager.DeleteAll();
            var orderManager = new NorthwindRedisCacheManager<Order>(new GeneralRedisCache<Order>(redisHostname));
            orderManager.DeleteAll();
            var productManager = new NorthwindRedisCacheManager<Product>(new GeneralRedisCache<Product>(redisHostname));
            productManager.DeleteAll();
            var categoryManager = new NorthwindRedisCacheManager<Category>(new GeneralRedisCache<Category>(redisHostname));
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
