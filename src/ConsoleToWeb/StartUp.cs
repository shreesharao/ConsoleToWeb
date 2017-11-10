using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace ConsoleToWeb
{
    public class StartUp
    {
        public StartUp()
        {
            Configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").Build();
        }

        private IConfiguration Configuration;

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvc();

        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment,IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStarted.Register(OnStarted);
            applicationLifetime.ApplicationStopping.Register(OnStopping);
            applicationLifetime.ApplicationStopped.Register(OnStopped);


            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseBrowserLink();
            }

            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{Controller=Home}/{Action=Index}/{id?}");
            });

            applicationBuilder.UseStaticFiles();
        }

        private void OnStopping()=>Console.WriteLine($"Application stopping");
        
        private void OnStarted() => Console.WriteLine($"Application started");
        private void OnStopped() => Console.WriteLine($"Application stopped");
    }
}
