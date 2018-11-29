using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DidSayIt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => { options.Listen(IPAddress.Any, 5000);
                    options.Listen(IPAddress.Any, 5001,
                        configure =>
                        {
                            configure.UseHttps(Environment.GetEnvironmentVariable("certname") ?? throw new Exception("Pfx cert required."),
                                Environment.GetEnvironmentVariable("certpass") ?? throw new Exception("Pfx password required."));
                        });
                })
                .UseStartup<Startup>();
    }
}
