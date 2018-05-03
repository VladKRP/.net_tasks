using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ORMSample.Domain;
using Dapper.Contrib.Extensions;

namespace ORMSample
{
    public class DapperSampleRepository : IReadOnlyDBQueries, IWritableDBQueries
    {
        private readonly string _connectionString;

        public DapperSampleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetProductsWithCategoryAndSuppliers()
        {
            var query = $"select * from Northwind.Products as Product" +
                $" inner join Northwind.Categories as Category on Product.CategoryID = Category.CategoryID" +
                $" inner join Northwind.Suppliers as Supplier on Product.SupplierID = Supplier.SupplierID";

            IEnumerable<Product> resultProducts = new List<Product>();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                resultProducts = connection.Query<Product, Category, Supplier, Product>(query, (product, category, supplier) =>
                {
                    product.Supplier = supplier;
                    product.Category = category;
                    return product;
                }, splitOn: "ProductID, CategoryID, SupplierID");

            }
            return resultProducts;
        }

        public IEnumerable<EmployeeRegion> GetEmployeesWithRegion()
        {
            string query = @"select emp.EmployeeID, emp.FirstName, reg.RegionID, reg.RegionDescription
                             from Northwind.Employees as emp  
                             inner join Northwind.EmployeeTerritories as et on et.EmployeeID = emp.EmployeeID
                             inner join Northwind.Territories as ter on et.TerritoryID = ter.TerritoryID
                             inner join Northwind.Regions reg on ter.RegionID = reg.RegionID
                             group by emp.EmployeeID, emp.FirstName, reg.RegionID, reg.RegionDescription";

            IEnumerable<EmployeeRegion> employeesRegions = new List<EmployeeRegion>();

            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {

                employeesRegions = connection.Query<EmployeeRegion, Region, EmployeeRegion>(query, (eRegion, region) => {
                    eRegion.Region = region;
                    return eRegion;
                }, splitOn: "RegionID");

            }
            return employeesRegions;
        }

        public IEnumerable<EmployeesInRegion> GetAmountOfEmployeesByRegion()
        {
            string query = @"select Region, count(EmployeeID) as 'EmployeeAmount' from Northwind.Employees group by Region";
            IEnumerable<EmployeesInRegion> employeesInRegion = new List<EmployeesInRegion>();
            
            using (IDbConnection connection = new SqlConnection(_connectionString))
                employeesInRegion = connection.Query<EmployeesInRegion>(query);
            return employeesInRegion;
        }

