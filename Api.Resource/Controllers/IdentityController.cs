using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "client_id")]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// 获取当前的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }
    }
}