using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Identity.Contexts
{
    public class StoreIdentityDbContext:IdentityDbContext<AppUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
        {
        }
    }
}
