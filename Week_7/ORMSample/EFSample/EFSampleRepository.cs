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
    public class EFSampleRepository: IDisposable
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

        

        /// Task 1 Query
        public IEnumerable<CustomerOrdersWithProducts> GetOrdersByCategory(Category category)
        {
            IEnumerable<CustomerOrdersWithProducts> customerOrders = new List<CustomerOrdersWithProducts>();
            if (category != null)
            {
                customerOrders = _context.Orders.Include(x => x.Customer)
                                                .Include(x => x.OrderDetail)
                                                .Where(x => x.Customer != null && x.OrderDetail != null)
                                                .Join(_context.Products.Include(x => x.Category).Where(x => x.Category != null),
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
                                                     OrderDetails = cproducts.Select(prod => new OrderDetailDTO()
                                                     {
                                                         OrderID = prod.Order.OrderID,
                                                         ProductName = prod.Product.ProductName,
                                                         OrderDate = prod.Order.OrderDate,
                                                         UnitPrice = prod.Product.UnitPrice,
                                                         ShipAddress = prod.Order.ShipAddress
                                                     })
                                                 });
            }
            return customerOrders;
        }

        private bool isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(_context);
        }
    }

}

