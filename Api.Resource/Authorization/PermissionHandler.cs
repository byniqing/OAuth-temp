using Api.Resource.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Api.Resource.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        /// <summary>
        /// 可以获取上下文
        /// </summary>
        private readonly IHttpContextAccessor _accessor;
        private readonly UserStore _userStore;
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor, UserStore userStore)
        {
            _accessor = accessor;
            Schemes = schemes;
            _userStore = userStore;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //获取上下文
            var httpContext = _accessor.HttpContext;

            //ids会去认证服务器认证，成功了则为true
            var IsAuth = context.User.Identity.IsAuthenticated; //是否已认证
            if (context.User != null && IsAuth)
            {
                /*
                 如果是获取用户，token里面的id和接口中传参数的id必须一致
                 因为自己只能看自己的数据，这里要怎么做？？
                 接口一个一个判断不方便，这里能否获取参数中的id？？
                 */
                var claims = context.User.Claims;

                var subjectId = int.Parse(claims.Where(x => x.Type == "sub").FirstOrDefault().Value);
                var user = _userStore.Find(subjectId);
                var endpoint = (RouteEndpoint)context.Resource;

                //这里可以控制器相关信息
                var metadata = endpoint.Metadata;
                var descriptor = metadata.GetMetadata<ControllerActionDescriptor>();
                var actionName = descriptor.ActionName;

                var route1 = descriptor.AttributeRouteInfo.Template; //同样可以获取
                var route = endpoint.RoutePattern.RawText;
                var IsInAction = claims.Any(_ => _.Type == "action" && _.Value.ToLower() == route.ToLower());

                //用户会有多个角色，不能只获取第一个，所以直接判断是否办好admin 角色
                var IsInRole = claims.Any(x => x.Type == JwtClaimTypes.Role && x.Value.ToLower() == "admin");
                if (IsInRole || IsInAction)
                {
                    context.Succeed(requirement);
                }
                else context.Fail();
                //if (context.User.IsInRole("admin")) //这里区分大小写,如果是Admin ,则是false
                //{
                //    context.Succeed(requirement);
                //}
            }
            else context.Fail();
            return Task.CompletedTask;
        }
        //protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        //{
        //    //ids会去认证服务器认证，成功了则为true
        //    var IsAuth = context.User.Identity.IsAuthenticated; //是否已认证
        //    if (context.User != null && IsAuth)
        //    {
        //        var subjectId = int.Parse(context.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value);

        //        var user = _userStore.Find(subjectId);
        //        var claims = context.User.Claims;
        //        var role = context.User.Claims.Where(x => x.Type == JwtClaimTypes.Role).FirstOrDefault().Value;
        //        if (context.User.IsInRole(role)) //这里区分大小写
        //        {
        //        }
        //        //if (context.User.IsInRole(user.Role)) //这里区分大小写
        //        //{
        //        //}
        //        if (context.User.IsInRole("admin")) //这里区分大小写
        //        {
        //            context.Succeed(requirement);
        //        }
        //        else
        //        {

        //            //判断请求是否拥有凭据，即有没有登录
        //            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
        //            if (defaultAuthenticate != null)
        //            {
        //                /*
        //                  这里注意:
        //                  RouteEndpoint和HttpContext 获取的路径是不一样的，前缀多了 / 
        //                  route：api/controller/action
        //                  path：/api/controller/action
        //                 */
        //                var endpoint = (RouteEndpoint)context.Resource;
        //                var route = endpoint.RoutePattern.RawText;
        //                var httpContext = _accessor.HttpContext;
        //                var path = httpContext.Request.Path;

        //                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
        //                if (result?.Principal != null)
        //                {
        //                    httpContext.User = result.Principal;
        //                }
        //                var s = JwtClaimTypes.Scope;
        //                var userIdClaim = context.User.FindFirst(_ => _.Type == JwtClaimTypes.Subject);
        //                var roleId = int.Parse(context.User.Claims.Where(x => x.Type == "role_id").FirstOrDefault().Value);

        //                //判断是否是第三方

        //                //判断权限

        //                //如果是修改，只能修改自己的，

        //                if (userIdClaim != null)
        //                {
        //                    if (_userStore.CheckPermission(subjectId, roleId, route))
        //                    {
        //                        context.Succeed(requirement);
        //                    }
        //                }


        //                //if (userIdClaim != null)
        //                //{
        //                //    if (_userStore.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
        //                //    {
        //                //        context.Succeed(requirement);
        //                //    }
        //                //}


        //                // context.Fail();
        //            }

        //        }
        //    }
        //    return;
        //    //return Task.CompletedTask;
        //}
    }
}
