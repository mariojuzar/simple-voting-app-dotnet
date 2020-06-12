using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VotingService.Library.Const;
using VotingService.Library.Exception;
using VotingService.Library.Pagination;
using VotingService.Models.DAL;
using VotingService.Models.Requests;
using VotingService.Models.Responses;
using VotingService.Services.Interface;

namespace VotingService.Services.Impl
{
    public class VotingItemService : IVotingItemService
    {
        voting_dbContext dbContext;

        public VotingItemService(voting_dbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public VotingItemResponse CreateVotingItem(VotingItemCreationRequest request, Guid UserID)
        {
            if (request.DueDate < DateTime.Now)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.VOTING_ITEM_DUE_DATE_NOT_VALID.ToString());
            }
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    VotingItems votingItem = new VotingItems();
                    votingItem.VotingItemId = Guid.NewGuid();
                    votingItem.Name = request.Name;
                    votingItem.Description = request.Description;
                    votingItem.CreatorUserId = UserID;
                    votingItem.SupportersCount = 0;
                    votingItem.DueDate = request.DueDate;
                    votingItem.CreatedDate = DateTime.Now;
                    votingItem.CreatedBy = user.Email;
                    votingItem.UpdatedDate = DateTime.Now;
                    votingItem.UpdatedBy = user.Email;

                    List<VotingCategories> createdVotingCategories = new List<VotingCategories>();
                    List<Guid> listCategories = new List<Guid>();
                    foreach (Guid id in request.Categories)
                    {
                        Categories categories = dbContext.Categories.Find(id);
                        if (categories != null)
                        {
                            VotingCategories votingCategories = new VotingCategories();
                            votingCategories.VotingCategoryId = Guid.NewGuid();
                            votingCategories.CategoryId = categories.CategoryId;
                            votingCategories.VotingItemId = votingItem.VotingItemId;
                            votingCategories.CreatorUserId = user.UserId;
                            votingCategories.CreatedDate = DateTime.Now;
                            votingCategories.CreatedBy = user.Email;
                            votingCategories.UpdatedDate = DateTime.Now;
                            votingCategories.UpdatedBy = user.Email;
                            createdVotingCategories.Add(votingCategories);
                            listCategories.Add(categories.CategoryId);
                        }
                    }

                    dbContext.VotingItems.Add(votingItem);
                    dbContext.VotingCategories.AddRange(createdVotingCategories);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    List<Categories> categoriesList = dbContext.Categories.Where(cat => listCategories.Contains(cat.CategoryId)).ToList();

