using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Services.Contract
{
    public interface ITokenServices
    {
        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);
    }
}
