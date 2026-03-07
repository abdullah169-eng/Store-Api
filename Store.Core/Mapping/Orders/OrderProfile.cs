using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.DTOs.Auth;
using Store.Core.DTOs.Orders;
using Store.Core.Entities.Order;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Mapping.Orders
{
    public class OrderProfile:Profile
    {
        public OrderProfile(IConfiguration configuration)
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryMethodCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost))
                .ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => $"{configuration["BaseUrl"]}{src.Product.ProductImageUrl}"))
                .ReverseMap();
        }
    }
}
