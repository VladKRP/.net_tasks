

namespace ORMSample.Domain { 

	public class EmployeeTerritory {

		public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public string TerritoryID { get; set; }
        public Territory Territory { get; set; }
    }
}