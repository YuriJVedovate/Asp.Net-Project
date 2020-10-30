using TalkToAPI.Database;
using TalkToAPI.Models;
using TalkToAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {

        private readonly TalkToContext _base;

        public TokenRepository(TalkToContext banco )
        {
            _base = banco;
        }

        public Token GetToken(string refreshToken)
        {
            return _base.tokens.FirstOrDefault(a => a.RefreshToken == refreshToken && a.Used == false);
        }

        public void RegisterToken(Token token)
        {
            _base.tokens.Add(token);
            _base.SaveChanges();

        }

        public void UpdateToken(Token token)
        {
            _base.tokens.Update(token);
            _base.SaveChanges();
        }

    }
}
