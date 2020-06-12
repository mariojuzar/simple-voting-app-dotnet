using IdentityService.Library.Const;
using IdentityService.Library.Exceptions;
using IdentityService.Library.Helper;
using IdentityService.Library.Kafka;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models.DAL;
using IdentityService.Models.Requests;
using IdentityService.Models.Responses;
using IdentityService.Services.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IdentityService.Services.Impl
{
    public class AccountService : IAccountService
    {
        private const String topicKafka = "identity.service.user";

        auth_dbContext dbContext;

        KafkaProducer kafkaProducer;

        public AccountService(auth_dbContext context, ILoggerManager logger)
        {
            dbContext = context;
            kafkaProducer = new KafkaProducer(topicKafka, logger);
        }

        public AccountResponse LoginAccount(AccountLoginRequest request, Guid SessionID)
        {
            Sessions session = dbContext.Sessions
                .Where(s => s.SessionId.Equals(SessionID) && s.LastActivity < DateTime.Now.AddDays(1))
                .FirstOrDefault();
            if (session == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.INVALID_LOGIN_SESSION.ToString());
            }

            String passHash = PasswordHelper.ConvertToSHA512(request.Password);
            Users user = dbContext.Users
                .Where(u => u.Email.Equals(request.Email))
                .FirstOrDefault();

            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            Users checkPass = dbContext.Users
                .Where(u => u.Email.Equals(request.Email) && u.PasswordHash.Equals(passHash))
                .FirstOrDefault();
            if (checkPass == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.WRONG_COMBINATION_EMAIL_AND_PASSWORD.ToString());
            }

            Genders gender = dbContext.Genders.Find(checkPass.GenderId);

            session.IsLogin = true;
            session.UserId = checkPass.UserId;
            session.LastActivity = DateTime.Now;

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Sessions.Update(session);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_LOGIN.ToString());
                }
            }

            return ConstructResponse(checkPass, gender);
        }

        public AccountResponse LoginAccountAdmin(AccountLoginRequest request, Guid SessionID)
        {
            Sessions session = dbContext.Sessions
                .Where(s => s.SessionId.Equals(SessionID) && s.LastActivity < DateTime.Now.AddDays(1))
                .FirstOrDefault();
            if (session == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.INVALID_LOGIN_SESSION.ToString());
            }

            String passHash = PasswordHelper.ConvertToSHA512(request.Password);
            Users user = dbContext.Users
                .Where(u => u.Email.Equals(request.Email))
                .FirstOrDefault();

            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            Roles role = dbContext.Roles.Where(r => r.Name.Equals("Admin")).FirstOrDefault();

            List<UserRoles> userRoles = dbContext.UserRoles
                .Where(ur => ur.RoleId.Equals(role.RoleId) && ur.UserId.Equals(user.UserId))
                .ToList();

            if (userRoles == null || userRoles.Count == 0)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.INVALID_LOGIN_USER_IS_NOT_ADMIN.ToString());
            } 

            Users checkPass = dbContext.Users
                .Where(u => u.Email.Equals(request.Email) && u.PasswordHash.Equals(passHash))
                .FirstOrDefault();
            if (checkPass == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.WRONG_COMBINATION_EMAIL_AND_PASSWORD.ToString());
            }

            Genders gender = dbContext.Genders.Find(checkPass.GenderId);

            session.IsLogin = true;
            session.UserId = checkPass.UserId;
            session.LastActivity = DateTime.Now;

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Sessions.Update(session);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_LOGIN.ToString());
                }
            }

            return ConstructResponse(checkPass, gender);
        }

        public AccountLogoutResponse LogoutAccount(AccountLogoutRequest request, Guid SessionID)
        {
            Sessions session = dbContext.Sessions
                .Where(s => s.SessionId.Equals(SessionID))
                .FirstOrDefault();
            if (session == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.INVALID_LOGIN_SESSION.ToString());
            }

            session.IsLogin = false;
            session.UserId = null;
            session.LastActivity = DateTime.Now;

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Sessions.Update(session);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_LOGOUT.ToString());
                }
            }

            return new AccountLogoutResponse() { SuccessLogout = true };
        }

        public AccountResponse RegisterAccount(AccountCreationRequest request, Guid SessionID)
        {
            Sessions session = dbContext.Sessions
                .Where(s => s.SessionId.Equals(SessionID) && s.LastActivity < DateTime.Now.AddDays(1))
                .FirstOrDefault();
            if (session == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.INVALID_SESSION.ToString());
            }

            Users checkUser = dbContext.Users
                .Where(u => u.Email.Equals(request.Email))
                .FirstOrDefault();

            if (checkUser != null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_ALREADY_REGISTERED.ToString());
            }

            Genders gender = dbContext.Genders.Find(request.Gender);
            if (gender == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.NOT_VALID_GENDER.ToString());
            }

            Users newUser = new Users();
            newUser.UserId = Guid.NewGuid();
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.Email = request.Email;
            newUser.PasswordHash = PasswordHelper.ConvertToSHA512(request.Password);
            newUser.GenderId = request.Gender;
            newUser.Age = request.Age;
            newUser.CreatedDate = DateTime.Now;
            newUser.CreatedBy = request.Email;
            newUser.UpdatedDate = DateTime.Now;
            newUser.UpdatedBy = request.Email;

            Roles role = dbContext.Roles.Where(r => r.Name.Equals("Client")).FirstOrDefault();
            UserRoles userRoles = new UserRoles();
            userRoles.UserId = newUser.UserId;
            userRoles.RoleId = role.RoleId;

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Users.Add(newUser);
                    dbContext.UserRoles.Add(userRoles);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_REGISTER_ACCOUNT.ToString());
                }
            }

            var task = kafkaProducer.SendToKafka(JsonConvert.SerializeObject(ConstructKafkaRequest(newUser, gender, "CREATE")));
            return ConstructResponse(newUser, gender);
        }

        private AccountResponse ConstructResponse(Users user, Genders gender)
        {
            AccountResponse response = new AccountResponse();
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            response.Gender = gender.Name;
            response.GenderID = user.GenderId;
            response.Age = user.Age;
            return response;
        }

        private KafkaEntityWrapper<KafkaUserRequest> ConstructKafkaRequest(Users user, Genders gender, String actionType)
        {
            KafkaEntityWrapper<KafkaUserRequest> kafkaEntity = new KafkaEntityWrapper<KafkaUserRequest>();
            
            KafkaUserRequest request = new KafkaUserRequest();
            request.userId = user.UserId;
            request.firstName = user.FirstName;
            request.lastName = user.LastName;
            request.email = user.Email;
            request.gender = gender.Name;
            request.genderId = user.GenderId;
            request.age = user.Age;
            request.createdDate = user.CreatedDate;
            request.createdBy = user.CreatedBy;
            request.updatedDate = user.UpdatedDate;
            request.updatedBy = user.UpdatedBy;

            kafkaEntity.actionType = actionType;
            kafkaEntity.data = request;

            return kafkaEntity;
        }
    }
}
