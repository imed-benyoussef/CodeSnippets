using System;
using Aiglusoft.IAM.Shared.Utilities;

namespace Aiglusoft.IAM.Application.Exceptions
{
    public class AppException : Exception
    {
        public string Code { get; }

        public AppException( string message) : base(message)
        {
            Code = GenerateCodeFromClassName();
        }
        public AppException(string code, string message) : base(message)
        {
            Code = string.IsNullOrEmpty(code) ? GenerateCodeFromClassName() : code;
        }

        public AppException(string code, string message, Exception innerException) : base(message, innerException)
        {
            Code = string.IsNullOrEmpty(code) ? GenerateCodeFromClassName() : code;
        }

        private string GenerateCodeFromClassName()
        {
            var className = GetType().Name;
            if (className.EndsWith("Exception"))
            {
                className = className.Substring(0, className.Length - "Exception".Length);
            }
            return className.ToSnakeCase();
        }
    }

    public class InvalidRequestException : AppException
    {
        public InvalidRequestException(string message) : base(null, message) { }
    }

    public class InvalidClientException : AppException
    {
        public InvalidClientException(string message) : base(null, message) { }
    }

    public class InvalidRedirectUriException : AppException
    {
        public InvalidRedirectUriException(string message) : base(null, message) { }
    }

    public class UnsupportedResponseTypeException : AppException
    {
        public UnsupportedResponseTypeException(string message) : base(null, message) { }
    }

    public class UserAlreadyExistsException : AppException
    {
        public UserAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
