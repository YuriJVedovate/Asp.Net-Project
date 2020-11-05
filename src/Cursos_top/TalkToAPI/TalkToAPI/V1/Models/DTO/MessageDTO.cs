using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkToAPI.V1.Models.DTO
{
    public class MessageDTO : BaseDTO
    {
        public int Id { get; set; }
        public ApplicationUser ByUser { get; set; }
        public string ByUserId { get; set; }
        public ApplicationUser ForUser { get; set; }
        public string ForUserId { get; set; }
        public string Messages { get; set; }
        public bool Deleted { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime? Update { get; set; }    }
}
