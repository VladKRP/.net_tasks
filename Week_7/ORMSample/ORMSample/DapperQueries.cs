using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ORMSample.Domain;

namespace ORMSample
{
    public class DapperQueries:IReadOnlyDBQueries
    {
        private readonly string _connectionString;

        public DapperQueries(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetProductsWithCategoryAndSuppliers()
        {
            IEnumerable<Product> resultProducts = new List<Product>();

            var query = $"select * from Northwind.Products as Product" +
                $" inner join Northwind.Categories as Category on Product.CategoryID = Category.CategoryID" +
                $" inner join Northwind.Suppliers as Supplier on Product.SupplierID = Supplier.SupplierID";


            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                resultProducts = connection.Query<Product, Category, Supplier, Product>(query, (product, category, supplier) => {
                    product.Supplier = supplier;
                    product.Category = category;
                    return product;
                }, splitOn:"SupplierID, CategoryID");
                
            }
            return resultProducts;
        }

        public IEnumerable<EmployeeRegion> GetEmployeesWithRegion()
        {
            IEnumerable<EmployeeRegion> employeesRegions = new List<EmployeeRegion>();

            string query = @"select * from Northwind.Employees as Employee
                            inner join Northwind.EmployeeTerritories et on Employee.EmployeeID = et.EmployeeID
                            inner join Northwind.Territories t on et.TerritoryID = t.TerritoryID
                            inner join Northwind.Region Region on t.RegionID = Region.RegionID";

            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                employeesRegions = connection.Query<EmployeeRegion, EmployeeTerritory, Territory, Region, EmployeeRegion>(query,
                 (employeeRegion, eterritory, territory, region) =>
                 {
                     employeeRegion.Region = region;
                     return employeeRegion;
                 }, splitOn: "TerritoryID, RegionID");
            }
                return employeesRegions;

            throw new NotImplementedException();
        }

        public IEnumerable<EmployeesInRegion> GetAmountOfEmployeesByRegion()
        {
            IEnumerable<EmployeesInRegion> employeesInRegion = new List<EmployeesInRegion>();
            string query = @"select Region, count(EmployeeID) as 'EmployeeAmount' from Northwind.Employees group by Region";
            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                employeesInRegion = connection.Query<EmployeesInRegion>(query);
            }
            return employeesInRegion;
        }

        public IEnumerable<EmployeeSuppliers> GetEmployeeWithSuppliers()
        {
            throw new NotImplementedException();
        }
    }
}
