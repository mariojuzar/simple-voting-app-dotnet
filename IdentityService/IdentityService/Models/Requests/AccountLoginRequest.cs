using IdentityService.Library.Helper;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.Requests
{
    public class AccountLoginRequest
    {
        [Required]
        [ValidEmail]
        public String Email { get; set; }

        [Required]
        [ValidPassword]
        public String Password { get; set; }
    }
}
