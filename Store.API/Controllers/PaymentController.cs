using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core.Services.Contract;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntentAsync(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var basket =await _paymentServices.CreateOrUpdatePaymentIntentIdAsync(basketId);
            if (basket == null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }
    }
}
