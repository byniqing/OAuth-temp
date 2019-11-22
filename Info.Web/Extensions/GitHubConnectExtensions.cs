using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Web.Extensions
{
    public static class GitHubConnectExtensions
    {
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder)
              => builder.AddGitHub(GitHubDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, Action<GitHubOptions> configureOptions)
            => builder.AddGitHub(GitHubDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string authenticationScheme, Action<GitHubOptions> configureOptions)
            => builder.AddGitHub(authenticationScheme, GitHubDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<GitHubOptions> configureOptions)
            => builder.AddOAuth<GitHubOptions, GitHubHandler>(authenticationScheme, displayName, configureOptions);
    }
}
