using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "reqscope")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("comment")]
        public IActionResult GetComment()
        {
            return Ok(new { name = "获取我的评论成功" });
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            return Ok(new { name = "获得我的好友列表" });
        }
    }
}