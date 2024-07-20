using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Localization;
using Aiglusoft.IAM.Domain.Exceptions;

namespace Aiglusoft.IAM.Application.CommandHandlers
{
  public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailSender _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;
        private readonly IStringLocalizer<DomainMessages> _localizer;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IEmailSender emailService,
            IConfiguration configuration,
            ILogger<ForgotPasswordCommandHandler> logger,
            IStringLocalizer<DomainMessages> localizer)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"User with email {request.Email} not found.");

                throw new UserNotFoundException(request.Email, _localizer);

            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            var token = _jwtTokenService.GenerateAccessToken(claims, DateTime.Now.AddMinutes(30));

            // Use the issuer URL from the configuration as the base URL for the reset link
            var issuer = _configuration["Jwt:Issuer"];
            var resetLink = $"{issuer}/reset-password?token={token}";

            // Send email
            await _emailService.SendEmailAsync(user.Email, "Password Reset Request", $"Please reset your password using the following link: {resetLink}");

            
        }

    }


    //public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
    //{
    //    private readonly IVerificationCodeService _verificationCodeService;
    //    private readonly IUserRepository _userRepository;

    //    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    //    {
    //       var user = await _userRepository.GetByIdAsync(request.UserId);
    //        // 2. Valider le code de vérification

    //        //user.VerifyEmail(request.VerificationCode);

    //        //bool isValid = await _verificationCodeService.ValidateVerificationCode(request.UserId, request.VerificationCode);
    //        //if (!isValid)
    //        //{
    //        //    throw new Exception("Invalid verification code");
    //        //}

    //        //// 3. Marquer l'adresse email de l'utilisateur comme vérifiée dans la base de données
    //        //await _verificationCodeService.MarkEmailAsVerified(request.UserId);

    //        return true;
    //    }
    //}

}
