using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VotingService.Models.Requests
{
    public class CategoryCreationRequest
    {
        [Required]
        public String Name { get; set; }

    }
}
