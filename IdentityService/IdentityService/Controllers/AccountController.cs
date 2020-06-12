using System;
using System.Collections.Generic;
using System.Linq;
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
    [ApiController]
    [Route(ApiPath.ACCOUNT)]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IAccountService accountService;

        public AccountController(ILoggerManager logger, auth_dbContext context)
        {
            _logger = logger;
            accountService = new AccountService(context, logger);
        }

        [HttpPost]
        [Route(ApiPath.ACCOUNT_REGISTER)]
        public BaseResponse<AccountResponse> RegisterAccount([FromBody] AccountCreationRequest request, [FromHeader] Guid SessionID)
        {
            return BaseResponse<AccountResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                accountService.RegisterAccount(request, SessionID));
        }

        [HttpPost]
        [Route(ApiPath.ACCOUNT_LOGIN)]
        public BaseResponse<AccountResponse> LoginAccount([FromBody] AccountLoginRequest request, [FromHeader] Guid SessionID)
        {
            return BaseResponse<AccountResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                accountService.LoginAccount(request, SessionID));
        }

        [HttpPost]
        [Route(ApiPath.ACCOUNT_LOGIN_ADMIN)]
        public BaseResponse<AccountResponse> LoginAccountAdmin([FromBody] AccountLoginRequest request, [FromHeader] Guid SessionID)
        {
            return BaseResponse<AccountResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                accountService.LoginAccountAdmin(request, SessionID));
        }

        [HttpPost]
        [Route(ApiPath.ACCOUNT_LOGUT)]
        public BaseResponse<AccountLogoutResponse> LogoutAccount([FromBody] AccountLogoutRequest request, [FromHeader] Guid SessionID)
        {
            return BaseResponse<AccountLogoutResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                accountService.LogoutAccount(request, SessionID));
        }

    }
}
