using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Info.Controllers
{
    public class AuthorizeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="token">实体类</param>
        /// <returns></returns>
        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        public void Refresh_token()
        {
            /*
             用户刷新token必须要做有效性判断
             比如传access_token过来跟数据库access_token比较。
             */
        }
    }
}