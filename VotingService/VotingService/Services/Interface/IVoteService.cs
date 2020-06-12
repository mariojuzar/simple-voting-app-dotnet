using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingService.Models.Requests;
using VotingService.Models.Responses;

namespace VotingService.Services.Interface
{
    public interface IVoteService
    {
        VotingItemResponse VoteForItem(VoteRequest request, Guid UserID);
    }
}
