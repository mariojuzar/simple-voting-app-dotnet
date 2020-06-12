using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingService.Models.Requests
{
    public class VotingItemCreationRequest
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public List<Guid> Categories { get; set; }
    }
}
