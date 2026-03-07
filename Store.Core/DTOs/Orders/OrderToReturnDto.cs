using Store.Core.DTOs.Auth;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.DTOs.Orders
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public string status { get; set; }
        public AddressDto ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string? PaymentIntentId { get; set; }= string.Empty;
    }
}
