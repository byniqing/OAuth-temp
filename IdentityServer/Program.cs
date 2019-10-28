using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IdentityServer.Data;

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
            CreateHostBuilder(args).Build()
                .MigrationDbContext<ApplicationDbContext>((context, service) =>
                {
                    new ApplicationDbContextSeed().AsyncSpeed(context, service).Wait();
                })
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5008");
                });
    }
}
