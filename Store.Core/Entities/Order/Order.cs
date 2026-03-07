using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Entities.Order
{
    public class Order:BaseEntity<int>
    {
        public Order() { }
        public Order(string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now;
        public OrderStatus status { get; set; }= OrderStatus.Pending;
        public Address ShipToAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GetTotal()=> Subtotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }
    }
}
