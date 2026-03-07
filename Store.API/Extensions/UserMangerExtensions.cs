using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.API.Errors;
using Store.Core.Entities.Identity;
using System.Security.Claims;

namespace Store.API.Extensions
{
    public static class UserMangerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return null;
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return null;
            return user;
        }
    }
}
