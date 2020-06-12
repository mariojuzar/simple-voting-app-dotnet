using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VotingService.Models.Requests
{
    public class VotingItemUpdateRequest
    {
        [Required]
        public Guid VotingItemID { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public List<Guid> Categories { get; set; }
    }
}

