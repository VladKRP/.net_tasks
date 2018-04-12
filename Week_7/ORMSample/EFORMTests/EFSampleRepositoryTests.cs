using EFORMSample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFORMTests
{
    [TestClass]
    public class EFSampleRepositoryTests
    {

        [TestMethod]
        public void EF_GetOrdersByCategory_Test()
        {
            EFSampleRepository repository = new EFSampleRepository();

            Category category = new Category()
            {
                CategoryID = 2,
                CategoryName = "Seafood"
            };

            var result = repository.GetOrdersByCategory(category);
        }

    }
}
