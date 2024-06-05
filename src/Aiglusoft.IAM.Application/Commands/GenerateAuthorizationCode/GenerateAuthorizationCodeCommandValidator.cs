namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    using FluentValidation;

    public class GenerateAuthorizationCodeCommandValidator : AbstractValidator<GenerateAuthorizationCodeCommand>
    {
        public GenerateAuthorizationCodeCommandValidator()
        {
            // Validation for OAuth 2.0 and OpenID Connect common parameters
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("ClientId is required.");

            RuleFor(x => x.RedirectUri)
                .NotEmpty().WithMessage("RedirectUri is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("RedirectUri must be a valid URI.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.");

            RuleFor(x => x.ResponseType)
                .NotEmpty().WithMessage("ResponseType is required.")
                .Must(BeAValidResponseType).WithMessage("ResponseType must be 'code', 'id_token', or 'token'.");

            // OAuth 2.0 specific validation
            When(x => x.Scope != null && !x.Scope.Contains("openid"), () =>
            {
                RuleFor(x => x.Scope)
                    .NotEmpty().WithMessage("Scope is required.");
            });

            // OpenID Connect specific validation
            When(x => x.Scope != null && x.Scope.Contains("openid"), () =>
            {
                RuleFor(x => x.Scope)
                    .NotEmpty().WithMessage("Scope is required.")
                    .Must(scope => scope.Contains("openid")).WithMessage("Scope must include 'openid' for OpenID Connect.");

                RuleFor(x => x.Nonce)
                    .NotEmpty().WithMessage("Nonce is required for OpenID Connect.");

                RuleFor(x => x.CodeChallenge)
                    .NotEmpty().WithMessage("CodeChallenge is required for PKCE.");

                RuleFor(x => x.CodeChallengeMethod)
                    .NotEmpty().WithMessage("CodeChallengeMethod is required for PKCE.")
                    .Must(method => method == "S256" || method == "plain").WithMessage("CodeChallengeMethod must be either 'S256' or 'plain'.");
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
