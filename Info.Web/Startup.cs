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
             ע�⣺ע��ĸ��÷���IHttpContextAccessor�ǵ���AddHttpContextAccessor Extension������
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
                 ʹ��cookie��Ϊ���ص�¼�û�
                 */
                options.DefaultScheme = cookie;
                /*
                 * ����Ҫ�û���¼ʱ�����ǽ�ʹ��cookieЭ��
                 * ����ִ��AddCookie()�Ĵ������
                 */
                options.DefaultChallengeScheme = cookie;
                /*
                 ָ��oidcΪĬ�ϵ�¼��ʽ
                 Ҳ����˵û����Ȩ��ִ��oidc��Handler 
                 */
                //options.DefaultChallengeScheme = "oidc";

                //options.DefaultAuthenticateScheme = "Cookies";
                //Ĭ��,û�е�½��oidc�����������OpenID Connect
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.DisplayName;
                //options.DefaultChallengeScheme = "oidc";
                //options.DefaultAuthenticateScheme = "oidc";

            })
             .AddCookie(cookie, options =>
            {
                /*
                 ��������¼�ɹ���û������cookie����ʱ��
                 ��ô��cookie�ǻỰ����ġ�������������رգ���һֱ����
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
            //    //options.LogoutPath = new PathString��Constants.SignOut��;
            //    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            //openid connectЭ��Ĵ������
            .AddOpenIdConnect(oidc, "OpenID Connect", options =>
            {
                var authServiceConfig = Configuration.Get<AuthServiceSettings>();
                var app = Configuration.Get<AuthServiceSettings>();

                //options.SignInScheme = "Cookies";
                //options.SignInScheme = OpenIdConnectDefaults.DisplayName;
                options.Authority = "http://localhost:5008"; //��Ȩ��������ַ
                options.RequireHttpsMetadata = false;
                options.ClientId = "5440496238";
                options.ClientSecret = "saQR67zTYy";
                //����identityserver�����Ƴ־û���cookie��
                options.SaveTokens = true;
                /*
                 ָʾ�����֤�Ự�����ڣ�����cookies��Ӧƥ��

                �����֤���Ƶġ�������Ʋ��ṩ��������Ϣ

                Ȼ��ʹ�������Ự�����ڡ�����Ĭ��������ǽ��õġ�
                 */
                //options.UseTokenLifetime = true;
                // options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);


                //options.AccessDeniedPath = "";
                //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                //{

                //};
                //options.Scope.Clear(); //���ɾ������Ĭ�ϵ�scope

                //options.AccessDeniedPath //����Զ�̵�¼ȡ��ҳ�棿��
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");

                //���������������Ҫ��Ȩ�޺��û���Ϣ��ǰ��ķ���������
                //options.Scope.Add("email");
                options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                //options.Scope.Add("offline_access");

                options.Scope.Add("comment"); //apiȨ��
                //options.Scope.Add("info");
                //options.ResponseType = OpenIdConnectResponseType.Code;// "id_token code";// OpenIdConnectResponseType.CodeIdToken;
                //����
                options.ResponseType = ResponseTypes.Code;
                /*
               ������ȥ����UserInfoEndpoint��ȡ����Ϣ��󶨵�access_token��
               */
                options.GetClaimsFromUserInfoEndpoint = true;
                /*
                 ͨ��ClaimActions.MapJsonKey("�Զ���key��","JwtClaimTypes.Subject") 
                 ��ProfileService���ص�Claimsӳ�䵽User.Claims
                 ǰ����GetClaimsFromUserInfoEndpoint=true
                 */
                options.ClaimActions.MapJsonKey("sub89", "sub");
                options.ClaimActions.MapJsonKey("subject", JwtClaimTypes.Subject);
                //options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
                //options.ClaimActions.MapJsonKey("email", "email");
                //options.ClaimActions.MapJsonKey("name", "name");
                options.ClaimActions.MapCustomJson("role", jobject => jobject.GetString("role"));
                options.ClaimActions.MapCustomJson("����Claim", all => all.ToString());
                //options.CallbackPath = $"/signin-oidc-{provider.Key}";
                //options.SignedOutCallbackPath = $"/signout-callback-oidc-{provider.Key}";
                //options.SignedOutCallbackPath = "/signin-oidc/home";
                //ע���¼�
                options.Events = new OpenIdConnectEvents
                {
                    /*
                     Զ���쳣����
                     ����Ȩ������ȡ����½����ȡ����Ȩ      
                     */
                    OnRemoteFailure = OAuthFailureHandler =>
                    {
                        //��ת��ҳ
                        OAuthFailureHandler.Response.Redirect("/");
                        OAuthFailureHandler.HandleResponse();
                        return Task.FromResult(0);
                    },
                    //https://blog.codingmilitia.com/2019/06/22/aspnet-024-from-zero-to-overkill-integrating-identityserver4-part4-back-for-front
                    //���ض�������ṩ������������֤֮ǰ����
                    //OnRedirectToIdentityProvider = context => 
                    //{
                    //    /*
                    //     �����SPAҳ�棬ֱ������ĳ��ҳ�棬���Բ���
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
             ˳�����Ҫ��������Ȩ��ͨ��
             ���˳����֤����Ȩ
             ִ��˳����֤����Ȩ����Ϊ��û����֤ͨ������û��Ҫ�ж�Ȩ����
             */
            app.UseAuthentication(); //�������֤�м��
            app.UseAuthorization(); //�������Ȩ�м��

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
