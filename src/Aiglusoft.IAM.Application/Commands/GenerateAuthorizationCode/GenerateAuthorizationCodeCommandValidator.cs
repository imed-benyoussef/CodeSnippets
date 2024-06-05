namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    using FluentValidation;

    public class GenerateAuthorizationCodeCommandValidator : AbstractValidator<GenerateAuthorizationCodeCommand>
    {
        public GenerateAuthorizationCodeCommandValidator()
        {
            // Validation for OAuth 2.0 and OpenID Connect common parameters
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("ClientId is required.").WithErrorCode("invalid_request");

            RuleFor(x => x.RedirectUri)
                .NotEmpty().WithMessage("RedirectUri is required.").WithErrorCode("invalid_request")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("RedirectUri must be a valid URI.").WithErrorCode("invalid_request");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.").WithErrorCode("invalid_request");

            RuleFor(x => x.ResponseType)
                .NotEmpty().WithMessage("ResponseType is required.").WithErrorCode("unsupported_response_type")
                .Must(BeAValidResponseType).WithMessage("ResponseType must be 'code', 'id_token', or 'token'.").WithErrorCode("unsupported_response_type");

            // OAuth 2.0 specific validation
            When(x => x.Scope != null && !x.Scope.Contains("openid"), () =>
            {
                RuleFor(x => x.Scope)
                    .NotEmpty().WithMessage("Scope is required.").WithErrorCode("invalid_scope");
            });

            // OpenID Connect specific validation
            When(x => x.Scope != null && x.Scope.Contains("openid"), () =>
            {
                RuleFor(x => x.Scope)
                    .NotEmpty().WithMessage("Scope is required.").WithErrorCode("invalid_scope")
                    .Must(scope => scope.Contains("openid")).WithMessage("Scope must include 'openid' for OpenID Connect.").WithErrorCode("invalid_scope");

                RuleFor(x => x.Nonce)
                    .NotEmpty().WithMessage("Nonce is required for OpenID Connect.").WithErrorCode("invalid_request");

                RuleFor(x => x.CodeChallenge)
                    .NotEmpty().WithMessage("CodeChallenge is required for PKCE.").WithErrorCode("invalid_request");

                RuleFor(x => x.CodeChallengeMethod)
                    .NotEmpty().WithMessage("CodeChallengeMethod is required for PKCE.").WithErrorCode("invalid_request")
                    .Must(method => method == "S256" || method == "plain").WithMessage("CodeChallengeMethod must be either 'S256' or 'plain'.").WithErrorCode("invalid_request");
            });
        }

        private bool BeAValidResponseType(string responseType)
        {
            var validResponseTypes = new[] { "code", "id_token", "token" };
            var responseTypes = responseType.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return responseTypes.All(rt => validResponseTypes.Contains(rt));
        }
    }


}
