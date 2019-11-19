using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Info.AuthTokenHelpers
{
    /// <summary>
    /// 提供一种简单的方法来确保用户的访问令牌是最新的
    /// </summary>
    public interface ITokenRefresher
    {
        /// <summary>
        /// 如果需要，尝试刷新当前用户的访问令牌。
        /// </summary>
        /// <param name="refreshToken">当前刷新令牌</param>
        /// <param name="expiresAt">当前令牌过期信息</param>
        /// <param name="ct">异步取消令牌。</param>
        /// <returns>
        /// <code>True</code>
        /// 如果不需要刷新或刷新执行成功
        /// <code>False</code> 否则.
        /// </returns>
        Task<TokenRefreshResult> TryRefreshTokenIfRequiredAsync(
            string refreshToken,
            string expiresAt,
            CancellationToken ct);
    }
}
