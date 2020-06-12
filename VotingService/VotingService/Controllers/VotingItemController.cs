using System;
using System.Collections.Generic;
using System.Net;
using IdentityService.Controllers;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using VotingService.Library.Pagination;
using VotingService.Models.DAL;
using VotingService.Models.Requests;
using VotingService.Models.Responses;
using VotingService.Services.Impl;
using VotingService.Services.Interface;

namespace VotingService.Controllers
{
    [Route(ApiPath.VOTING_ITEMS)]
    [ApiController]
    public class VotingItemController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IVotingItemService votingItemService;

        public VotingItemController(ILoggerManager logger, voting_dbContext dbContext)
        {
            _logger = logger;
            votingItemService = new VotingItemService(dbContext);
        }

        [HttpPost]
        public BaseResponse<VotingItemResponse> CreateVotingItem([FromBody] VotingItemCreationRequest request, [FromHeader] Guid UserID)
        {
            return BaseResponse<VotingItemResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                votingItemService.CreateVotingItem(request, UserID));
        }

        [HttpPut]
        public BaseResponse<VotingItemResponse> UpdateVotingItem([FromBody] VotingItemUpdateRequest request, [FromHeader] Guid UserID)
        {
            return BaseResponse<VotingItemResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                votingItemService.UpdateVotingItem(request, UserID));
        }

        [HttpDelete]
        [Route(ApiPath.VOTING_ITEMS + ApiPath.ID)]
        public BaseResponse<VotingItemResponse> DeleteVotingItem(Guid id, [FromHeader] Guid UserID)
        {
            return BaseResponse<VotingItemResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                votingItemService.DeleteVotingItem(id, UserID));
        }

        [HttpGet]
        [Route(ApiPath.VOTING_ITEMS + ApiPath.ID)]
        public BaseResponse<VotingItemResponse> GetVotingItem(Guid id, [FromHeader] Guid UserID)
        {
            return BaseResponse<VotingItemResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                votingItemService.GetVotingItem(id, UserID));
        }

        [HttpGet]
        public BaseResponse<List<VotingItemResponse>> GetAllVotingItem()
        {
            return BaseResponse<List<VotingItemResponse>>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                votingItemService.GetAllVotingItem());
        }

        [HttpGet]
        [Route(ApiPath.VOTING_ITEMS + ApiPath.SEARCH)]
        public BaseResponse<PagingResponse<VotingItemResponse>> SearchVotingItem(
            [FromQuery] VotingItemSearchParameter parameter, [FromHeader] Guid UserID)
        {
            return BaseResponse<PagingResponse<VotingItemResponse>>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                constructPagingResponse(votingItemService.SearchVotingItem(parameter, UserID)));
        }

        private PagingResponse<VotingItemResponse> constructPagingResponse(PagedList<VotingItemResponse> result)
        {
            PagingResponse<VotingItemResponse> response = new PagingResponse<VotingItemResponse>();
            response.CurrentPage = result.CurrentPage;
            response.PageSize = result.PageSize;
            response.TotalCount = result.TotalCount;
            response.TotalPages = result.TotalPages;
            response.Result = result;
            return response;
        }
    }
}
