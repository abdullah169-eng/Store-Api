using System.Text.Json;

namespace Store.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var response = _environment.IsDevelopment() ? 
                    new Errors.ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) 
                    : new Errors.ApiErrorResponse(context.Response.StatusCode);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response,options));
            }
        }
    }
}
