using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Responses
{
    public class PrivilegeResponse
    {
        public Boolean IsAllowed { get; set; }

        public Guid UserID { get; set; }
    }
}
