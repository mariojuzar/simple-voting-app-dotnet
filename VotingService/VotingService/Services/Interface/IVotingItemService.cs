using System;
using System.Collections.Generic;
using VotingService.Library.Pagination;
using VotingService.Models.Requests;
using VotingService.Models.Responses;

namespace VotingService.Services.Interface
{
    public interface IVotingItemService
    {
        VotingItemResponse CreateVotingItem(VotingItemCreationRequest request, Guid UserID);

        VotingItemResponse UpdateVotingItem(VotingItemUpdateRequest request, Guid UserID);

        VotingItemResponse DeleteVotingItem(Guid id, Guid UserID);

        VotingItemResponse GetVotingItem(Guid id, Guid UserID);

        List<VotingItemResponse> GetAllVotingItem();

        PagedList<VotingItemResponse> SearchVotingItem(VotingItemSearchParameter searchParameter, Guid UserID); 
    }
}
