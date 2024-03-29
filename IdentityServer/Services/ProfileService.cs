﻿using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    /// <summary>
    ///  Profile就是用户资料，ids 4里面定义了一个IProfileService的接口用来获取用户的一些信息
    ///  ，主要是为当前的认证上下文绑定claims。我们可以实现IProfileService从外部创建claim扩展到ids4里面。
    ///  然后返回
    ///  获取用户Claims
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ProfileService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 用户请求userinfo endpoint时会触发该方法
        /// http://xxx:port/connect/userinfo
        /// 是ids4内部触发的，看流程图即可知道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            /*
             context.ValidatedRequest.ClientId
            "Info.Client"
            context.ValidatedRequest.ClientClaims
            Count = 1
                [0]: {role: thirdParty}
            context.Client.Claims
            Count = 1
                [0]: {role: thirdParty}

             */
            //判断是否有请求Claim信息
            //if (context.RequestedClaimTypes.Any())
            //{

            //}


            /*
             问题：
             当授权给第三方后，第三方可以用token访问资源服务器获取资源
             但，自己的账号也可以访问资源服务器获取资源，
             但，自己的权限肯定大于第三方，
             比如：第三方只有查看权限，没有修改资料权限
             这个时候，第三方和自己都有token传给资源服务器，
             资源服务器需要区分这个token是第三方来的，还是自己来的
             这里区分，就可以用role，角色区分，因为不同的角色权限是不同的
             */

            //在已经验证的请求中，可以获取一开始给第三方赋予的角色,但UserInfoEndpoint时候，
            //ValidatedRequest会为空，
            //var a = context.ValidatedRequest.ClientClaims.FirstOrDefault(w => w.Type == "role").Value;


            //或者直接从client中获取
            var clientRole = context.Client.Claims.FirstOrDefault(_ => _.Type == "role").Value;


            //获得登录用户的ID
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
                throw new ArgumentException("Invalid subject identifier");

            var cks = await _roleManager.FindByNameAsync(clientRole);
            var myi = cks.Id;


            var myrol = await _roleManager.FindByNameAsync(clientRole); //这里的角色名称是标准化的名称，所以不用管大小写
            var myrol1 = await _roleManager.FindByNameAsync("ThirdParty");

            //获取RoleClaims
            var mycls = await _roleManager.GetClaimsAsync(myrol);


            var role_id = _roleManager.Roles.First(_ => _.Name == clientRole).Id.ToString();

            var claims = GetClaimsFromUser(user).ToList();

            claims.AddRange(new List<Claim>
            {
               new Claim(JwtClaimTypes.Role, clientRole),//返回第三方的角色
               new Claim("role_id", role_id) //角色ID
            });

            claims.AddRange(mycls);

            //var issuedClaims = claims.ToList();

            context.IssuedClaims = claims;

            //context.IssuedClaims = claims.ToList();
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                //ClaimTypes.NameIdentifier
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtClaimTypes.Name,user.UserName)
            };
            //获取用户角色，这里用户角色不返回了。在其他地方返回第三方的角色
            //var role = await _userManager.GetRolesAsync(user);
            //role.ToList().ForEach(f =>
            //{
            //    claims.Add(new Claim(JwtClaimTypes.Role, f));
            //});

            var ac = user.Email;
            //if (!string.IsNullOrWhiteSpace(user.Email))
            //    claims.Add(new Claim("name", user.Email));

            //if (!string.IsNullOrWhiteSpace(user.Name))
            //    claims.Add(new Claim("name", user.Name));

            //if (!string.IsNullOrWhiteSpace(user.LastName))
            //    claims.Add(new Claim("last_name", user.LastName));

            //if (!string.IsNullOrWhiteSpace(user.City))
            //    claims.Add(new Claim("address_city", user.City));

            //if (!string.IsNullOrWhiteSpace(user.Country))
            //    claims.Add(new Claim("address_country", user.Country));

            //if (!string.IsNullOrWhiteSpace(user.State))
            //    claims.Add(new Claim("address_state", user.State));

            //if (!string.IsNullOrWhiteSpace(user.Street))
            //    claims.Add(new Claim("address_street", user.Street));

            //if (!string.IsNullOrWhiteSpace(user.ZipCode))
            //    claims.Add(new Claim("address_zip_code", user.ZipCode));

            if (_userManager.SupportsUserEmail)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }

            if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }

            return claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }

                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.Now;
            }
        }
    }
}
