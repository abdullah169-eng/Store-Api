using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.API.Errors;
using Store.API.Extensions;
using Store.Core.DTOs.Auth;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using Store.Services.Services.Tokens;
using System.Security.Claims;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(
            IUserServices userServices,
            UserManager<AppUser> userManager,
            ITokenServices tokenServices,
            IMapper mapper
            )
        {
            _userServices = userServices;
            _userManager = userManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginAsync(LoginDto login)
        {
            var user = await _userServices.LoginAsync(login);
            if (user == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterAsync(RegisterDto register)
        {
            var user = await _userServices.RegisterAsync(register);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid Registertion"));
            return Ok(user);
        }
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail= User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "User Not Found"));
            return Ok(new UserDto
            {
                Email = userEmail,
                DisplayName = user.DisplayName,
                Token = await _tokenServices.CreateTokenAsync(user,_userManager)
            });
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "User Not Found"));
            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
        [Authorize]
        [HttpPost("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "User Not Found"));
            user.Address = _mapper.Map<Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Problem Updating The User"));
            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
    }
}
