using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Info.AuthTokenHelpers
{
    public class TokenRefresher : ITokenRefresher
    {
        private static readonly TimeSpan TokenRefreshThreshold = TimeSpan.FromSeconds(30);

        private readonly HttpClient _httpClient;
        /*
        IDiscoveryCache是IdentityModelNuGet包中提供的帮助程序，
        它使我们能够从OpenID Connect提供程序的发现终结点获取信息（并将其缓存）
        */
        private readonly IDiscoveryCache _discoveryCache;
        private readonly ILogger<TokenRefresher> _logger;

        public TokenRefresher(
            HttpClient httpClient,
            IDiscoveryCache discoveryCache,
            ILogger<TokenRefresher> logger)
        {
            _httpClient = httpClient;
            _discoveryCache = discoveryCache;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TokenRefreshResult> TryRefreshTokenIfRequiredAsync(
            string refreshToken,
            string expiresAt,
            CancellationToken ct)
        {
            //检查我们是否有刷新令牌，如果没有，则失败。
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return TokenRefreshResult.Failed();
            }

            //检查令牌的到期日期是否低于阈值，如果不是，我们可以NoRefreshNeeded立即返回。
            if (!DateTime.TryParse(expiresAt, out var expiresAtDate) || expiresAtDate >= GetRefreshThreshold())
            {
                return TokenRefreshResult.NoRefreshNeeded();
            }

            //使用提取提供商信息_discoveryCache。
            var discovered = await _discoveryCache.GetAsync();
            /*
             RequestRefreshTokenAsync是IdentityModelNuGet包的扩展方法，可简化刷新令牌请求的创建
             */
            var tokenResult = await _httpClient.RequestRefreshTokenAsync(
                 new RefreshTokenRequest
                 {
                     Address = discovered.TokenEndpoint,
                     ClientId = "WebFrontend",
                     ClientSecret = "secret",
                     RefreshToken = refreshToken
                 }, ct);

            ////如果令牌刷新成功，则返回成功并提供所需的信息，否则返回失败。
            if (tokenResult.IsError)
            {
                _logger.LogDebug(
                    "Unable to refresh token, reason: {refreshTokenErrorDescription}",
                    tokenResult.ErrorDescription);
                return TokenRefreshResult.Failed();
            }

            //如果令牌刷新成功，则返回成功并提供所需的信息
            var newAccessToken = tokenResult.AccessToken;
            var newRefreshToken = tokenResult.RefreshToken;
            var newExpiresAt = CalculateNewExpiresAt(tokenResult.ExpiresIn);

            return TokenRefreshResult.Success(newAccessToken, newRefreshToken, newExpiresAt);
        }

        private static string CalculateNewExpiresAt(int expiresIn)
        {
            // TODO: abstract usages of DateTime to ease unit tests
            return (DateTime.UtcNow + TimeSpan.FromSeconds(expiresIn)).ToString("o", CultureInfo.InvariantCulture);
        }

        private static DateTime GetRefreshThreshold()
        {
            // TODO: abstract usages of DateTime to ease unit tests
            return DateTime.UtcNow + TokenRefreshThreshold;
        }
    }
}
