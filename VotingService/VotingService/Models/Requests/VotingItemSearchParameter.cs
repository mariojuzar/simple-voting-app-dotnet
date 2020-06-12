using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VotingService.Models.Requests
{
    public class VotingItemSearchParameter
    {
        public int Page { get; set; } = 0;

        public int Size { get; set; } = 10;

        public String Name { get; set; }

        public List<Guid> Categories { get; set; }
    }
}
