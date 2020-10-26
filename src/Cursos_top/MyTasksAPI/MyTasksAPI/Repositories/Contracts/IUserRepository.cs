using MyTasksAPI.Models;

namespace MyTasksAPI.Repositories.Contracts
{
    public interface IUserRepository
    {
        void Register(ApplicationUser user, string password);

        ApplicationUser Get(string email, string password);

    }
}
