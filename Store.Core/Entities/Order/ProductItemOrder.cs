using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Entities.Order
{
    public class ProductItemOrder
    {
        public ProductItemOrder() { }
        public ProductItemOrder(int productId, string productName, string productImageUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ProductImageUrl = productImageUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
