using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain.Entities;
using MediatR;
using System;

namespace Aiglusoft.IAM.Application.Commands
{
    public class AuthorizeCommand : IRequest<string>
    {
        public string ResponseType { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string Scope { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string Nonce { get; set; }
        public string Prompt { get; set; }
        public string MaxAge { get; set; }
        public string Display { get; set; }
        public string AcrValues { get; set; }
        public string IdTokenHint { get; set; }
        public string LoginHint { get; set; }
    }

    public class TokenCommand : IRequest<TokenResponse>
    {
        public string GrantType { get; set; }
        public string Code { get; set; }
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }



    public class RegisterUserCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommand : IRequest<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
