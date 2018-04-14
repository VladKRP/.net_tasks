

namespace ORMSample.Domain { 

	public class Product {

		public System.Int32 ProductID { get; set; }

		public System.String ProductName { get; set; }

		public System.Int32? SupplierID { get; set; }

		public System.Int32? CategoryID { get; set; }

		public System.String QuantityPerUnit { get; set; }

		public System.Decimal? UnitPrice { get; set; }

		public System.Int32? UnitsInStock { get; set; }

		public System.Int32? UnitsOnOrder { get; set; }

		public System.Int32? ReorderLevel { get; set; }

		public System.Boolean Discontinued { get; set; }

	}

}