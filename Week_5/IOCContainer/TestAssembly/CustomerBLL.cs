using IOCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssembly
{
    [ImportConstructor]
    public class CustomerBLL
    {
        private readonly ICustomerDAL _customerDAL;
        private readonly Logger _logger;

        public CustomerBLL(ICustomerDAL dal, Logger logger)
        {
            _customerDAL = dal;
            _logger = logger;
        }
    }

    public class CustomerBLL2
    {
        [Import]
        public ICustomerDAL CustomerDAL { get; set; }
        [Import]
        public Logger Logger { get; set; }
    }
}
