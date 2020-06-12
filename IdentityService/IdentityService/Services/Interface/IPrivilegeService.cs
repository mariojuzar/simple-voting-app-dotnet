using IdentityService.Models.Requests;
using IdentityService.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Services.Interface
{
    public interface IPrivilegeService
    {
        PrivilegeResponse CheckPrivilige(PrivilegeRequest request);
    }
}
