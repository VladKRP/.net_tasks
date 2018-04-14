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

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Shipper> Shippers { get; set; }

        public DbSet<Territory> Territories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Region> Regions { get; set; }

        //v1.1
        public DbSet<EmployeeCreditCard> EmployeeCredirCards { get; set; }
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
