

using System.Collections.Generic;

namespace ORMSample.Domain { 

	public class Territory {

		public string TerritoryID { get; set; }

		public string TerritoryDescription { get; set; }

        public int RegionID { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}