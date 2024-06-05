using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class ExceptionMapping
    {
        private readonly Dictionary<Type, (int StatusCode, string Message)> _exceptionMap;

        public ExceptionMapping()
        {
            _exceptionMap = new Dictionary<Type, (int StatusCode, string Message)>
        {
            { typeof(UnauthorizedAccessException), (StatusCodes.Status401Unauthorized, "Unauthorized access.") },
            { typeof(ArgumentNullException), (StatusCodes.Status400BadRequest, "A required parameter was null.") },
            { typeof(KeyNotFoundException), (StatusCodes.Status404NotFound, "The requested resource was not found.") },
            // Ajoutez d'autres exceptions et messages ici
        };
        }

        public bool TryGetExceptionDetails(Type exceptionType, out (int StatusCode, string Message) details)
        {
            return _exceptionMap.TryGetValue(exceptionType, out details);
        }
    }
}
