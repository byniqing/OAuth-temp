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
                options.LoginPath = "/account";
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
                //options.SignInScheme = "Cookies";
                //options.SignInScheme = OpenIdConnectDefaults.DisplayName;
                options.Authority = "http://localhost:5008"; //��Ȩ��������ַ
                options.RequireHttpsMetadata = false;
                options.ClientId = "Info.Client";
                options.ClientSecret = "secret";
                //����identityserver�����Ƴ־û���cookie��
                options.SaveTokens = true;
                //options.AccessDeniedPath //����Զ�̵�¼ȡ��ҳ�棿��
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");
                //options.Scope.Add("email");
                options.Scope.Add("offline_access");
                options.Scope.Add("OtherInfo"); //apiȨ��
                options.ResponseType = OpenIdConnectResponseType.Code;// "id_token code";// OpenIdConnectResponseType.CodeIdToken;
                options.GetClaimsFromUserInfoEndpoint = true;
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
             ִ��˳����Ȩ����֤
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
