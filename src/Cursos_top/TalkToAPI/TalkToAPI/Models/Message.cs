using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ByUserId")]
        public ApplicationUser ByUser { get; set; }

        public string ByUserId { get; set; }

        [ForeignKey("ForUserId")]
        public ApplicationUser ForUser { get; set; }

        public string ForUserId { get; set; }

        public string Messages { get; set; }

        public DateTime SendTime { get; set; }
    }
}
