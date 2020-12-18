using System;
using System.Threading;
using AutoMapper;
using IAIOT_alpha_0_1_1.Config;
using IAIOT_alpha_0_1_1.DataListeningService;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace IAIOT_alpha_0_1_1
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
            services.AddControllers();
            //IdentitySever4身份验证服务器注入
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiScopes(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<CustomProfileService>()
                .AddDeveloperSigningCredential();

            //注入接口文档服务 - Injection interface visualization tool
            //Injection interface visualization tool
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = "AIOT Cloud platform basic interface documentation",
                    Version = "v1.0",
                    Description = "This API is the basic project device management platform for implementing the AIOT Cloud platform"
                });
            });

            //注入实体框架 - Inject EntityFramework framework
            services.AddDbContext<IAIOTCloudContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }));

            //注入Redis缓存相关命令 - Inject Redis Server
            var redisConnectionStr = Configuration.GetConnectionString("Redis:ConnectionStr");
            var redisInstanceName = Configuration.GetConnectionString("Redis:InstanceName");
            var redisDefaultDB = Configuration.GetConnectionString("Redis:DefaultDB");
            //services.AddSingleton(new RedisHelper(redisConnectionStr, redisInstanceName, int.Parse(redisDefaultDB)));

            //获取当前服务器绑定的ip地址 - acquire the current sever binds ipaddress
            services.AddSingleton(serviceProvider =>
            {
                var server = serviceProvider.GetRequiredService<IServer>();
                return server.Features.Get<IServerAddressesFeature>();
            });

            //注入AutoMapper映射框架 - Inject AutoMapper Config
            services.AddAutoMapper(typeof(AutoMapperConfig));

            //启动高性能TCP侦听核心
            int IOCPServerPort = int.Parse(Configuration.GetValue<string>("IOCPServer:Port"));
            int IOCPServerListenNum = int.Parse(Configuration.GetValue<string>("IOCPServer:ListenCount"));
            new DataCenterCore(IOCPServerPort, IOCPServerListenNum).CoreOpen();

            //注入数据服务
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ISysUserServices<TSysUsers>, SysUserServices>();
            services.AddTransient<IProjectsService<TProjects>, ProjectsService>();
            services.AddTransient<IDevicesService<TDevices>, DevicesService>();
            services.AddTransient<ISensorService<TSensors>, SensorService>();

            //Inject the Authentication 
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServerBaseAddress"];
                    options.RequireHttpsMetadata = false;
                    //set time offset to check apiresouce expire=>ClockSkew 
                    options.JwtValidationClockSkew = TimeSpan.FromSeconds(3600);
                    //options.ApiName = "Home";//不设置此参数，代表所有接口全部使用权限
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //添加IdentityServer4中间件 - Add IdentityServer4 middleware
            app.UseIdentityServer();
            //添加身份验证框架中间件 - Add Authentication middleware
            app.UseAuthentication();

            //添加接口可视化工具中间件 - Add interface visualization middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "AIOT Cloud platform interface  (V 1.0)");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
