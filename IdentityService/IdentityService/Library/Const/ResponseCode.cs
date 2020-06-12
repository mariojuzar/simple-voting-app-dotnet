using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Library.Const
{
    public enum ResponseCode
    {
        SESSION_NOT_EXIST,
        FAILED_CREATED_SESSION,
        USER_NOT_EXIST,
        INVALID_LOGIN_USER_IS_NOT_ADMIN,
        WRONG_COMBINATION_EMAIL_AND_PASSWORD,
        INVALID_LOGIN_SESSION,
        INVALID_SESSION,
        FAILED_TO_LOGIN,
        FAILED_TO_LOGOUT,
        FAILED_TO_REGISTER_ACCOUNT,
        USER_ALREADY_REGISTERED,
        NOT_VALID_GENDER
    }
}
