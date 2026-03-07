using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Entities
{
    public class ProductBrand:BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
