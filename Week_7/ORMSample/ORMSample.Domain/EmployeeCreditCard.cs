using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Domain
{
    public class EmployeeCreditCard
    {
        public int CardNumber { get; set; }

        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public string CardHolder { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
