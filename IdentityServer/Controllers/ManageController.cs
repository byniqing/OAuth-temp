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
using IdentityServer.ViewModels;
using IdentityServer4.Stores;
using IdentityServer.Date;
using IdentityServer.Common;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Controllers
{
    /// <summary>
    /// 应用管理
    /// </summary>
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IConfigurationDbContext _configurationDbContext;
        private readonly IResourceStore _resourceStore;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IIdentityServerInteractionService _interaction;
        public ManageController(ConfigurationDbContext configurationDbContext,
            IIdentityServerInteractionService interaction,
            IConfigurationDbContext ic1,
        IResourceStore resourceStore, ApplicationDbContext applicationDbContext)
        {
            _configurationDbContext = configurationDbContext;
            _resourceStore = resourceStore;
            _applicationDbContext = applicationDbContext;
            _interaction = interaction;
        }
        public async Task<IActionResult> Index()
        {
            //http://www.cocoachina.com/articles/112766
            //var cks12 = HttpContext.RequestServices.GetRequiredService<ConfigurationDbContext>();

            ApplicationViewModel vm = null;
            var userId = int.Parse(User.FindFirstValue("sub"));

            //var client = _configurationDbContext.Clients;
            var app = _applicationDbContext.userClients.FirstOrDefault(_ => _.UserId == userId);
            if (app != null)
            {
                var client = await GetClientAsync(app.ClientId);
                //var myApp = client.FirstOrDefault(_ => _.Id == app.ClientId);
                if (client != null)
                {
                    vm = new ApplicationViewModel
                    {
                        ClientId = client.ClientId,
                        //ClientSecrets = client.ClientSecrets.Find(_ => _.ClientId == client.Id).Value,
                        ClientSecrets = client.Claims.Find(c => c.Type == "secret").Value,
                        Created = client.Created,
                        Enable = client.Enabled,
                        ClientName = client.ClientName
                    };
                }

            }
            return View(vm);
        }

        public Task<IdentityServer4.EntityFramework.Entities.Client> GetClientAsync(int id)
        {
            return _configurationDbContext.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .Where(x => x.Id == id)
                    .SingleOrDefaultAsync();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        private ScopeViewModel CreateScopeViewModel(IdentityResource scope)
        {
            return new ScopeViewModel
            {
                Checked = scope.Required,
                Required = scope.Required,
                Description = scope.Description,
                DisplayName = scope.DisplayName,
                Emphasize = scope.Emphasize,
                Name = scope.Name
            };
        }
        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Checked = scope.Required,
                Required = scope.Required,
                Description = scope.Description,
                DisplayName = scope.DisplayName,
                Emphasize = scope.Emphasize,
                Name = scope.Name
            };
        }
        [HttpGet]
        public async Task<IActionResult> CreatePc()
        {
            var ab = _configurationDbContext.IdentityResources;

            //获取所有启用的resource
            var resources = await _resourceStore.GetAllEnabledResourcesAsync();
            var vm = new ManageViewModel();
            if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
            {
                vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x)).ToArray();
                vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(x)).ToArray();
            }

            return View(vm);
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
            var userId = int.Parse(User.FindFirstValue("sub"));
            //记录用户申请的授权
            var user = _applicationDbContext.userClients.FirstOrDefault(_ => _.UserId == userId && _.Type == ApplicationType.web.ToString());
            if (user != null)
            {
                ViewBag.error = "只能创建一个web应用";
            }
            else
            {
                ViewBag.error = "";
                //client.ClientName = User.FindFirstValue("email");
                client.RedirectUris = new string[] { client.ClientUri + new PathString("/signin-oidc") };
                //client.RedirectUris = new string[] { client.ClientUri + "signin-oidc" };
                client.PostLogoutRedirectUris = new string[] { client.ClientUri + new PathString("/signout-callback-oidc") };
                client.AlwaysIncludeUserClaimsInIdToken = true;
                client.AllowOfflineAccess = true;
                //client.IncludeJwtId = true;
                client.ClientId = Utils.GetRandomNum(10);
                var secrets = Utils.GetRandom(10);
                client.ClientSecrets = new List<Secret> {
                        new Secret(secrets.Sha256()) //secret
                    };
                client.AlwaysSendClientClaims = true;
                client.Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Role, "thirdParty"), // 第三方角色
                    new Claim("secret", secrets), //把密钥保存起来
                    //new Claim("source","web"),
                    //new Claim("name",User.FindFirstValue("email")), //一个用户只能创建一种类型
                };
                //client.Enabled = false; //如果要审核，就false，因为要审核
                var entity = _configurationDbContext.Clients.Add(client.ToEntity()).Entity;
                var rows = _configurationDbContext.SaveChanges();
                if (rows > 0)
                {
                    //记录用户申请的授权
                    _applicationDbContext.userClients.Add(new UserClient
                    {
                        ClientId = entity.Id,
                        UserId = userId,
                        Type = ApplicationType.web.ToString()
                    });
                    _applicationDbContext.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
            //return View(nameof(Index));
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