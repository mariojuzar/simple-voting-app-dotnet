using System;
using System.Collections.Generic;
using System.Net;
using IdentityService.Controllers;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using VotingService.Models;
using VotingService.Models.DAL;
using VotingService.Models.Requests;
using VotingService.Models.Responses;
using VotingService.Services.Impl;
using VotingService.Services.Interface;

namespace VotingService.Controllers
{
    [Route(ApiPath.CATEGORY)]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly ICategoryService categoryService;

        public CategoryController(ILoggerManager logger, voting_dbContext dbContext)
        {
            _logger = logger;
            categoryService = new CategoryService(dbContext);
        }

        [HttpPost]
        public BaseResponse<CategoryResponse> CreateCategory([FromBody] CategoryCreationRequest request, [FromHeader] Guid UserID)
        {
            return BaseResponse<CategoryResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                categoryService.CreateCategory(request, UserID));
        }

        [HttpPut]
        public BaseResponse<CategoryResponse> UpdateCategory([FromBody] CategoryUpdateRequest request, [FromHeader] Guid UserID)
        {
            return BaseResponse<CategoryResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                categoryService.UpdateCategory(request, UserID));
        }

        [HttpGet]
        public BaseResponse<List<CategoryResponse>> GetAll([FromHeader] Guid UserID)
        {
            return BaseResponse<List<CategoryResponse>>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                categoryService.GetAllCategories(UserID));
        }

        [HttpGet]
        [Route(ApiPath.CATEGORY + ApiPath.ID)]
        public BaseResponse<CategoryResponse> GetCategory(Guid id, [FromHeader] Guid UserID)
        {
            return BaseResponse<CategoryResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                categoryService.GetCategory(id, UserID));
        }

        [HttpDelete]
        [Route(ApiPath.CATEGORY + ApiPath.ID)]
        public BaseResponse<CategoryResponse> DeleteCategory(Guid id, [FromHeader] Guid UserID)
        {
            return BaseResponse<CategoryResponse>.ConstructResponse(
                HttpStatusCode.OK,
                HttpStatusCode.OK.ToString(),
                categoryService.DeleteCategory(id, UserID));
        }

    }
}
