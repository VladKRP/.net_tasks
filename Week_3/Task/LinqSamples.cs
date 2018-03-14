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
        [Description("This sample return customers wich sum of orders more then value sumBarrier")]
        public void Linq1()
        {
            var sumBarrier = 6000;
            var customers = dataSource.Customers.Where(customer => customer.Orders.Select(order => order.Total).Sum() > sumBarrier);

            foreach (var customer in customers)
            {
                ObjectDumper.Write(customer);
            }
        }

        [Category("Linq")]
        [Title("Task 2")]
        [Description("This sample return customers with list of suppliers that lives in same country and city with customer")]
        public void Linq2()
        {
            var customersWithSuppliers = dataSource.Customers.Select(customer => new
            {
                customer = customer,
                suppliers = dataSource.Suppliers.Where(supplier =>
                                                       supplier.Country.Equals(customer.Country) &&
                                                       supplier.City.Equals(customer.City))
            });
            foreach (var customerWithSuppliers in customersWithSuppliers)
            {
                ObjectDumper.Write(customerWithSuppliers.customer);
                foreach (var supplier in customerWithSuppliers.suppliers)
                {
                    ObjectDumper.Write(supplier);
                }
            }
        }
        [Category("Linq")]
        [Title("Task 3")]
        [Description("This sample return customers that has order with price more that determined price")]
        public void Linq3()
        {
            int price = 850;
            var customers = dataSource.Customers.Where(customer => customer.Orders.Any(order => order.Total > price));
            foreach (var customer in customers)
            {
                ObjectDumper.Write(customer);
            }
        }

        [Category("Linq")]
        [Title("Task 4")]
        [Description("This sample return customers with they first purchase date")]
        public void Linq4()
        {
            var customersWithStartDate = dataSource.Customers.Where(customer => customer.Orders.Count() > 0)
                                                             .Select(customer => new
                                                             {
                                                                 customer = customer,
                                                                 startDate = customer.Orders.Min(order => order.OrderDate)
                                                             });

            foreach (var customerWithStartDate in customersWithStartDate)
            {
                ObjectDumper.Write(customerWithStartDate.customer);
                ObjectDumper.Write(customerWithStartDate.startDate);
            }
        }

        [Category("Linq")]
        [Title("Task 5")]
        [Description("This sample return customers with they first purchase date and sorted by year, month, customer purchase and name ")]
        public void Linq5()
        {
            var customersWithStartDate = dataSource.Customers
                                                    .Where(customer => customer.Orders.Count() > 0)
                                                    .Select(customer => new
                                                    {
                                                        customer = customer,
                                                        startDate = customer.Orders.Min(order => order.OrderDate)
                                                    })
                                                     .OrderBy(customer => customer.startDate.Year)
                                                     .ThenBy(customer => customer.startDate.Month)
                                                     //.ThenByDescending(customer => customer.customer.Orders)
                                                     .ThenBy(customer => customer.customer.CompanyName);
            foreach (var customerWithStartDate in customersWithStartDate)
            {
                ObjectDumper.Write(customerWithStartDate.customer);
                ObjectDumper.Write(customerWithStartDate.startDate);
            }
        }


        [Category("Linq")]
        [Title("Task 6")]
        [Description("This sample return customers with wrong postal code or not filled region or incorrect phone number")]
        public void Linq6()
        {
            var customers = dataSource.Customers.Where(customer =>
                                             string.IsNullOrWhiteSpace(customer.PostalCode) || !customer.PostalCode.All(code => char.IsNumber(code)) ||
                                             string.IsNullOrWhiteSpace(customer.Region) ||
                                             string.IsNullOrEmpty(customer.Phone) || (customer.Phone.IndexOf("(") < 0 && customer.Phone.IndexOf(")") < 0));
            foreach (var customer in customers)
            {
                ObjectDumper.Write(customer);
            }
        }

        [Category("Linq")]
        [Title("Task 7")]
        [Description("This sample group products by categories, then by existence on stock, then by price ")]
        public void Linq7()
        {
            var groupedByCategoryProducts = dataSource.Products.GroupBy(product => product.Category);
            //var groupedByExistenceInStock = groupedByCategoryProducts.Select(gcProduct => gcProduct.GroupBy(product => product.UnitsInStock > 0));

            foreach (var group in groupedByCategoryProducts)
            {
                ObjectDumper.Write(group.Key);
                foreach (var product in group)
                {
                    ObjectDumper.Write(product);
                }

            }
        }

        [Category("Linq")]
        [Title("Task 8")]
        [Description("This sample group all products by 3 category: cheap, medium and expensive price")]
        public void Linq8()
        {
            var groupedByPrice = dataSource.Products.GroupBy(product => product.UnitPrice);


            foreach (var groupedProducts in groupedByPrice)
            {
                ObjectDumper.Write(groupedProducts.Key);
                foreach (var product in groupedProducts)
                {
                    ObjectDumper.Write(product);
                }

            }
        }

        [Category("Linq")]
        [Title("Task 9")]
        [Description("This sample calculate average income and purchase intensity for each city")]
        public void Linq9()
        {
            var groupedCustomers = dataSource.Customers.GroupBy(customer => customer.City);
            var citiesOrdersStats = groupedCustomers.Select(group => new
            {
                city = group.Key,
                averageIncome = group.Average(customer => customer.Orders.Count() > 0 ? customer.Orders.Select(order => order.Total).Average() : 0),
                averagePurchaseIntensity = group.Average(customer => customer.Orders.Count())
            });

            foreach (var cityStats in citiesOrdersStats)
            {
                ObjectDumper.Write(cityStats);
            }
        }

        [Category("Linq")]
        [Title("Task 10")]
        [Description("This sample calculate client activity only by monthes, only by years and by month and year")]
        public void Linq10()
        {
            var orders = dataSource.Customers.SelectMany(customer => customer.Orders);
            var monthActivityStats = orders.GroupBy(order => order.OrderDate.Month)
                                           .Select(gOrder => new
                                           {
                                               month = gOrder.Key,
                                               clientActivity = gOrder.Count()
                                           });

            var yearActivityStats = orders.GroupBy(order => order.OrderDate.Year)
                                          .Select(gOrder => new
                                          {
                                              year = gOrder.Key,
                                              clientActivity = gOrder.Count()
                                          });

            //var monthYearActivityStats = orders.Distinct(order)

            foreach (var activity in monthActivityStats)
                ObjectDumper.Write(activity);

            foreach (var activity in yearActivityStats)
                ObjectDumper.Write(activity);
        }

    }
}
