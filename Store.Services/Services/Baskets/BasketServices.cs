using AutoMapper;
using Store.Core.DTOs.Baskets;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using static StackExchange.Redis.Role;

namespace Store.Services.Services.Baskets
{
    public class BasketServices:IBasketServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketServices(IBasketRepository basketRepository, IMapper mapper) {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto?> GetBasketAsync(string id)
        {
            return _mapper.Map<CustomerBasketDto>(await _basketRepository.GetBasketAsync(id));
        }
        public async Task<CustomerBasketDto?> CreateOrUpdateBasketAsync(CustomerBasketDto customerBasket)
        {
            var basket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(customerBasket));
            if (basket is null) return null;
            return _mapper.Map<CustomerBasketDto>(basket);
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
