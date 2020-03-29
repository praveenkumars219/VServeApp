using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vserver_proto.Entities
{
    public class LoginModel
    {
        public String UserId { get; set; }
        public String Password { get; set; }
        public String BaseUrl { get; set; }
    }
}
