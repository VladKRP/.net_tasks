using EFORMSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFORMTests
{
    [TestClass]
    public class EFSampleRepositoryTests
    {

        private const string connectionString = @"Data Source=EPBYBREW0300\SQLEXPRESS;Initial Catalog=Northwind;
                                            Integrated Security=True;Connect Timeout=60;Encrypt=False;
                                            TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public EFSampleRepositoryTests() {}

        [TestMethod]
        public void EF_GetOrdersByCategory_Test()
        {
            NorthwindContext context = new NorthwindContext(connectionString);//required proper connection string
            EFSampleRepository repository = new EFSampleRepository();

            Category category = new Category() { CategoryName = "Shoes" };

            var result = repository.GetOrdersByCategory(category).ToList();
            Console.WriteLine(result);
            Assert.IsNotNull(result);//check based on data, filled by migration mechanism
        }

    }
}
