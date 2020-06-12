using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VotingService.Library.Const;
using VotingService.Library.Exception;
using VotingService.Models.DAL;
using VotingService.Models.Requests;
using VotingService.Models.Responses;
using VotingService.Services.Interface;

namespace VotingService.Services.Impl
{
    public class VoteService : IVoteService
    {
        voting_dbContext dbContext;

        public VoteService(voting_dbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public VotingItemResponse VoteForItem(VoteRequest request, Guid UserID)
        {
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            VotingItems votingItem = dbContext.VotingItems
                .Where(v => v.VotingItemId.Equals(request.VotingItemID) && v.DueDate > DateTime.Now).FirstOrDefault();
            if (votingItem == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.VOTING_ITEM_NOT_EXIST_OR_EXPIRED.ToString());
            }

            UserVotes userVote = dbContext.UserVotes
                .Where(us => us.UserId.Equals(UserID) && us.VotingItemId.Equals(request.VotingItemID)).FirstOrDefault();

            if (userVote != null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_ALREADY_VOTED.ToString());
            }

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    UserVotes newVotes = new UserVotes();
                    newVotes.UserId = UserID;
                    newVotes.VotingItemId = request.VotingItemID;
                    newVotes.CreatedBy = user.Email;
                    newVotes.UpdatedBy = user.Email;

                    votingItem.SupportersCount += 1;

                    dbContext.UserVotes.Add(newVotes);
                    dbContext.VotingItems.Update(votingItem);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_VOTE.ToString());
                }
            }

            List<Guid> listCategories = constructListCategoryID(votingItem.VotingCategories.ToList());
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
            if (DateTime.Now < response.DueDate)
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
