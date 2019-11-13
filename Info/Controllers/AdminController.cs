using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Info.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var ck = User.Identity.Name;
            return View();
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Callback(int a)
        {
            var result1 = await HttpContext.AuthenticateAsync();
            var result = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.DisplayName);
            //var result = await HttpContext.AuthenticateAsync("oidc");
            if (result?.Succeeded != true)
            {
                //throw new Exception("External authentication error");
            }
            var id_token = result.Properties.GetTokenValue("id_token");
            var access_token = result.Properties.GetTokenValue("access_token");

            //  var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            return RedirectToAction("UserInfo", "Admin");
        }
    }
}