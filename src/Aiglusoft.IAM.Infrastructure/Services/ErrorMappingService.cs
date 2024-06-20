using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Net;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class ErrorMappingService
    {
        private readonly IDictionary<Type, HttpStatusCode> _exceptionStatusCodeMapping = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(InvalidRequestException), HttpStatusCode.BadRequest },
            { typeof(InvalidClientException), HttpStatusCode.Unauthorized },
            { typeof(InvalidRedirectUriException), HttpStatusCode.BadRequest },
            { typeof(UnsupportedResponseTypeException), HttpStatusCode.BadRequest },
            // Add other mappings as necessary
        };

        public (HttpStatusCode statusCode, string error, string errorDescription) Map(Exception exception)
        {
            if (exception is AppException appException)
            {
                var errorCode = string.IsNullOrEmpty(appException.Code)
                    ? GenerateCodeFromClassName(appException.GetType().Name)
                    : appException.Code;

                if (_exceptionStatusCodeMapping.TryGetValue(exception.GetType(), out var statusCode))
                {
                    return (statusCode, errorCode, appException.Message);
                }

                // Default to internal server error for unhandled AppException
                return (HttpStatusCode.InternalServerError, errorCode, appException.Message);
            }

            // Default to internal server error for other unhandled exceptions
            return (HttpStatusCode.InternalServerError, "server_error", "An unexpected error occurred.");
        }

        private string GenerateCodeFromClassName(string className)
        {
            if (className.EndsWith("Exception"))
            {
                className = className.Substring(0, className.Length - "Exception".Length);
            }
            return className.ToSnakeCase();
        }
    }
}

