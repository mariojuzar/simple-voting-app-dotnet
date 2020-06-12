using ApiGatewayService.Library.Exceptions;
using IdentityService.Library.Logger.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace IdentityService.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        if (contextFeature.Error.GetType().Name.Equals("BusinessLogicException"))
                        {
                            BusinessLogicException businessLogicException = (BusinessLogicException)contextFeature.Error.GetBaseException();
                            await context.Response.WriteAsync(BaseResponse<String>.ConstructResponse(
                                Status: businessLogicException.Code,
                                Message: businessLogicException.ErrorMessage,
                                null
                            ).ToString());
                        }
                        else
                        {
                            await context.Response.WriteAsync(BaseResponse<String>.ConstructResponse(
                                Status: HttpStatusCode.InternalServerError,
                                Message: "Internal Server Error",
                                null
                            ).ToString());
                        }
                    }
                });
            });
        }
    }
}
