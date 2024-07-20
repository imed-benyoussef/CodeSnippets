using Aiglusoft.IAM.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Model.CodeValidators
{
    public class CodeValidator : Entity<Guid>, IAggregateRoot
    {
        public string Code { get; private set; }
        public CodeStatus Status { get; private set; }
        public VerificationType Type { get; private set; }
        public string Target { get; private set; }  // Email or phone number

        private DateTime createdAt;
        private DateTime expiresAt;


        private CodeValidator() { }

        public CodeValidator(string target, VerificationType type, TimeSpan validityDuration)
        {
            Id = Guid.NewGuid();
            Code = GenerateRandomCode();
            expiresAt = DateTime.UtcNow.Add(validityDuration);
            Status = CodeStatus.Active;
            Type = type;
            Target = target;

            // Émission de l'événement de domaine
            AddDomainEvent(new CodeValidatorCreatedEvent(Id, Code, Type, Target));
        }

        public void MarkAsUsed()
        {
            if (Status == CodeStatus.Used)
                throw new InvalidOperationException("This verification code has already been used.");

            Status = CodeStatus.Used;
        }

        public void MarkAsExpired()
        {
            Status = CodeStatus.Expired;
        }

        public void MarkAsCancelled()
        {
            Status = CodeStatus.Cancelled;
        }

        public bool IsValid(string code, string target)
        {
            return Target == target && Status == CodeStatus.Active && Code == code && DateTime.UtcNow <= expiresAt;
        }


        private string GenerateRandomCode()
        {
            return new Random().Next(100000, 999999).ToString(); // Example for a 6-digit code
        }
    }
}
