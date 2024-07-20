using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Events
{
    public class EmailSetDomainEvent : IDomainEvent
    {
        public string UserId { get; }
        public string Email { get; }
        public string EmailVerification { get; }

        public EmailSetDomainEvent(string userId, string email, string emailVerification)
        {
            UserId = userId;
            Email = email;
            EmailVerification = emailVerification;
        }
    }
}
