using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using OneForAll.EFCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using OneForAll.Core.Extension;
using Base.Host.Model;
using Base.Host.Models;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using Base.Host.Filters;
using OneForAll.Core.Upload;
using OneForAll.File;
using Base.Host.Hubs;
using Base.HttpService.Models;
using Base.Public.Models;
using MongoDB.Driver;

namespace Base.Host
{
    public class Startup
    {
        const string CORS = "Cors";
        const string AUTH = "Auth";
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

            var corsConfig = new CorsConfig();
            Configuration.GetSection(CORS).Bind(corsConfig);
            if (corsConfig.Origins.Contains("*") || !corsConfig.Origins.Any())
            {
                // 不限制跨域
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

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"系统基础服务接口文档 {version}",
                        Description = $"OneForAll Base Web API {version}"
                    });
                });


                var xmlHostFile = BASE_HOST.Append(".xml");
                var xmlAppFile = BASE_APPLICATION.Append(".xml");
                var xmlDomainFile = BASE_DOMAIN.Append(".xml");
                var xmlHostPath = Path.Combine(AppContext.BaseDirectory, xmlHostFile);
                var xmlAppPath = Path.Combine(AppContext.BaseDirectory, xmlAppFile);
                var xmlDomainPath = Path.Combine(AppContext.BaseDirectory, xmlDomainFile);
                c.IncludeXmlComments(xmlHostPath, true);
                c.IncludeXmlComments(xmlAppPath);
                c.IncludeXmlComments(xmlDomainPath);

                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Description = "身份授权，直接在下框中输入Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
            #endregion

            #region Http

            var serviceConfig = new HttpServiceConfig();
            Configuration.GetSection(HTTP_SERVICE_KEY).Bind(serviceConfig);
            var props = OneForAll.Core.Utility.ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(e.GetValue(serviceConfig).ToString());
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });

            #endregion

            #region IdentityServer4

            var authConfig = new AuthConfig();
            Configuration.GetSection(AUTH).Bind(authConfig);
            services.AddAuthentication(authConfig.Type)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = authConfig.Url;
                options.RequireHttpsMetadata = false;
            });

            #endregion

            #region SingleR
            services.AddSignalR();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(config =>
            {
                config.AllowNullDestinationValues = false;
            }, Assembly.Load(BASE_HOST));
            #endregion

            #region DI

            services.AddSingleton<HttpServiceConfig>();
            services.AddSingleton<IUploader, Uploader>();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<OneForAll_BaseContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]));

            #endregion

            #region Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"];
                options.InstanceName = Configuration["Redis:InstanceName"];
            });
            #endregion

            #region Mvc
            services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ApiModelStateFilter>();
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion

            #region MongoDb

            var mongoDbClient = new MongoClient(Configuration["MongoDb:ConnectionString"]);
            var mongoDb = mongoDbClient.GetDatabase(Configuration["MongoDb:DatabaseName"]);
            services.AddSingleton(mongoDb);

            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Http
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // 基础
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IEFCoreRepository<>));

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(OneForAll_BaseContext)).Named<DbContext>("OneForAll_BaseContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("OneForAll_BaseContext"))
               .AsImplementedInterfaces();

            var authConfig = new AuthConfig();
            Configuration.GetSection(AUTH).Bind(authConfig);
            builder.Register(s => authConfig).SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                    });
                });
            }

            DirectoryHelper.Create(Path.Combine(Directory.GetCurrentDirectory(), @"upload"));
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"upload")),
                RequestPath = new PathString("/resources"),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            });

            app.UseCors(CORS);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ApiLogMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SysArticleHub>("/SysArticleHub");
                endpoints.MapControllers();
            });
        }
    }
}
