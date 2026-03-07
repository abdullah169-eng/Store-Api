using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Specifications.Products
{
    public class ProductSpecParams
    {
        private string? search;
        public string? Search { 
            set { search = value.ToLower(); } 
            get { return search; } }
        public string? sort { set; get; }
        public int? typeId { set; get; }
        public int? brandId { set; get; }
        public int pageSize { set; get; }=10;
        public int pageIndex { set; get; } = 1;
    }
}
