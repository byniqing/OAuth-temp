using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Info.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public LoginController(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> Login(string returnUrl)
        //{
        //    //获取所有注册的scheme
        //    var schemes = await _schemeProvider.GetAllSchemesAsync();
        //    var providers = schemes
        //        .Where(x => x.DisplayName != null)
        //        .Select(x => new ExternalProvider
        //        {
        //            DisplayName = x.DisplayName,
        //            AuthenticationScheme = x.Name
        //        }).ToList();

        //    var lv = new LoginViewModel
        //    {
        //        ReturnUrl = returnUrl,
        //        Username = "",
        //        ExternalProviders = providers.ToArray()
        //    };
        //    return View(lv);
        //}

        /// <summary>
        /// oidc测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OAuth()
        {
            //获取所有注册的scheme
            var schemes = _schemeProvider.GetAllSchemesAsync().Result;

            string scheme = "oidc"; //ConfigureServices中注册的服务是oidc
            string schemeTe = "oidc";
            schemeTe = OpenIdConnectDefaults.DisplayName;

            //var userResult = await HttpContext.AuthenticateAsync(scheme);
            //var user = userResult.Principal;
            //var props1 = userResult.Properties;
            //userResult.Properties.GetTokenValue("access_token");

            if (User.Identity.IsAuthenticated)
            {
                return View("UserInfo");
            }
            //var userResult = await HttpContext.AuthenticateAsync(scheme);

            //var user = userResult.Principal;
            //var props1 = userResult.Properties;


            var props = new AuthenticationProperties
            {
                //指定授权成功后回调的Action
                RedirectUri = Url.Action(nameof(Callback)),

                /*
                 可以传些额外信息，回调会传回来
                 可以通过
                  var result = await HttpContext.AuthenticateAsync("oidc");
                  result.Properties.Items["returnUrl"]
                 */
                //Items =
                //    {
                //        { "returnUrl", returnUrl },
                //        { "scheme", provider },
                //    }
            };
            return Challenge(props, scheme);


            /*
             如果这样，授权服务器回调就是当前Action
             */
            //return Challenge(scheme);


            //await HttpContext.ChallengeAsync(scheme);
            //return await ProcessWindowsLoginAsync("/Account/Authorize");

        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync("oidc");
            var result1 = await HttpContext.AuthenticateAsync();
            if (result?.Succeeded != true)
            {
                //throw new Exception("External authentication error");
            }
            var id_token = result.Properties.GetTokenValue("id_token");
            var access_token = result.Properties.GetTokenValue("access_token");

            //  var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            return RedirectToAction("UserInfo", "Account");
        }

        [HttpGet]
        public void Lognout()
        {
            //清除本地cookie
            HttpContext.SignOutAsync("Cookies");
            //然后重定向到IdentityServer。 IdentityServer将清除它的cookie
            //依赖包： Microsoft.AspNetCore.Authentication.OpenIdConnect
            HttpContext.SignOutAsync(OpenIdConnectDefaults.DisplayName);


            //HttpContext.SignOutAsync("idsrv");
            //return RedirectToAction("index", "home");
        }
    }
}