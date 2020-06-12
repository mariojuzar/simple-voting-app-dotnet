using IdentityService.Models.DAL;
using IdentityService.Models.Responses;
using IdentityService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Services.Impl
{
    public class GenderService : IGenderService
    {
        auth_dbContext dbContext;

        public GenderService(auth_dbContext context)
        {
            dbContext = context;
        }

        public List<GenderResponse> GetAllGenders()
        {
            List<Genders> genders = dbContext.Genders.ToList();

            List<GenderResponse> responses = new List<GenderResponse>();
            foreach (Genders g in genders)
            {
                GenderResponse response = new GenderResponse();
                response.GenderId = g.GenderId;
                response.Name = g.Name;
                responses.Add(response);
            }

            return responses;
        }
    }
}
