using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Domain
{
    public class EmployeeRegion
    {
        public int EmployeeID { get; set; }

        public Employee Employee { get; set; }

        public Region Region { get; set; }

        //public int RegionID { get; set; }

        //public string RegionDescription { get; set; }

    }
    //public class EmployeeRegion
    //{
    //    public Employee Employee { get; set; }

    //    public Region Region { get; set; }

    //}
}
