using Aiglusoft.IAM.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ErrorMappingService _errorMappingService;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ErrorMappingService errorMappingService, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _errorMappingService = errorMappingService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
          
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, error, errorDescription) = _errorMappingService.Map(exception);

            _logger.LogError("Error details - StatusCode: {StatusCode}, Error: {Error}, Description: {Description}", statusCode, error, errorDescription);

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
