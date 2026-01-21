using Autofac;
using Autofac.Core;
using Base.Host.Filters;
using Base.Host.Model;
using Base.Host.Models;
using Base.Host.Providers;
using Base.HttpService.Models;
using Base.Public.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneForAll.Core.Extension;
using OneForAll.Core.UploadFile;
using OneForAll.File;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Host
{
    public class Startup
    {
        const string CORS = "Cors";
        const string AUTH = "Auth";
        const string QUARTZ = "Quartz";
        const string BASE_HOST = "Base.Host";
        const string BASE_APPLICATION = "Base.Application";
        const string BASE_DOMAIN = "Base.Domain";
        const string BASE_REPOSITORY = "Base.Repository";
        private readonly string HTTP_SERVICE_KEY = "HttpService";
        private readonly string HTTP_SERVICE = "Base.HttpService";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors

            // 读取配置文件中的 CORS（跨域）设置
            var corsConfig = Configuration.GetSection(CORS).Get<CorsConfig>();

            if (corsConfig.Origins.Contains("*") || !corsConfig.Origins.Any())
            {
                // 添加跨域策略：允许所有来源、所有请求头、所有HTTP方法
                services.AddCors(option => option.AddPolicy(CORS, policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                ));
            }
            else
            {
                services.AddCors(option => option.AddPolicy(CORS, policy => policy
                    .WithOrigins(corsConfig.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod().
                    AllowCredentials()
                ));
            }

            #endregion

            #region Jwt

            var authConfig = Configuration.GetSection(AUTH).Get<AuthConfig>();
            services.AddSingleton(authConfig);

            // 1. 添加 JWT Bearer 认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false, // 由Jwt Role + 菜单权限校验，为保证系统简洁性，暂时不验证aud
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authConfig.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.JwtKey))
                };
            });

            // 2. 添加授权
            services.AddAuthorization();

            #endregion

            #region Http

            // 读取 HTTP 客户端相关配置
            var serviceConfig = Configuration.GetSection(HTTP_SERVICE_KEY).Get<HttpServiceConfig>();
            // 使用反射获取配置类的所有属性
            var props = OneForAll.Core.Utility.ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                var url = e.GetValue(serviceConfig)?.ToString();
                // 跳过没配置Url
                if (url.IsNullOrEmpty())
                    return;

                // 为每一个非空的服务地址注册一个命名的 HttpClient
                // 并设置 BaseAddress 和默认请求头（如 ClientId）
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(url);
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });

            // 将 HttpServiceConfig 作为单例注入，供后续使用
            services.AddSingleton<HttpServiceConfig>();

            #endregion

            #region AutoMapper
            services.AddAutoMapper(config =>
            {
                config.AllowNullDestinationValues = false;
            }, Assembly.Load(BASE_HOST));
            #endregion

            #region DI

            services.AddSingleton<HttpServiceConfig>();
            services.AddScoped<IUploader, Uploader>();
            services.AddScoped<ITenantProvider, TenantProvider>();

            #endregion

            #region Mvc

            // 注入 HttpContextAccessor，便于在服务中访问当前 HTTP 上下文
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 添加 MVC 控制器支持，并禁用端点路由（兼容旧版 MVC 路由）
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            })
            // 使用 Newtonsoft.Json 进行 JSON 序列化配置
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // 启用 PascalCase 命名
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";        // 统一日期格式
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
            });

            #endregion

            #region Redis

            // 读取 Redis 配置
            var redisOption = Configuration.GetSection("Redis").Get<RedisOption>();
            if (redisOption?.IsEnabled == true)
            {
                // 如果启用了 Redis，则使用 StackExchangeRedis 作为分布式缓存
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisOption.ConnectionString; // 连接字符串
                    options.InstanceName = redisOption.InstanceName;      // 实例名前缀
                    if (!redisOption.Password.IsNullOrEmpty())
                        options.ConfigurationOptions.Password = redisOption.Password; // 密码（如有）
                });
            }
            else
            {
                // 否则使用内存缓存（适用于开发或单机环境）
                services.AddDistributedMemoryCache();
            }

            #endregion

            #region Quartz

            // 读取 Quartz 定时任务配置
            var quartzConfig = Configuration.GetSection(QUARTZ).Get<QuartzScheduleJobConfig>();
            services.AddSingleton(quartzConfig); // 注册配置为单例

            // 注册自定义 Job 工厂和调度器工厂
            services.AddSingleton<IJobFactory, ScheduleJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // 如果定时任务功能已启用
            if (quartzConfig != null && quartzConfig.IsEnabled)
            {
                // 添加后台服务来启动和管理 Quartz 调度器
                services.AddHostedService<QuartzJobHostedService>();
                // 加载程序集（通过反射加载任务类型）
                var assembly = Assembly.Load(BASE_HOST);
                // 构建任务所在的命名空间（例如：YourApp.QuartzJobs）
                var jobNamespace = $"{BASE_HOST}.QuartzJobs";

                // 遍历配置中列出的每个定时任务
                quartzConfig.ScheduleJobs.ForEach(e =>
                {
                    var typeName = $"{jobNamespace}.{e.TypeName}";
                    var jobType = assembly.GetType(typeName);
                    if (jobType != null)
                    {
                        e.JobType = jobType;
                        // 将任务类型注册为单例（供 JobFactory 创建实例）
                        services.AddSingleton(e.JobType);
                    }
                });
            }

            #endregion

            #region MongoDb

            var mongoDbOption = Configuration.GetSection("MongoDb").Get<MongoDbOption>();
            if (mongoDbOption?.IsEnabled == true)
            {
                services.AddSingleton(_ => new MongoClient(mongoDbOption.ConnectionString));

                services.AddSingleton(provider =>
                {
                    var client = provider.GetRequiredService<MongoClient>();
                    return client.GetDatabase(mongoDbOption.DatabaseName);
                });
            }

            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Http数据服务
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // 应用层
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            // 领域层
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            // 仓储层
            builder.Register(p =>
            {
                var optionBuilder = new DbContextOptionsBuilder<SysDbContext>();
                optionBuilder.UseSqlServer(Configuration["ConnectionStrings:Default"]);
                return optionBuilder.Options;
            }).AsSelf();

            builder.RegisterType<SysDbContext>().Named<DbContext>("SysDbContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("SysDbContext"))
               .AsImplementedInterfaces();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 如果当前环境是开发环境，则启用开发者异常页面，便于调试错误
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 创建用于文件上传的目录（upload），如果不存在则自动创建
            DirectoryHelper.Create(Path.Combine(Directory.GetCurrentDirectory(), @"upload"));

            // 启用静态文件服务，指定特定目录和访问路径
            app.UseStaticFiles(new StaticFileOptions()
            {
                // 指定静态文件的物理路径为项目根目录下的 "upload" 文件夹
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"upload")),
                // 设置该目录的访问路径为 "/resources"
                RequestPath = new PathString("/resources"),
                // 在响应前添加响应头，允许跨域访问（CORS）
                OnPrepareResponse = (context) =>
                {
                    context.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                }
            });

            // 启用默认文件支持（例如访问目录时自动查找 index.html 等默认页）
            app.UseDefaultFiles();

            // 启用路由中间件，为后续的端点映射做准备
            app.UseRouting();

            // 启用跨域策略（CORS），策略名称为 CORS（需在 ConfigureServices 中定义）
            app.UseCors(CORS);

            // 启用身份认证中间件
            app.UseAuthentication();

            // 启用授权中间件（检查用户是否有权限访问资源）
            app.UseAuthorization();

            // 使用自定义中间件：全局异常处理
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // 使用自定义中间件：API 请求日志记录
            app.UseMiddleware<ApiLogMiddleware>();

            // 配置终结点路由，将控制器（Controllers）映射到请求管道
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // 映射所有控制器的路由
            });
        }
    }
}
