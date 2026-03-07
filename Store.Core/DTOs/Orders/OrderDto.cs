using Store.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.DTOs.Orders
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}
