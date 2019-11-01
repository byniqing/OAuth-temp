using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Resource.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var obj = context.Resource;
                    //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
                    var filterContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext);
                    var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;

                    if (httpContext == null)
                    {
                        httpContext = _accessor.HttpContext;
                    }


                    var userIdClaim = context.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier);
                    //if (userIdClaim != null)
                    //{
                    //    if (_userStore.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                    //    {
                    //        context.Succeed(requirement);
                    //    }
                    //}
                }
            }
            return Task.CompletedTask;
        }
    }
}
