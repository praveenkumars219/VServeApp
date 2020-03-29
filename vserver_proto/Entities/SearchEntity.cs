using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vserver_proto.Entities
{
    public class SearchEntity
    {
        public string Work { get; set; }
        public string Location { get; set; }
        public double SalaryRange { get; set; }
        public int Experience { get; set; }
    }
}
