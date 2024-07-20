using Aiglusoft.IAM.Domain.Model.CodeValidators;
using MediatR;

namespace Aiglusoft.IAM.Domain.Events
{
    public class CodeValidatorCreatedEvent : IDomainEvent
    {
        public Guid CodeValidatorId { get; }
        public string Code { get; }
        public VerificationType Type { get; }
        public string Target { get; }  // Email address or phone number

        public CodeValidatorCreatedEvent(Guid codeValidatorId, string code, VerificationType type, string target)
        {
            CodeValidatorId = codeValidatorId;
            Code = code;
            Type = type;
            Target = target;
        }
    }
}
