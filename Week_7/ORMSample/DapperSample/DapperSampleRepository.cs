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
using ORMSample.Interfaces;
using DapperSamples;

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
            var query = @"select Product.ProductID, Product.ProductName, Product.QuantityPerUnit, Product.UnitPrice,
                                 Product.UnitsInStock,Product.UnitsOnOrder,Product.ReorderLevel,Product.Discontinued,
                                 Category.CategoryID, Category.CategoryName, Category.Description, Category.Picture,
                                 Supplier.SupplierID, Supplier.CompanyName, Supplier.ContactName, Supplier.ContactTitle,
                                 Supplier.Address, Supplier.City, Supplier.Region, Supplier.PostalCode, Supplier.Country,
                                 Supplier.Phone, Supplier.Fax, Supplier.HomePage
                                     from dbo.Products as Product
                                     inner join dbo.Categories as Category on Product.CategoryID = Category.CategoryID
                                     inner join dbo.Suppliers as Supplier on Product.SupplierID = Supplier.SupplierID";

            IEnumerable<Product> resultProducts = new List<Product>();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                resultProducts = connection.Query<Product, Category, Supplier, Product>(query, (product, category, supplier) =>
                {  
                    product.Category = category;
                    product.Supplier = supplier;
                    return product;
                }, splitOn: "CategoryID, SupplierID");
            }
            return resultProducts;
        }

        public IEnumerable<EmployeeRegion> GetEmployeesWithRegion()
        {
            string query = @"select ter.TerritoryID, emp.EmployeeID, emp.FirstName, emp.LastName, emp.Title, emp.TitleOfCourtesy,
                                    emp.Address, emp.City, emp.Region, emp.PostalCode, emp.Country, emp.BirthDate, emp.HireDate,
                                    emp.HomePhone,emp.Extension, emp.ReportsTo,
                                    reg.RegionID, reg.RegionDescription
                                         from dbo.Employees as emp
                                         inner join dbo.EmployeeTerritories as eter on emp.EmployeeID = eter.EmployeeID
                                         inner join dbo.Territories as ter on  eter.TerritoryID = ter.TerritoryID
                                         inner join dbo.Region as reg on ter.RegionID = reg.RegionID";
            IEnumerable<EmployeeRegion> employeesRegions = new List<EmployeeRegion>();

            using (IDbConnection connection =
                new SqlConnection(_connectionString))
            {
                var result  = connection.Query<EmployeeRegion, Employee, Region, EmployeeRegion>(query, (eRegion, emp, region) => {
                    eRegion.Employee = emp;
                    eRegion.Region = region;
                    return eRegion;
                }, splitOn: "EmployeeID,RegionID").GroupBy(firstSupplier => firstSupplier.Employee.EmployeeID);
                employeesRegions = result.Select(firstSupplier => firstSupplier.FirstOrDefault());
            }
            return employeesRegions;
        }

        public IEnumerable<EmployeesInRegion> GetAmountOfEmployeesByRegion()
        {
            string query = @"select COUNT(distinct et.EmployeeID) as 'EmployeeAmount',  reg.RegionID, reg.RegionDescription
                            from dbo.Employees as emp  
                            inner join dbo.EmployeeTerritories as et on et.EmployeeID = emp.EmployeeID
                            inner join dbo.Territories as ter on et.TerritoryID = ter.TerritoryID
                            inner join dbo.Region reg on ter.RegionID = reg.RegionID
                            group by reg.RegionID, reg.RegionDescription";
            IEnumerable<EmployeesInRegion> employeesInRegion = new List<EmployeesInRegion>();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                employeesInRegion = connection.Query<EmployeesInRegion, Region, EmployeesInRegion>(query, (result, region) =>
                {
                    result.Region = region;
                    return result;
                }, splitOn:"RegionID");
            }
                
            return employeesInRegion;
        }

        public IEnumerable<EmployeeSuppliers> GetEmployeeWithSuppliers()
        {
            string query = @"select odet.OrderID, 
                                emp.EmployeeID, emp.FirstName, emp.LastName, emp.Title, emp.TitleOfCourtesy,
                                emp.Address, emp.City, emp.Region, emp.PostalCode, emp.Country, emp.BirthDate, emp.HireDate,
                                emp.HomePhone,emp.Extension, emp.ReportsTo,
                                sup.SupplierID, sup.CompanyName, sup.ContactName, sup.Address, sup.City, sup.Country
                                    from dbo.Orders as ord 
                                    inner join dbo.Employees as emp on ord.EmployeeID = emp.EmployeeID
                                    inner join dbo.[Order Details] as odet on ord.OrderID = odet.OrderID
                                    inner join dbo.Products as prod on odet.ProductID = prod.ProductID
                                    inner join dbo.Suppliers as sup on prod.SupplierID = sup.SupplierID";

            IEnumerable<EmployeeSuppliers> employeesSuppliers = new List<EmployeeSuppliers>();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.Query<EmployeeSupplier, Employee, Supplier, EmployeeSupplier>(query,
                    (employeeSupplier, employee, supplier) =>
                    {
                        employeeSupplier.Employee = employee;
                        employeeSupplier.Supplier = supplier;
                        return employeeSupplier;
                    }, splitOn: "EmployeeID,SupplierID");

                var groupedEmployees = result.GroupBy(esup => esup.Employee.EmployeeID);
                employeesSuppliers = groupedEmployees.Select(gemp => new EmployeeSuppliers()
                {

                    Employee = gemp.FirstOrDefault().Employee,
                    Suppliers = gemp.Select(sup => sup.Supplier).Distinct(new SupplierEqualityComparer())
                });
            }
            return employeesSuppliers;

    }

        

        public void AddEmployeeWithTerritories(Employee employee)
        {
            var createEmployeeQuery = @"insert into dbo.Employees(LastName,FirstName,
                Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,
                PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath) values(@LastName,@FirstName,
                @Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,
                @PostalCode,@Country,@HomePhone,@Extension,@Photo,@Notes,@ReportsTo,@PhotoPath)";

            var lastEmployeeQuery = @"select top 1 EmployeeID from dbo.Employees 
                                      where FirstName = @FirstName and LastName = @LastName and BirthDate = @BirthDate 
                                      order by EmployeeID desc";

            var setEmployeeTerritoriesQuery = @"insert into dbo.EmployeeTerritories(EmployeeID, TerritoryID) values(@EmployeeID, @TerritoryID)";

            var territoryGetQuery = "select TerritoryID from dbo.Territories where TerritoryID = @TerritoryID";

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
                        var isTerritoryExists = connection.Query<int>(territoryGetQuery, new { territory.TerritoryID }).Any();
                        if (isTerritoryExists)
                            connection.Execute(setEmployeeTerritoriesQuery, new { EmployeeID = addedEmployeeId, territory.TerritoryID });
                    }
                }

            }
        }

        public void ChangeProductsCategory(Category currentCategory, Category newCategory)
        {
            var updateQuery = "update dbo.Products set CategoryID = @NewCategoryID where CategoryID = @CurrentCategoryID ";
            using (IDbConnection connection = new SqlConnection(_connectionString))
                connection.Execute(updateQuery, new { NewCategoryID = newCategory.CategoryID, CurrentCategoryID = currentCategory.CategoryID });
        }

        public void AddProductsWithSuppliersAndCategories(IEnumerable<Product> products)
        {

            var insertProductQuery = @"insert into dbo.Products
                (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
                values(@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)";

            var insertCategoryQuery = @"insert into dbo.Categories(CategoryName, Description, Picture) 
                                        values(@CategoryName, @Description, @Picture);
                                        select scope_identity()";
            var insertSupplierQuery = @"insert into dbo.Suppliers(CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage)
                                        values(@CompanyName,@ContactName,@ContactTitle,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax,@HomePage);
                                        select scope_identity()";

            var getCategoryByNameQuery = "select CategoryID from dbo.Categories where CategoryName = @CategoryName";

            var getSupplierByNameQuery = "select SupplierID from dbo.Suppliers where ContactName = @SupplierName";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                foreach (var product in products)
                {
                    var categoryId = connection.Query<int>(getCategoryByNameQuery, new { product.Category.CategoryName });
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

                    var supplierId = connection.Query<int>(getSupplierByNameQuery,  new { SupplierName = product.Supplier.ContactName });
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
            var updateQuery = @"update dbo.[Order Details] set ProductID = @NewProductID 
                                where ProductID = @CurrentProductID and
                                      OrderID in (select OrderID from dbo.Orders where ShippedDate is null)";

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute(updateQuery, new { NewProductID = sameProduct.ProductID, CurrentProductID = orderProduct.ProductID });
            }
                
        }
    }
}
