using Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.Validators
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
        string ErrorMessage { get; }
        string ErrorCode { get; }
    }


    public class ClientIdSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "ClientId is required.";
        public string ErrorCode => "invalid_request";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return !string.IsNullOrEmpty(command.ClientId);
        }
    }
    public class RedirectUriSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "RedirectUri is required and must be a valid URI.";
        public string ErrorCode => "invalid_request";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return !string.IsNullOrEmpty(command.RedirectUri) && Uri.IsWellFormedUriString(command.RedirectUri, UriKind.Absolute);
        }
    }
    public class StateSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "State is required.";
        public string ErrorCode => "invalid_request";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return !string.IsNullOrEmpty(command.State);
        }
    }
    public class ResponseTypeSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "ResponseType must be 'code', 'id_token', or 'token'.";
        public string ErrorCode => "unsupported_response_type";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            var validResponseTypes = new[] { "code", "id_token", "token" };
            var responseTypes = command.ResponseType.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return responseTypes.All(rt => validResponseTypes.Contains(rt));
        }
    }
    public class ScopeSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "Scope is required and must include 'openid' for OpenID Connect.";
        public string ErrorCode => "invalid_scope";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return !string.IsNullOrEmpty(command.Scope) && command.Scope.Contains("openid");
        }
    }
    public class NonceSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "Nonce is required for OpenID Connect.";
        public string ErrorCode => "invalid_request";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return command.ResponseType.Contains("id_token") && !string.IsNullOrEmpty(command.Nonce);
        }
    }
    public class CodeChallengeSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "CodeChallenge is required for PKCE.";
        public string ErrorCode => "invalid_request";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return !string.IsNullOrEmpty(command.CodeChallenge);
        }
    }
    public class CodeChallengeMethodSpecification : ISpecification<GenerateAuthorizationCodeCommand>
    {
        public string ErrorMessage => "CodeChallengeMethod must be either 'S256' or 'plain'.";
        public string ErrorCode => "invalid_request";

        public bool IsSatisfiedBy(GenerateAuthorizationCodeCommand command)
        {
            return command.CodeChallengeMethod == "S256" || command.CodeChallengeMethod == "plain";
        }
    }

}
