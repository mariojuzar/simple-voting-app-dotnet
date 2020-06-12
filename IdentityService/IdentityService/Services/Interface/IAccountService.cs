using IdentityService.Models.Requests;
using IdentityService.Models.Responses;
using System;

namespace IdentityService.Services.Interface
{
    public interface IAccountService
    {
        AccountResponse RegisterAccount(AccountCreationRequest request, Guid SessionID);

        AccountResponse LoginAccount(AccountLoginRequest request, Guid SessionID);

        AccountResponse LoginAccountAdmin(AccountLoginRequest request, Guid SessionID);

        AccountLogoutResponse LogoutAccount(AccountLogoutRequest request, Guid SessionID);
    }
}
