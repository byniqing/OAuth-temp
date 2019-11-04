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
    //[Authorize(Roles = "Admin")]
    //或者
    [Authorize(Policy = "A_S_O")]
    public class AdminController : ControllerBase
    {
        [HttpGet("success")]
        public string Get()
        {
            var ck = User.Claims.Where(w => w.Type == "role").FirstOrDefault().Value;
            var ck1 = User.Claims.Where(w => w.Type == "role").FirstOrDefault();
            var ck2 = User.Claims.Where(w => w.Type == "role");
            var ck3 = User.Claims.Where(w => w.Type == "role").ToList();
            return "success";
        }
    }
}