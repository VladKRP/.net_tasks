using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFORMSample
{
    public class NorthwindContext: DbContext
    {

        public NorthwindContext():base("NorthwindEFDB") {}

        public NorthwindContext(string connectionString):base(connectionString){ }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Customer> Customers { get; set; }

        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<OrderDetail> OrderDetails { get; set; }

        public IDbSet<Shipper> Shippers { get; set; }

        public IDbSet<Territory> Territories { get; set; }

        public IDbSet<Supplier> Suppliers { get; set; }

        public IDbSet<Region> Regions { get; set; }

        //v1.1
        public IDbSet<EmployeeCreditCard> EmployeeCredirCards { get; set; }
        //

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasMany(x => x.Territories).WithMany(x => x.Employees);
            modelBuilder.Entity<OrderDetail>().HasKey(x => x.OrderID).HasRequired(x => x.Order);
            modelBuilder.Entity<Order>().HasOptional(x => x.OrderDetail);

            //v1.1
            modelBuilder.Entity<EmployeeCreditCard>().HasKey(x => x.CardNumber).HasRequired(x => x.Employee);
            //

            //v1.2
            modelBuilder.Entity<Region>().ToTable("Region");
            //

            base.OnModelCreating(modelBuilder);
        }
    }
}
