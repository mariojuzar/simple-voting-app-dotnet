using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class Privileges
    {
        public Privileges()
        {
            RolePrivileges = new HashSet<RolePrivileges>();
        }

        public Guid PrivilegeId { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Prefix { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<RolePrivileges> RolePrivileges { get; set; }
    }
}
