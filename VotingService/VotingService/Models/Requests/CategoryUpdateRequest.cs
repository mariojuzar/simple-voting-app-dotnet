using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VotingService.Models
{
    public class CategoryUpdateRequest
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
