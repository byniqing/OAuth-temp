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
    资源服务器保护你的api。
    依赖nuget包：IdentityServer4.AccessTokenValidation
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
            //默认的认证方式是Bearer认证,如果怕拼写错误，可以直接用常量
            services.AddAuthentication("Bearer")
            //配置要验证的信息
            .AddIdentityServerAuthentication(options =>
            {
                //令牌或者说AccessToken颁发的地址，Token中会包含该地址
                //第一次会去认证服务器获取配置信息
                options.Authority = "http://localhost:5008"; //必填
                //options.ApiName = "userinfo";
                /*
                 如果第三方没有获取用户信息，也就是scope，则会401，有可能有多个，干脆不填写
                 */
                //options.ApiName = "用户信息";  //资源名称，认证服务注册的资源列表名称一致，
                //options.ApiSecret = "secret";
                //options.SaveToken = true;
                options.SupportedTokens = SupportedTokens.Both;
                options.RequireHttpsMetadata = false;//暂时取消Https验证，
            });

            //基于策略的授权,[Authorize(Policy = "ck")]
            services.AddAuthorization(options =>
            {
                //验证权限 ，说明必须是admin的role ,区分大小写
                //options.AddPolicy("ck", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("Client", policy => policy.RequireClaim("Client").Build());
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));

                //https://www.helplib.com/GitHub/article_137905

                //验证Claim 策略 [Authorize(Policy = "client_id")]
                //options.AddPolicy("client_id", policy => policy.RequireClaim("client_id")); //必须包含client_id

                /*
                 编写作用域策略
                验证作用域
                 */
                options.AddPolicy("OtherInfo", policy => policy.RequireScope("OtherInfo"));
                //options.AddPolicy("oidc1", policy => policy.RequireScope("oidc1"));

                //基于scope策略
                options.AddPolicy("reqscope",
                    policy => policy.Requirements.Add(new PermissionRequirement("", JwtClaimTypes.Scope, "")));

                //如果想配置不同的多个策略,意思是要同时满足以下策略
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
                 //自定义返回时间格式
                 //options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                 //options.JsonSerializerOptions.Converters.Add(new DateTimeNullConverter()); //可空的
                 //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                 options.JsonSerializerOptions.PropertyNamingPolicy = null; //取消默认驼峰
             });

            //路由小写
            services.AddRouting(options => options.LowercaseUrls = true);

            //全局日志记录过滤器
            //services.AddMvc(m => m.Filters.Add<UserOwnerFilter>());

            //创建全局授权策略，用了全局的。就不不用上面的授权
            //也不需要再控制器上面加  Authorize标签
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

            app.UseAuthentication(); //认证中间件

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
