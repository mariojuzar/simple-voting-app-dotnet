using IdentityService.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Services.Interface
{
    interface IGenderService
    {
        List<GenderResponse> GetAllGenders();
    }
}
