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
                }, splitOn:"ProductID, CategoryID, SupplierID");
                
            }
            return resultProducts;
        }

        public IEnumerable<EmployeeRegion> GetEmployeesWithRegion()
        {
            IEnumerable<EmployeeRegion> employeesRegions = new List<EmployeeRegion>();

            string query = @"select distinct res.* from (
                             select emp.EmployeeID, emp.FirstName, reg.RegionID, reg.RegionDescription
                             from Northwind.Employees as emp  
                             inner join Northwind.EmployeeTerritories as et on et.EmployeeID = emp.EmployeeID
                             inner join Northwind.Territories as ter on et.TerritoryID = ter.TerritoryID
                             inner join Northwind.Regions reg on ter.RegionID = reg.RegionID) as res";

            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                employeesRegions = connection.Query<EmployeeRegion, Employee, Region, EmployeeRegion>(query,
                 (employeeRegion, employee, region) =>
                 {
                     employeeRegion.Employee = employee;
                     employeeRegion.Region = region;
                     return employeeRegion;
                 }, splitOn: "EmployeeID,RegionID");
            }
            return employeesRegions;
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
            IEnumerable<EmployeeSuppliers> employeesSuppliers = new List<EmployeeSuppliers>();
            string query = @"select * from Northwind.Orders as Orders
                            inner join Northwind.[Order Details] as OrderDetail on Orders.OrderID = OrderDetail.OrderID
                            inner join Northwind.Products as Product on OrderDetail.ProductID = Product.ProductID
                            inner join Northwind.Employees as Employee on Orders.EmployeeID = Employee.EmployeeID
                            inner join Northwind.Suppliers as Supplier on Product.SupplierID = Supplier.SupplierID";

            string query2 = @"select * from Northwind.Employees as Employee
                            inner join Northwind.Orders as Orders on Employee.EmployeeID = Orders.EmployeeID
                            inner join Northwind.[Order Details] as OrderDetail on Orders.OrderID = OrderDetail.OrderID
                            inner join Northwind.Products as Product on OrderDetail.ProductID = Product.ProductID
                            inner join Northwind.Suppliers as Supplier on Product.SupplierID = Supplier.SupplierID";
            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                employeesSuppliers = connection.Query<EmployeeSuppliers, Order, OrderDetail, Product, Employee, Supplier, EmployeeSuppliers>(query2,
                    (employeeSupplier, order, orderDetail, product, employee, supplier) => {
                        employeeSupplier.Employee = employee;
                        employeeSupplier.Suppliers = supplier;
                        return employeeSupplier;
                    }, splitOn: "OrderID,ProductID,SupplierID ");
            }
            return employeesSuppliers;
            throw new NotImplementedException();
        }

        public void ChangeProductToAnother()
        {
            var query = "select * from Northwind.Orders where ShippedDate is null";

            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                var res = connection.Query(query);
            }
        }
    }
}
