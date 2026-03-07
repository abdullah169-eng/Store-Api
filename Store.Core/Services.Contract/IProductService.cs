using Store.Core.DTOs.Products;
using Store.Core.Helper;
using Store.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Services.Contract
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec);
        Task<ProductDto> GetProductByIdAsync(int Id);
        Task<IEnumerable<BrandTypeDto>> GetAllBrandsAsync();
        Task<IEnumerable<BrandTypeDto>> GetAllTypesAsync();
    }
}