                    return constructResponse(votingItem, categoriesList);
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_CREATED_DATA.ToString());
                }
            } 
        }

        public VotingItemResponse DeleteVotingItem(Guid id, Guid UserID)
        {
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            VotingItems votingItem = dbContext.VotingItems.Find(id);
            if (votingItem == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_NOT_EXIST.ToString());
            }

            List<VotingCategories> votingCategories = dbContext.VotingCategories
                .Where(vc => vc.VotingItemId.Equals(votingItem.VotingItemId))
                .ToList();

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    List<UserVotes> votes = dbContext.UserVotes.Where(vote => vote.VotingItemId.Equals(id)).ToList();
                    if (votes.Count > 0)
                    {
                        dbContext.UserVotes.RemoveRange(votes);
                    }
                    
                    if (votingCategories.Count > 0)
                    {
                        dbContext.VotingCategories.RemoveRange(votingCategories);
                    }
                    
                    dbContext.VotingItems.Remove(votingItem);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_DELETE_DATA.ToString());
                }
            }

            List<Guid> listCategories = constructListCategoryID(votingItem.VotingCategories.ToList());
            List<Categories> categories = dbContext.Categories.Where(cat => listCategories.Contains(cat.CategoryId)).ToList();

            return constructResponse(votingItem, categories);
        }

        public VotingItemResponse UpdateVotingItem(VotingItemUpdateRequest request, Guid UserID)
        {
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            VotingItems votingItem = dbContext.VotingItems
                .Where(vi => vi.VotingItemId.Equals(request.VotingItemID) && vi.CreatorUserId.Equals(UserID))
                .FirstOrDefault();
            if (votingItem == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_NOT_EXIST.ToString());
            }

            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    votingItem.Name = request.Name;
                    votingItem.Description = request.Description;
                    votingItem.DueDate = request.DueDate;
                    votingItem.UpdatedDate = DateTime.Now;
                    votingItem.UpdatedBy = user.Email;

                    List<VotingCategories> oldVotingCategories = dbContext.VotingCategories
                        .Where(vc => vc.VotingItemId.Equals(request.VotingItemID) && vc.CreatorUserId.Equals(UserID))
                        .ToList();

                    List<VotingCategories> createdVotingCategories = new List<VotingCategories>();
                    List<Guid> listCategories = new List<Guid>();
                    foreach (Guid id in request.Categories)
                    {
                        Categories categories = dbContext.Categories.Find(id);
                        if (categories != null)
                        {
                            VotingCategories votingCategories = new VotingCategories();
                            votingCategories.VotingCategoryId = Guid.NewGuid();
                            votingCategories.CategoryId = categories.CategoryId;
                            votingCategories.VotingItemId = votingItem.VotingItemId;
                            votingCategories.CreatorUserId = user.UserId;
                            votingCategories.CreatedDate = DateTime.Now;
                            votingCategories.CreatedBy = user.Email;
                            votingCategories.UpdatedDate = DateTime.Now;
                            votingCategories.UpdatedBy = user.Email;
                            createdVotingCategories.Add(votingCategories);
                            listCategories.Add(categories.CategoryId);
                        }
                    }

                    dbContext.VotingCategories.RemoveRange(oldVotingCategories);
                    dbContext.VotingCategories.AddRange(createdVotingCategories);
                    dbContext.VotingItems.Update(votingItem);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    List<Categories> categoriesList = dbContext.Categories.Where(cat => listCategories.Contains(cat.CategoryId)).ToList();

                    return constructResponse(votingItem, categoriesList);
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_UPDATE_DATA.ToString());
                }
            }
        }

        public PagedList<VotingItemResponse> SearchVotingItem(VotingItemSearchParameter searchParameter, Guid UserID)
        {
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            var votingItems = dbContext.VotingItems.Where(vi => vi.CreatorUserId.Equals(UserID));

            if (!String.IsNullOrEmpty(searchParameter.Name))
            {
                votingItems = votingItems.Where(vi => vi.Name.Contains(searchParameter.Name));
            }

            if (searchParameter.Categories != null && searchParameter.Categories.Count > 0)
            {
                List<VotingCategories> votingCategories = dbContext.VotingCategories
                    .Where(vc => vc.CreatorUserId.Equals(UserID) && searchParameter.Categories.Contains(vc.CategoryId))
                    .ToList();
                List<Guid> searchVotingCategories = constructListVotingItemID(votingCategories);

                votingItems = votingItems.Where(vi => searchVotingCategories.Contains(vi.VotingItemId));
            }

            List<VotingItems> result = votingItems.ToList();
            List<VotingItemResponse> responses = new List<VotingItemResponse>();

            foreach (VotingItems votingItem in result)
            {
                List<VotingCategories> votingCategories = dbContext.VotingCategories
                    .Where(vc => vc.VotingItemId.Equals(votingItem.VotingItemId))
                    .ToList();
                List<Guid> listCategories = constructListCategoryID(votingCategories);
                List<Categories> categories = dbContext.Categories.Where(cat => listCategories.Contains(cat.CategoryId)).ToList();

                responses.Add(constructResponse(votingItem, categories));
            }

            return PagedList<VotingItemResponse>.ToPagedList(responses, searchParameter.Page, searchParameter.Size);
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

        public VotingItemResponse GetVotingItem(Guid id, Guid UserID)
        {
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

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

        private List<Guid> constructListVotingItemID(List<VotingCategories> votingCategories)
        {
            List<Guid> result = new List<Guid>();
            foreach (VotingCategories v in votingCategories)
            {
                result.Add(v.VotingItemId);
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
