
namespace Aiglusoft.IAM.Application.Exceptions
{
    using System;

    public class OAuthException : Exception
    {
        public string Error { get; }
        public string ErrorDescription { get; }

        public OAuthException(string error, string errorDescription)
            : base($"{error}: {errorDescription}")
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }

}
