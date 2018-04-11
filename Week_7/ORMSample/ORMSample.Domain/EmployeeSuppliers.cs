using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Domain
{
    public class EmployeeSuppliers
    {
        public Employee Employee { get; set; }

        public Supplier Suppliers { get; set; }
    }
}
