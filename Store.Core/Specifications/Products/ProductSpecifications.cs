using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Specifications.Products
{
    public class ProductSpecifications: BaseSpecifications<Product,int>
    {
        public ProductSpecifications(int Id):base(P=>P.Id==Id)
        {
            AddIncludes();
        }
        public ProductSpecifications(ProductSpecParams productSpec, bool forCount = false) :base(P=>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
            (!productSpec.typeId.HasValue || P.TypeId== productSpec.typeId.Value) &&
            (!productSpec.brandId.HasValue || P.BrandId== productSpec.brandId.Value)
            )
        {
            if (!forCount)
            {
                if (!string.IsNullOrEmpty(productSpec.sort))
                {
                    switch (productSpec.sort)
                    {
                        case "priceAsc":
                            AddOrderBy(P => P.Price);
                            break;
                        case "priceDesc":
                            AddOrderByDescending(P => P.Price);
                            break;
                        default:
                            AddOrderBy(P => P.Name);
                            break;
                    }
                }
                else
                {
                    AddOrderBy(P => P.Name);
                }
                AddIncludes();
                ApplyPagination(productSpec.pageSize, productSpec.pageSize * (productSpec.pageIndex - 1));
            }
        }
        private void AddIncludes()
        {
            Includes.Add(P => P.Type);
            Includes.Add(P => P.Brand);
        }
    }
}
