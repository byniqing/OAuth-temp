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
using IdentityServer.Date;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ApplicationDb")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsFactory>();


            //ע��ids�м��
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/account";
                //Ĭ��ֵ�ǿ��Ը����Լ��޸�
                //options.UserInteraction.LoginUrl = "/Account/Login"; //û����Ȩ����ת�ĵ�½ҳ��
                //options.UserInteraction.LogoutUrl = ""; //�˳�ҳ��
                //options.UserInteraction.ConsentUrl = ""; //ͬ����Ȩҳ��
            })
            .AddDeveloperSigningCredential()//���ÿ�������ʱǩ��ƾ��
            //in-men ��ʽ����Ϣ���ӵ��ڴ���
           .AddInMemoryApiResources(Config.GetApiResources())
           .AddInMemoryIdentityResources(Config.GetIdentityResource())
           .AddInMemoryClients(Config.GetClients())
           //.AddInMemoryPersistedGrants()
           //.AddTestUsers(Config.GetTestUsers());
           .AddAspNetIdentity<ApplicationUser>() //����nuget����IdentityServer4.AspNetIdentity
           .AddProfileService<ProfileService>();

            //.Services.AddScoped<IProfileService, ProfileService>();
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>()
            //   .AddDefaultTokenProviders()
            //   .AddClaimsPrincipalFactory<UserClaimsFactory>();

            //services.AddIdentity<ApplicationUser, ApplicationRole>()
            // .AddEntityFrameworkStores<ApplicationDbContext>()
            // .AddDefaultTokenProviders()
            // .AddClaimsPrincipalFactory<UserClaimsFactory>();

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
             ��Ȩ��������Ȩ������������Ҫʹ����Ȩ�м��
             */
            app.UseAuthorization();

            /*
             ��֤��
             */
            //app.UseAuthentication();
            //ʹ��ids�м��
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