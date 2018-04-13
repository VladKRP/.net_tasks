using EFORMSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFORMTests
{
    [TestClass]
    public class EFSampleRepositoryTests
    {

        public EFSampleRepositoryTests() { }

        [TestMethod]
        public void EF_GetOrdersByCategory_Test()
        {
            EFSampleRepository repository = new EFSampleRepository();

            Category category = new Category() { CategoryName = "Shoes" };

            var result = repository.GetOrdersByCategory(category);
            Assert.IsNotNull(result);
        }

    }
}
