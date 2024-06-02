using FluentValidation;

namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    public class GenerateAuthorizationCodeCommandValidator : AbstractValidator<GenerateAuthorizationCodeCommand>
    {
        public GenerateAuthorizationCodeCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId is required.");
            RuleFor(x => x.RedirectUri).NotEmpty().WithMessage("RedirectUri is required.")
                                      .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("RedirectUri must be a valid URL.");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required.");
        }
    }

}
