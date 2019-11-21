using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Info.AuthTokenHelpers
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly ITokenRefresher _tokenRefresher;

        public CustomCookieAuthenticationEvents(ITokenRefresher tokenRefresher)
        {
            _tokenRefresher = tokenRefresher;
        }

        /// <summary>
        /// 服务端的验证逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var result = await _tokenRefresher.TryRefreshTokenIfRequiredAsync(
                context.Properties.GetTokenValue("refresh_token"),
                context.Properties.GetTokenValue("expires_at"),
                CancellationToken.None);

            if (!result.IsSuccessResult)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else if (result.TokensRenewed)
            {
                context.Properties.UpdateTokenValue("access_token", result.AccessToken);
                context.Properties.UpdateTokenValue("refresh_token", result.RefreshToken);
                context.Properties.UpdateTokenValue("expires_at", result.ExpiresAt);
                context.ShouldRenew = true;
            }
        }
    }
}
