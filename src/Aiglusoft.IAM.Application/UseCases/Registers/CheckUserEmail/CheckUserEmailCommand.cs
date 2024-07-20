using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using MediatR;

namespace Aiglusoft.IAM.Application.UseCases.Registers.CheckUserEmail
{
    public class CheckUserEmailCommand : IRequest<TokenResponse>
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateOnly Birthdate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }


}
