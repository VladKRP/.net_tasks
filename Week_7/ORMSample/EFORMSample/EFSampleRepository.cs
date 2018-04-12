using ORMSample.Domain;
using ORMSample.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EFORMSample
{
    public class EFSampleRepository
    {
        private readonly NorthwindContext _context;

        public EFSampleRepository()
        {
            _context = new NorthwindContext();
        }

        public EFSampleRepository(NorthwindContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Task 1
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<CustomerOrdersWithProducts> GetOrdersByCategory(Category category)//check execution required
        {
            IEnumerable<CustomerOrdersWithProducts> customerOrders = new List<CustomerOrdersWithProducts>();
            if (category != null)
            {
                customerOrders = _context.Orders.Include("Customer, OrderDetail")
                                       .Where(x => x.Customer != null && x.OrderDetail != null)
                                       .Join(_context.Products.Include("Category").Where(x => x.Category != null),
                                              order => order.OrderDetail.ProductID,
                                              product => product.ProductID,
                                              (order, product) => new
                                              {
                                                  Order = order,
                                                  Product = product
                                              })
                                        .Where(x => x.Product.Category.CategoryName == category.CategoryName)
                                        .GroupBy(x => x.Order.Customer.ContactName)
                                        .Select(cproducts => new CustomerOrdersWithProducts()
                                        {
                                            CustomerName = cproducts.Key,
                                            ProductsNames = cproducts.Select(ordProd => ordProd.Product.ProductName),
                                            Orders = cproducts.Select(ordProd => ordProd.Order)
                                        });
            }
            return customerOrders;
        }  
    }

}

