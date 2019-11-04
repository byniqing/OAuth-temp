using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IdentityServer.Date;

namespace IdentityServer
{
    public class Program
    {
        //.MigrationDbContext<ApplicationDbContext>((context, service) =>
        //{
        //    new ApplicationDbContextSeed().AsyncSpeed(context, service).Wait();
        //})
        public static void Main(string[] args)
        {
            //dotnet run /seed 
            //不想每次run都执行初始化数据，可以通过cmd args 方式
            var seed = args.Contains("/seed");
            if (seed)
            {
                args = args.Except(new[] { "/seed" }).ToArray();
            }
            var host = CreateHostBuilder(args).Build();
            if (seed)
            {
                //DbContextSeed.EnsureSeedData(host.Services);
                host.MigrationDbContext<ApplicationDbContext>((context, service) =>
                {
                    new DbContextSeed().ApplicationDbAsyncSpeed(context, service).Wait();
                    new DbContextSeed().ConfigurationDbAsyncSpeed(service).Wait();
                });
            }

            host.Run();


            //优化前
            //CreateHostBuilder(args).Build()
            //    .MigrationDbContext<ApplicationDbContext>((context, service) =>
            //    {
            //        new DbContextSeed().ApplicationDbAsyncSpeed(context, service).Wait();
            //        new DbContextSeed().ConfigurationDbAsyncSpeed(service).Wait();
            //    })
            //    .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                    webBuilder.UseUrls("http://localhost:5008");
                });
    }
}
