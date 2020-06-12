using System;
using System.Collections.Generic;

namespace VotingService.Models.DAL
{
    public partial class VotingCategories
    {
        public Guid VotingCategoryId { get; set; }
        public Guid VotingItemId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Users CreatorUser { get; set; }
        public virtual VotingItems VotingItem { get; set; }
    }
}
