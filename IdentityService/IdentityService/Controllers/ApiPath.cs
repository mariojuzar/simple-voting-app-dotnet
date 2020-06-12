using System;

namespace IdentityService.Controllers
{
    public class ApiPath
    {
        private const String BASE_PATH_V1 = "/auth/v1";

        public const String ACCOUNT = BASE_PATH_V1 + "/account";
        public const String ACCOUNT_REGISTER = ACCOUNT + "/register";
        public const String ACCOUNT_LOGIN = ACCOUNT + "/login";
        public const String ACCOUNT_LOGIN_ADMIN = ACCOUNT_LOGIN + "/admin";
        public const String ACCOUNT_LOGUT = ACCOUNT + "/logout";

        public const String PRIVILEGE = BASE_PATH_V1 + "/privilege";
        public const String PRIVILEGE_CHECK = PRIVILEGE + "/check";

        public const String SESSION = BASE_PATH_V1 + "/session";

        public const String GENDER = ACCOUNT + "/gender";
    }
}
