namespace Task.DB
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Table("Order Details")]
    public partial class Order_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }


    class OrderDetailSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var orderDetail = (Order_Detail)obj;
            orderDetail = ResolveObjectCircularLoop(orderDetail);
            info.AddValue("OrderID", orderDetail.OrderID);
            info.AddValue("ProductID", orderDetail.ProductID);
            info.AddValue("UnitPrice", orderDetail.UnitPrice);
            info.AddValue("Quantity", orderDetail.Quantity);
            info.AddValue("Discount", orderDetail.Discount);
            info.AddValue("Order", orderDetail.Order);
            info.AddValue("Product", orderDetail.Product);

            Order_Detail ResolveObjectCircularLoop(Order_Detail orderdetail)
            {
                if (orderdetail.Order != null)
                {
                    orderdetail.Order.Customer = null;
                    orderdetail.Order.Employee = null;
                    orderdetail.Order.Shipper = null;
                    orderdetail.Order.Order_Details = new List<Order_Detail>();
                }

                if (orderdetail.Product != null)
                {
                    orderdetail.Product.Category = null;
                    orderdetail.Product.Supplier = null;
                    orderdetail.Product.Order_Details = new List<Order_Detail>();
                }
                return orderdetail;
            }

        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var orderDetail = (Order_Detail)obj;
            orderDetail.OrderID = info.GetInt32("OrderID");
            orderDetail.ProductID = info.GetInt32("ProductID");
            orderDetail.UnitPrice = info.GetDecimal("UnitPrice");
            orderDetail.Discount = info.GetSingle("Discount");
            orderDetail.Order = (Order)info.GetValue("Order", typeof(Order));
            orderDetail.Product = (Product)info.GetValue("Product", typeof(Product));
            return orderDetail;
        }
    }
}
