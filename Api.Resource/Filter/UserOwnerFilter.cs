using Microsoft.AspNetCore.Http;
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
        public override  void OnActionExecuting(ActionExecutingContext context)
        {
            //context.Result = new BadRequestObjectResult(new { data = 0, code = 0, msg = "0" });
            base.OnActionExecuting(context);
            
        }

        /// <summary>
        /// OnActionExecuted 在执行操作方法之后由 core 框架调用
        /// </summary>
        /// <param name="context"></param>
        public override async void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);


            var request = context.HttpContext.Request;

            var IsAuth = context.HttpContext.User.Identity.IsAuthenticated; //是否已认证

            /*
              QueryString 中获取userid
              url格式：api/identity/info?userid=110
              方法格式：(int userid)
              */
            if (request.QueryString.HasValue)
            {
                var cid = request.Query["userid"];
            }
            //httpContext.Request.RouteValues.ContainsKey("userid")

            /*
              url格式：api/identity/Firend/{userid}
              方法格式：(int userid)
             */
            if (request.RouteValues.ContainsKey("userid"))
            {
                var bn = request.RouteValues["userid"];
            }
            //var bn1 = request.RouteValues["userid"];
            //httpContext.Request.RouteValues.TryGetValue("f", out object c89);

            /*
             url格式：api/identity/Firend
             方法格式：(int type,[FromBody] int userid)
             或者：([FromForm]Info info)
             */
            if (request.HasFormContentType) //是否可以通过Form获取参数 ,表单传值
            {
                var c9 = request.Form["userid"];
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
                        //httpContext.Request.EnableRewind();
                        request.Body.Position = 0;
                        var content = await stream.ReadToEndAsync();
                        var pa = JObject.Parse(content);
                        if (pa.ContainsKey("userId"))
                        {
                            var idss = pa["userId"];
                        }
                    }
                }
            }
        }
    }
}
