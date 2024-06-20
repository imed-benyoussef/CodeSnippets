using Aiglusoft.IAM.Infrastructure.Services;
using Microsoft.AspNetCore.Http;


namespace Aiglusoft.IAM.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ErrorMappingService _errorMappingService;

        public ErrorHandlingMiddleware(RequestDelegate next, ErrorMappingService errorMappingService)
        {
            _next = next;
            _errorMappingService = errorMappingService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, error, errorDescription) = _errorMappingService.Map(exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = new
            {
                error,
                error_description = errorDescription
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
