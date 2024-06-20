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

}
