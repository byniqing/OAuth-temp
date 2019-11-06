using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Date
{
    /// <summary>
    /// 数据初始化
    /// </summary>
    public class DbContextSeed
    {
        private UserManager<ApplicationUser> _userManger;
        private RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="server">依赖注入的容器，这里可以获取依赖注入</param>
        /// <returns></returns>
        public async Task ApplicationDbAsyncSpeed(ApplicationDbContext context, IServiceProvider server)
        {
            try
            {
                _userManger = server.GetRequiredService<UserManager<ApplicationUser>>();
                _roleManager = server.GetService<RoleManager<ApplicationRole>>();
                var logger = server.GetRequiredService<ILogger<ApplicationDbContext>>();
                logger.LogInformation("speed Init");

                //创建角色， context.Roles.Any
                if (!_roleManager.Roles.Any())
                {
                    var role = new ApplicationRole
                    {
                        Name = "Admin",
                        NormalizedName = "Admin"
                    };
                    var roleResult = await _roleManager.CreateAsync(role);
                    await _roleManager.CreateAsync(new ApplicationRole { Name="System",NormalizedName="System"});

                    //第三方
                    var trole = new ApplicationRole
                    {
                        Name = "ThirdParty",
                        NormalizedName = "thirdParty"
                    };
                    var third =  await _roleManager.CreateAsync(trole);
                    //action 指动作，意思是该角色可以做的事情，不管是是CRUD，都可以理解为一个action
                    await _roleManager.AddClaimAsync(trole, new Claim("action", "api/Identity/OtherInfo"));
                    await _roleManager.AddClaimAsync(trole, new Claim("action", "api/Identity/oidc1"));
                }

                //if (!context.Users.Any())
                //{
                //    context.Users.AddRange(useCustomizationData
                //        ? GetUsersFromFile(contentRootPath, logger)
                //        : GetDefaultUser());

                //    await context.SaveChangesAsync();
                //}

                //https://www.cnblogs.com/rocketRobin/p/9070684.html
                //如果没有用户，则创建一个
                if (!_userManger.Users.Any())
                {
                    var defaultUser = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "cnblogs@163.com",
                        //PasswordHash = "",
                        //Avatar = "https://www.cnblogs.com/images/logo_small.gif",
                        SecurityStamp = "cnblogs", //设置密码的加密key
                    };
                    var userResult = await _userManger.CreateAsync(defaultUser, "123456");

                    if (userResult.Succeeded)
                    {

                        //把用户添加到角色权限组
                        //await _userManger.AddToRoleAsync(defaultUser, "admin"); //添加一个角色
                        await _userManger.AddToRolesAsync(defaultUser, new[] { "Admin", "System" }); //添加多个角色
                        if (!userResult.Succeeded)
                        {
                            logger.LogError("创建失败");
                            //logger.LogInformation("初始化用户失败");
                            userResult.Errors.ToList().ForEach(e =>
                            {
                                logger.LogError(e.Description);
                            });
                        }


                        //可以添加用户的claim
                        var result11 = _userManger.AddClaimsAsync(defaultUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "姓名"),
                            new Claim(JwtClaimTypes.Email, "cnblogs@163.com"),
                            new Claim(JwtClaimTypes.Role, "admin")
                        }).Result;
                    }
                    else
                    {
                        logger.LogError("创建失败:" + userResult.Errors);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception("初始化用户失败" + ex.Message);
            }
        }

        /// 因为现在没有通过UI去录入api,client等信息
        /// 所有可以先init一些默认信息写入数据库
        /// </summary>
        /// <param name="app"></param>
        public async Task ConfigurationDbAsyncSpeed(IServiceProvider server)
        {
            try
            {
                //ApplicationServices返回的就是IServiceProvider，依赖注入的容器
                using (var scope = server.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

                    logger.LogInformation("ConfigurationDbAsyncSpeed Init");

                    //Update-Database
                    scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                    //添加
                    //var grant = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
                    //grant.PersistedGrants.Add(new IdentityServer4.EntityFramework.Entities.PersistedGrant
                    //{
                    //    ClientId = ""
                    //});
                    //grant.DeviceFlowCodes.Add(new IdentityServer4.EntityFramework.Entities.DeviceFlowCodes { 

                    //});

                    var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                    /*
                     相当于手动执行 Update-Database -Context PersistedGrantDbContext
                     执行多次不会异常，如果有迁移更新，则会更新，没有迁移更新，也不会异常
                     */
                    configurationDbContext.Database.Migrate();

                    //client 第三方客户端信息
                    if (!configurationDbContext.Clients.Any())
                    {
                        foreach (var client in Config.GetClients())
                        {
                            //client.ToEntity() 会把当前实体映射到EF实体
                            configurationDbContext.Clients.Add(client.ToEntity());
                        }
                        await configurationDbContext.SaveChangesAsync();
                    }

                    //ApiResources Api资源信息
                    if (!configurationDbContext.ApiResources.Any())
                    {
                        foreach (var api in Config.GetApiResources())
                        {
                            configurationDbContext.ApiResources.Add(api.ToEntity());
                        }
                        await configurationDbContext.SaveChangesAsync();
                    }

                    //IdentityResources 身份信息
                    if (!configurationDbContext.IdentityResources.Any())
                    {
                        foreach (var identity in Config.GetIdentityResource())
                        {
                            configurationDbContext.IdentityResources.Add(identity.ToEntity());
                        }
                        await configurationDbContext.SaveChangesAsync();
                    }

                    logger.LogInformation("ConfigurationDbAsyncSpeed end");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConfigurationDbAsyncSpeed 初始化用户失败" + ex.Message);
            }
        }

        public static void EnsureSeedData(IServiceProvider server)
        {
            throw new Exception("初始化用户失败");
        }
    }
}
