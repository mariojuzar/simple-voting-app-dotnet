using IdentityService.Models;
using IdentityService.Models.DAL;
using System;

namespace IdentityService.Services.Interface
{
    public interface ISessionService
    {
        SessionResponse CreateSession(String IpAddress, String userAgent);

        SessionResponse GetSession(String IpAddress, String userAgent);

        void UpdateLastActivity(Sessions session);
    }
}
