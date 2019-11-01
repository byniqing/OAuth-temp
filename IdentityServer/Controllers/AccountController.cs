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
using Microsoft.AspNetCore.Identity;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Common;
using IdentityServer4.Events;
using IdentityServer4.Extensions;

namespace IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        //private readonly TestUserStore _userStore;
        //private readonly IClientStore _clientStore;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public AccountController(
              IEventService events,
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             IIdentityServerInteractionService interaction)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
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
            var configurationDbContext = HttpContext.RequestServices.GetRequiredService<ConfigurationDbContext>();

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
                var user = await _userManager.FindByEmailAsync(model.Email);
                // 验证用户名和密码ValidateCredentials
                //_signinManager.CheckPasswordSignInAsync 是登陆
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {

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
                    //撤销
                    //await _interaction.RevokeUserConsentAsync(context.ClientId);
                    await _signInManager.SignInAsync(user, props);
                    //_userManager.ChangePhoneNumberAsync
                    //_userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (context != null)
                    {
                        //登录成功，跳转到授权页面
                        return Redirect(model.ReturnUrl);
                    }

                    //判断是否是本地登录
                    //判断是否是returnUrl页面，是自己网站带的returnUrl也不会通过
                    //指示在登录或同意后，返回URL是否是用于重定向的有效URL。
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
                //添加错误，这样会在错误页面上显示错误信息
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }
            //var vm = new LoginViewModel
            //{
            //    EnableLocalLogin = true,
            //    ReturnUrl = "121212",
            //    Email = "nsky",
            //};
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
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout(string logoutId, string returnurl)
        {
            //获取logoid
            var _logoutid = await _interaction.CreateLogoutContextAsync();

            ////await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            ////return RedirectToAction("index", "Home");
            //await HttpContext.SignOutAsync();
            ////Redirect(logout.PostLogoutRedirectUri);
            //return Redirect(logout.PostLogoutRedirectUri);
            ////return View("login");



            //string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            //// this triggers a redirect to the external provider for sign-out
            //return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);

            //await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            //await HttpContext.SignOutAsync(); //本地退出
            await _signInManager.SignOutAsync();//本地退出

            if (!string.IsNullOrWhiteSpace(logout.PostLogoutRedirectUri))
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }
            var refererUrl = Request.Headers["Referer"].ToString();
            return Redirect(refererUrl);
        }

    }
}