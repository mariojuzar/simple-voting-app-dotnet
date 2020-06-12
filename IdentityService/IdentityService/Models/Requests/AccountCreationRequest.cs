using IdentityService.Library.Helper;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.Requests
{
    public class AccountCreationRequest
    {
        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        [ValidEmail]
        public String Email { get; set; }

        [Required]
        [ValidPassword]
        public String Password { get; set; }

        [Required]
        public Guid Gender { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
