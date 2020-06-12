using System;
using System.Collections.Generic;

namespace VotingService.Models.DAL
{
    public partial class Users
    {
        public Users()
        {
            Categories = new HashSet<Categories>();
            UserVotes = new HashSet<UserVotes>();
            VotingCategories = new HashSet<VotingCategories>();
            VotingItems = new HashSet<VotingItems>();
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid GenderId { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Categories> Categories { get; set; }
        public virtual ICollection<UserVotes> UserVotes { get; set; }
        public virtual ICollection<VotingCategories> VotingCategories { get; set; }
        public virtual ICollection<VotingItems> VotingItems { get; set; }
    }
}
