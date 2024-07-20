using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Domain.Exceptions;
using Aiglusoft.IAM.Shared.Utilities;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class ErrorMappingService
    {
        private readonly IDictionary<Type, HttpStatusCode> _exceptionStatusCodeMapping = new Dictionary<Type, HttpStatusCode>
    {
        { typeof(InvalidClientException), HttpStatusCode.Unauthorized },
        { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
        { typeof(Application.Exceptions.UnauthorizedAccessException), HttpStatusCode.Unauthorized },
        { typeof(InvalidRequestException), HttpStatusCode.BadRequest },
        { typeof(InvalidRedirectUriException), HttpStatusCode.BadRequest },
        { typeof(UnsupportedResponseTypeException), HttpStatusCode.BadRequest },
        // Add other mappings as necessary
    };

        public (HttpStatusCode statusCode, string error, string errorDescription) Map(Exception exception)
        {
           var errorCode = GetErrorCode(exception);

            if (exception as AppException != null || exception as DomainException != null)
            {

                if (_exceptionStatusCodeMapping.TryGetValue(exception.GetType(), out var statusCode))
                {
                    return (statusCode, errorCode, exception.Message);
                }

                // Default to BadRequest for unhandled IErrorCodeException
                return (HttpStatusCode.BadRequest, errorCode, exception.Message);
            }

            // Default to internal server error for other unhandled exceptions
            return (HttpStatusCode.InternalServerError, "server_error", "An unexpected error occurred.");
        }

        public string GetErrorCode(Exception exception)
        {
            var code = "";
            var codeProperty = exception.GetType().GetProperty("Code");
            if (codeProperty != null && codeProperty.CanRead)
            {
                code = codeProperty.GetValue(exception) as string;
            }
            if (string.IsNullOrEmpty(code))
            {
                code = exception.GetType().Name;
                if (code.EndsWith("Exception"))
                {
                    code = code.Substring(0, code.Length - "Exception".Length);
                }
            }
  

            return code!.ToSnakeCase();
        }


    }

}

