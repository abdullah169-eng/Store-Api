using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.DTOs.Baskets;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Mapping.Baskets
{
    public class BasketProfile : Profile
    {
        public BasketProfile() 
        {
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}
