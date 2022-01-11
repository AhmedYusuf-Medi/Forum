using Forum.Service.Common.Exceptions;
using Forum.Service.Common.Message;

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Forum.WebApi.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class , AllowMultiple = true)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public AuthorizationAttribute(string[] roles)
        {
            this.Roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Request.Cookies["Role"];

            if (!this.Roles.Contains(role))
            {
                throw new UnAuthorizationException(Message.ExceptionMessages.Unauthorized);
            }
        }

        public string[] Roles { get; set; }
    }
}