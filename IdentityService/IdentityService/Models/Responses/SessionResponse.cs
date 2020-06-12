using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class SessionResponse
    {
        public Guid SessionId { get; set; }
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public Boolean IsLogin { get; set; } = false;
    }
}
