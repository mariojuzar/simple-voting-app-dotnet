using System;
using System.Net;
using IdentityService.Controllers;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using VotingService.Models.DAL;
using VotingService.Models.Requests;
using VotingService.Models.Responses;
using VotingService.Services.Impl;
using VotingService.Services.Interface;

namespace VotingService.Controllers
{
    [Route(ApiPath.VOTE)]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IVoteService voteService;

        public VoteController(ILoggerManager logger, voting_dbContext dbContext)
        {
            _logger = logger;
            voteService = new VoteService(dbContext);
        }

        [HttpPost]
        public BaseResponse<VotingItemResponse> CastVote([FromBody] VoteRequest request, [FromHeader] Guid UserID)
        {
            return BaseResponse<VotingItemResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                voteService.VoteForItem(request, UserID));
        }
    }
}
