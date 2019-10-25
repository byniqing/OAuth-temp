using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.ViewModels;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AccountController111 : Controller
    {
        private readonly TestUserStore _userStore;
        private readonly IIdentityServerInteractionService _interaction;
        public AccountController111(
            TestUserStore userStore,
            IIdentityServerInteractionService interaction)
        {
            _userStore = userStore;
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 没有授权跳转到登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            //ViewBag.returnUrl11 = returnUrl;
            return View();
        }
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    //ViewBag.returnUrl11 = returnUrl;
        //    return View();
        //}
        /// <summary>
        /// 用户发起登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel login)
        {
            if (login.button.ToLower() == "cancel")
            {
                var request = await _interaction.GetAuthorizationContextAsync(login.ReturnUrl);
                if (request != null)
                {
                    ConsentResponse grantedConsent = ConsentResponse.Denied;
                    await _interaction.GrantConsentAsync(request, grantedConsent);
                    return Redirect(login.ReturnUrl);
                }
                else
                {
                    //异常处理
                }
            }
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByEmailAsync(login.Email);
                var user = _userStore.FindByUsername(login.Email);
                if (user != null)
                {
                    #region 方法2 或者直接用identityserver4封装的扩展方法 这样登录才正确
                    var p = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.Now.AddDays(5)
                    };

                    //identityserver 是身份验证 ，identity 所以要用该方法
                    /*
                     * 
                     */
                    //await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync//(HttpContext, user.SubjectId, user.Username, p);

                    //或者 TestUserStore 登陆
                    //await HttpContext.SignInAsync(user.SubjectId, user.Username, p);

                    //idsrv验证sub是必须的
                    var claims1 = new List<Claim> {
                        new Claim(JwtClaimTypes.Subject,user.SubjectId),
                        new Claim(JwtClaimTypes.Name,user.Username)
                     };
                    var claimIdentity1 = new ClaimsIdentity(claims1, OAuthDefaults.DisplayName);
                    var claimsPrincipal1 = new ClaimsPrincipal(claimIdentity1);

                    await HttpContext.SignInAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme,
                        claimsPrincipal1,
                        new AuthenticationProperties
                        {
                            IsPersistent = true, //
                            ExpiresUtc = DateTime.Now.AddDays(5)
                        });
                    #endregion
                    if (_interaction.IsValidReturnUrl(login.ReturnUrl))
                    {
                        //登录成功，跳转到同意授权页面
                        return Redirect(login.ReturnUrl);
                    }
                    return Redirect("~/");
                }
            }
            //else
            return View("login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            ////await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            ////return RedirectToAction("index", "Home");
            //await HttpContext.SignOutAsync();
            ////Redirect(logout.PostLogoutRedirectUri);
            //return Redirect(logout.PostLogoutRedirectUri);
            ////return View("login");

            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            await HttpContext.SignOutAsync();
            if (!string.IsNullOrWhiteSpace(logout.PostLogoutRedirectUri))
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }
            var refererUrl = Request.Headers["Referer"].ToString();
            return Redirect(refererUrl);
        }
    }
}