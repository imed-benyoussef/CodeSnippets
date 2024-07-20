using Aiglusoft.IAM.Domain.Events;

namespace Aiglusoft.IAM.Domain.Model.UserAggregates
{
    public class User : Entity<string>, IAggregateRoot
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string? LastName { get; private set; }
        public DateOnly? Birthdate { get; private set; }
        public string? Gender { get; private set; }
        public string Email { get; private set; }
        public string? EmailVerificationHash { get; private set; }
        public bool EmailVerified { get; private set; }
        public string? PasswordHash { get; private set; }
        public string? SecurityStamp { get; private set; }
        public bool IsActive { get; private set; }

        public DateTime createdAt;
        public DateTime? updatedAt;


        private List<UserClaim> _claims;
        public IReadOnlyCollection<UserClaim> Claims => _claims.AsReadOnly();

        private List<UserRole> _roles;

        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

        
        public User()
        {
            Id =Guid.NewGuid().ToString() ;
            _claims = new List<UserClaim>();
            _roles = new List<UserRole>();
        }

        public User(string username, string email, string passwordHash, string securityStamp, string firstName, string lastName, DateOnly birthdate, string gender) : this()
        {

            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            EmailVerified = false;
            _claims = new List<UserClaim>();
            SecurityStamp = securityStamp;

            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
            Gender = gender;
        }

       

        public void SetUsername(string username)
        {
            Username = username;
        }

        // Methods to update user profile information
        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetBirthdate(DateOnly birthdate)
        {
            Birthdate = birthdate;
        }

        public void SetGender(string gender)
        {
            Gender = gender;
        }

        // Methods to manage email verification
        public void SetEmail(string email, string emailVerification, string emailVerificationHash)
        {
            if (email == Email)
                return;

            Email = email;
            EmailVerificationHash = emailVerificationHash;
            EmailVerified = false;


            // Ajouter l'événement de domaine
            AddDomainEvent(new EmailSetDomainEvent(Id, email, emailVerification));
        }

        public void VerifyEmail()
        {
            EmailVerified = true;
        }


        public void SetPassword(string passwordHash, string securityStamp)
        {
            PasswordHash = passwordHash;
            SecurityStamp = securityStamp;
        }

    }
}

