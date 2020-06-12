using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VotingService.Library.Const;
using VotingService.Library.Exception;
using VotingService.Models;
using VotingService.Models.DAL;
using VotingService.Models.Requests;
using VotingService.Models.Responses;
using VotingService.Services.Interface;

namespace VotingService.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        voting_dbContext dbContext;

        public CategoryService(voting_dbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CategoryResponse CreateCategory(CategoryCreationRequest request, Guid UserID)
        {
            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Categories category = dbContext.Categories
                        .Where(c => c.Name.Equals(request.Name) && c.CreatorUserId.Equals(UserID)).FirstOrDefault();
                    Users user = dbContext.Users.Find(UserID);
                    
                    if (category != null)
                    {
                        throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_ALREADY_EXIST.ToString());
                    }
                    if (user == null)
                    {
                        throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
                    }

                    Categories createdCategory = new Categories();

                    createdCategory.Name = request.Name;
                    createdCategory.CreatorUserId = UserID;
                    createdCategory.CreatedDate = DateTime.Now;
                    createdCategory.CreatedBy = user.Email;
                    createdCategory.UpdatedDate = DateTime.Now;
                    createdCategory.UpdatedBy = user.Email;

                    dbContext.Categories.Add(createdCategory);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    CategoryResponse response = new CategoryResponse();
                    response.CategoryId = createdCategory.CategoryId;
                    response.Name = createdCategory.Name;
                    return response;
                }
                catch (BusinessLogicException e)
                {
                    transaction.Rollback();
                    throw e;
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_CREATED_DATA.ToString());
                }
            }
        }

        public CategoryResponse DeleteCategory(Guid CategoryID, Guid UserID)
        {
            using(IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Categories category = dbContext.Categories
                        .Where(c => c.CategoryId.Equals(CategoryID) && c.CreatorUserId.Equals(UserID)).FirstOrDefault();

                    Users user = dbContext.Users.Find(UserID);

                    if (category == null)
                    {
                        throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_NOT_EXIST.ToString());
                    }
                    if (user == null)
                    {
                        throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
                    }

                    dbContext.Categories.Remove(category);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    CategoryResponse response = new CategoryResponse();
                    response.CategoryId = category.CategoryId;
                    response.Name = category.Name;
                    return response;
                }
                catch (BusinessLogicException e)
                {
                    transaction.Rollback();
                    throw e;
                }
                catch 
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_DELETE_DATA.ToString());
                }
            }
        }

        public List<CategoryResponse> GetAllCategories(Guid UserID)
        {
            List<Categories> categories = dbContext.Categories.Where(c => c.CreatorUserId.Equals(UserID)).ToList();
            Users user = dbContext.Users.Find(UserID);
            if (user == null)
            {
                throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
            }

            List<CategoryResponse> result = new List<CategoryResponse>();
            foreach (Categories category in categories)
            {
                CategoryResponse response = new CategoryResponse();
                response.CategoryId = category.CategoryId;
                response.Name = category.Name;
                result.Add(response);
            }
            return result;
        }

        public CategoryResponse GetCategory(Guid CategoryID, Guid UserID)
        {
            Categories category = dbContext.Categories.Where(c => c.CategoryId.Equals(CategoryID) && c.CreatorUserId.Equals(UserID)).FirstOrDefault();
            Users user = dbContext.Users.Find(UserID);
            if (category != null && user != null)
            {
                return new CategoryResponse() { CategoryId = category.CategoryId, Name = category.Name };
            }
            throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_NOT_EXIST.ToString());
        }

        public CategoryResponse UpdateCategory(CategoryUpdateRequest updateRequest, Guid UserID)
        {
            using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Categories category = dbContext.Categories
                        .Where(c => c.CategoryId.Equals(updateRequest.CategoryId) && c.CreatorUserId.Equals(UserID)).FirstOrDefault();

                    Users user = dbContext.Users.Find(UserID);

                    if (category == null)
                    {
                        throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.DATA_NOT_EXIST.ToString());
                    }
                    if (user == null)
                    {
                        throw new BusinessLogicException(HttpStatusCode.BadRequest, ResponseCode.USER_NOT_EXIST.ToString());
                    }

                    category.Name = updateRequest.Name;
                    category.UpdatedDate = DateTime.Now;
                    category.UpdatedBy = user.Email;

                    dbContext.Update(category);
                    dbContext.SaveChanges();
                    transaction.Commit();

                    CategoryResponse response = new CategoryResponse();
                    response.CategoryId = category.CategoryId;
                    response.Name = category.Name;
                    return response;
                }
                catch (BusinessLogicException e)
                {
                    transaction.Rollback();
                    throw e;
                }
                catch
                {
                    transaction.Rollback();
                    throw new BusinessLogicException(HttpStatusCode.InternalServerError, ResponseCode.FAILED_TO_UPDATE_DATA.ToString());
                }
            }
        }
    }
}
