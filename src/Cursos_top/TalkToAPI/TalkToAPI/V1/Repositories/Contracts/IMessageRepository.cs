using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Repositories.Contracts
{
    public interface IMessageRepository
    {
        Message GetOne(int id);

        List<Message> GetAll(string user1Id, string user2Id);

        void Register(Message message);

        void Update(Message message);


    }
}
