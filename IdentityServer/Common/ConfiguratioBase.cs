using IdentityServer.ViewModels;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Common
{
    public class ConfiguratioBase
    {
        public static ScopeViewModel CreateScopeViewModel(IdentityResource scope)
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
        public static ScopeViewModel CreateScopeViewModel(Scope scope)
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

        public static Task<IdentityServer4.EntityFramework.Entities.Client> GetClientAsync(IConfigurationDbContext context, int id)
        {
            return context.Clients
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

    }
}
