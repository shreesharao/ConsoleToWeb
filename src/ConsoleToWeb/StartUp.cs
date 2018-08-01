using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using ConsoleToWeb.Middleware;
using Microsoft.Extensions.FileProviders;
using ConsoleToWeb.Options;
using ConsoleToWeb.Services.Abstractions;
using ConsoleToWeb.Services.Implementations;
using ConsoleToWeb.Models;

namespace ConsoleToWeb
{
    public class Startup
    {
        #region properties
        private static IConfiguration Configuration;
        #endregion


        #region public methods
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            #region Building Configuration

            var configurationBuilder = new ConfigurationBuilder().
              SetBasePath(Directory.GetCurrentDirectory()).
              AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
              //AddXmlFile().
              AddEnvironmentVariables();  //add this at the end to override other settings
                                          //AddCommandLine(string[] args).  //This is not possible. Do this in program.cs

            if (hostingEnvironment.IsDevelopment())
            {
                configurationBuilder.AddUserSecrets("9485ea03-9089-40cd-8230-71b4d47bdbfc"); //secret id comes from .csproj
            }

            Configuration = configurationBuilder.Build();

            #endregion

            Console.WriteLine($"{this}:01:{Configuration.GetConnectionString("DefaultConnection")}");
            Console.WriteLine($"{this}:02:{Configuration["Logging:Default"]}");
            Console.WriteLine($"{this}:03:{Configuration.GetValue<string>("Logging:Default", defaultValue: "Debug")}");  //we can specify default value
            Console.WriteLine($"{this}:04:UserSecrets:userName:{Configuration["UserSecrets:userName"]}");
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //options pattern
            serviceCollection.Configure<LogOption>(Configuration.GetSection("Logging"));
            serviceCollection.Configure<UserSecretsOptions>(Configuration.GetSection("UserSecrets"));  //i am not using this class anywhere

            //adding mvc service
            serviceCollection.AddMvc();

            //adding logging service
            serviceCollection.AddLogging(logBuilder =>
            {
                logBuilder.
                            AddConsole(options=>options.IncludeScopes=true).   //without adding the provider messages will not be logged.
                            AddConfiguration(Configuration.GetSection("Logging")).   //adding the filter from configuration
                            AddFilter<Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider>(category: "Microsoft", level:LogLevel.None).  //adding the filter usng code.this overrides configuration
                            //AddProvider().
                            //AddEventLog(). --will not work targeeting .net core
                            SetMinimumLevel(LogLevel.Information);
            });

            #region Custom services
            //there is no way to resolve a service to a specific implementation like in Unity container.
            //check SO -https://stackoverflow.com/questions/39174989/how-to-register-multiple-implementations-of-the-same-interface-in-asp-net-core
            serviceCollection.AddScoped<INotifier, EmailNotifier>();
            serviceCollection.AddScoped<INotifier, ConsoleNotifier>();
            
            //serviceCollection.AddSingleton(typeof(INotifier),typeof(ConsoleNotifier));
            #endregion

            #region Database service
            serviceCollection.AddDbContext<EmployeeModel>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLServerConnection"));

            });
            #endregion
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
