using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.API.Middlewares;
using Store.API.ProgramHelper;
using Store.Core;
using Store.Core.Mapping.Products;
using Store.Core.Services.Contract;
using Store.Repository;
using Store.Repository.Data;
using Store.Repository.Data.Contexts;
using Store.Repository.Repositories;
using Store.Services.Services.Products;
using System.Reflection;
using System.Threading.Tasks;

namespace Store.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDependencies(builder.Configuration);   

            var app = builder.Build();

            await app.AddMiddlewaresAsync();

            app.Run();
        }
    }
}
