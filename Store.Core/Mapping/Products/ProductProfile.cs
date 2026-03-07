using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.DTOs.Products;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Mapping.Products
{
    public class ProductProfile:Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(D=>D.BrandName,options=>options.MapFrom(S=>S.Brand.Name))
                .ForMember(D=>D.TypeName,options=>options.MapFrom(S=>S.Type.Name))
                .ForMember(D => D.PictureUrl, options => options.MapFrom(S => $"{configuration["BaseUrl"]}{S.PictureUrl}"));
            CreateMap<ProductBrand, BrandTypeDto>();
            CreateMap<ProductType, BrandTypeDto>();
        }
    }
}
