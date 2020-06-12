using System;
using System.Collections.Generic;
using VotingService.Models;
using VotingService.Models.Requests;
using VotingService.Models.Responses;

namespace VotingService.Services.Interface
{
    public interface ICategoryService
    {
        CategoryResponse CreateCategory(CategoryCreationRequest request, Guid UserID);

        CategoryResponse GetCategory(Guid CategoryID, Guid UserID);

        CategoryResponse UpdateCategory(CategoryUpdateRequest updateRequest, Guid UserID);

        List<CategoryResponse> GetAllCategories(Guid UserID);

        CategoryResponse DeleteCategory(Guid CategoryID, Guid UserID);
    }
}
