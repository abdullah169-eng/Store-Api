using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.Core;
using Store.Core.DTOs.Orders;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using System.Security.Claims;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderServices orderServices, IMapper mapper,IUnitOfWork unitOfWork)
        {
            _orderServices = orderServices;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var address = _mapper.Map<Address>(model.ShipToAddress);
            var order = await _orderServices.CreateOrderAsync(email, model.BasketId, model.DeliveryMethodId, address);
            if (order == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var orders = await _orderServices.GetOrdersForSpecificUserAsync(email);
            return Ok(_mapper.Map<List<OrderToReturnDto>>(orders));
        }
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetSpecificOrderAsync(int orderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var order =  await _orderServices.GetOrderByIdForSpecificUserAsync(email,orderId);
            if (order == null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }
        [HttpGet("DeliveyMethods")]
        public async Task<IActionResult> GetDeliveyMethodsAsync(int orderId) 
        {
            var deliveyMethods = await _unitOfWork.Repository<DeliveryMethod,int>().GetAllAsync();
            if (deliveyMethods == null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(deliveyMethods);
        }
    }
}
