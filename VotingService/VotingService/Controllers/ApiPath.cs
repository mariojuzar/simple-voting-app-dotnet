using System;

namespace IdentityService.Controllers
{
    public class ApiPath
    {
        private const String BASE_PATH_V1 = "/vote-service/v1";

        public const String ID = "/{id}";

        public const String SEARCH = "/search";

        public const String VOTING_ITEMS = BASE_PATH_V1 + "/voting-items";

        public const String VOTE = BASE_PATH_V1 + "/votes";

        public const String VOTE_SEARCH = BASE_PATH_V1 + "/search";

        public const String CATEGORY = BASE_PATH_V1 + "/categories";
    }
}
