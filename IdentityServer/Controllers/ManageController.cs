using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    /// <summary>
    /// 应用管理
    /// </summary>
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}