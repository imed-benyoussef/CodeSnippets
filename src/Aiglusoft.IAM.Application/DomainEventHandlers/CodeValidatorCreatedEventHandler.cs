using Aiglusoft.IAM.Domain.Events;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using Aiglusoft.IAM.Domain.Services;
using MediatR;

namespace Aiglusoft.IAM.Application.DomainEventHandlers
{
    public class CodeValidatorCreatedEventHandler : INotificationHandler<CodeValidatorCreatedEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

        public CodeValidatorCreatedEventHandler(IEmailSender emailSender, ISmsSender smsSender)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public async Task Handle(CodeValidatorCreatedEvent notification, CancellationToken cancellationToken)
        {
            switch (notification.Type)
            {
                case VerificationType.Email:
                    await _emailSender.SendEmailAsync(notification.Target, "Your verification code", $"Your code is: {notification.Code}");
                    break;
                case VerificationType.Sms:
                    await _smsSender.SendSmsAsync(notification.Target, $"Your verification code is {notification.Code}");
                    break;
            }
        }
    }
}
