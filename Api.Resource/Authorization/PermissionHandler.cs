using Api.Resource.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Resource.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;
        private readonly UserStore _userStore;

        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor, UserStore userStore)
        {
            _accessor = accessor;
            Schemes = schemes;
            _userStore = userStore;

        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //ids会去认证服务器认证，成功了则为true
            var IsAuth = context.User.Identity.IsAuthenticated; //是否已认证
            if (context.User != null && IsAuth)
            {
                var subjectId = int.Parse(context.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value);

                var user = _userStore.Find(subjectId);
                var claims = context.User.Claims;
                var role = context.User.Claims.Where(x => x.Type == JwtClaimTypes.Role).FirstOrDefault().Value;
                if (context.User.IsInRole(role)) //这里区分大小写
                {
                }
                //if (context.User.IsInRole(user.Role)) //这里区分大小写
                //{
                //}
                if (context.User.IsInRole("admin")) //这里区分大小写
                {
                    context.Succeed(requirement);
                }
                else
                {

                    //判断请求是否拥有凭据，即有没有登录
                    var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                    if (defaultAuthenticate != null)
                    {
                        /*
                          这里注意:
                          RouteEndpoint和HttpContext 获取的路径是不一样的，前缀多了 / 
                          route：api/controller/action
                          path：/api/controller/action
                         */
                        var endpoint = (RouteEndpoint)context.Resource;
                        var route = endpoint.RoutePattern.RawText;
                        var httpContext = _accessor.HttpContext;
                        var path = httpContext.Request.Path;

                        var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                        if (result?.Principal != null)
                        {
                            httpContext.User = result.Principal;
                        }
                        var s = JwtClaimTypes.Scope;
                        var userIdClaim = context.User.FindFirst(_ => _.Type == JwtClaimTypes.Subject);
                        var roleId = int.Parse(context.User.Claims.Where(x => x.Type == "role_id").FirstOrDefault().Value);

                        //判断是否是第三方

                        //判断权限

                        //如果是修改，只能修改自己的，

                        if (userIdClaim != null)
                        {
                            if (_userStore.CheckPermission(subjectId, roleId, route))
                            {
                                context.Succeed(requirement);
                            }
                        }


                        //if (userIdClaim != null)
                        //{
                        //    if (_userStore.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                        //    {
                        //        context.Succeed(requirement);
                        //    }
                        //}


                        // context.Fail();
                    }

                }
            }
            return;
            //return Task.CompletedTask;
        }
    }
}
