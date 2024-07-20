using System;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Aiglusoft.IAM.Infrastructure.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IUserRepository _userRepository;

        public UserFactory(IHashPasswordService hashPasswordService, IUserRepository userRepository)
        {
            _hashPasswordService = hashPasswordService;
            _userRepository = userRepository;
        }

        public User CreateUser(string username, string email, string password, string firstName, string lastName, DateOnly birthdate, string gender)
        {
            
            var securityStamp = string.IsNullOrEmpty(password)? "" :  Guid.NewGuid().ToString() ;
            var passwordHash = string.IsNullOrEmpty(password) ? "" : _hashPasswordService.HashPassword(password, securityStamp);

            var user = new User(username, email, passwordHash, securityStamp, firstName: firstName, lastName: lastName, birthdate: birthdate, gender: gender);

            return user;
        }
    }


  

}
