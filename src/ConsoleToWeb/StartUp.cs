using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using ConsoleToWeb.Middleware;
using Microsoft.Extensions.FileProviders;

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

            //applicationBuilder.Run(async context => await context.Response.WriteAsync("hello"));  //smallest middleware

            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseBrowserLink();
            }

            //custom middleware
            applicationBuilder.UseFilesMiddleware();

            //shows how to use mapwhen?
            applicationBuilder.MapWhen(context => context.Request.IsHttps, CallBackMethod);

            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{Controller=Home}/{Action=Index}/{id?}");
            });

            //static files middleware
            applicationBuilder.UseStaticFiles();

            //If static files is located outisde wwwroot uncommnet below code
            /*applicationBuilder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "@StaticFilesDirectory")),
                RequestPath=new PathString("/StaticFiles")

            });*/
        }

        private void CallBackMethod(IApplicationBuilder obj)
        {
            Console.WriteLine("HTTPS request");
        }

        private void OnStopping()=>Console.WriteLine($"Application stopping");
        private void OnStarted() => Console.WriteLine($"Application started");
        private void OnStopped() => Console.WriteLine($"Application stopped");
    }
}
