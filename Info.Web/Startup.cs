using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using Newtonsoft.Json.Linq;
using Info.Date;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static IdentityModel.OidcConstants;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Info.AuthTokenHelpers;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using Info.Configuration;
using System.Net.Http;
using Authentication.GitHub;

namespace Info
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //    services
            //.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            /*
             注意：注册的更好方法IHttpContextAccessor是调用AddHttpContextAccessor Extension方法，
             */
            //services.AddHttpContextAccessor();

            //services.AddHttpClient();
            //services.Configure<AuthServiceSettings>(Configuration);

            //services.add
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                return configuration.GetSection("AuthServiceSettings");
            });

            services.AddSingleton<IDiscoveryCache>(serviceProvider =>
            {
                //var authServiceConfig = serviceProvider.GetRequiredService<AuthServiceSettings>();
                //var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                //return new DiscoveryCache(authServiceConfig.Authority, () => factory.CreateClient());

                //var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache("http://localhost:5008");
            });

            services
                .AddTransient<CustomCookieAuthenticationEvents>()
                .AddTransient<ITokenRefresher, TokenRefresher>()
                .AddTransient<AccessTokenHttpMessageHandler>()
                .AddTransient<HttpClient>()
                .AddHttpContextAccessor();

            //services
            //    .AddHttpClient<ITokenRefresher, TokenRefresher>();


            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            /*
              Add-Migration init -Context InfoDbContext
              Update-Database -Context InfoDbContext
             */
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddControllersWithViews();
            services.AddDbContext<InfoDbContext>(_ =>
            {
                _.UseSqlServer(Configuration.GetConnectionString("InfoDb"));
            });
            var oidc = OpenIdConnectDefaults.DisplayName; //OpenIdConnect
            var cookie = CookieAuthenticationDefaults.AuthenticationScheme;
            services.AddAuthentication(options =>
            {
                /*
                 使用cookie作为本地登录用户
                 */
                options.DefaultScheme = cookie;
                /*
                 * 当需要用户登录时，我们将使用cookie协议
                 * 将会执行AddCookie()的处理程序
                 */
                options.DefaultChallengeScheme = cookie;
                /*
                 指定oidc为默认登录方式
                 也就是说没有授权则执行oidc的Handler 
                 */
                //options.DefaultChallengeScheme = "oidc";

                //options.DefaultAuthenticateScheme = "Cookies";
                //默认,没有登陆走oidc处理，即下面的OpenID Connect
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.DisplayName;
                //options.DefaultChallengeScheme = "oidc";
                //options.DefaultAuthenticateScheme = "oidc";

            })
             .AddCookie(cookie, options =>
            {
                /*
                 第三方登录成功，没有设置cookie过期时间
                 那么该cookie是会话级别的。即。浏览器不关闭，会一直在线
                 */
                options.LoginPath = "/account";
                //options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
                //options.EventsType = typeof(CustomCookieAuthenticationEvents);
            })
            // .AddMicrosoftAccount(microsoftOptions =>
            //{
            //    //https://www.netnr.com/home/list/109
            //    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
            //    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            //})
             .AddGitHub(GitHubDefaults.AuthenticationScheme, GitHubDefaults.DisplayName, options =>
             {
                 options.ClientId = "8b4e1c7979b3b9705109";
                 options.ClientSecret = "643aa3afb7b5a5d7e38685dd8be308278fc506d5";
             })
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
            //{
            //    //options.LoginPath = new PathString(Constants.SignIn);
            //    //options.LogoutPath = new PathString（Constants.SignOut）;
            //    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            //openid connect协议的处理程序
            .AddOpenIdConnect(oidc, "OpenID Connect", options =>
            {
                var authServiceConfig = Configuration.Get<AuthServiceSettings>();
                var app = Configuration.Get<AuthServiceSettings>();

                //options.SignInScheme = "Cookies";
                //options.SignInScheme = OpenIdConnectDefaults.DisplayName;
                options.Authority = "http://localhost:5008"; //授权服务器地址
                options.RequireHttpsMetadata = false;
                options.ClientId = "5440496238";
                options.ClientSecret = "saQR67zTYy";
                //来自identityserver的令牌持久化在cookie中
                options.SaveTokens = true;
                /*
                 指示身份验证会话生存期（例如cookies）应匹配

                身份验证令牌的。如果令牌不提供生存期信息

                然后将使用正常会话生存期。这在默认情况下是禁用的。
                 */
                //options.UseTokenLifetime = true;
                // options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);


                //options.AccessDeniedPath = "";
                //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                //{

                //};
                //options.Scope.Clear(); //这会删除所有默认的scope

                //options.AccessDeniedPath //定义远程登录取消页面？？
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");

                //向服务器发起，我想要的权限和用户信息，前提的服务器允许
                //options.Scope.Add("email");
                options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                //options.Scope.Add("offline_access");

                options.Scope.Add("comment"); //api权限
                //options.Scope.Add("info");
                //options.ResponseType = OpenIdConnectResponseType.Code;// "id_token code";// OpenIdConnectResponseType.CodeIdToken;
                //或者
                options.ResponseType = ResponseTypes.Code;
                /*
               这样会去请求UserInfoEndpoint获取到信息后绑定到access_token中
               */
                options.GetClaimsFromUserInfoEndpoint = true;
                /*
                 通过ClaimActions.MapJsonKey("自定义key名","JwtClaimTypes.Subject") 
                 把ProfileService返回的Claims映射到User.Claims
                 前提是GetClaimsFromUserInfoEndpoint=true
                 */
                options.ClaimActions.MapJsonKey("sub89", "sub");
                options.ClaimActions.MapJsonKey("subject", JwtClaimTypes.Subject);
                //options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
                //options.ClaimActions.MapJsonKey("email", "email");
                //options.ClaimActions.MapJsonKey("name", "name");
                options.ClaimActions.MapCustomJson("role", jobject => jobject.GetString("role"));
                options.ClaimActions.MapCustomJson("所有Claim", all => all.ToString());
                //options.CallbackPath = $"/signin-oidc-{provider.Key}";
                //options.SignedOutCallbackPath = $"/signout-callback-oidc-{provider.Key}";
                //options.SignedOutCallbackPath = "/signin-oidc/home";
                //注册事件
                options.Events = new OpenIdConnectEvents
                {
                    /*
                     远程异常触发
                     在授权服务器取消登陆或者取消授权      
                     */
                    OnRemoteFailure = OAuthFailureHandler =>
                    {
                        //跳转首页
                        OAuthFailureHandler.Response.Redirect("/");
                        OAuthFailureHandler.HandleResponse();
                        return Task.FromResult(0);
                    },
                    //https://blog.codingmilitia.com/2019/06/22/aspnet-024-from-zero-to-overkill-integrating-identityserver4-part4-back-for-front
                    //在重定向到身份提供程序进行身份验证之前调用
                    //OnRedirectToIdentityProvider = context => 
                    //{
                    //    /*
                    //     如果是SPA页面，直接请求某个页面，可以捕获
                    //     */
                    //    if (!context.HttpContext.Request.Path.StartsWithSegments("/auth/login"))
                    //    {
                    //        context.HttpContext.Response.StatusCode = 401;
                    //        context.HandleResponse();
                    //    }
                    //    return Task.CompletedTask;
                    //}
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            /*
             顺序很重要，否则授权不通过
             添加顺序：认证，授权
             执行顺序：认证，授权，因为都没有认证通过，就没必要判断权限了
             */
            app.UseAuthentication(); //先添加认证中间件
            app.UseAuthorization(); //在添加授权中间件

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
