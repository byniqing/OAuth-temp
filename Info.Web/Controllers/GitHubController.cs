using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Info.Web.Controllers
{
    public class GitHubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            return Challenge( provider);
        }
        [HttpGet]
        public void CallBack()
        {

        }
    }
}