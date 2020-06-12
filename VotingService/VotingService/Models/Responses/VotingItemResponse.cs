using System;
using System.Collections.Generic;

namespace VotingService.Models.Responses
{
    public class VotingItemResponse
    {
        public Guid VotingItemID { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public int SupportersCount { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<CategoryResponse> Categories { get; set; }

        public Boolean IsExpired { get; set; } = false;
    }
}
