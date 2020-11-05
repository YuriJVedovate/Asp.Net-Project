using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.Repositories.Contracts
{
    public interface ITokenRepository
    {
        void RegisterToken(Token token);

        Token GetToken(string refreshToken);

        void UpdateToken(Token token);
    }
}
