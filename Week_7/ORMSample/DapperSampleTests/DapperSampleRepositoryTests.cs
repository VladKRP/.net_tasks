using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORMSample;
using ORMSample.Domain;

namespace ORMSampleTests
{
    [TestClass]
    public class DapperSampleRepositoryTests
    {
        private readonly DapperSampleRepository _dapperQueries;
        private const string ConnectionString = @"";//required

        public DapperSampleRepositoryTests()
        {
            _dapperQueries = new DapperSampleRepository(ConnectionString);
        }

        [TestMethod]
        public void GetProductsWithCategoryAndSuppliers_Test()
        {
            var products  = _dapperQueries.GetProductsWithCategoryAndSuppliers();
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Any());
            Assert.IsNotNull(products.ElementAt(0).Category);
            Assert.IsNotNull(products.ElementAt(0).Supplier);
        }

        [TestMethod]
        public void GetEmployeesWithRegion_Test()
        {
            var employeesRegions = _dapperQueries.GetEmployeesWithRegion();
            Assert.IsNotNull(employeesRegions);
            Assert.IsTrue(employeesRegions.Any());
        }

        [TestMethod]
        public void GetAmountOfEmployeesByRegion_Test()
        {
            var amount = _dapperQueries.GetAmountOfEmployeesByRegion();
            Assert.IsNotNull(amount);
            Assert.IsTrue(amount.Any());
        }

        [TestMethod]
        public void GetEmployeeWithSuppliers_Test()
        {
            var employeeSuppliers = _dapperQueries.GetEmployeeWithSuppliers();
            Assert.IsNotNull(employeeSuppliers);
            Assert.IsTrue(employeeSuppliers.Any());
        }


        [TestMethod]
        public void AddEmployeeWithTerritories_Test()
        {
            var territories = new List<Territory>()
            {
                new Territory(){ TerritoryID = "10" },
                new Territory(){ TerritoryID = "01581" },
            };
            var employee = new Employee() { FirstName = "Victor", LastName = "Victory", BirthDate = new DateTime(1988,12,21), HireDate = DateTime.Now, Territories = territories };

            _dapperQueries.AddEmployeeWithTerritories(employee);
        }

        [TestMethod]
        public void ChangeProductsCategory_Test()
        {
            var category = new Category() { CategoryID = 2 };
            var newCategory = new Category() { CategoryID = 3 };

            _dapperQueries.ChangeProductsCategory(category, newCategory);
        }

        [TestMethod]
        public void AddProductsWithSuppliersAndCategories_Test()
        {
            Product[] products =
            {
                new Product()
                {
                    ProductName = "Google Air Pods",
                    Category = new Category(){ CategoryName = "Headphones"},
                    Supplier = new Supplier(){ CompanyName = "Google"}
                    
                },
                new Product()
                {
                    ProductName = "Apple Watch",
                    Category = new Category(){ CategoryName = "Smartwatch"},
                    Supplier = new Supplier() { ContactName = "Dummy Fikus", CompanyName = "Apple Inc."},
                },

                new Product()
                {
                    ProductName = "Huawei P20",
                    Category = new Category(){ CategoryName = "Smartphone"},
                    Supplier = new Supplier() { ContactName = "Kate Smith", CompanyName = "Huawei"},
                },
            };


            _dapperQueries.AddProductsWithSuppliersAndCategories(products);
        }


        [TestMethod]
        public void ReplaceProductWhileOrderNotShipped_Test()
        {
            var product = new Product() { ProductID = 17, ProductName = "Pizza Alfa", Discontinued = true };
            var newProduct = new Product() { ProductID = 76, ProductName = "Pizza 4 sezones", Discontinued = false };

            _dapperQueries.ReplaceProductWhileOrderNotShipped(product, newProduct);
        }


    }
}
