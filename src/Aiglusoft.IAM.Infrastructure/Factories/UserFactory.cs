using System;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Factories;
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

        public User CreateUser(string username, string email, string password)
        {
            var securityStamp = Guid.NewGuid().ToString();
            var passwordHash = _hashPasswordService.HashPassword(password, securityStamp);
            return new User(username, email, passwordHash, securityStamp);
        }
    }
}
