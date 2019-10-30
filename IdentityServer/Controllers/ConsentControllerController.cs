﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using IdentityServer4.Models;
using IdentityServer.ViewModels;
using IdentityServer.Common;

namespace IdentityServer.Controllers
{
    /// <summary>
    /// 同意授权，处理
    /// </summary>
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(IClientStore clientStore,
            IResourceStore resourceStore,
            IIdentityServerInteractionService interaction,
             ILogger<ConsentController> logger)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _interaction = interaction;
            _logger = logger;
        }
        /// <summary>
        /// 登录成功，跳转到同意授权页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);
            if (vm != null)
            {
                return View("index", vm);
            }
            return View("Error");
        }

        /// <summary>
        ///  用户同意和拒绝，触发
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var result = await ProcessConsent(model);
            if (result.IsRedirect)
            {
                //var ck = new Uri(result.RedirectUri);
                return Redirect(result.RedirectUri);
            }
            if (result.ShowView)
            {
                return View("Index", result.ViewModel);
            }

            return View("Error");
        }
        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            var result = new ProcessConsentResult();

            // 验证URL是否有效
            var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (request == null) return result;
            ConsentResponse grantedConsent = null;
            if (model.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;
            }
            else if (model.Button == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented.ToList();

                    //获取所有
                    var identitResource = await _resourceStore.FindIdentityResourcesByScopeAsync(request.ScopesRequested);

                    if (identitResource != null && identitResource.Any())
                    {
                        //获取不显示在界面，但是必须项Required的
                        identitResource.Where(i => !i.ShowInDiscoveryDocument && i.Required).ToList()
                            .ForEach(f => { scopes.Add(f.Name); });
                    }
                    //if (ConsentOptions.EnableOfflineAccess == false)
                    //{
                    //    scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess);
                    //}

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }
            if (grantedConsent != null)
            {
                // 把同意的结果发送给 identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                //var ck = new Uri(model.ReturnUrl);
                //result.RedirectUri = "http://localhost:5006/";
                result.ClientId = request.ClientId;
            }
            else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }
            return result;
        }


        public async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            ConsentViewModel cvm = null;
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (request != null)
            {
                var client = await _clientStore.FindClientByIdAsync(request.ClientId);
                if (client != null)
                {

                    cvm = new ConsentViewModel
                    {
                        ClientName = client.ClientName,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        AllowRememberConsent = client.AllowRememberConsent,
                        ReturnUrl = returnUrl,
                        RememberConsent = model?.RememberConsent ?? true,
                        ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),
                    };

                    var resources = await _resourceStore.FindResourcesByScopeAsync(request.ScopesRequested);

                    var apiResource = await _resourceStore.FindApiResourcesByScopeAsync(request.ScopesRequested);
                    var identitResource = await _resourceStore.FindIdentityResourcesByScopeAsync(request.ScopesRequested);

                    if (apiResource != null && apiResource.Any())
                    {
                        cvm.ResourceScopes = apiResource.SelectMany(i => i.Scopes)

                            .Select(s => CreateScopeViewModel(s, cvm.ScopesConsented.Contains(s.Name) || model == null));
                    }

                    if (identitResource != null && identitResource.Any())
                    {
                        cvm.IdentityScopes = identitResource
                            //过滤掉不显示在界面的项
                            .Where(w => w.ShowInDiscoveryDocument)
                            .Select(s => CreateScopeViewModel(s, cvm.ScopesConsented.Contains(s.Name) || model == null));
                    }

                    if (resources != null && (resources.ApiResources.Any() || resources.IdentityResources.Any()))
                    {

                    }
                }
            }
            return cvm;
        }
        private ScopeViewModel CreateScopeViewModel(IdentityResource scope, bool check)
        {
            return new ScopeViewModel
            {
                Checked = check || scope.Required,
                Required = scope.Required,
                Description = scope.Description,
                DisplayName = scope.DisplayName,
                Emphasize = scope.Emphasize,
                Name = scope.Name
            };
        }
        private ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Checked = check || scope.Required,
                Required = scope.Required,
                Description = scope.Description,
                DisplayName = scope.DisplayName,
                Emphasize = scope.Emphasize,
                Name = scope.Name
            };
        }
    }
}