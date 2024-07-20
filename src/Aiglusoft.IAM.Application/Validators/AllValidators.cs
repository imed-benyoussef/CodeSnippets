using Aiglusoft.IAM.Application.Commands;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Resources;

namespace Aiglusoft.IAM.Application.Validators
{
    public class AuthorizeCommandValidator : AbstractValidator<AuthorizeCommand>
    {
        public AuthorizeCommandValidator()
        {
            

            RuleFor(x => x.ResponseType).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode("invalid_request").WithMessage("ResponseType is required.")
                .Must(BeAValidResponseType).WithErrorCode("unsupported_response_type").WithMessage("Invalid response_type.");

            RuleFor(x => x.ClientId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode("invalid_request").WithMessage("ClientId is required.");

            RuleFor(x => x.RedirectUri).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode("invalid_request").WithMessage("RedirectUri is required.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithErrorCode("invalid_request").WithMessage("RedirectUri must be a valid URI.");

            RuleFor(x => x.State).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode("invalid_request").WithMessage("State is required.");

            RuleFor(x => x.Scope).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode("invalid_request").WithMessage("Scope is required.");

            RuleFor(x => x.CodeChallenge).Cascade(CascadeMode.Stop)
                .NotEmpty().When(x => x.ResponseType == "code").WithErrorCode("invalid_request").WithMessage("CodeChallenge is required for PKCE.");

            RuleFor(x => x.CodeChallengeMethod).Cascade(CascadeMode.Stop)
                .NotEmpty().When(x => x.ResponseType == "code").WithErrorCode("invalid_request").WithMessage("CodeChallengeMethod is required for PKCE.")
                .Must(method => method == "S256" || method == "plain").WithErrorCode("invalid_request").WithMessage("CodeChallengeMethod must be either 'S256' or 'plain'.")
                .When(x => !string.IsNullOrEmpty(x.CodeChallenge));

            RuleFor(x => x.Nonce).Cascade(CascadeMode.Stop)
                .NotEmpty().When(x => x.Scope.Contains("openid")).WithErrorCode("invalid_request").WithMessage("Nonce is required for OpenID Connect.");

            RuleFor(x => x.Display).Cascade(CascadeMode.Stop)
                .Must(BeAValidDisplayType).WithErrorCode("invalid_request").WithMessage("Invalid display type.");

            RuleFor(x => x.Prompt).Cascade(CascadeMode.Stop)
                .Must(BeAValidPromptType).WithErrorCode("invalid_request").WithMessage("Invalid prompt type.");

            RuleFor(x => x.MaxAge).Cascade(CascadeMode.Stop)
                .Must(BeAValidMaxAge).WithErrorCode("invalid_request").WithMessage("Invalid max_age value.");
        }

        private bool BeAValidResponseType(string responseType)
        {
            var validResponseTypes = new[] { "code", "id_token", "token", "code id_token", "code token", "id_token token", "code id_token token" };
            return validResponseTypes.Contains(responseType);
        }

        private bool BeAValidDisplayType(string display)
        {
            var validDisplayTypes = new[] { "page", "popup", "touch", "wap" };
            return string.IsNullOrEmpty(display) || validDisplayTypes.Contains(display);
        }

        private bool BeAValidPromptType(string prompt)
        {
            var validPromptTypes = new[] { "none", "login", "consent", "select_account" };
            return string.IsNullOrEmpty(prompt) || validPromptTypes.Contains(prompt);
        }

        private bool BeAValidMaxAge(string maxAge)
        {
            return string.IsNullOrEmpty(maxAge) || int.TryParse(maxAge, out _);
        }
    }

    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer["EmailRequired"]).WithErrorCode("EmailRequired")
                .EmailAddress().WithMessage(localizer["InvalidEmail"]).WithErrorCode("InvalidEmail");
        }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(localizer["UsernameRequired"]).WithErrorCode("UsernameRequired");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizer["PasswordRequired"]).WithErrorCode("PasswordRequired");
        }
    }


}
