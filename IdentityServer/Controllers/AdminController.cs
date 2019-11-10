using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityServer.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public AdminController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ApiResource()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ApiResource(ApiResource apiResource)
        {
            _configurationDbContext.ApiResources.Add(apiResource.ToEntity());
            _configurationDbContext.SaveChanges();
            return View();
        }
        [HttpGet]
        public IActionResult IdentityResource()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IdentityResource(IdentityResource identityResource)
        {
            _configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
            _configurationDbContext.SaveChanges();
            return View();
        }
    }
}