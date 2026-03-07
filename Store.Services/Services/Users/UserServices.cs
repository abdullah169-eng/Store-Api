using Microsoft.AspNetCore.Identity;
using Store.Core.DTOs.Auth;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Services.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;
        private readonly ITokenServices _tokenServices;
        public UserServices(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger, ITokenServices tokenServices)
        {
            _userManager = userManager;
            _signInManger = signInManger;
            _tokenServices = tokenServices;
        }
        public async Task<UserDto> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) return null;
            var result = await _signInManger.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded) return null;
            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };
        }
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        public async Task<UserDto> RegisterAsync(RegisterDto register)
        {
            if (await CheckEmailExistAsync(register.Email)) return null;
            var user = new AppUser()
            {
                DisplayName = register.DisplayName,
                Email = register.Email,
                UserName = register.Email.Split("@")[0],
                PhoneNumber = register.Phone
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded) return null;
            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };
        }
    }
}
