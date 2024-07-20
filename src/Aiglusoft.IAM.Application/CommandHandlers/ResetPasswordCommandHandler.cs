

using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Aiglusoft.IAM.Application.CommandHandlers
{
  public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IJwtTokenService _jwtTokenService;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IHashPasswordService hashPasswordService,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = _jwtTokenService.ValidateToken(request.Token);

                var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new SecurityTokenException("Invalid token.");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new SecurityTokenException("Invalid token.");
                }

                var securityStamp = Guid.NewGuid().ToString();
                var passwordHash = _hashPasswordService.HashPassword(request.NewPassword, securityStamp);

                user.SetPassword(passwordHash, securityStamp);

                await _userRepository.UpdateAsync(user);

                
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token.", ex);
            }
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
