using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel;

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
            //判断授权用户和userid是否是同一个人

            var subjectId = User.Claims.Where(x => x.Type == JwtClaimTypes.Subject).FirstOrDefault().Value;

            var id = User.Claims.Select(s => s.Subject.FindAll(f => f.Value == userid));

            return Ok(new { name = User.Identity.Name });
        }

        /// <summary>
        /// 获取我的好友
        /// </summary>
        /// <returns></returns>
        [HttpGet("Firend")]
        public ActionResult GetFirend()
        {
            return Ok(new { name = User.Identity.Name });
        }

    }
}