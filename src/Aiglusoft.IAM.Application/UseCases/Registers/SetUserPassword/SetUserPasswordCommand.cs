using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Application.UseCases.Emails.VerifyEmail;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.UseCases.Registers.RegisterUserPassword
{
    public class SetUserPasswordCommand : IRequest<TokenResponse>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
