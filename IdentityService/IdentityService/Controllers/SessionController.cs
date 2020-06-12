using System;
using System.Net;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using IdentityService.Models.DAL;
using IdentityService.Services.Impl;
using IdentityService.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UAParser;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route(ApiPath.SESSION)]
    public class SessionController : Controller
    {
        private readonly ILoggerManager _logger;

        private readonly ISessionService sessionService;

        Parser uaParser = Parser.GetDefault();

        public SessionController(ILoggerManager logger, auth_dbContext context)
        {
            _logger = logger;
            sessionService = new SessionService(context);
        }

        [HttpGet]
        public BaseResponse<SessionResponse> GetSessions()
        {
            String remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            String ua = Request.Headers["User-Agent"].ToString();
            ClientInfo c = uaParser.Parse(ua);
            String userAgent = constructUserAgent(c, ua);
            
            SessionResponse response = sessionService.GetSession(remoteIpAddress, userAgent);
            HttpContext.Response.Cookies.Append(
                "SessionID",
                response.SessionId.ToString(),
                new CookieOptions() { Path = "/", MaxAge = DateTime.Now.AddDays(1).TimeOfDay}
                );

            return BaseResponse<SessionResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                response);
        }

        [HttpPost]
        public BaseResponse<SessionResponse> CreateSessions()
        {
            String remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            String ua = Request.Headers["User-Agent"].ToString();
            ClientInfo c = uaParser.Parse(Request.Headers["User-Agent"].ToString());
            String userAgent = constructUserAgent(c, ua);

            SessionResponse response = sessionService.CreateSession(remoteIpAddress, userAgent);
            HttpContext.Response.Cookies.Append(
                "SessionID",
                response.SessionId.ToString(),
                new CookieOptions() { Path = "/", MaxAge = DateTime.Now.AddDays(1).TimeOfDay }
                );

            return BaseResponse<SessionResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                response);
        }

        private String constructUserAgent(ClientInfo clientInfo, String userAgent)
        {
            return clientInfo.UA.Family + " v" + clientInfo.UA.Major + "." + clientInfo.UA.Minor + "." + clientInfo.UA.Patch + " " +
                clientInfo.OS.Family + " v" + clientInfo.OS.Major + "." + clientInfo.OS.Minor + "." + clientInfo.OS.Patch + " " +
                clientInfo.Device.Family + " " + clientInfo.Device.Brand + clientInfo.Device.Model + clientInfo.Device.IsSpider + "/" + userAgent;
        }
    }
}
