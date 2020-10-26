using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyTasksAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("UserId")]
        public string FullName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}

