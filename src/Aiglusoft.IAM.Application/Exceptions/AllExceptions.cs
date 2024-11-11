using System;
using System.Globalization;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Exceptions;
using Aiglusoft.IAM.Shared.Utilities;
using Microsoft.Extensions.Localization;

namespace Aiglusoft.IAM.Application.Exceptions
{
    public class AppException : Exception
    {
        public virtual string? Code { get; }

        public AppException()
        {

        }

        public AppException(string message) : base(message)
        {
        }
        public AppException(string code, string message) : base(message)
        {
            Code = code;
        }

        public AppException(IStringLocalizer localizer, string resourceKey, params object[] args)
        : this(string.Format(localizer[resourceKey], args))
        {

        }

        public AppException(string code, IStringLocalizer localizer, string resourceKey, params object[] args)
        : this(code, string.Format(localizer[resourceKey], args))
        {

        }

    }

    public class InvalidRequestException : AppException
    {
        public InvalidRequestException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }

    public class InvalidClientException : AppException
    {
        public InvalidClientException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }

    public class InvalidRedirectUriException : AppException
    {
        public InvalidRedirectUriException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }

    public class UnsupportedResponseTypeException : AppException
    {
        public UnsupportedResponseTypeException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }

    public class UserAlreadyExistsException : AppException
    {
        public UserAlreadyExistsException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }

    public class UnauthorizedAccessException : AppException
    {
        public UnauthorizedAccessException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }


    public class InvalidVerificationCodeException : AppException
    {
        public InvalidVerificationCodeException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }


    public class ClientNotFoundException : AppException
    {
        public ClientNotFoundException(IStringLocalizer localizer, string resourceKey, params object[] args) : base(localizer, resourceKey, args)
        {
        }
    }





}
