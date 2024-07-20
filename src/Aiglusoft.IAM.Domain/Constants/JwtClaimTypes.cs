using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Constants
{
    public static class JwtClaimTypes
    {
        // Standard JWT claims
        public const string Sub = "sub"; // Subject
        public const string Jti = "jti"; // JWT ID
        public const string Iat = "iat"; // Issued At
        public const string Exp = "exp"; // Expiration Time
        public const string Nbf = "nbf"; // Not Before
        public const string Aud = "aud"; // Audience
        public const string Iss = "iss"; // Issuer

        // Standard OpenID Connect claims
        public const string Name = "name"; // Full name
        public const string GivenName = "given_name"; // First name
        public const string FamilyName = "family_name"; // Last name
        public const string MiddleName = "middle_name"; // Middle name
        public const string Nickname = "nickname"; // Nickname
        public const string PreferredUsername = "preferred_username"; // Preferred username
        public const string Profile = "profile"; // Profile URL
        public const string Picture = "picture"; // Picture URL
        public const string Website = "website"; // Website URL
        public const string Email = "email"; // Email address
        public const string EmailVerified = "email_verified"; // Email verified
        public const string Gender = "gender"; // Gender
        public const string Birthdate = "birthdate"; // Birthdate
        public const string Zoneinfo = "zoneinfo"; // Timezone
        public const string Locale = "locale"; // Locale
        public const string PhoneNumber = "phone_number"; // Phone number
        public const string PhoneNumberVerified = "phone_number_verified"; // Phone number verified
        public const string Address = "address"; // Address
        public const string UniqueName = "unique_name"; // Unique name

        // Custom claims for roles and permissions
        public const string Role = "role"; // Role
        public const string Permissions = "permissions"; // Permissions

        // Additional claims commonly used in OAuth2
        public const string Scope = "scope"; // Scope
        public const string ClientId = "client_id"; // Client ID
        public const string Azp = "azp"; // Authorized party
        public const string Amr = "amr"; // Authentication method reference

        // Microsoft-specific claims
        public const string AuthTime = "auth_time"; // Authentication time
        public const string Acr = "acr"; // Authentication context class reference
        public const string Sid = "sid"; // Session ID

        // Claims commonly found in other identity providers
        public const string IdentityProvider = "idp"; // Identity provider
        public const string ExternalUserId = "external_user_id"; // External user ID

        // Custom claims for user information
        public const string AesKey = "aes:key"; // Example custom claim 1
        //public const string CustomClaim2 = "custom_claim_2"; // Example custom claim 2

        // Claims for XML SOAP
        public const string XmlSoapName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"; // Full name
        public const string XmlSoapGivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"; // Given name
        public const string XmlSoapSurname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"; // Surname
        public const string XmlSoapEmail = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"; // Email address
        public const string XmlSoapRole = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"; // Role
        public const string XmlSoapDateOfBirth = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth"; // Date of birth
        public const string XmlSoapGender = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";
        public const string XmlSoapNameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    }


}
