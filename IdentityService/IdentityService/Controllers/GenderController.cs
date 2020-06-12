using System.Collections.Generic;
using System.Net;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using IdentityService.Models.DAL;
using IdentityService.Models.Responses;
using IdentityService.Services.Impl;
using IdentityService.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route(ApiPath.GENDER)]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IGenderService genderService;

        public GenderController(ILoggerManager logger, auth_dbContext dbContext)
        {
            _logger = logger;
            genderService = new GenderService(dbContext);
        }

        [HttpGet]
        public BaseResponse<List<GenderResponse>> GetAll()
        {
            return BaseResponse<List<GenderResponse>>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                genderService.GetAllGenders());
        }
    }
}
