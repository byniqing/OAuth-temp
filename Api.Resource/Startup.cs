using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Api.Resource.Authorization;
using Api.Resource.Filter;
using Api.Resource.Library;
using Api.Resource.Models;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Api.Resource
{
    /*
    ��Դ�������������api��
    ����nuget����IdentityServer4.AccessTokenValidation
    */
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
            //Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme
            var scheme = JwtBearerDefaults.AuthenticationScheme;
            //Ĭ�ϵ���֤��ʽ��Bearer��֤,�����ƴд���󣬿���ֱ���ó���
            services.AddAuthentication("Bearer")
            //����Ҫ��֤����Ϣ
            .AddIdentityServerAuthentication(options =>
            {
                //���ƻ���˵AccessToken�䷢�ĵ�ַ��Token�л�����õ�ַ
                //��һ�λ�ȥ��֤��������ȡ������Ϣ
                options.Authority = "http://localhost:5008"; //����
                //options.ApiName = "userinfo";
                /*
                 ���������û�л�ȡ�û���Ϣ��Ҳ����scope�����401���п����ж�����ɴ಻��д
                 */
                //options.ApiName = "�û���Ϣ";  //��Դ���ƣ���֤����ע�����Դ�б�����һ�£�
                //options.ApiSecret = "secret";
                //options.SaveToken = true;
                options.SupportedTokens = SupportedTokens.Both;
                options.RequireHttpsMetadata = false;//��ʱȡ��Https��֤��
            });

            //���ڲ��Ե���Ȩ,[Authorize(Policy = "ck")]
            services.AddAuthorization(options =>
            {
                //��֤Ȩ�� ��˵��������admin��role ,���ִ�Сд
                //options.AddPolicy("ck", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("Client", policy => policy.RequireClaim("Client").Build());
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));

                //https://www.helplib.com/GitHub/article_137905

                //��֤Claim ���� [Authorize(Policy = "client_id")]
                //options.AddPolicy("client_id", policy => policy.RequireClaim("client_id")); //�������client_id

                /*
                 ��д���������
                ��֤������
                 */
                options.AddPolicy("OtherInfo", policy => policy.RequireScope("OtherInfo"));
                //options.AddPolicy("oidc1", policy => policy.RequireScope("oidc1"));

                //����scope����
                options.AddPolicy("reqscope",
                    policy => policy.Requirements.Add(new PermissionRequirement("", JwtClaimTypes.Scope, "")));

                //��������ò�ͬ�Ķ������,��˼��Ҫͬʱ�������²���
                //options.AddPolicy("reqscope", policy =>
                //{
                //    //Requirements
                //    policy.RequireScope("OtherInfo");
                //    policy.RequireClaim("client_id");
                //    //policy.Requirements.Add(null);
                //    //policy.AddRequirements(new PermissionRequirement(""));
                //});
            });

            services.AddSingleton<UserStore>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddControllers();
            services.AddControllers(_ =>
            {
                _.Filters.Add(new UserOwnerFilter());
            }).AddJsonOptions(options =>
             {
                 //�Զ��巵��ʱ���ʽ
                 //options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                 //options.JsonSerializerOptions.Converters.Add(new DateTimeNullConverter()); //�ɿյ�
                 //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                 options.JsonSerializerOptions.PropertyNamingPolicy = null; //ȡ��Ĭ���շ�
             });

            //·��Сд
            services.AddRouting(options => options.LowercaseUrls = true);

            //ȫ����־��¼������
            //services.AddMvc(m => m.Filters.Add<UserOwnerFilter>());

            //����ȫ����Ȩ���ԣ�����ȫ�ֵġ��Ͳ������������Ȩ
            //Ҳ����Ҫ�ٿ����������  Authorize��ǩ
            //services.AddControllers(options =>
            //{
            //    //require scope1 or scope2
            //    var policy = ScopePolicy.Create("OtherInfo", "scope2");

            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); //��֤�м��

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
