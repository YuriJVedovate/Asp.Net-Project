using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ByUserId")]
        public ApplicationUser ByUser { get; set; }

        [Required]
        public string ByUserId { get; set; }

        [ForeignKey("ForUserId")]
        public ApplicationUser ForUser { get; set; }

        [Required]
        public string ForUserId { get; set; }

        [Required]
        public string Messages { get; set; }

        public bool Deleted { get; set; }

        public DateTime SendTime { get; set; }

        public DateTime? Update { get; set; }
    }
}
