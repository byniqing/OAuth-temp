using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Registe(LoginInputModel model)
        {
            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser, model.Password);
            if (identityResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}