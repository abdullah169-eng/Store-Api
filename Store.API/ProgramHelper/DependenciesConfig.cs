using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.Core;
using Store.Core.Entities.Identity;
using Store.Core.Mapping.Auth;
using Store.Core.Mapping.Baskets;
using Store.Core.Mapping.Orders;
using Store.Core.Mapping.Products;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Repository;
using Store.Repository.Data.Contexts;
using Store.Repository.Identity.Contexts;
using Store.Repository.Repositories;
using Store.Services.Orders;
using Store.Services.Services.Baskets;
using Store.Services.Services.Caches;
using Store.Services.Services.Payments;
using Store.Services.Services.Products;
using Store.Services.Services.Tokens;
using Store.Services.Services.Users;
using System.Text;

namespace Store.API.ProgramHelper
{
    public static class DependenciesConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();  // Already Exist
            services.AddSwagger();
            services.AddDbContext(configuration);
            services.AddUserDefinedServices();
            services.AddAutoMapper(configuration);
            services.AddValidation();
            services.AddRedisServices(configuration);
            services.AddIdentityServices();
            services.AddAuthenticationServices(configuration);
            return services;
        }
        #region Swagger
        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        #endregion
        #region DbContext
        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultIdentityConnection"));
            });
            return services;
        }
        #endregion
        #region UserDefines
        private static IServiceCollection AddUserDefinedServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketServices, BasketServices>();
            services.AddScoped<ICacheServices, CacheServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IPaymentServices, PaymentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        #endregion
        #region Mapper
        private static IServiceCollection AddAutoMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
            return services;
        }
        #endregion
        #region Validation
        private static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new
                        {
                            Name = e.Key,
                            Message = e.Value.Errors.First().ErrorMessage
                        }).ToArray();
                    return new BadRequestObjectResult(new
                    {
                        Message = "Validation Errors",
                        Errors = errors
                    });
                };
            });
            return services;
        }
        #endregion
        #region Redis
        private static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                sp =>ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
            return services;
        }
        #endregion
        #region IdentityServices
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser,IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }
        #endregion
        #region AuthenticationServices
        private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                }
            );
            return services;
        }
        #endregion
    }
}
