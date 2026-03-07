using Microsoft.AspNetCore.Identity;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            // Users
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "1234567890",
                    Address = new Address
                    {
                        FName = "Admin",
                        LName = "User",
                        Street = "123 Main St",
                        City = "Cairo",
                        Country = "Egypt"
                    }
                };
                await _userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}
