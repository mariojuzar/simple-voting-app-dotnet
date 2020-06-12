using System.Net;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using IdentityService.Models.DAL;
using IdentityService.Models.Requests;
using IdentityService.Models.Responses;
using IdentityService.Services.Impl;
using IdentityService.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route(ApiPath.PRIVILEGE)]
    [ApiController]
    public class PrivilegeController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IPrivilegeService privilegeService;

        public PrivilegeController(ILoggerManager logger, auth_dbContext context)
        {
            _logger = logger;
            privilegeService = new PrivilegeService(context);
        }

        [HttpPost]
        [Route(ApiPath.PRIVILEGE_CHECK)]
        public BaseResponse<PrivilegeResponse> CheckPrivilege([FromBody] PrivilegeRequest request)
        {
            return BaseResponse<PrivilegeResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                privilegeService.CheckPrivilige(request));
        }
    }
}
