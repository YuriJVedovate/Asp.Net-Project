using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalkToAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.Database
{
    public class TalkToContext : IdentityDbContext<ApplicationUser>
    {
        public TalkToContext(DbContextOptions<TalkToContext> options) : base(options)
        {

        }

        public DbSet<Message> Message { get; set; }
        public DbSet<Token> tokens { get; set; }
    }
}
