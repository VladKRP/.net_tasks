

namespace ORMSample.Domain { 

	public class Order {

		public System.Int32 OrderID { get; set; }

		public System.String CustomerID { get; set; }

		public System.Int32? EmployeeID { get; set; }

		public System.DateTime? OrderDate { get; set; }

		public System.DateTime? RequiredDate { get; set; }

		public System.DateTime? ShippedDate { get; set; }

		public System.Int32? ShipVia { get; set; }

		public System.Decimal? Freight { get; set; }

		public System.String ShipName { get; set; }

		public System.String ShipAddress { get; set; }

		public System.String ShipCity { get; set; }

		public System.String ShipRegion { get; set; }

		public System.String ShipPostalCode { get; set; }

		public System.String ShipCountry { get; set; }

	}

}