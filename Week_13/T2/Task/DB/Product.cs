using Task.DB;

namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Serializable]
    public partial class Product: ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        private Product(SerializationInfo info, StreamingContext context)
        {
            ProductID = info.GetInt32("ProductID");
            ProductName = info.GetString("ProductName");
            SupplierID = info.GetInt32("SupplierID");
            CategoryID = info.GetInt32("CategoryID");
            QuantityPerUnit = info.GetString("QuantityPerUnit");
            UnitPrice = info.GetInt32("UnitPrice");
            UnitsInStock = info.GetInt16("UnitsInStock");
            UnitsOnOrder = info.GetInt16("UnitsOnOrder");
            ReorderLevel = info.GetInt16("ReorderLevel");
            Discontinued = info.GetBoolean("Discontinued");
            Category = LoadCategoryWithoutProducts();
            Order_Details = LoadOrderDetailsWithoutReferences();
            Supplier = LoadSupplierWithoutProducts();

            Category LoadCategoryWithoutProducts(){
                var category = info.GetValue("Category", typeof(Category)) as Category;
                if (category != null && category.Products.Count > 0)
                {
                    category.Products = new List<Product>();
                }
                return category;
            }

            ICollection<Order_Detail> LoadOrderDetailsWithoutReferences()
            {
                var order_details = info.GetValue("Order_Details", typeof(ICollection<Order_Detail>)) as ICollection<Order_Detail>;
                if(order_details != null && order_details.Count > 0)
                {
                    foreach(var orderDetail in order_details)
                    {
                        orderDetail.Order = null;
                        orderDetail.Product = null;
                    }
                }
                return order_details;
            }

            Supplier LoadSupplierWithoutProducts()
            {
                var supplier = info.GetValue("Supplier", typeof(Supplier)) as Supplier;
                if(supplier != null && supplier.Products.Count > 0)
                    supplier.Products = new List<Product>();
                return supplier;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ProductID", ProductID);
            info.AddValue("ProductName", ProductName);
            info.AddValue("SupplierID", SupplierID);
            info.AddValue("CategoryID", CategoryID);
            info.AddValue("QuantityPerUnit", QuantityPerUnit);
            info.AddValue("UnitPrice", UnitPrice);
            info.AddValue("UnitsInStock", UnitsInStock);
            info.AddValue("UnitsOnOrder", UnitsOnOrder);
            info.AddValue("ReorderLevel", ReorderLevel);
            info.AddValue("Discontinued", Discontinued);
            info.AddValue("Category", Category);
            info.AddValue("Order_Details", Order_Details);
            info.AddValue("Supplier", Supplier);
        }
    }
}