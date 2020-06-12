using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityService.Controllers;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingService.Models.DAL;
using VotingService.Models.Responses;
using VotingService.Services.Impl;
using VotingService.Services.Interface;

namespace VotingService.Controllers
{
    [Route(ApiPath.VOTE_SEARCH)]
    [ApiController]
    public class VoteSearchController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IVoteSearchService searchService;

        public VoteSearchController(ILoggerManager logger, voting_dbContext dbContext)
        {
            _logger = logger;
            searchService = new VoteSearchService(dbContext);
        }

        [HttpGet]
        public BaseResponse<List<VotingItemResponse>> GetAllVotingItems()
        {
            return BaseResponse<List<VotingItemResponse>>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                searchService.GetAllVotingItem());
        }

        [HttpGet]
        [Route(ApiPath.VOTE_SEARCH + ApiPath.ID)]
        public BaseResponse<VotingItemResponse> GetVotingItemById(Guid id)
        {
            return BaseResponse<VotingItemResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                searchService.GetById(id));
        }
    }
}
