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
            services.AddControllersWithViews();
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
                options.LoginPath = "/account";
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
                //options.SignInScheme = "Cookies";
                //options.SignInScheme = OpenIdConnectDefaults.DisplayName;
                options.Authority = "http://localhost:5008"; //授权服务器地址
                options.RequireHttpsMetadata = false;
                options.ClientId = "Info.Client";
                options.ClientSecret = "secret";
                //来自identityserver的令牌持久化在cookie中
                options.SaveTokens = true;
                //options.AccessDeniedPath //定义远程登录取消页面？？
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");
                //options.Scope.Add("email");
                options.Scope.Add("offline_access");
                options.Scope.Add("OtherInfo"); //api权限
                options.ResponseType = OpenIdConnectResponseType.Code;// "id_token code";// OpenIdConnectResponseType.CodeIdToken;
                options.GetClaimsFromUserInfoEndpoint = true;
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
             执行顺序：授权，认证
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
