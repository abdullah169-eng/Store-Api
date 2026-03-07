using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Specifications.Orders
{
    public class OrderSpecificationsWithPaymentIntentId:BaseSpecifications<Order,int>
    {
        public OrderSpecificationsWithPaymentIntentId(string paymentIntentId) 
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(o => o.OrderItems);
            Includes.Add(o => o.DeliveryMethod);
        }
    }
}
