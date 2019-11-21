using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.MiddleWare
{
    public class CorsMiddleware
    {
        //https://www.cnblogs.com/wmlunge/p/10655209.html
        //https://www.jianshu.com/p/bdd78b3d6f1a
        private readonly RequestDelegate next;

        public CorsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(CorsConstants.Origin))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                context.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS,HEAD,PATCH");
                context.Response.Headers.Add("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"]);
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                if (context.Request.Method.Equals("OPTIONS"))
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return;
                }
            }

            await next(context);
        }
    }
}
