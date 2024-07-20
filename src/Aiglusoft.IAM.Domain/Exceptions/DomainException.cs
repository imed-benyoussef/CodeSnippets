using Aiglusoft.IAM.Shared.Utilities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Exceptions
{
    public interface IErrorCodeException
    {
        string Code { get; }
    }

    public interface IGlobalizedException
    {
        Type LocalizerType { get; }
        string ResourceKey { get; }
        object[] Parameters { get; }
    }

    public class DomainException : Exception , IErrorCodeException
    {
        public virtual string? Code { get; }

        public DomainException()
        {
                
        }

        public DomainException(string message) : base(message)
        {
        }
        public DomainException(string code, string message) : base(message)
        {
            Code = code;
        }

        public DomainException(IStringLocalizer<DomainMessages> localizer, string resourceKey, params object[] args)
        : this(string.Format(localizer[resourceKey], args))
        {
           
        }

        public DomainException(string code, IStringLocalizer<DomainMessages> localizer, string resourceKey, params object[] args)
        : this(code, string.Format(localizer[resourceKey], args))
        {
           
        }

    }


    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(string email, IStringLocalizer<DomainMessages> localizer)
            : base(localizer, "UserNotFound", email)
        {
        }
    }

}
