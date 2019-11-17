using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Info.Date;
using Info.Models;
using Info.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Info.Controllers
{
    public class RegisterController : Controller
    {
        private readonly InfoDbContext _infoDbContext;
        public RegisterController(InfoDbContext infoDbContext)
        {
            _infoDbContext = infoDbContext;
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
        public IActionResult Index(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                _infoDbContext.Add(new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PassWord = Utils.MD5(model.Password)
                });
                var rows = _infoDbContext.SaveChanges();
                if (rows > 0) //return RedirectToAction(nameof(Index));
                    return RedirectToRoute(new { controller = "Account", action = "Index" });
            }
            ModelState.AddModelError(string.Empty, "输入有误！");
            return View();
        }
    }
}