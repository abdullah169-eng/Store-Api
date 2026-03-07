using Microsoft.Extensions.Configuration;
using Store.Core;
using Store.Core.DTOs.Baskets;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using Product=Store.Core.Entities.Product;

namespace Store.Services.Services.Payments
{
    public class PaymentService : IPaymentServices
    {
        private readonly IBasketServices _basketServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketServices basketServices,IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _basketServices = basketServices;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var basket = await _basketServices.GetBasketAsync(basketId);
            if (basket == null) return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod,int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }
            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product= await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
            var subTotal= basket.Items.Sum(i => i.Quantity * i.Price);
            var service= new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                paymentIntent=await service.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = (long) (subTotal*100+shippingPrice*100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                });
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else 
            {
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, new PaymentIntentUpdateOptions
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100)
                });
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            basket=await _basketServices.CreateOrUpdateBasketAsync(basket);
            if (basket == null) return null;
            return basket;
        }
    }
}
