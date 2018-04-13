namespace EFORMSample.Migrations
{
    using ORMSample.Domain;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFORMSample.NorthwindContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NorthwindContext context)
        {
            if (!context.Database.Exists())
                return;

            ////Task 1 Seeding

                ////Task 3 Seeding
                if (!context.Categories.Any())
            {
                Category[] categories =
               {
                    new Category(){ CategoryName = "Electronic", Description = "Description conserning electric devices" },
                    new Category(){ CategoryName = "Food", Description = "Food description"},
                    new Category(){ CategoryName = "Shoes", Description = "Shoes description"},
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Categories, categories);
                context.SaveChanges();
            }
                ////

            if (!context.Products.Any())
            {
                Product[] products =
                {
                    new Product() {
                        ProductName = "Nike Revolution U4",
                        UnitPrice = 90, UnitsOnOrder = 15, UnitsInStock = 100, ReorderLevel = 3,
                        Discontinued = false, CategoryID = 3
                    },
                    new Product() {
                        ProductName = "FILA",
                        UnitPrice = 160, UnitsOnOrder = 23, UnitsInStock = 124, ReorderLevel = 4,
                        Discontinued = false, CategoryID = 3
                    },
                    new Product() {
                        ProductName = "Asus Vivo Book",
                        UnitPrice = 660, UnitsOnOrder = 13, UnitsInStock = 65, ReorderLevel = 2,
                        Discontinued = false,CategoryID = 1
                    }
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Products, products);
                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                Customer[] customers =
                {
                    new Customer(){ CustomerID = "25", ContactName = "Jone Mill", CompanyName = "ABC", EstablishDate = DateTime.Now},
                    new Customer(){ CustomerID = "38", ContactName = "Polly Sebas", CompanyName = "XYZ", EstablishDate = DateTime.Now}
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Customers, customers);
                context.SaveChanges();
            }

            if (!context.Employees.Any())
            {
                Employee[] employees =
                {
                    new Employee() {
                        FirstName = "Mark",
                         LastName = "Levi"
                    },
                    new Employee() {
                        FirstName = "Fillip",
                         LastName = "Blank"
                    },
                    new Employee() {
                        FirstName = "Polly",
                         LastName = "Adams"
                    }
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Employees, employees);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                Order[] orders =
                {
                    new Order() {
                        CustomerID = "25",
                        EmployeeID = 1 
                    },
                    new Order() {
                        CustomerID = "25",
                        EmployeeID = 1
                    },
                    new Order() {
                        CustomerID = "38",
                        EmployeeID = 2
                    }
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Orders, orders);
                context.SaveChanges();
            }

            if (!context.OrderDetails.Any())
            {

                OrderDetail[] orderDetails =
                {
                    new OrderDetail(){ OrderID = 1, ProductID = 2,  UnitPrice = 100, Quantity = 23, Discount = 12},
                    new OrderDetail(){  OrderID = 2, ProductID = 1,  UnitPrice = 33, Quantity = 2, Discount = 1},
                    new OrderDetail(){  OrderID = 3, ProductID = 3,  UnitPrice = 1, Quantity = 33, Discount = 5 }
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.OrderDetails, orderDetails);
                context.SaveChanges();
            }

            ////


            ////Task 3 Seeding

            if (!context.Regions.Any())
            {
                Region[] regions =
                {
                    new Region(){ RegionDescription = "West"},
                    new Region(){ RegionDescription = "South"}
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Regions, regions);
                context.SaveChanges();
            }

            if (!context.Territories.Any())
            {
                Territory[] territories =
                {
                    new Territory(){ TerritoryID = "1", TerritoryDescription = "TER 1" , RegionID = 1},
                    new Territory(){ TerritoryID = "2", TerritoryDescription = "TER 2", RegionID = 2}
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Territories, territories);
                context.SaveChanges();
            }
            ////


            base.Seed(context);
        }
    }
}
