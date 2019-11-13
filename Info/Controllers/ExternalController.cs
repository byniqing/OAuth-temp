using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using IdentityModel;
using Info.Date;
using Info.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Info.Controllers
{
    public class ExternalController : Controller
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly InfoDbContext _infoDbContext;
        public ExternalController(IAuthenticationSchemeProvider schemeProvider, InfoDbContext infoDbContext)
        {
            _schemeProvider = schemeProvider;
            _infoDbContext = infoDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            var schemes = _schemeProvider.GetAllSchemesAsync().Result;
            var schemeTe = OpenIdConnectDefaults.DisplayName;
            //判断是否已经授权
            if (User.Identity.IsAuthenticated)
            {
                return View("UserInfo");
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                /*
                 因为是手动触发到授权服务器
                 所以也要手动指定授权成功后的回调方法
                 */
                RedirectUri = Url.Action(nameof(Callback)),
                //成功后，ids4会回传过来
                Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
            };

            return Challenge(props, provider);
        }
        /// <summary>
        /// 授权服务器授权成功后 回调方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var acc = User.Identity.IsAuthenticated;
            // read external identity from the temporary cookie
            //var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            var result = await HttpContext.AuthenticateAsync("OpenIdConnect");
            var result1 = await HttpContext.AuthenticateAsync();
            if (result?.Succeeded != true)
            {
                //throw new Exception("External authentication error");
            }

            //授权成功，把用户信息添加到数据库

            /*
             result.Properties 中包含了很多信息，根据想要拿取即可
             比如：access_token，refresh_token
             */
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/"; //这是
            //result.Properties.Items["scheme"];
            //result.Properties.Items[".Token.access_token"];
            //result.Properties.Items[".Token.refresh_token"];

            var ide = result.Principal.Identities.ToList();

            var id_token = result.Properties.GetTokenValue("id_token");
            var access_token = result.Properties.GetTokenValue("access_token");

            //刷新token必须存在服务端，不能暴露给客户端
            var refresh_token = result.Properties.GetTokenValue("refresh_token");

            var parts = access_token.Split('.');
            var header = parts[0];
            var claims = parts[1];
            var json = JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims)));
            var auth_time = Utils.ConvertSecondsToDateTime(long.Parse(json.GetValue("auth_time").ToString())); //颁发时间
            var exp = Utils.ConvertSecondsToDateTime(long.Parse(json.GetValue("exp").ToString())); //过期时间
            var uu = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(Base64Url.Decode(claims)));

            //var my = _infoDbContext.users.FirstOrDefault(_ => _.BindId == uu.BindId);
            //if (my != null)
            //{
            //    var userEntity = _infoDbContext.Add(new User
            //    {
            //        Email = json.GetValue("email").ToString(),
            //        UserName = json.GetValue("name").ToString()
            //    }).Entity;
            //    var a = GrantsType.access_token;
            //    _infoDbContext.Add(new PersistedGrant
            //    {
            //        UserId = userEntity.Id,
            //        CreateTime = auth_time,
            //        Expiration = exp,
            //        Token = access_token,
            //        Type = GrantsType.access_token.ToString()
            //    });

            //    _infoDbContext.Add(new PersistedGrant
            //    {
            //        UserId = userEntity.Id,
            //        CreateTime = auth_time,
            //        Expiration = exp,
            //        Token = refresh_token,
            //        Type = GrantsType.refresh_token.ToString()
            //    });

            //    await _infoDbContext.SaveChangesAsync();
            //}
            //else
            //{
            //    //更新token

            //}

            //return RedirectToAction("UserInfo", "Account");
            return View("UserInfo");
        }
    }
}