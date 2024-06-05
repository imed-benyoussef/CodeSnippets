namespace Aiglusoft.IAM.Server.Middleware
{
    using Aiglusoft.IAM.Infrastructure.Services;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionMapping _exceptionMapping;

        public ErrorMiddleware(RequestDelegate next, ExceptionMapping exceptionMapping)
        {
            _next = next;
            _exceptionMapping = exceptionMapping;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (_exceptionMapping.TryGetExceptionDetails(exception.GetType(), out var errorInfo))
            {
                context.Response.StatusCode = errorInfo.StatusCode;
                return context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Message = errorInfo.Message
                }.ToString());
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred."
                }.ToString());
            }
        }

        private class ErrorDetails
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }

            public override string ToString()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
        }
    }


}
