using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalkToAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Slogan { get; set; }
    }
}