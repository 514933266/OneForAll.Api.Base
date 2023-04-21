using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sys.Host
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args) =>
            CreateWebHostBuilder(args).Build().Run();

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                webBuilder.UseIIS();
                webBuilder.UseStartup<Startup>();
            });
    }
#pragma warning restore CS1591
}
