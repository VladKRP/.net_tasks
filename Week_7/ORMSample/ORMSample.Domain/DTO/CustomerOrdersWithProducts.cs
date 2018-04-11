using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Domain.DTO
{
    public class CustomerOrdersWithProducts
    {

        public string CustomerName { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public IEnumerable<string> ProductsNames { get; set; }
    }
}
