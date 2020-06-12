using System;
using System.Collections.Generic;

namespace IdentityService.Models.DAL
{
    public partial class Genders
    {
        public Genders()
        {
            Users = new HashSet<Users>();
        }

        public Guid GenderId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
