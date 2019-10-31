using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            services.AddAuthentication(scheme)
            //配置要验证的信息
            .AddIdentityServerAuthentication(options =>
            {
                //令牌或者说AccessToken颁发的地址，Token中会包含该地址
                //第一次会去认证服务器获取配置信息
                options.Authority = "http://localhost:5008"; //必填
                //options.ApiName = "userinfo";
                options.ApiName = "用户信息";
                options.ApiSecret = "secret";
                //options.SaveToken = true;
                options.RequireHttpsMetadata = false;//暂时取消Https验证，
            });

            //基于策略的授权,[Authorize(Policy = "ck")]
            services.AddAuthorization(options =>
            {
                //说明必须是admin的role ,区分大小写
                //options.AddPolicy("ck", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("Client", policy => policy.RequireClaim("Client").Build());
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                //options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
                options.AddPolicy("client_id", policy => policy.RequireClaim("client_id")); //必须包含client_id
            });

            services.AddControllers();
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
                endpoints.MapControllers();
            });
        }
    }
}
