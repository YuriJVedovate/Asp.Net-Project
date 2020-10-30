using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyTasksAPI.Models
{
    public class Task
    {
        [Key]
        public int IdTaskApi { get; set; }
        public int IdTaskApp { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Local { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool CompletionStatus { get; set; }
        public bool ExclusionStatus { get; set; }
        public DateTime Insert { get; set; }
        public DateTime Update { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }


    }
}