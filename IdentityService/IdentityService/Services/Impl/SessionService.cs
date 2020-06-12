using IdentityService.Library.Const;
using IdentityService.Library.Exceptions;
using IdentityService.Models;
using IdentityService.Models.DAL;
using IdentityService.Services.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Net;

namespace IdentityService.Services.Impl
{
    public class SessionService : ISessionService
    {
        auth_dbContext dbContext;

        public SessionService(auth_dbContext context)
        {
            dbContext = context;
        }

        public SessionResponse CreateSession(string IpAddress, string userAgent)
        {
            Sessions session = dbContext.Sessions
                .Where(s => s.IpAddress.Equals(IpAddress) && s.UserAgent.Equals(userAgent) && s.LastActivity < DateTime.Now.AddDays(1))
                .FirstOrDefault();

            if (session != null)
            {
                if (session.IsLogin && session.UserId != null)
                {
                    Users user = dbContext.Users.Find(session.UserId);
                    UpdateLastActivity(session);
                    return constructResponse(session, user);
                }
                UpdateLastActivity(session);
                return constructResponse(session);
            }

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Sessions createdSession = new Sessions();
                    createdSession.SessionId = Guid.NewGuid();
                    createdSession.IpAddress = IpAddress;
                    createdSession.IsLogin = false;
                    createdSession.UserAgent = userAgent;
                    createdSession.LastActivity = DateTime.Now;

                    dbContext.Sessions.Add(createdSession);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    return constructResponse(createdSession);
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_CREATED_SESSION.ToString());
                }
            }

        }

        public SessionResponse GetSession(String IpAddress, String userAgent)
        {
            Sessions session = dbContext.Sessions
                .Where(s => IpAddress == s.IpAddress && userAgent == s.UserAgent && s.LastActivity < DateTime.Now.AddDays(1))
                .FirstOrDefault();

            if (session != null)
            {
                if (session.IsLogin && session.UserId != null)
                {
                    Users user = dbContext.Users.Find(session.UserId);
                    UpdateLastActivity(session);
                    return constructResponse(session, user);
                }
                UpdateLastActivity(session);
                return constructResponse(session);
            }

            throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.SESSION_NOT_EXIST.ToString());
        }

        public void UpdateLastActivity(Sessions session)
        {
            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    session.LastActivity = DateTime.Now;

                    dbContext.Sessions.Update(session);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        private SessionResponse constructResponse(Sessions sessions)
        {
            SessionResponse response = new SessionResponse();
            response.SessionId = sessions.SessionId;
            response.IsLogin = sessions.IsLogin;
            return response;
        }

        private SessionResponse constructResponse(Sessions sessions, Users user)
        {
            SessionResponse response = constructResponse(sessions);
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            return response;
        }
    }
}
