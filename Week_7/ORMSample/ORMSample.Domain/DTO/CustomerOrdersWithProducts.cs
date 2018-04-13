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

        public IEnumerable<OrderDetailDTO> OrderDetails { get; set; }
    }
}
