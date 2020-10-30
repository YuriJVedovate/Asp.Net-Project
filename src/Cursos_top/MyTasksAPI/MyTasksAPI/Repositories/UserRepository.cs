using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyTasksAPI.Models;
using MyTasksAPI.Repositories.Contracts;
using System;
using System.Text;

namespace MyTasksAPI.Repositories.Contracts
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public void Register(ApplicationUser user, string password)
        {
            var result = _userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var erro in result.Errors)
                {
                    sb.Append(erro.Description);
                }
                throw new Exception("Usuario não cadastrado! {} " + sb.ToString());
            }
        }

        public ApplicationUser Get(string email, string password)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (_userManager.CheckPasswordAsync(user, password).Result)
            {
                return user;
            }
            else
            {
                throw new Exception("Usuário não localizado!");
            }
        }

        public ApplicationUser Get(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }
    }
}
