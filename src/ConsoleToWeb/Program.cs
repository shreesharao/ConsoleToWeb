using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ConsoleToWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            //StartWebHost();
            BuildWebHostUsingWebHostBuilder().Run();
            //BuildWebHostUsingDefaultBuilder().Run();
            //BuildWebHostUsingConfigurationBuilder(args).Run();

        }

        private static void StartWebHost()
        {
            using (var host = WebHost.Start(app => app.Response.WriteAsync("hello")))
            {
                Console.WriteLine("started");
                host.WaitForShutdown();
            }
        }

        private static IWebHost BuildWebHostUsingWebHostBuilder()
        {
            var webHostBuilder = new WebHostBuilder();

            return webHostBuilder.
                                CaptureStartupErrors(true).
                                UseContentRoot(Directory.GetCurrentDirectory()).
                                 UseUrls("http://localhost:3510").  //can not have https
                                 //UseHttpSys().
                                UseKestrel(options:options=> {
                                    
                                }).   //overrides previous setting
                                UseIISIntegration().
                                UseStartup<StartUp>().
                                UseSetting(WebHostDefaults.PreventHostingStartupKey, "true").  //could not see it working
                              
                                Build();

        }

        private static IWebHost BuildWebHostUsingDefaultBuilder()
        {
            return WebHost.CreateDefaultBuilder().
                UseStartup<StartUp>().
                Build();
        }

        private static IWebHost BuildWebHostUsingConfigurationBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder().
                                                        SetBasePath(Directory.GetCurrentDirectory()).
                                                        AddJsonFile("appsettings.json", optional: true).
                                                        AddCommandLine(args).  //use dotnet watch run --urls http://localhost:3000 to overrride urls
                                                        Build();

            return WebHost.CreateDefaultBuilder().
                                                 UseStartup<StartUp>().
                                                 UseConfiguration(configuration).
                                                 Build();

        }


    }
}
