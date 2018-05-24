using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Domain
{
    public class EmployeeSupplier
    {
        public Employee Employee { get; set; }

        public Supplier Supplier { get; set; }
    }


    public class EmployeeSuppliers
    {
        public Employee Employee { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
    }

}
