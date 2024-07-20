using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.UseCases.Emails.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<TokenResponse>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
