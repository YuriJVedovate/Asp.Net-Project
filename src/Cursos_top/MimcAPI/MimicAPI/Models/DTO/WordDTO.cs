using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Models.DTO
{
    public class WordDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }
        public bool Status { get; set; }
        public DateTime Creation { get; set; }
        public DateTime? Update { get; set; }
    }
}
