using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingService.Models.Responses;

namespace VotingService.Services.Interface
{
    interface IVoteSearchService
    {
        List<VotingItemResponse> GetAllVotingItem();

        VotingItemResponse GetById(Guid id);
    }
}
