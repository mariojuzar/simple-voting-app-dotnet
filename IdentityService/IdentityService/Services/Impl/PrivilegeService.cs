using IdentityService.Models.DAL;
using IdentityService.Models.Requests;
using IdentityService.Models.Responses;
using IdentityService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityService.Services.Impl
{
    public class PrivilegeService : IPrivilegeService
    {
        auth_dbContext dbContext;

        public PrivilegeService(auth_dbContext context)
        {
            dbContext = context;
        }

        public PrivilegeResponse CheckPrivilige(PrivilegeRequest request)
        {
            Sessions sessions = dbContext.Sessions.Find(request.SessionID);

            if (sessions == null || !sessions.IsLogin)
            {
                return new PrivilegeResponse() { IsAllowed = false };
            }

            Users user = dbContext.Users.Find(sessions.UserId);

            if (user == null)
            {
                return new PrivilegeResponse() { IsAllowed = false };
            }

            List<Guid> userRoles = dbContext.UserRoles
                .Where(ur => ur.UserId.Equals(user.UserId))
                .Select(s => s.RoleId)
                .ToList();

            if (userRoles == null || userRoles.Count == 0)
            {
                return new PrivilegeResponse() { IsAllowed = false };
            }

            if (IsIncludeAdmin(userRoles)) return new PrivilegeResponse() { IsAllowed = true, UserID = user.UserId };

            List<Guid> rolePrivileges = dbContext.RolePrivileges
                .Where(rp => userRoles.Contains(rp.RoleId))
                .Select(rp => rp.PriviliegeId)
                .ToList();

            Privileges privileges = dbContext.Privileges
                .Where(p => p.Path.Equals(request.UrlPath) && p.Method.Equals(request.Method) 
                    && p.Prefix.Equals(request.Prefix) && rolePrivileges.Contains(p.PrivilegeId))
                .FirstOrDefault();

            if (privileges == null)
            {
                return new PrivilegeResponse() { IsAllowed = false };
            }

            return new PrivilegeResponse() { IsAllowed = true, UserID = user.UserId };
        }

        private bool IsIncludeAdmin(List<Guid> userRoles)
        {
            Roles adminRole = dbContext.Roles.Where(r => r.Name.Equals("Admin")).FirstOrDefault();
            
            foreach (Guid ur in userRoles)
            {
                if (ur.Equals(adminRole.RoleId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
