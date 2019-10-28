using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using IdentityServer.Models;
using IdentityModel;

namespace IdentityServer.Services
{
    /// <summary>
    /// 处理后一旦身份信息过期就会调用本方法重新创建身份信息
    /// </summary>
    public class UserClaimsFactory : IUserClaimsPrincipalFactory<ApplicationUser>
    {
        //private readonly IUserStoreService _storeService;
        public async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            //var claims = await _storeService.GetAllClaimsByUser(user);
            //var claims = new List<Claim> { };
            var claims = new List<Claim> {
                new Claim(JwtClaimTypes.Subject,"2"),
                new Claim(JwtClaimTypes.Name,"1")
             };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return await Task.FromResult(claimsPrincipal);

            //throw new NotImplementedException();
        }
    }
}
