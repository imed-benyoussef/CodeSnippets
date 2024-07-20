namespace Aiglusoft.IAM.Domain.Model.UserAggregates
{
    public class UserRole
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string Role { get; private set; }

        public UserRole(string userId, string role)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Role = role;
        }
    }
}

