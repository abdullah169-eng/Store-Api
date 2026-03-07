using Store.Core.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Services.Contract
{
    public interface IPaymentServices
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);
    }
}
