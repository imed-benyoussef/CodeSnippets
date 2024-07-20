using Aiglusoft.IAM.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.DomainEventHandlers
{
    public partial class EmailSetDomainEventHandler : INotificationHandler<EmailSetDomainEvent>
    {
        public Task Handle(EmailSetDomainEvent notification, CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }
    }
}
