using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class Users
    {
        public Users()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid GenderId { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Genders Gender { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
