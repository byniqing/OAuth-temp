using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4.AspNetIdentity;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("conn")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //配置密码注册方式
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
            });

            //注册ids中间件
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/account";
                //默认值是可以根据自己修改
                //options.UserInteraction.LoginUrl = "/Account/Login"; //没有授权，跳转的登陆页面
                //options.UserInteraction.LogoutUrl = ""; //退出页面
                //options.UserInteraction.ConsentUrl = ""; //同意授权页面
            })
            .AddDeveloperSigningCredential()//设置开发者临时签名凭据
            //in-men 方式把信息添加到内存中
           .AddInMemoryApiResources(Config.GetApiResources())
           .AddInMemoryIdentityResources(Config.GetIdentityResource())
           .AddInMemoryClients(Config.GetClients())
           //.AddTestUsers(Config.GetTestUsers())
           .AddAspNetIdentity<ApplicationUser>() //依赖nuget包：IdentityServer4.AspNetIdentity
           .AddProfileService<ProfileService>();

            services.AddControllersWithViews();
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
            app.UseCookiePolicy();
            app.UseRouting();

            /*
             授权，这是授权服务器，所以要使用授权中间件
             */
            app.UseAuthorization();

            /*
             认证，
             */
            //app.UseAuthentication();
            //使用ids中间件
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
