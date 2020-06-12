using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class RolePrivileges
    {
        public Guid RolePrivilegeId { get; set; }
        public Guid PriviliegeId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual Privileges Priviliege { get; set; }
        public virtual Roles Role { get; set; }
    }
}
