using AutoMapper;
using Store.Core;
using Store.Core.DTOs.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specifications;
using Store.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Services.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec= new ProductSpecifications(productSpec);
            var mappedProduct= _mapper.Map<IEnumerable<ProductDto>>(await _unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec));
            var specCount = new ProductSpecifications(productSpec,true);
            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync (specCount);
            return new PaginationResponse<ProductDto>(productSpec.pageIndex, productSpec.pageSize, count, mappedProduct);
        }
        public async Task<ProductDto> GetProductByIdAsync(int Id)
        {
            var spec = new ProductSpecifications(Id);
            return _mapper.Map<ProductDto>(await _unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec));
        }
        public async Task<IEnumerable<BrandTypeDto>> GetAllBrandsAsync()
            => _mapper.Map<IEnumerable<BrandTypeDto>>(await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
        public async Task<IEnumerable<BrandTypeDto>> GetAllTypesAsync()
            => _mapper.Map<IEnumerable<BrandTypeDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
    }
}