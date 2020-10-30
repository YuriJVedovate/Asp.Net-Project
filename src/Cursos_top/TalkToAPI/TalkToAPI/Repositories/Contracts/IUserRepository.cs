using TalkToAPI.Models;

namespace TalkToAPI.Repositories.Contracts
{
    public interface IUserRepository
    {
        void Register(ApplicationUser user, string password);

        ApplicationUser Get(string email, string password);

        ApplicationUser Get(string id);

    }
}
