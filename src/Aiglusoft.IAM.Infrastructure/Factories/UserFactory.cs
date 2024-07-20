using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using Aiglusoft.IAM.Domain.Services;

namespace Aiglusoft.IAM.Infrastructure.Factories
{
  public class UserFactory : IUserFactory
  {
    private readonly IHashPasswordService _hashPasswordService;

    public UserFactory(IHashPasswordService hashPasswordService)
    {
      _hashPasswordService = hashPasswordService;
    }


    public User CreateUser(string username, string email, string password, string firstName, string lastName, DateOnly birthdate, string gender)
    {
      var securityStamp = Guid.NewGuid().ToString();
      var passwordHash = _hashPasswordService.HashPassword(password, securityStamp);
      return new User(username: username, email: email, passwordHash: passwordHash, securityStamp: securityStamp, firstName: firstName, lastName: lastName, birthdate: birthdate, gender: gender);
    }
  }
}
