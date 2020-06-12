using System;
using System.Collections.Generic;

namespace VotingService.Models.DAL
{
    public partial class VotingItems
    {
        public VotingItems()
        {
            UserVotes = new HashSet<UserVotes>();
            VotingCategories = new HashSet<VotingCategories>();
        }

        public Guid VotingItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CreatorUserId { get; set; }
        public int SupportersCount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Users CreatorUser { get; set; }
        public virtual ICollection<UserVotes> UserVotes { get; set; }
        public virtual ICollection<VotingCategories> VotingCategories { get; set; }
    }
}
