using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Responses
{
    public class AccountResponse
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String Gender { get; set; }

        public Guid GenderID { get; set; }

        public int Age { get; set; }
    }
}
