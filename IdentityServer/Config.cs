using IdentityModel;
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
                 new IdentityResources.Address(),
                new IdentityResources.Email(),
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
                ApiSecrets = { new Secret("trtrt".Sha256()) },
                Properties = new Dictionary<string, string> { { "a", "b" } },
                //Enabled = true, //是否启用
                //作用域，对应下面的Cliet的 AllowedScopes
                Scopes = {
                     new Scope{
                        Name="OtherInfo",
                        Description="描述",
                        DisplayName="获得您的评论",
                        Required=true,
                        Emphasize=true, //是否强调
                        UserClaims=new List<string>{ JwtClaimTypes.Role}
                    },
                    new Scope{
                        Name="oidc1", //这里是指定客户端能使用的范围名称 , 是唯一的
                        Description="描述",
                        DisplayName="获得你的个人信息，好友关系",
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
            var code = new Client
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
                    //"90",
                    //"address",
                    "OtherInfo",
                    "address",
                    "oidc1"
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
            };

            var ClientCredentials = new Client
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
                AllowedGrantTypes = { GrantType.ClientCredentials },
                //RequireConsent = true, //不现实授权页面
                //ClientClaimsPrefix = "",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Role, "admin")
                },

                /*
                 如果客户端使用的认证是
                 */
                //AllowedGrantTypes = GrantTypes.Hybrid,
                //允许客户端的作用域，包括用户信息和APi资源权限
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OpenId, //直接用封装的变量也行
                                                                   //"openid", //直接用字符串也行
                    IdentityServerConstants.StandardScopes.Email,
                    //IdentityServerConstants.StandardScopes.OfflineAccess,
                    //"offline_access",
                    //"90",
                    //"address",
                    "OtherInfo",
                    "address"
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
                //注销后重定向的地址
                PostLogoutRedirectUris = {
                    "http://localhost:5009/signout-callback-oidc"
                },
                /*
                 开启后，客户端才能options.Scope.Add("offline_access")
                 */
                AllowOfflineAccess = true, ////offline_access(开启refresh token)
                //AccessTokenLifetime = 3600, //token有效期，默认是3600秒 （一个小时）
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
            };
            return new List<Client> {
               code,
               new Client
                {
                    ClientId = "android",
                    ClientSecrets = new List<Secret> {
                        new Secret("secret".Sha256())
                    },
                    //Claims={ new Claim(JwtClaimTypes.Role, "Admin"),new Claim(JwtClaimTypes.Role, "System") },
                    //或者
                    Claims=new List<Claim>{
                            //自己内部用，设置大权限
                            new Claim(JwtClaimTypes.Role, "Admin"),
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
                }
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