        public IEnumerable<EmployeeSuppliers> GetEmployeeWithSuppliers()
        {
            string query = @"select emp.EmployeeID, sup.SupplierID from Northwind.Employees as emp
                            inner join Northwind.Orders as ord on emp.EmployeeID = ord.EmployeeID
                            inner join Northwind.[Order Details] as odet on ord.OrderID = odet.OrderID
                            inner join Northwind.Products as prod on odet.ProductID = prod.ProductID
                            inner join Northwind.Suppliers as sup on prod.SupplierID = sup.SupplierID";

            IEnumerable<EmployeeSuppliers> employeesSuppliers = new List<EmployeeSuppliers>();
            
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                //employeesSuppliers = connection.Query<EmployeeSuppliers, Employee, Supplier, EmployeeSuppliers>(query,
                //    (employeeSupplier, employee, supplier) =>
                //    {
                //        employeeSupplier.Employee = employee;
                //        employeeSupplier.Suppliers = supplier;
                //        return employeeSupplier;
                //    }, splitOn: "EmployeeID,OrderID,ProductID,SupplierID ");

                employeesSuppliers = connection.Query<EmployeeSupplier>(query)
                    .GroupBy(x => x.EmployeeID)
                    .Select(x => new EmployeeSuppliers() { EmployeeID = x.Key, SuppliersID = x.Select(y => y.SupplierID) });
            }
            return employeesSuppliers;
        }

        public void AddEmployeeWithTerritories(Employee employee)
        {
            var createEmployeeQuery = @"insert into Northwind.Employees(LastName,FirstName,
                Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,
                PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath) values(@LastName,@FirstName,
                @Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,
                @PostalCode,@Country,@HomePhone,@Extension,@Photo,@Notes,@ReportsTo,@PhotoPath)";

            var lastEmployeeQuery = @"select top 1 EmployeeID from Northwind.Employees 
                                      where FirstName = @FirstName and LastName = @LastName and BirthDate = @BirthDate 
                                      order by EmployeeID desc";

            var setEmployeeTerritoriesQuery = @"insert into Northwind.EmployeeTerritories(EmployeeID, TerritoryID) values(@EmployeeID, @TerritoryID)";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var employeeAddingResult = connection.Execute(createEmployeeQuery, new
                {
                    employee.LastName,
                    employee.FirstName,
                    employee.Title,
                    employee.TitleOfCourtesy,
                    employee.BirthDate,
                    employee.HireDate,
                    employee.Address,
                    employee.City,
                    employee.Region,
                    employee.PostalCode,
                    employee.Country,
                    employee.HomePhone,
                    employee.Extension,
                    employee.Photo,
                    employee.Notes,
                    employee.ReportsTo,
                    employee.PhotoPath
                });

                var addedEmployeeId = connection.Query<int>(lastEmployeeQuery, new { employee.FirstName, employee.LastName, employee.BirthDate }).Single();

                if (employee.Territories != null)
                {
                    foreach (var territory in employee.Territories)
                    {
                        var isTerritoryExists = connection.Query<int?>("select TerritoryID from Northwind.Territories where TerritoryID = @TerritoryID", new { territory.TerritoryID }).Any();
                        if (isTerritoryExists)
                            connection.Execute(setEmployeeTerritoriesQuery, new { EmployeeID = addedEmployeeId, territory.TerritoryID });
                    }
                }

            }
        }

        public void ChangeProductsCategory(Category currentCategory, Category newCategory)
        {
            var updateQuery = "update Northwind.Products set CategoryID = @NewCategoryID where CategoryID = @CurrentCategoryID ";
            using (IDbConnection connection = new SqlConnection(_connectionString))
                connection.Execute(updateQuery, new { NewCategoryID = newCategory.CategoryID, CurrentCategoryID = currentCategory.CategoryID });
        }

        public void AddProductsWithSuppliersAndCategories(IEnumerable<Product> products)
        {

            var insertProductQuery = @"insert into Northwind.Products
                (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
                values(@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)";

            var insertCategoryQuery = @"insert into Northwind.Categories(CategoryName, Description, Picture) 
                                        values(@CategoryName, @Description, @Picture);
                                        select scope_identity()";
            var insertSupplierQuery = @"insert into Northwind.Suppliers(CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage)
                                        values(@CompanyName,@ContactName,@ContactTitle,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax,@HomePage);
                                        select scope_identity()";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                foreach (var product in products)
                {
                    var categoryId = connection.Query<int>("select CategoryID from Northwind.Categories where CategoryName = @CategoryName",
                        new { product.Category.CategoryName });
                    if (!categoryId.Any())
                    {
                        product.CategoryID = connection.Query<int>(insertCategoryQuery, new
                        {
                            product.Category.CategoryName,
                            product.Category.Description,
                            product.Category.Picture
                        }).Single();
                    }
                    else
                        product.CategoryID = categoryId.SingleOrDefault();

                    var supplierId = connection.Query<int>("select SupplierID from Northwind.Suppliers where ContactName = @SupplierName",
                        new { SupplierName = product.Supplier.ContactName });
                    if (!supplierId.Any())
                    {
                        product.SupplierID = connection.Query<int>(insertSupplierQuery, new
                        {
                            product.Supplier.CompanyName,
                            product.Supplier.ContactName,
                            product.Supplier.ContactTitle,
                            product.Supplier.Address,
                            product.Supplier.City,
                            product.Supplier.Region,
                            product.Supplier.PostalCode,
                            product.Supplier.Country,
                            product.Supplier.Phone,
                            product.Supplier.Fax,
                            product.Supplier.HomePage
                        }).Single();
                    }
                    else
                        product.SupplierID = supplierId.SingleOrDefault();

                    if(product.SupplierID.HasValue && product.CategoryID.HasValue)
                    {
                        var productInsertingResult = connection.Execute(insertProductQuery, new
                        {
                            product.ProductName,
                            product.SupplierID,
                            product.CategoryID,
                            product.QuantityPerUnit,
                            product.UnitPrice,
                            product.UnitsInStock,
                            product.UnitsOnOrder,
                            product.ReorderLevel,
                            product.Discontinued
                        });
                    }
                    
                }
            }
        }

        public void ReplaceProductWhileOrderNotShipped(Product orderProduct, Product sameProduct)
        {
            var updateQuery = @"update Northwind.[Order Details] set ProductID = @NewProductID 
                                where ProductID = @CurrentProductID and
                                      OrderID in (select OrderID from Northwind.Orders where ShippedDate is null)";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute(updateQuery, new { NewProductID = sameProduct.ProductID, CurrentProductID = orderProduct.ProductID });
            }
                
        }
    }
}
