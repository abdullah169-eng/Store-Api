using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Specifications.Orders
{
    public class OrderSpecifications:BaseSpecifications<Order,int>
    {
        public OrderSpecifications(string buyerEmail)
            :base(O=>O.BuyerEmail==buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);
        }
        public OrderSpecifications(string buyerEmail, int orderId)
            :base(O=>O.BuyerEmail==buyerEmail && O.Id==orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);
        }
    }
}
