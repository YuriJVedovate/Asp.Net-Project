using MyTasksAPI.Database;
using MyTasksAPI.Models;
using MyTasksAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTasksAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {

        private readonly MyTasksContext _base;

        public TokenRepository(MyTasksContext banco )
        {
            _base = banco;
        }

        public Token GetToken(string refreshToken)
        {
            return _base.Token.FirstOrDefault(a => a.RefreshToken == refreshToken && a.Used == false);
        }

        public void RegisterToken(Token token)
        {
            _base.Token.Add(token);
            _base.SaveChanges();

        }

        public void UpdateToken(Token token)
        {
            _base.Token.Update(token);
            _base.SaveChanges();
        }

    }
}
