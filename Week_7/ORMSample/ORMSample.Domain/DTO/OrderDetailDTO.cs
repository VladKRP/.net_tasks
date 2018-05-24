using System;
using System.Net;
using System.Net.Http;


namespace ORMSample.Domain.DTO
{
    public class OrderDetailDTO {

        public int OrderID { get; set; }

        public string ProductName { get; set; }

        public decimal? UnitPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public string ShipAddress { get; set; }

    }
}