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
            services.AddAuthentication(scheme)
            //����Ҫ��֤����Ϣ
            .AddIdentityServerAuthentication(options =>
            {
                //���ƻ���˵AccessToken�䷢�ĵ�ַ��Token�л�����õ�ַ
                //��һ�λ�ȥ��֤��������ȡ������Ϣ
                options.Authority = "http://localhost:5008"; //����
                //options.ApiName = "userinfo";
                options.ApiName = "�û���Ϣ";
                options.ApiSecret = "secret";
                //options.SaveToken = true;
                options.RequireHttpsMetadata = false;//��ʱȡ��Https��֤��
            });

            //���ڲ��Ե���Ȩ,[Authorize(Policy = "ck")]
            services.AddAuthorization(options =>
            {
                //˵��������admin��role ,���ִ�Сд
                //options.AddPolicy("ck", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("Client", policy => policy.RequireClaim("Client").Build());
                //options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                //options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
                //options.AddPolicy("A_S_O", policy => policy.RequireRole("Admin", "System", "Others"));
                options.AddPolicy("client_id", policy => policy.RequireClaim("client_id")); //�������client_id
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

            app.UseAuthentication(); //��֤�м��

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
