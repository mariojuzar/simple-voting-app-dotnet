using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class Roles
    {
        public Roles()
        {
            RolePrivileges = new HashSet<RolePrivileges>();
            UserRoles = new HashSet<UserRoles>();
        }

        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<RolePrivileges> RolePrivileges { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
