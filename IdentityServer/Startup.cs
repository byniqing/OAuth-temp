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
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;

namespace IdentityServer
{
    //dotnet ef ������ https://www.cnblogs.com/fengyifeng/p/11598983.html
    //https://github.com/aspnet/EntityFrameworkCore/issues/14016
    //EF Core����ʱ������SDK��һ���� ,����cli Ҫ��cmd �Ȱ�װ
    //dotnet tool install --global dotnet-ef --version 3.0.0-*  
    #region MyRegion
    /*
    Add-Migration init -Context ApplicationDbContext -Output Date\Migrations\ApplicationDb
    Add-Migration init -context ConfigurationDbContext -output Date\Migrations\ConfigurationDb
    Add-Migration init -context PersistedGrantDbContext -output Date\Migrations\PersistedGrantDb

    Update-Database -Context ApplicationDbContext
    Update-Database -Context ConfigurationDbContext
    Update-Database -Context PersistedGrantDbContext
    ------------------------------------------------
    dotnet ef migrations add -c ConfigurationDbContex -o Date\Migrations\ConfigurationDb

    dotnet ef database update -c ConfigurationDbContex

    */
    #endregion
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

            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            #region ������ֱ��3��Context�ֱ��3��db
            //���ڹ���identity�û���Ϣ��user��userRole... ASP.NET Identity
            var applicationDb = Configuration.GetConnectionString("ApplicationDb");
            // ����洢ͬ�⡢��Ȩ���롢ˢ�����ƺ��������ƣ�(codes, tokens, consents)
            var persistedGrantDb = Configuration.GetConnectionString("PersistedGrantDb");
            /*
             * �������ݿ��жԿͻ��ˡ���Դ�� CORS ���õ����ô洢��
             * client,ApiResources,IdentityResource��Ϣ
             */
            var configurationDb = Configuration.GetConnectionString("ConfigurationDb");
            #endregion


            /*
             * �ֱ�����DbContext���������3��DbContext
             * 
             */
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationDb));
            //services.AddDbContext<ConfigurationDbContext>(options => options.UseSqlServer(configurationDb));
            //services.AddDbContext<PersistedGrantDbContext>(options => options.UseSqlServer(persistedGrantDb));

            //����identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //��������ע�᷽ʽ
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
            });

            //ע��ids�м��
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/account";
                //Ĭ��ֵ�ǿ��Ը����Լ��޸�
                //options.UserInteraction.LoginUrl = "/Account/Login"; //û����Ȩ����ת�ĵ�½ҳ��
                //options.UserInteraction.LogoutUrl = ""; //�˳�ҳ��
                //options.UserInteraction.ConsentUrl = ""; //ͬ����Ȩҳ��
            })
            //.AddExtensionGrantValidator<Authertication.SmsAuthCodeValidator>()
            .AddDeveloperSigningCredential()//���ÿ�������ʱǩ��ƾ��
                                            //in-men ��ʽ����Ϣ��ӵ��ڴ���
                                            //.AddInMemoryApiResources(Config.GetApiResources())
                                            //.AddInMemoryIdentityResources(Config.GetIdentityResource())
                                            //.AddInMemoryClients(Config.GetClients())
                                            //.AddProfileService<ProfileService>() //���������Զ��巵�ص�profiel��Ϣ
                                            //.AddTestUsers(Config.GetTestUsers())
           .AddAspNetIdentity<ApplicationUser>() //����nuget����IdentityServer4.AspNetIdentity
           .AddConfigurationStore(options =>     //����nuget����IdentityServer4.EntityFramework
            {
                options.ConfigureDbContext = (build) =>
                {
                    build.UseSqlServer(configurationDb,
                    sql => sql.MigrationsAssembly(migrationAssembly)); //����Ϊ��������ά��Ǩ�Ƶĳ���ʵ�����Ա������ط�����migrations
                };
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = build =>
                {
                    build.UseSqlServer(persistedGrantDb, sql => sql.MigrationsAssembly(migrationAssembly));
                };
            })
            .AddProfileService<ProfileService>();
            //.Services.AddTransient<IProfileService, ProfileService>(); ;
            //.AddConfigurationStore<ConfigurationDbContext>(o => { });

            //.AddProfileService<ProfileService>();
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
