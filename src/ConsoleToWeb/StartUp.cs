using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using ConsoleToWeb.Middleware;
using Microsoft.Extensions.FileProviders;
using ConsoleToWeb.Options;

namespace ConsoleToWeb
{
    public class StartUp
    {
        #region properties
        private static IConfiguration Configuration;
        #endregion


        #region public methods
        public StartUp(IHostingEnvironment hostingEnvironment)
        {
            #region Building Configuration

            var configurationBuilder = new ConfigurationBuilder().
              SetBasePath(Directory.GetCurrentDirectory()).
              AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
              //AddXmlFile().
              AddEnvironmentVariables();  //add this at the end to override other settings
              //AddCommandLine(string[] args).  //This is not possible. Do this in program.cs

            if(hostingEnvironment.IsDevelopment())
            {
                configurationBuilder.AddUserSecrets("9485ea03-9089-40cd-8230-71b4d47bdbfc"); //secret is comes from .csproj
            }

            Configuration= configurationBuilder.Build();

            #endregion

            Console.WriteLine(Configuration.GetConnectionString("DefaultConnection"));
            Console.WriteLine(Configuration["Logging:Default"]);
            Console.WriteLine(Configuration.GetValue<string>("Logging:Default", defaultValue: "Debug"));  //we can specify default value
            Console.WriteLine($"UserSecrets:userName:{Configuration["UserSecrets:userName"]}");
        }
        
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //options pattern
            serviceCollection.Configure<LogOption>(Configuration.GetSection("Logging"));
            serviceCollection.Configure<UserSecretsOptions>(Configuration.GetSection("UserSecrets"));  //i am not using this class anywhere

            //adding mvc service
            serviceCollection.AddMvc();

        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment, IApplicationLifetime applicationLifetime)
        {
            #region IApplicationLifetime Usage
            applicationLifetime.ApplicationStarted.Register(OnStarted);
            applicationLifetime.ApplicationStopping.Register(OnStopping);
            applicationLifetime.ApplicationStopped.Register(OnStopped);
            #endregion

            #region IHostingEnvironment Usage
            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseBrowserLink();
                applicationBuilder.UseExceptionHandler(appplicationBuilder =>
                {
                    appplicationBuilder.Run(context => context.Response.WriteAsync("Custom exception message"));
                });

                //user secrets
                
            }
            #endregion

            #region smallest middleware
            //applicationBuilder.Run(async context => await context.Response.WriteAsync("hello"));  //smallest middleware
            #endregion

            #region custom middleware

            //custom middleware
            applicationBuilder.UseFilesMiddleware();
            #endregion

            #region Hanlder
            //In asp.net core modules and hanlders are changed to middlewares. We can configure the handler middleware based on below code where we can branch the request based on extension
            //shows how to use mapwhen?
            applicationBuilder.MapWhen(context => context.Request.IsHttps, CallBackMethod);
            #endregion

            #region MVC

            //applicationBuilder.UseMvcWithDefaultRoute();   //uses the same root as below method
            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{Controller=Home}/{Action=Index}/{id?}");
            });

            #endregion

            #region static files middleware
            //static files middleware
            applicationBuilder.UseStaticFiles();

            //If static files is located outisde wwwroot uncommnet below code
            applicationBuilder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwrootalias")),
                RequestPath = new PathString("/wwwroot"),
                OnPrepareResponse = StaticFileResponseContext =>
                {
                    StaticFileResponseContext.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }

            });
            #endregion


        }
        
        #endregion


        #region private methods
        private void CallBackMethod(IApplicationBuilder obj)
        {
            Console.WriteLine("HTTPS request");
        }

        private void OnStopping() => Console.WriteLine($"Application stopping");
        private void OnStarted() => Console.WriteLine($"Application started");
        private void OnStopped() => Console.WriteLine($"Application stopped");
        #endregion


    }
}
