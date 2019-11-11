using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Controllers
{
    /// <summary>
    /// 应用管理
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public ManageController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatePc()
        {
            return View();
        }
        /// <summary>
        /// 创建pc应用
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreatePc(Client client)
        {
            //var AllowedGrantTypes = Request.Form["AllowedGrantTypes"];
            //var AllowedScopes = Request.Form["AllowedScopes"];

            //是否已经添加过

            client.RedirectUris = new string[] { client.ClientUri + new PathString("/signin-oidc") };
            //client.RedirectUris = new string[] { client.ClientUri + "signin-oidc" };
            client.PostLogoutRedirectUris = new string[] { client.ClientUri + new PathString("/signout-callback-oidc") };
            client.AlwaysIncludeUserClaimsInIdToken = true;
            client.AllowOfflineAccess = true;
            client.ClientId = Utils.GetRandomNum(10);
            var secrets = Utils.GetRandom(10);
            client.ClientSecrets = new List<Secret> {
                        new Secret(secrets.Sha256()) //secret
                    };
            client.Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Role, "thirdParty"), // 第三方角色
                    new Claim("secret", secrets) //把密钥保存起来
                };
            client.Enabled = false; //默认是false，因为要审核
            _configurationDbContext.Clients.Add(client.ToEntity());
            //_configurationDbContext.SaveChanges();
            return View();
        }
        [HttpGet]
        public IActionResult CreateAndroid()
        {
            return View();
        }
        /// <summary>
        /// 创建移动端应用
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateAndroid(Client client)
        {
            client.ClientId = Utils.GetRandomNum(10);
            var secrets = Utils.GetRandom(10);
            client.ClientSecrets = new List<Secret> {
                        new Secret(secrets.Sha256()) //secret
                    };
            client.Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Role, "System"), // 第三方角色
                    new Claim("secret", secrets) //把密钥保存起来
                };
            client.Enabled = false; //默认是false，因为要审核
            _configurationDbContext.Clients.Add(client.ToEntity());
            _configurationDbContext.SaveChanges();
            return View();
        }
    }
}