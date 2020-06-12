using System;
using System.Collections.Generic;

namespace VotingService.Models.DAL
{
    public partial class UserVotes
    {
        public Guid UserVoteId { get; set; }
        public Guid UserId { get; set; }
        public Guid VotingItemId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Users User { get; set; }
        public virtual VotingItems VotingItem { get; set; }
    }
}
