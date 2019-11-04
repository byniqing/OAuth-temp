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
                //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                //{
                   
                //};
                //options.Scope.Clear(); //���ɾ������Ĭ�ϵ�scope
                 
                //options.AccessDeniedPath //����Զ�̵�¼ȡ��ҳ�棿��
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");

                //���������������Ҫ��Ȩ�޺��û���Ϣ��ǰ��ķ���������
                //options.Scope.Add("email");
                options.Scope.Add("offline_access");
                options.Scope.Add("OtherInfo"); //apiȨ��
                options.Scope.Add("oidc1");
                options.ResponseType = OpenIdConnectResponseType.Code;// "id_token code";// OpenIdConnectResponseType.CodeIdToken;
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
