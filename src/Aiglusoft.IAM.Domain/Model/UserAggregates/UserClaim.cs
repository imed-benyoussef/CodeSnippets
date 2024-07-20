namespace Aiglusoft.IAM.Domain.Model.UserAggregates
{
    public class UserClaim
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string ClaimType { get; private set; }
        public string ClaimValue { get; private set; }

        public UserClaim(string userId, string claimType, string claimValue)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}

