

namespace ORMSample.Domain { 

	public class Employee {

		public System.Int32 EmployeeID { get; set; }

		public System.String LastName { get; set; }

		public System.String FirstName { get; set; }

		public System.String Title { get; set; }

		public System.String TitleOfCourtesy { get; set; }

		public System.DateTime? BirthDate { get; set; }

		public System.DateTime? HireDate { get; set; }

		public System.String Address { get; set; }

		public System.String City { get; set; }

		public System.String Region { get; set; }

		public System.String PostalCode { get; set; }

		public System.String Country { get; set; }

		public System.String HomePhone { get; set; }

		public System.String Extension { get; set; }

		public System.Byte[] Photo { get; set; }

		public System.String Notes { get; set; }

		public System.Int32? ReportsTo { get; set; }

		public System.String PhotoPath { get; set; }

	}

}