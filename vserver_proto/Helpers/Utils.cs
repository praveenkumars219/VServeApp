using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vserver_proto.Helpers
{
    public static class Utils
    {
        public static string GetUrlFromHost(this string host,string scheme,string localUrl)
        {
            return string.IsNullOrWhiteSpace(localUrl) ? scheme + @"://" + host + "/" : scheme + @"://" + host + "/" + localUrl;
        }
    }
}
