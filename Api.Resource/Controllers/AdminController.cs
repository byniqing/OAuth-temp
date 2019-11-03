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
            return "success";
        }
    }
}