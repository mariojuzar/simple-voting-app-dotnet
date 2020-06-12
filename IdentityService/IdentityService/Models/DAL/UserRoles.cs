using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class UserRoles
    {
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Roles Role { get; set; }
        public virtual Users User { get; set; }
    }
}
