using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Filter
{
    /// <summary>
    /// 全局验证，资源所有者
    /// token中的用户id，和接口中参数中的用户id（如果有）必须一致
    /// 因为自己只能查看自己和修改自己的数据
    /// </summary>
    public class UserOwnerFilter : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            //context.Result = new BadRequestObjectResult(new { data = 0, code = 0, msg = "0" });
            base.OnActionExecuting(context);

            var request = context.HttpContext.Request;

            var IsAuth = context.HttpContext.User.Identity.IsAuthenticated; //是否已认证
            if (IsAuth)
            {
                var subjectId = int.Parse(context.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value);
                var userId = 0;
                /*
                  QueryString 中获取userId
                  url格式：api/identity/info?userid=110
                  方法格式：(int userId)
                  */
                if (request.QueryString.HasValue)
                {
                    userId = Convert.ToInt32(request.Query["userId"]);
                }

                /*
                  url格式：api/identity/Firend/{userid}
                  方法格式：(int userid)
                 */
                if (request.RouteValues.ContainsKey("userId"))
                {
                    //request.RouteValues.TryGetValue("userid", out object uid);
                    userId = Convert.ToInt32(request.RouteValues["userId"]);
                }

                /*
                 url格式：api/identity/Firend
                 方法格式：(int type,[FromBody] int userid)
                 或者：([FromForm]Info info)
                 */
                if (request.HasFormContentType) //是否可以通过Form获取参数 ,表单传值
                {
                    userId = Convert.ToInt32(request.Form["userId"]);
                }
                else //body 传输
                {
                    /* 这样必须是post 且 application/json
                    url格式：api/identity/Firend
                    方法格式：(Info info)
                  */
                    if (request.Method.ToLower() == "post")
                    {
                        using (var stream = new StreamReader(request.Body))
                        {
                            request.EnableBuffering();
                            request.Body.Position = 0;
                            var content = await stream.ReadToEndAsync();
                            var jObject = JObject.Parse(content);
                            if (jObject.ContainsKey("userId"))
                            {
                                userId = Convert.ToInt32(jObject["userId"]);
                            }
                        }
                    }
                }
                if (subjectId != userId) context.Result = new BadRequestObjectResult(new { data = -1, code = StatusCodes.Status403Forbidden, msg = "登录用户有误" });
            }
        }

        /// <summary>
        /// OnActionExecuted 在执行操作方法之后由 core 框架调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

        }
    }
}
