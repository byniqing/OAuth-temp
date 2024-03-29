﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config1
    {
        /// <summary>
        /// 用户的资源信息
        /// </summary>
        /// <returns></returns>
        public static List<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource{
                    /*
                     
                     */
                    Name="offline_access", //这样客户端才能收到refresh_token
                    DisplayName="离线访问",
                    Description="用于返回refresh_token",
                      UserClaims = new List<string> { JwtClaimTypes.Role }
                    //Required=true, //是否必须，如果为true ，则授权页面不能取消勾选
                    //Emphasize=true
                }
            };
        }
        /// <summary>
        /// 定义用户可以访问的资源
        /// </summary>
        /// <returns></returns>
        public static List<ApiResource> GetApiResources()
        {
            var oidc = new ApiResource
            {
                Name = "OAuth.ApiName", //这是资源名称
                Description = "2",
                DisplayName = "33",
                UserClaims = new List<string> { JwtClaimTypes.Role },
                Scopes = {
                     new Scope{
                        Name="OtherInfo",
                        Description="描述",
                        DisplayName="获取你的其他信息",
                        UserClaims=new List<string>{ JwtClaimTypes.Role}
                    },
                    new Scope{
                        Name="oidc1", //这里是指定客户端能使用的范围名称 , 是唯一的
                        Description="描述",
                        DisplayName="获得你的个人信息，好友关系",
                        Emphasize=true,
                        Required=true,
                        //ShowInDiscoveryDocument=true,
                    },
                    new Scope{
                        Name="oidc2",
                        Description="描述",
                        DisplayName="分享内容到你的博客",
                        Emphasize=true,
                        Required=true,
                    }
                }
            };
            return new List<ApiResource> {
                /*
                 具有单个作用域的简单API，这样定义的话，作用域（scope）和Api名称（ApiName）相同
                 */
                //new ApiResource("api","描述"),

                 //如果需要更多控制，则扩展版本
                //new ApiResource{
                //    Name="userinfo", //资源名称，对应客户端的：ApiName，必须是唯一的
                //    Description="描述",
                //    DisplayName="", //显示的名称
                  
                //    //ApiSecrets =
                //    //{
                //    //    new Secret("secret11".Sha256())
                //    //},

                //    //作用域，对应下面的Cliet的 AllowedScopes
                //    Scopes={
                //        new Scope
                //        {
                //            Name = "apiInfo.read_full",
                //            DisplayName = "完全的访问权限",
                //            UserClaims={ "super" }
                //        },
                //        new Scope
                //        {
                //            Name = "apiinfo.read_only",
                //            DisplayName = "只读权限"
                //        }
                //    },
                //},
                oidc
            };
        }
        /// <summary>
        /// 客户端合法性验证
        /// 在同意授权页面显示给用户看的
        /// </summary>
        /// <returns></returns>
        public static List<Client> GetClients()
        {
            var oidc = new Client
            {
                ClientId = "Info.Client",
                ClientName = "Info客户端",
                ClientSecrets = { new Secret("secret".Sha256()) },
                ClientUri = "http://www.cnblogs.com", //客户端
                LogoUri = "https://www.cnblogs.com/images/logo_small.gif",
                /*
                  response_type(响应类型)		    Flow（流程）
                    code			        Authorization Code Flow
                    id_token		        Implicit Flow
                    id_token token		    Implicit Flow
                    code id_token		    Hybrid Flow
                    code token		        Hybrid Flow
                    code id_token token		Hybrid Flow

                Client端传的类型跟授权服务器授权类型必须一一对应
                AllowedGrantTypes模式决定了响应类型
                 */

                //允许的授权类型
                AllowedGrantTypes = { GrantType.Hybrid },
                //RequireConsent = true, //不现实授权页面
                //ClientClaimsPrefix = "",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Role,"admin")
                },

                /*
                 如果客户端使用的认证是
                 */
                //AllowedGrantTypes = GrantTypes.Hybrid,
                AllowedScopes ={
                    /*
                         Profile就是用户资料，ids 4里面定义了一个IProfileService的接口用来获取用户的一些信息，主要是为当前的认证上下文绑定claims。我们可以实现IProfileService从外部创建claim扩展到ids4里面。
                         */
                        IdentityServerConstants.StandardScopes.Profile,
                          /*
                         openid是必须要的。因为客户端接受的的是oidc
                         客户端会根据oidc和SubjectId获取用户信息，
                         所以：Profile也必须要，Profile 就是用户信息

                        如果没有Profile ，就没有办法确认身份
                         */
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                    //IdentityServerConstants.StandardScopes.OfflineAccess,
                    //"address",
                    "OtherInfo"
                    },

                //客户端默认传过来的是这个地址，如果跟这个不一直就会异常
                /*
                   授权成功后，返回地址
                   客户端哪里触发调用的地址，就会回调当前地址
                   比如：访问admin控制器未授权，调整授权服务器成功后，就会回调到admin页面
                   "http://localhost:5006/signin-oidc"
                 */
                RedirectUris = {
                     "http://localhost:5009/signin-oidc"
                },
                //注销后重定向的地址
                PostLogoutRedirectUris = {
                     "http://localhost:5009/signout-callback-oidc"
                },
                AllowOfflineAccess = true, ////offline_access(开启refresh token)

                /*
                 这样就会把返回的profile信息包含在idtoken中
                 */
                AlwaysIncludeUserClaimsInIdToken = true,
                //AccessTokenLifetime token 过期时间
            };
            var oauth = new Client
            {
                ClientId = "OAuth.Client",
                ClientName = "博客园",
                ClientSecrets = { new Secret("secret".Sha256()) },
                //AllowedGrantTypes={GrantType.Hybrid }
                AllowedGrantTypes = GrantTypes.Hybrid,
                //RequireConsent=true,
                ClientUri = "http://www.cnblogs.com", //客户端
                LogoUri = "https://www.cnblogs.com/images/logo_small.gif",

                //允许访问的资源
                AllowedScopes ={
                        "OAuth1","OAuth2","OAuth3"
                    },

                /*
                 授权成功后，返回地地址
                 登录后重定向的地址列表，可以有多个
                 */
                RedirectUris = { "http://localhost:5005/OAuth" },
                //注销后重定向的地址
                PostLogoutRedirectUris = { "http://localhost:5005" },
                //RefreshTokenUsage= TokenUsage.ReUse
                //AllowOfflineAccess = true,
                //AllowAccessTokensViaBrowser = true
            };
            return new List<Client> {
               oidc,
               //oauth
                
           };
        }

        /// <summary>
        /// 可以登录IdentityServer（我系统）的用户
        /// 也就是必须是这些用户才能使用我系统
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser
                {
                    SubjectId="1000", //用户ID
                    Username="nsky@163.com", //用户名
                    Password="123", //密码
                    /*
                     * 这里的信息就是： IdentityServerConstants.StandardScopes.Profile
                     */
                    Claims=new List<Claim>{
                        new Claim("name","nsky"),
                        new Claim("email","cnblgos@sina.com"),
                        new Claim("website","http://www.cnblogs.com"),
                        new Claim("role","admin"),
                        //在System.Security.Claims中国
                        new Claim(ClaimTypes.Actor,"头像"),
                        //在IdentityModel中
                        new Claim(JwtClaimTypes.Address,"地址"),
                    }
                }
            };
        }
    }
}
