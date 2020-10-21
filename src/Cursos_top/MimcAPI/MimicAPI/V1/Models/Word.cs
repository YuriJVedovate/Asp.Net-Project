using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Models
{
    public class Word
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Difficulty { get; set; }

        public bool Status { get; set; }

        public DateTime Creation { get; set; }

        public DateTime? Update { get; set; }

    }
}
