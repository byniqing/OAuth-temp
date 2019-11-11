using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Info.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Info.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        public AccountController(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                /*
                 获取所有注册的scheme
                这里就是AddOpenIdConnect 中添加的DisplayName参数
                 */
                var schemes = await _schemeProvider.GetAllSchemesAsync();
                var providers = schemes
                    .Where(x => x.DisplayName != null)
                    .Select(x => new ExternalProvider
                    {
                        DisplayName = x.DisplayName,
                        AuthenticationScheme = x.Name
                    }).ToList();


                var cookies = CookieAuthenticationDefaults.AuthenticationScheme;
                var c = OAuthDefaults.DisplayName;

                if (model.Email == "nsky@163.com" && model.Password == "123")
                {
                    AuthenticationProperties props = null;
                    if (model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true, //持久化
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration) //过期时间
                        };
                    };
                    var claims = new List<Claim> {
                    //Subject：颁发者处最终用户的唯一标识符
                    new Claim(JwtClaimTypes.Subject,"1000"),
                    //Id：仅仅是标识符
                    new Claim(JwtClaimTypes.Id,"1000"),
                    new Claim(JwtClaimTypes.Name,"nsky")
                 };
                    var claimIdentity = new ClaimsIdentity(claims, cookies);
                    var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

                    await HttpContext.SignInAsync(cookies, claimsPrincipal, props);

                }
                // var refererUrl = Request.Headers["Referer"].ToString();
                var returnUrl = Request.Query["ReturnUrl"].ToString();
                returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
                return Redirect(returnUrl);
            }
            //模型验证失败，跳转到当前页面，会显示错误信息
            return View();
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.

            ///Account/Logout?logoutId=966
            //如果想ids4服务器回调某个aciton。则主动传RedirectUri即可
            //string url = Url.Action("Logouts", new { logoutId = 966 });
            //return SignOut(new AuthenticationProperties { RedirectUri = url }, "OpenIdConnect");

            //清除本地cookie
            //HttpContext.SignOutAsync("OpenIdConnect");

            var result = await HttpContext.AuthenticateAsync();

            /*
             只有是第三方登陆的。退出的时候才通知第三方退出
             */
            if (result.Properties.Items.ContainsKey("scheme"))
            {
                var scheme = result.Properties.Items["scheme"].ToString();
                //或者
                var cd = result.Properties.Items[".AuthScheme"];
                return SignOut("Cookies", scheme);
                //return SignOut("Cookies", "OpenIdConnect");
            }

            #region 这只是本地退出，不会远程退出
            await HttpContext.SignOutAsync(); //这只是本地退出，不会远程退出
            /*
             这里会执行cookieHandler处理程序(即：AddCookid()的配置)
             会跳转到 /account
             */
            //HttpContext.ChallengeAsync();

            //登出，直接跳转到首页
            return Redirect("/");
            #endregion
        }
        [HttpGet]
        public IActionResult Logouts(int logoutId)
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}