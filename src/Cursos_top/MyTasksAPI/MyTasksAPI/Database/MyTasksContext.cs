using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTasksAPI.Models;

namespace MyTasksAPI.Database
{
    public class MyTasksContext : IdentityDbContext<ApplicationUser>
    {

        public MyTasksContext(DbContextOptions<MyTasksContext> options) : base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Token> Token { get; set; }

    }
}
