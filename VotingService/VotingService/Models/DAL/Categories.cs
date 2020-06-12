using System;
using System.Collections.Generic;

namespace VotingService.Models.DAL
{
    public partial class Categories
    {
        public Categories()
        {
            VotingCategories = new HashSet<VotingCategories>();
        }

        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Users CreatorUser { get; set; }
        public virtual ICollection<VotingCategories> VotingCategories { get; set; }
    }
}
