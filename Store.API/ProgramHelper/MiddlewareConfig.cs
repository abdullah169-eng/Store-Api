using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.API.Middlewares;
using Store.Core.Entities.Identity;
using Store.Repository.Data;
using Store.Repository.Data.Contexts;
using Store.Repository.Identity;
using Store.Repository.Identity.Contexts;

namespace Store.API.ProgramHelper
{
    public static class MiddlewareConfig
    {
        public static async Task<WebApplication> AddMiddlewaresAsync(this WebApplication app)
        {
            await app.DbMigrateAsync();
            app.ErrorHandling();
            app.GlobalConfig();
            return app;
        }
        private static async Task<WebApplication> DbMigrateAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var Context = Services.GetService<StoreDbContext>();
            var IdentityContext = Services.GetService<StoreIdentityDbContext>();
            var userManager = Services.GetService<UserManager<AppUser>>();
            var LoggerFactory = Services.GetService<ILoggerFactory>();
            try
            {
                await Context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(Context);
                await IdentityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "There are Problems during Apply Migrations");
            }
            return app;
        }
        private static WebApplication ErrorHandling(this WebApplication app)
        {
            // ***(MyComment)*** Configure Middleware Pipeline
            app.UseMiddleware<ExceptionMiddleware>();
            // ***(MyComment)*** Configure No Endpoint Found 
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            return app;
        }
        private static WebApplication GlobalConfig(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.MapGet("/", () => Results.Redirect("/swagger"));
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            return app;
        }
    }
}