namespace Aiglusoft.IAM.Server.Models.V1.Requests
{
    public class CheckUserEmailRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthdate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }

    public class VerifyEmailRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class SetUserPasswordRequest
    {
        public string Password { get; set; }

    }

    public class AddPhoneRequest
    {
        public string Phone { get; set; }
    }

    public class VerifyPhoneRequest
    {
        public string Phone { get; set; }
        public string Code { get; set; }
    }

}
