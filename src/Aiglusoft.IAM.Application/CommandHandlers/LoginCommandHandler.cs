using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Aiglusoft.IAM.Application.CommandHandlers
{
  //public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
  //{
  //    private readonly IUserRepository _userRepository;
  //    private readonly IUserFactory _userFactory;

  //    private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;
  //    public RegisterUserCommandHandler(IUserRepository userRepository, IUserFactory userFactory, IStringLocalizer<ErrorMessages> localizerErrorMessages)
  //    {
  //        _userRepository = userRepository;
  //        _userFactory = userFactory;
  //        _localizerErrorMessages = localizerErrorMessages;
  //    }

  //    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
  //    {
  //        // Check if the username is already in use
  //        var existingUserByUsername = await _userRepository.GetByUsernameAsync(request.Username);
  //        if (existingUserByUsername != null)
  //        {
  //            throw new UserAlreadyExistsException(_localizerErrorMessages, "UserAlreadyExists");
  //        }

  //        // Check if the email is already in use
  //        var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
  //        if (existingUserByEmail != null)
  //        {
  //            throw new UserAlreadyExistsException(_localizerErrorMessages, "UserAlreadyExists");
  //        }

  //        // Create a new user
  //        var user = _userFactory.CreateUser(request.Username, request.Email, request.Password, "","");
  //        await _userRepository.AddAsync(user);

  //        return user.UserId;
  //    }
  //}


  public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;

        public LoginCommandHandler(IUserRepository userRepository, IHashPasswordService hashPasswordService, IStringLocalizer<ErrorMessages> localizerErrorMessages)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _localizerErrorMessages = localizerErrorMessages;
        }

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username) ?? await _userRepository.GetByEmailAsync(request.Username);
            if (user == null || !_hashPasswordService.VerifyPassword(request.Password, user.PasswordHash, user.SecurityStamp))
            {
                throw new Exceptions.UnauthorizedAccessException(_localizerErrorMessages, "InvalidCredentials");
            }

            return new UserDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email
            };
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
