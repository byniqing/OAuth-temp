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
    //[Authorize(Policy = "reqscope")]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// 获取当前的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("OtherInfo")]
        [Authorize(policy: "OtherInfo")]
        public ActionResult Get()
        {
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }

        /// <summary>
        /// 获取当前的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("oidc1")]
        [Authorize(Policy = "oidc1")]
        public ActionResult Name()
        {
            return Ok(new { name = User.Identity.Name });
        }

    }
}