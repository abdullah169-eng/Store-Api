using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.DTOs.Baskets;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketServices _basketServices;

        public BasketController(IBasketServices basketServices)
        {
            _basketServices = basketServices;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBasket(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ApiErrorResponse(400, "Basket id is required"));
            var basket = await _basketServices.GetBasketAsync(id);
            if (basket == null)
                return NotFound(new ApiErrorResponse(404, $"Basket with id '{id}' was not found"));
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateBasket([FromBody] CustomerBasketDto customerBasket)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiErrorResponse(400, "Basket data is required"));
            var basket = await _basketServices.CreateOrUpdateBasketAsync(customerBasket);
            if (basket is null) return BadRequest(new ApiErrorResponse(400, "Problem saving basket!!"));
            return Ok(basket);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ApiErrorResponse(400, "Basket id is required")); var deleted = await _basketServices.DeleteBasketAsync(id);
            if (!deleted) 
                return NotFound(new ApiErrorResponse(404, $"Basket with id '{id}' was not found"));
            return Ok(new
            {
                message = "Basket deleted successfully",
                basketId = id
            });
        }
    }
}
