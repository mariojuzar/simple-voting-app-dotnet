using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VotingService.Library.Const;
using VotingService.Library.Exception;
using VotingService.Models.DAL;
using VotingService.Models.Responses;
using VotingService.Services.Interface;

namespace VotingService.Services.Impl
{
    public class VoteSearchService : IVoteSearchService
    {
        voting_dbContext dbContext;

        public VoteSearchService(voting_dbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<VotingItemResponse> GetAllVotingItem()
        {
            List<VotingItems> votingItems = dbContext.VotingItems.ToList();
            List<VotingItemResponse> responses = new List<VotingItemResponse>();

            foreach (VotingItems v in votingItems)
            {
                List<VotingCategories> votingCategories = dbContext.VotingCategories
                    .Where(vc => vc.VotingItemId.Equals(v.VotingItemId))
                    .ToList();
                List<Guid> listCategories = constructListCategoryID(votingCategories);
                List<Categories> categories = dbContext.Categories.Where(cat => listCategories.Contains(cat.CategoryId)).ToList();
                responses.Add(constructResponse(v, categories));
            }

            return responses;
        }

        public VotingItemResponse GetById(Guid id)
        {
            VotingItems votingItem = dbContext.VotingItems.Find(id);
            if (votingItem == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_NOT_EXIST.ToString());
            }

            List<VotingCategories> votingCategories = dbContext.VotingCategories
                .Where(vc => vc.VotingItemId.Equals(votingItem.VotingItemId))
                .ToList();
            List<Guid> listCategories = constructListCategoryID(votingCategories);
            List<Categories> categories = dbContext.Categories.Where(cat => listCategories.Contains(cat.CategoryId)).ToList();

            return constructResponse(votingItem, categories);
        }

        private List<Guid> constructListCategoryID(List<VotingCategories> votingCategories)
        {
            List<Guid> result = new List<Guid>();
            foreach (VotingCategories v in votingCategories)
            {
                result.Add(v.CategoryId);
            }
            return result;
        }

        private VotingItemResponse constructResponse(VotingItems votingItem, List<Categories> categories)
        {
            VotingItemResponse response = new VotingItemResponse();
            response.VotingItemID = votingItem.VotingItemId;
            response.Name = votingItem.Name;
            response.Description = votingItem.Description;
            response.SupportersCount = votingItem.SupportersCount;
            response.DueDate = votingItem.DueDate;
            response.CreatedDate = votingItem.CreatedDate;
            if (response.DueDate < DateTime.Now)
            {
                response.IsExpired = true;
            }
            response.Categories = constructCategoriesResponse(categories);

            return response;
        }

        private List<CategoryResponse> constructCategoriesResponse(List<Categories> categories)
        {
            List<CategoryResponse> responses = new List<CategoryResponse>();
            foreach (Categories c in categories)
            {
                CategoryResponse response = new CategoryResponse();
                response.CategoryId = c.CategoryId;
                response.Name = c.Name;
                responses.Add(response);
            }
            return responses;
        }
    }
}
