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
using IdentityServer.Authertication;
using Microsoft.AspNetCore.Authentication.Cookies;
using IdentityServer4.EntityFramework.Options;

namespace IdentityServer
{
    //dotnet ef 有问题 https://www.cnblogs.com/fengyifeng/p/11598983.html
    //https://github.com/aspnet/EntityFrameworkCore/issues/14016
    //EF Core运行时不再是SDK的一部分 ,所有cli 要在cmd 先安装
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

            #region 我这里分别把3个Context分别存3个db
            //用于管理identity用户信息，user，userRole... ASP.NET Identity
            var applicationDb = Configuration.GetConnectionString("ApplicationDb");
            // 负责存储同意、授权代码、刷新令牌和引用令牌，(codes, tokens, consents)
            var persistedGrantDb = Configuration.GetConnectionString("PersistedGrantDb");
            /*
             * 负责数据库中对客户端、资源和 CORS 设置的配置存储；
             * client,ApiResources,IdentityResource信息
             */
            var configurationDb = Configuration.GetConnectionString("ConfigurationDb");
            #endregion


            /*
             * 分别配置DbContext，我这里把3个DbContext
             * 
             */
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationDb));

            //services.AddDbContext<ConfigurationDbContext>(options => options.UseSqlServer(configurationDb));
            //services.AddDbContext<PersistedGrantDbContext>(options => options.UseSqlServer(persistedGrantDb));

            //配置identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //配置密码注册方式
            services.Configure<IdentityOptions>(options =>
            {
                //密码是否必须包含小写
                options.Password.RequireLowercase = false;
                //密码是否必须包含非字母数字字符
                options.Password.RequireNonAlphanumeric = false;
                //密码是否必须包含大写
                options.Password.RequireUppercase = false;
                //密码是否必须包含数字
                options.Password.RequireDigit = false;
                //获取或设置密码的最小长度。默认为6。
                options.Password.RequiredLength = 1;
            });

            #region 本地用cookie
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
            })
             .AddCookie(cookie, options =>
             {
                 options.LoginPath = "/account";
             });
            #endregion

            //注册ids中间件
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/account";
                //options.UserInteraction.ErrorUrl
                //默认值是可以根据自己修改
                //options.UserInteraction.LoginUrl = "/Account/Login"; //没有授权，跳转的登陆页面
                //options.UserInteraction.LogoutUrl = ""; //退出页面
                //options.UserInteraction.ConsentUrl = ""; //同意授权页面
            })
            //.AddExtensionGrantValidator<Authertication.SmsAuthCodeValidator>()
            .AddDeveloperSigningCredential()//设置开发者临时签名凭据
                                            //in-men 方式把信息添加到内存中
                                            //.AddInMemoryApiResources(Config.GetApiResources())
                                            //.AddInMemoryIdentityResources(Config.GetIdentityResource())
                                            //.AddInMemoryClients(Config.GetClients())
                                            //.AddProfileService<ProfileService>() //配置用于自定义返回的profiel信息
                                            //.AddTestUsers(Config.GetTestUsers())
           .AddAspNetIdentity<ApplicationUser>() //依赖nuget包：IdentityServer4.AspNetIdentity
           .AddConfigurationStore(options =>     //依赖nuget包：IdentityServer4.EntityFramework
            {
                options.ConfigureDbContext = (build) =>
                {
                    build.UseSqlServer(configurationDb,
                    //配置为此上下文维护迁移的程序集实例，以便其他地方调用migrations，因为引用的类库和当前上下文，不在同一个命名空间
                    sql => sql.MigrationsAssembly(migrationAssembly)); 
                };
            })
             .AddOperationalStore(options =>
             {
                 options.ConfigureDbContext = build =>
                 {
                     build.UseSqlServer(persistedGrantDb, sql => sql.MigrationsAssembly(migrationAssembly));
                 };
             })
             //自定义扩展字段
             // .AddOperationalStore<RegisterPersistedGrant>(options =>
             //{
             //    options.ConfigureDbContext = build =>
             //    {
             //        build.UseSqlServer(persistedGrantDb, sql => sql.MigrationsAssembly(migrationAssembly));
             //    };
             //})

             //.AddExtensionGrantValidator<SmsAuthCodeValidator>()
             //.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
             .AddExtensionGrantValidator<SmsAuthCodeValidator>()
            .AddProfileService<ProfileService>();
            //.Services.AddTransient<IProfileService, ProfileService>(); ;
            //.AddConfigurationStore<aa>(o => { 
            //o.conf
            //});

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
             认证，
             */
            app.UseAuthentication();
            /*
             授权，这是授权服务器，所以要使用授权中间件
             */
            app.UseAuthorization();
            
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
