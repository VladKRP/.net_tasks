// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();


        [Category("Linq")]
        [Title("Task 1")]
        [Description("This sample return customers wich sum of orders more then defined value")]
        public void Linq1()
        {
            var sumBarrier = 6000;
            var customers = dataSource.Customers.Where(customer => customer != null &&
                                                                   customer.Orders != null &&
                                                                   customer.Orders.Count() > 0 &&
                                                                   customer.Orders.Select(order => order.Total).Sum() > sumBarrier);

            ObjectDumper.Write($"With value 6000 customers count {customers.Count()}{Environment.NewLine}");
            ObjectDumper.Write(customers);

            sumBarrier = 10000;
            ObjectDumper.Write($"{Environment.NewLine}With value 10000 customers count {customers.Count()}{Environment.NewLine}");
            ObjectDumper.Write(customers);

        }


        [Category("Linq")]
        [Title("Task 2")]
        [Description("This sample return customers with list of suppliers that lives in same country and city with customer")]
        public void Linq2()
        {
            var customersWithSuppliers = dataSource.Customers.Where(customer => customer != null)
                                                             .Select(customer => new
                                                             {
                                                                 customer,
                                                                 suppliers = dataSource.Suppliers.Where(supplier => supplier != null &&
                                                                                                                    string.Equals(supplier.Country, customer.Country) &&
                                                                                                                    string.Equals(supplier.City, customer.City))
                                                             }).Where(cs => cs.suppliers.Count() > 0);

            ObjectDumper.Write(customersWithSuppliers, 1);
            ObjectDumper.Write(Environment.NewLine);

            var customerWithSuppliersWithGroupBy = dataSource.Customers.Join(dataSource.Suppliers,
                customer => new { customer.City, customer.Country },
                supplier => new { supplier.City, supplier.Country },
                (customer, supplier) => new
                {
                    customer,
                    supplier
                }).GroupBy(x => x.customer).Select(cs => new
                {
                    customer = cs.Key,
                    suppliers = cs.Select(x => x.supplier)
                });


            ObjectDumper.Write(customerWithSuppliersWithGroupBy, 1);
        }


        [Category("Linq")]
        [Title("Task 3")]
        [Description("This sample return customers that has order with price more that determined price")]
        public void Linq3()
        {
            int price = 6000;
            var customers = dataSource.Customers.Where(customer => customer != null &&
                                                                   customer.Orders != null &&
                                                                   customer.Orders.Count() > 0 &&
                                                                   customer.Orders.Any(order => order.Total > price));
            ObjectDumper.Write(customers, 1);
        }

        [Category("Linq")]
        [Title("Task 4")]
        [Description("This sample return customers with they first purchase date")]
        public void Linq4()
        {
            var customersWithStartDate = dataSource.Customers.Where(customer => customer != null && customer.Orders != null && customer.Orders.Count() > 0)
                                                             .Select(customer => new
                                                             {
                                                                 customer,
                                                                 startDate = customer.Orders.Min(order => order.OrderDate).ToString("MM/yyyy")
                                                             });

            ObjectDumper.Write(customersWithStartDate, 1);
        }

        [Category("Linq")]
        [Title("Task 5")]
        [Description("This sample return customers with they first purchase date and sorted by year, month, customer purchase and name ")]
        public void Linq5()
        {
            var customersWithStartDate = dataSource.Customers
                                                    .Where(customer => customer != null && customer.Orders.Count() > 0)
                                                    .Select(customer => new
                                                    {
                                                        customer,
                                                        startDate = customer.Orders.Min(order => order.OrderDate)
                                                    })
                                                     .OrderByDescending(c => c.startDate.Year)
                                                     .ThenByDescending(c => c.startDate.Month)
                                                     .ThenByDescending(c => c.customer.Orders.Sum(order => order.Total))
                                                     .ThenBy(c => c.customer.CompanyName)
                                                     .Select(c => new {
                                                         c.customer,
                                                         startDate = c.startDate.ToString("MM/yyyy")
                                                     });
            ObjectDumper.Write(customersWithStartDate, 1);
        }


        [Category("Linq")]
        [Title("Task 6")]
        [Description("This sample return customers with wrong postal code or not filled region or incorrect phone number")]
        public void Linq6()
        {
            var customers = dataSource.Customers.Where(customer => customer != null &&
                                                                   string.IsNullOrWhiteSpace(customer.PostalCode) ||
                                                                   !customer.PostalCode.All(code => char.IsDigit(code)) ||
                                                                   string.IsNullOrWhiteSpace(customer.Region) ||
                                                                   string.IsNullOrEmpty(customer.Phone) ||
                                                                   !new Regex(@"^\(\d+\)").Match(customer.Phone).Success);
            ObjectDumper.Write(customers);
        }

        [Category("Linq")]
        [Title("Task 7")]
        [Description("This sample group products by categories, then by existence on stock, then by price ")]
        public void Linq7()
        {
            var groupedByCategoryProducts = dataSource.Products.Where(product => product != null && !string.IsNullOrWhiteSpace(product.Category))
                                                               .GroupBy(product => product.Category)
                                                               .Select(gcProduct => new
                                                               {
                                                                   category = gcProduct.Key,
                                                                   productsByExistenceInStock = gcProduct.GroupBy(product => product.UnitsInStock > 0)
                                                                                                 .Select(gcuProduct => new
                                                                                                 {
                                                                                                     IsInStock = gcuProduct.Key,
                                                                                                     productsByPrice = gcuProduct.OrderBy(p => p.UnitPrice)
                                                                                                 })
                                                               });
            ObjectDumper.Write(groupedByCategoryProducts, 3);
        }


        enum PriceGroup
        {
            None,
            Cheap,
            Medium,
            Expensive
        };

        [Category("Linq")]
        [Title("Task 8")]
        [Description("This sample group all products by 3 category: cheap, medium and expensive price")]
        public void Linq8()
        {
            var productsGroupedByPriceGroup = dataSource.Products.Where(product => product != null)
                                                                 .Select(product => new
                                                                 {
                                                                     product,
                                                                     priceGroup = DefinePriceGroup(product.UnitPrice)
                                                                 }).GroupBy(pg => pg.priceGroup);
            ObjectDumper.Write(productsGroupedByPriceGroup, 2);
        }

        private PriceGroup DefinePriceGroup(decimal price)
        {
            PriceGroup priceGroup = PriceGroup.None;
            if (price < 100)
                priceGroup = PriceGroup.Cheap;
            else if (price >= 100 && price <= 200)
                priceGroup = PriceGroup.Medium;
            else if (price > 200)
                priceGroup = PriceGroup.Expensive;
            return priceGroup;
        }



        [Category("Linq")]
        [Title("Task 9")]
        [Description("This sample calculate average income and purchase intensity for each city")]
        public void Linq9()
        {
            var groupedCustomers = dataSource.Customers.Where(customer => customer != null && !string.IsNullOrWhiteSpace(customer.City))
                                                       .GroupBy(customer => customer.City);
            var citiesOrdersStats = groupedCustomers.Select(group => new
            {
                city = group.Key,
                averageIncome = group.Average(customer => customer.Orders != null && customer.Orders.Count() > 0 ? customer.Orders.Select(order => order.Total).Average() : 0),
                averagePurchaseIntensity = group.Average(customer => customer.Orders.Count())
            });
            ObjectDumper.Write(citiesOrdersStats, 1);
        }

        [Category("Linq")]
        [Title("Task 10")]
        [Description("This sample calculate client activity only by monthes, only by years and by month and year")]
        public void Linq10()
        {
            var orders = dataSource.Customers.Where(customer => customer != null && customer.Orders != null)
                                             .SelectMany(customer => customer.Orders);

            var monthActivityStats = orders.GroupBy(order => order.OrderDate.Month)
                                           .OrderBy(gOrder => gOrder.Key)
                                           .Select(gOrder => new
                                           {
                                               month = gOrder.Key,
                                               clientsActivity = gOrder.Count()
                                           });

            var yearActivityStats = orders.GroupBy(order => order.OrderDate.Year)
                                          .OrderBy(gOrder => gOrder.Key)
                                          .Select(gOrder => new
                                          {
                                              year = gOrder.Key,
                                              clientsActivity = gOrder.Count()
                                          });

            var monthYearActivityStats = orders.GroupBy(order => order.OrderDate.Year)
                                               .OrderBy(gOrder => gOrder.Key)
                                               .Select(gyOrder => new
                                               {
                                                   year = gyOrder.Key,
                                                   groupedOrders = gyOrder.GroupBy(order => order.OrderDate.Month)
                                                                           .OrderBy(gOrder => gOrder.Key)
                                                                           .Select(gmOrder => new
                                                                           {
                                                                               month = gmOrder.Key,
                                                                               clientsActivity = gmOrder.Count()
                                                                           })
                                               });
            ObjectDumper.Write(monthActivityStats, 1);
            ObjectDumper.Write(Environment.NewLine);
            ObjectDumper.Write(yearActivityStats, 1);
            ObjectDumper.Write(Environment.NewLine);
            ObjectDumper.Write(monthYearActivityStats, 2);
        }
    }
}
