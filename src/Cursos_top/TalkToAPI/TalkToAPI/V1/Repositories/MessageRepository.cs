using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalkToAPI.Database;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Repositories
{
    public class MessageRepository : IMessageRepository
    {

        private readonly TalkToContext _base;

        public MessageRepository(TalkToContext banco)
        {
            _base = banco;
        }



        public Message GetOne(int id)
        {
            return _base.Message.Find(id);
        }

        public List<Message> GetAll(string user1Id, string user2Id)
        {
            return _base.Message.Where(a => (a.ForUserId == user1Id || a.ForUserId == user2Id) && (a.ByUserId == user1Id || a.ByUserId == user2Id)).ToList();
        }

        public void Register(Message message)
        {
            _base.Message.Add(message);
            _base.SaveChanges();
        }

        public void Update(Message message)
        {
            _base.Message.Update(message);
            _base.SaveChanges();
        }

    }
}
