

namespace ORMSample.Domain { 

	public class OrderDetail {

		public int OrderID { get; set; }
        public Order Order { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

		public int Quantity { get; set; }

		public float Discount { get; set; }

	}

}