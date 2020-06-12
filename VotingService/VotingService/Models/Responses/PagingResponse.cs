using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingService.Models.Responses
{
    public class PagingResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public List<T> Result { get; set; }
    }
}
