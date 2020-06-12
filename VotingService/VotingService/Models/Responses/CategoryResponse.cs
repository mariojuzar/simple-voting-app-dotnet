using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingService.Models.Responses
{
    public class CategoryResponse
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
    }
}
