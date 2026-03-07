using Store.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Services.Contract
{
    public interface IUserServices
    {
        Task<UserDto> LoginAsync(LoginDto login);
        Task<UserDto> RegisterAsync(RegisterDto register);
        Task<bool> CheckEmailExistAsync(string email);
    }
}
