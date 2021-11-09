using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorisationService {
    public class Program {
        public static void Main(string[] args) {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, builder) => {
                    builder.AddUserSecrets<Program>();
                });
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration(c => c.AddJsonFile("Configuration/ocelot.json"));
                });
    }
}
