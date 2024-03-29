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
    public class Config
    {
        //https://www.jianshu.com/p/ad20944d9446
        //https://www.cnblogs.com/tianyamoon/p/9490953.html
        //https://www.processon.com/view/link/5a8fba84e4b0615ac05cc2c2
        /// <summary>
        /// 用户的身份信息
        /// </summary>
        /// <returns></returns>
        public static List<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                //如果直接用这个变量，授权界面显示的英文,不好
                //new IdentityResources.OpenId(),
                 //new IdentityResources.Profile(),
                 //new IdentityResources.Address(),
                //new IdentityResources.Email(),
                new IdentityResource{
                     Name="openid",
                     DisplayName="用户唯一标识",
                     Description="用户获取用户标识",
                     Required=true,
                     ShowInDiscoveryDocument=false //既然是必须的。就不用显示在界面
                },
                 new IdentityResource{
                     Name="profile",
                     DisplayName="获得您的昵称、头像、性别",
                     Description="一些基本信息",
                     Required=true,
                     //UserClaims=new List<string>{ JwtClaimTypes.Role}, //请求该资源的时候，必须包含该claim
                     ShowInDiscoveryDocument=false //既然是必须的。就不用显示在界面
                },
                new IdentityResource{
                    /*
                     
                     */
                    Name="offline_access", //这样客户端才能收到refresh_token
                    DisplayName="离线访问",
                    Description="用于返回refresh_token",
                    Required=true, //是否必须，如果为true ，则授权页面不能取消勾选
                    //Emphasize=true, 是否强调，默认为false 
                    //Enabled=false, //是否启用 ，默认为true
                    ShowInDiscoveryDocument=false //是否显示在界面给用户选择
                }
            };
        }
        /// <summary>
        /// 定义用户可以访问的资源
        /// </summary>
        /// <returns></returns>
        public static List<ApiResource> GetApiResources()
        {
            var test = new ApiResource("api", "Demo API", new[]
            {
                JwtClaimTypes.Subject,
                JwtClaimTypes.Email,
                JwtClaimTypes.Name,
                JwtClaimTypes.Role,
                JwtClaimTypes.PhoneNumber
            });

            var oidc = new ApiResource
            {
                Name = "用户信息", //这是资源名称
                Description = "获取用户的基本信息",
                DisplayName = "都可以是默认值",
                UserClaims = new List<string> { JwtClaimTypes.Role },
                //ApiSecrets = { new Secret("trtrt".Sha256()) }, //保存在[ApiSecrets]表中的
                //Properties = new Dictionary<string, string> { { "a", "b" } }, //保存在[dbo].[ApiProperties]表中
                //Enabled = true, //是否启用
                //作用域，对应下面的Cliet的 AllowedScopes
                Scopes = {
                     new Scope{
                        Name="comment",
                        Description="描述",
                        DisplayName="获得您的评论",
                        Required=true,
                        Emphasize=true, //是否强调
                        UserClaims=new List<string>{ JwtClaimTypes.Role}
                    },
                    new Scope{
                        Name="info", //这里是指定客户端能使用的范围名称 , 是唯一的
                        Description="描述",
                        DisplayName="获得你的好友列表",
                    }
                }
            };

            return new List<ApiResource> {
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
                AllowedGrantTypes = { GrantType.AuthorizationCode },
                //RequireConsent = true, //不现实授权页面
                //ClientClaimsPrefix = "",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Role, "thirdParty") // 第三方角色
                },
                //简写方式
                //AllowedScopes = { "openid", "profile", "email", "api" },
                /*
                 如果客户端使用的认证是
                 */
                //AllowedGrantTypes = GrantTypes.Hybrid,
                //允许客户端的作用域，包括用户信息和APi资源权限
                AllowedScopes = {
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
                    IdentityServerConstants.StandardScopes.OpenId, //直接用封装的变量也行
                                                                   //"openid", //直接用字符串也行
                    IdentityServerConstants.StandardScopes.Email,
                    //IdentityServerConstants.StandardScopes.OfflineAccess,
                    //"offline_access",
                    //"address",
                    "comment",
                    "info"
                },

                //客户端默认传过来的是这个地址，如果跟这个不一直就会异常
                /*
                   授权成功后，返回地址
                   客户端哪里触发调用的地址，就会回调当前地址
                   比如：访问admin控制器未授权，调整授权服务器成功后，就会回调到admin页面
                 */
                RedirectUris = {
                    "http://localhost:5009/signin-oidc"
                },
                FrontChannelLogoutUri = "http://localhost:5009/signout-oidc",
                //注销后重定向的地址
                PostLogoutRedirectUris = {
                    "http://localhost:5009/signout-callback-oidc"
                },
                AllowedCorsOrigins = { "http://localhost:5009" },
                /*
                 开启后，客户端才能options.Scope.Add("offline_access")
                 允许刷新tokoen
                 */
                AllowOfflineAccess = true, ////offline_access(开启refresh token)

                //刷新token过期时间固定，即比如设置3 则3天内没有刷新过就过期
                //RefreshTokenExpiration = TokenExpiration.Absolute,

                //刷新token过期时间为滑动，即比如设置3 则3天内有刷新过就从从刷新当天起从新开始为3天
                //但AbsoluteRefreshTokenLifetime 必须设置为0 。AbsoluteRefreshTokenLifetime默认是 30天
                //https://www.cnblogs.com/linys2333/p/11739225.html
                //RefreshTokenExpiration = TokenExpiration.Sliding,
                
                //AbsoluteRefreshTokenLifetime=0
                //AccessTokenLifetime = 3600, //token有效期，默认是3600秒 （一个小时）
                /*
                 这样就会把返回的profile信息包含在idtoken中
                 */
                AlwaysIncludeUserClaimsInIdToken = true,
                //AccessTokenLifetime token 过期时间

                // IdToken的有效期，默认5分钟
                //IdentityTokenLifetime = 300,
                // AccessToken的有效期，默认1小时
                //AccessTokenLifetime = 3600

                /*
                 AccessTokenLifetime = 1800,//设置AccessToken过期时间
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    
                    //RefreshTokenExpiration = TokenExpiration.Absolute,//刷新令牌将在固定时间点到期
                    AbsoluteRefreshTokenLifetime = 2592000,//RefreshToken的最长生命周期,默认30天
                    RefreshTokenExpiration = TokenExpiration.Sliding,//刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                    SlidingRefreshTokenLifetime = 3600,//以秒为单位滑动刷新令牌的生命周期。
                    //按照现有的设置，如果3600内没有使用RefreshToken，那么RefreshToken将失效。即便是在3600内一直有使用RefreshToken，RefreshToken的总生命周期不会超过30天。所有的时间都可以按实际需求调整。

                    AllowOfflineAccess = true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AllowedScopes = new List<string>
                    {
                        "api",
                        StandardScopes.OfflineAccess, //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                        StandardScopes.OpenId,//如果要获取id_token,必须在scopes中加上OpenId和Profile，id_token需要通过refresh_tokens获取AccessToken的时候才能拿到（还未找到原因）
                        StandardScopes.Profile//如果要获取id_token,必须在scopes中加上OpenId和Profile
                    }
                 */
            };
            
            #region 密码模式 ResourceOwnerPassword
            var resourceOwnerPassword = new Client
            {
                ClientId = "userinfo_pwd",
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },//客户端输入：password
                ClientSecrets = { new Secret("secret".Sha256()) },
                ClientName = "客户端名称",
                RefreshTokenUsage = TokenUsage.ReUse,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
                AllowedScopes = { "OtherInfo", "oidc1" } //
            };
            #endregion
            /*
             采用Client Credentials方式，即应用公钥、密钥方式获取Access Token，适用于任何类型应用，但通过它所获取的Access Token只能用于访问与用户无关的Open API，并且需要开发者提前向开放平台申请，成功对接后方能使用。认证服务器不提供像用户数据这样的重要资源，仅仅是有限的只读资源或者一些开放的 API。例如使用了第三方的静态文件服务，如Google Storage或Amazon S3。这样，你的应用需要通过外部API调用并以应用本身而不是单个用户的身份来读取或修改这些资源。这样的场景就很适合使用客户端证书授权,通过此授权方式获取Access Token仅可访问平台授权类的接口。

比如获取App首页最新闻列表,由于这个数据与用户无关，所以不涉及用户登录与授权,但又不想任何人都可以调用这个WebAPI，这样场景就适用[例:比如微信公众平台授权]。
             */
            #region ClientCredentials
            var clientCredentials = new Client
            {
                /******************客户端 请求对应的字段*******************
                 client_id：客户端的ID，必选
                 grant_type：授权类型，必选，此处固定值“code”
                 client_secret：客户端的密码，必选
                 scope：申请的权限范围，可选，如果传了必须是正确的，否则也不通过
                 ************************************/

                //这个Client集合里面，ClientId必须是唯一的
                ClientId = "780987652", // 客户端ID，客户端传过来的必须是这个，验证才能通过,
                AllowedGrantTypes = { GrantType.ClientCredentials },// 授权类型，指客户端可以使用的模式
                ClientSecrets = { new Secret("secret".Sha256()) }, //客户端密钥
                RequireClientSecret = false, //不验证secret ，一般是信得过的第三方

                ClientName = "客户端名称",
                Description = "描述",
                //Claims = new List<Claim> {
                //    new Claim("super","super")
                //},
                /*
                 权限范围，对应的ApiResouce，这里是客户端模式，对应的是用户资源，所以是ApiResouce
                 如果是oidc 这对应的是identityResouece，身份资源
                 所以是取决于AllowedGrantTypes的类型

                允许客户端访问的API作用域
                 */
                AllowedScopes = { "OtherInfo", "oidc1" }
            };

            #endregion
            #region 自定义
            var sms_auth_code = new Client
            {
                ClientId = "android",
                ClientSecrets = new List<Secret> {
                        new Secret("secret".Sha256())
                    },
                //Claims={ new Claim(JwtClaimTypes.Role, "Admin"),new Claim(JwtClaimTypes.Role, "System") },
                //或者
                Claims = new List<Claim>{
                            //自己内部用，设置大权限
                            //new Claim(JwtClaimTypes.Role, "Admin"),
                            new Claim(JwtClaimTypes.Role, "System"),
                    },
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AllowOfflineAccess = true,
                RequireClientSecret = false, //说明可以不传client_secret 除非很信任的程序
                AllowedGrantTypes = new List<string> { "sms_auth_code" },
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes = new List<string> {
                        //"gateway_api","user_api",
                         "OtherInfo",
                        "address",
                        "oidc1",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
            };
            #endregion
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
            };

            var js = new Client
            {
                ClientId = "3449072115",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,

                RedirectUris = { "http://localhost:5007/callback.html" },
                PostLogoutRedirectUris = { "http://localhost:5007/index.html" },
                AllowedCorsOrigins = { "http://localhost:5007" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "comment"
                }
            };
            return new List<Client> {
               oidc,
               js,
               sms_auth_code,
               //resourceOwnerPassword,
               //clientCredentials,
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
                        new Claim("Email","cnblgos@sina.com"),
                        new Claim("website","http://www.cnblogs.com"),
                        new Claim("a","admin"),
                        //ClaimTypes在System.Security.Claims中
                        new Claim(ClaimTypes.Actor,"头像"),
                        //JwtClaimTypes在IdentityModel中
                        new Claim(JwtClaimTypes.Address,"地址"),
                         //new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        //new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}

/*
集成微信：https://www.jianshu.com/p/2733f1ffb763
github:https://www.cnblogs.com/kudsu/p/11672610.html
https://www.jianshu.com/p/613ed2a9f768
https://tools.ietf.org/html/draft-ietf-oauth-device-flow-07#section-3.2
     */
