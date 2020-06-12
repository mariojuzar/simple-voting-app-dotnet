using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models.Requests
{
    public class PrivilegeRequest
    {
        [Required]
        public Guid SessionID { get; set; }

        [Required]
        public String UrlPath { get; set; }

        [Required]
        public String Method { get; set; }

        [Required]
        public String Prefix { get; set; }
    }
}
