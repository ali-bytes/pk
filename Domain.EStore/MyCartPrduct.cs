using System;
using System.Collections.Generic;

namespace Domain.EStore
{
    public class MyCartPrduct
    {
       
        public int CustomerId { get; set; }
        public int DeliveryId { get; set; }
        
        public int CartId { get; set; }

        public string Time { get; set; }

        public bool Status { get; set; }
        public bool DeliveryState { get; set; }
        public decimal Total { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryName { get; set; }
        public string GetWayName { get; set; }
        public int GetwayId { get; set; }

        public List<MyCartDetails> MyCartDetailse { get; set; }
    }

    public class MyCartDetails
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        public double Quntaty { get; set; }
        public double AvilabelQuntaty { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}