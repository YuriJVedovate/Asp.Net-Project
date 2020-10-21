using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helpers
{
    public class Pagination
    {
        public int NumberPage { get; set; }
        public int NumberRecords { get; set; }

        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }

}
