using Aiglusoft.IAM.Domain.Model.UserAggregates;

namespace Aiglusoft.IAM.Domain.Factories
{
    public interface IUserFactory
    {
        User CreateUser(string username, string email, string password, string firstName, string lastName, DateOnly birthdate, string gender);
    }
}
