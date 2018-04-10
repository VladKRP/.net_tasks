using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORMSample;

namespace ORMSampleTests
{
    [TestClass]
    public class DapperQueriesTests
    {
        private readonly DapperQueries _dapperQueries;

        public DapperQueriesTests()
        {
            _dapperQueries = new DapperQueries(@"Data Source=EPBYBREW0300\SQLEXPRESS;Initial Catalog=Northwind;
                                                 Integrated Security=True;Connect Timeout=60;Encrypt=False;
                                                 TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        [TestMethod]
        public void GetProductsWithCategoryAndSuppliers_Test()
        {
            var products  = _dapperQueries.GetProductsWithCategoryAndSuppliers();
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetEmployeesWithRegion_Test()
        {
            var employeesRegions = _dapperQueries.GetEmployeesWithRegion();
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetAmountOfEmployeesByRegion_Test()
        {
            var amount = _dapperQueries.GetAmountOfEmployeesByRegion();
            throw new NotImplementedException();
        }

        
    }
}
