using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Info.Controllers
{
    public class ExternalController : Controller
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private string a = null;
        private static string b = null;
        public ExternalController(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            a = provider;
            b = provider;

            var schemes = _schemeProvider.GetAllSchemesAsync().Result;
            var schemeTe = OpenIdConnectDefaults.DisplayName;
            //判断是否已经授权
            if (User.Identity.IsAuthenticated)
            {
                return View("UserInfo");
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                /*
                 因为是手动触发到授权服务器
                 所以也要手动指定授权成功后的回调方法
                 */
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
            };

            return Challenge(props, provider);
        }
        /// <summary>
        /// 授权服务器授权成功后 回调方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var acc = User.Identity.IsAuthenticated;
            // read external identity from the temporary cookie
            //var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var result = await HttpContext.AuthenticateAsync("OpenIdConnect");
            var result1 = await HttpContext.AuthenticateAsync();
            if (result?.Succeeded != true)
            {
                //throw new Exception("External authentication error");
            }

            //  var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";


            var ide = result.Principal.Identities.ToList();

            var id_token = result.Properties.GetTokenValue("id_token");
            var access_token = result.Properties.GetTokenValue("access_token");
            var refresh_token = result.Properties.GetTokenValue("refresh_token");


            //return RedirectToAction("UserInfo", "Account");
            return View("UserInfo");
        }
    }
}