using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.Request
{
    public class PrivilegeRequest
    {
        public Guid SessionID { get; set; }
        public String UrlPath { get; set; }
        public String Method { get; set; }
        public String Prefix { get; set; }

    }
}
