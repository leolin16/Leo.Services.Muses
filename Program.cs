using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Leo.Services.Muses
{
    public class Program
    {
        public static void Main(string[] args)
        {
			CreateHostBuilder(args).Build().EnsureSeedDataForMuses().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        // public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseStartup<Startup>()
        //         .UseKestrel(options => {
        //             options.Listen(IPAddress.Loopback, 5070);
        //             // options.Listen(IPAddress.Loopback, 5055, listenOptions => { listenOptions.UseHttps("../../sharedassets/certificates/LeolinLocalhost.pfx", "localhost"); });
        //         });
        //         // .Build();
    }
}
