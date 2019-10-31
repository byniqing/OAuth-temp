using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.ViewModels;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IdentityServer.Common;

namespace IdentityServer.Controllers
{
    /// <summary>
    /// TestUserStore 登陆演示
    /// 也就是GetTestUsers 配置的用户信息登陆，内存级别的
    /// </summary>
    public class TestController : Controller
    {
        private readonly TestUserStore _userStore;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        public TestController(
            TestUserStore userStore,
            IClientStore clientStore,
            IIdentityServerInteractionService interaction)
        {
            _userStore = userStore;
            _clientStore = clientStore;
            _interaction = interaction;
        }
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <param name="returnUrl">第三方跳转会携带returnUrl</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            //ViewBag.returnUrl11 = returnUrl;
            return View();
        }

        /// <summary>
        /// 本地登录也远程登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(LoginInputModel model)
        {
            /*
              获取请求授权的上下文，如果是第三方来的则有值，否则为空
             */
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // 用户单击了取消
            if (model.button == "cancel")
            {
                if (context != null)
                {
                    //通知被授权程序，取消授权
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    /*
                     不是来自授权请求，说明是自己程序取消登录，跳转到自己主页面
                     */
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                // 验证用户名和密码
                if (_userStore.ValidateCredentials(model.Email, model.Password))
                {
                    var user = _userStore.FindByUsername(model.Email);

                    #region identityserver4封装的扩展方法,也可以
                    ////idsrv验证sub是必须的
                    //var claims = new List<Claim> {
                    //    new Claim(JwtClaimTypes.Subject,user.SubjectId),
                    //    new Claim(JwtClaimTypes.Name,user.Username)
                    // };
                    //var claimIdentity = new ClaimsIdentity(claims, OAuthDefaults.DisplayName);
                    //var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

                    //await HttpContext.SignInAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme,
                    //    claimsPrincipal,
                    //    new AuthenticationProperties
                    //    {
                    //        IsPersistent = true, //
                    //        ExpiresUtc = DateTime.Now.AddDays(5)
                    //    }); 
                    #endregion

                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true, //持久化
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration) //过期时间
                        };
                    };

                    /*
                     依赖：Microsoft.AspNetCore.Http
                     登录成功
                     */
                    await HttpContext.SignInAsync(user.SubjectId, user.Username, props);

                    if (context != null)
                    {
                        //登录成功，跳转到授权页面
                        return Redirect(model.ReturnUrl);
                    }

                    //判断是否是本地登录
                    //if (_interaction.IsValidReturnUrl(login.ReturnUrl)) //这样也可以
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        //即不是第三方登录，也不是本地登录，那就抛异常咯
                        throw new Exception("invalid return URL");
                    }
                }
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }
            var vm = new LoginViewModel
            {
                EnableLocalLogin = true,
                ReturnUrl = "121212",
                Email = "nsky",
            };
            // something went wrong, show form with error
            //var vm = await BuildLoginViewModelAsync(model);
            return View();
        }
        //private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        //{
        //    var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        //    vm.Username = model.Email;
        //    vm.RememberLogin = model.RememberLogin;
        //    return vm;
        //}
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        [HttpGet("Logout1")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            ////await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            ////return RedirectToAction("index", "Home");
            //await HttpContext.SignOutAsync();
            ////Redirect(logout.PostLogoutRedirectUri);
            //return Redirect(logout.PostLogoutRedirectUri);
            ////return View("login");

            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            await HttpContext.SignOutAsync(); //本地退出
            if (!string.IsNullOrWhiteSpace(logout.PostLogoutRedirectUri))
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }
            var refererUrl = Request.Headers["Referer"].ToString();
            return Redirect(refererUrl);
        }
    }
}