using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.AuthTokenHelpers
{
    public static class Extensions
    {
        public static Task<string> GetAccessTokenAsync(this HttpContext context)
          => context.GetTokenAsync("access_token");
    }
}
