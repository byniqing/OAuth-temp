using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel;
using System.ComponentModel;

namespace Api.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [Authorize(Policy = "reqscope")]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// 获取当前的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("OtherInfo")]
        [DisplayName("测试)")]
        //[Authorize(policy: "OtherInfo")]
        public ActionResult Get()
        {
            //User.Identity.IsAuthenticated
            return new JsonResult(User.Claims.Select(
                c => new { c.Type, c.Value }));
        }

        /// <summary>
        /// 获取当前的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("oidc1")]
        //[Authorize(Policy = "oidc1")]
        public ActionResult Name(string userid)
        {
            //判断授权用户和userId是否是同一个人

            var subjectId = User.Claims.Where(x => x.Type == JwtClaimTypes.Subject).FirstOrDefault().Value;

            var id = User.Claims.Select(s => s.Subject.FindAll(f => f.Value == userid));

            return Ok(new { name = User.Identity.Name });
        }

        /// <summary>
        /// 获取我的好友
        /// </summary>
        /// <returns></returns>
        [HttpGet("Firend/{userId}")]
        public ActionResult GetFirend(int userId)
        {
            return Ok(new { name = "获取我的好友" });
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        [HttpGet("Update")]
        public string Update()
        {
            return "修改资料成功";
        }


        /// <summary>
        /// 这种方式：前端必须传 application/json 才能映射到
        /// 默认就是：[FromBody]
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public string UpdateShow(Info  info)
        {
            return "修改资料成功";
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("form")]
        public string fors([FromForm]Info info)
        {
            return "修改资料成功";
        }

        [HttpPost("h/{id}")]
        public string geth(int id, [FromBody] int userId)
        {
            return "修改资料成功";
        }
    }

    public class Info
    {
        public int userId { get; set; }
        public string name { get; set; }
    }
}