using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Domain
{
    public class EmployeeSupplier
    {
        public int EmployeeID { get; set; }

        public int SupplierID { get; set; }
    }


    public class EmployeeSuppliers
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public IEnumerable<int> SuppliersID { get; set; }
    }

}
