using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.DTOs
{
    public class UserDto
    {
        public string UserId { get; internal set; }
        public string Username { get; internal set; }
        public string Email { get; internal set; }
    }
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
        public string RefreshToken { get; set; } // Optional
    }


    public class UserNamesDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GeneralInfoDto
    {
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class EmailDto
    {
        public string Email { get; set; }
    }

    public class VerifyEmailDto
    {
        public string Code { get; set; }
    }

    public class PasswordDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class PhoneDto
    {
        public string PhoneNumber { get; set; }
    }

    public class VerifyPhoneDto
    {
        public string Code { get; set; }
    }

    public class TermsAcceptanceDto
    {
        public bool Accepted { get; set; }
    }


}
