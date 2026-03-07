using Store.Core.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Services.Contract
{
    public interface IBasketServices
    {
        Task<CustomerBasketDto?> GetBasketAsync(string basketId);
        Task<CustomerBasketDto?> CreateOrUpdateBasketAsync(CustomerBasketDto customerBasket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
