using Forum.Service.Common.Exceptions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace Forum.WebApi.Helpers
{
    public static class ExceptionHandler
    {
        public static Action<IApplicationBuilder> HandleExceptions()
        {
            return options =>
            {
                options.Run(
                async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var exceptionContext = context.Features.Get<IExceptionHandlerFeature>();

                    var exception = exceptionContext.Error.GetType().Name;

                    if (exception.Equals(nameof(BadRequestException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else if (exception.Equals(nameof(UnAuthorizationException)))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }

                    if (exceptionContext != null)
                    {
                        var err = $"Exception: {exceptionContext.Error.Message}";
                        await context.Response.WriteAsync(err);
                    }
                });
            };
        }
    }
}