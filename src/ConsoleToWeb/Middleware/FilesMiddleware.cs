using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ConsoleToWeb.Options;
namespace ConsoleToWeb
{
    public class FilesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LogOption _options;

        public FilesMiddleware(RequestDelegate next,IOptionsMonitor<LogOption> optionsMonitor, IOptionsSnapshot<LogOption> optionsSnapshot)
        {
            Console.WriteLine($"{nameof(FilesMiddleware)} reached");
            _next = next;
            _options = optionsSnapshot.Value;
            optionsMonitor.OnChange(ChangeListner); //this is invoked when we save appsettings.json

        }

        private void ChangeListner(LogOption arg1, string arg2)
        {
            Console.WriteLine($"NotifyChange :{arg1.Default}:{arg2?.ToString()}");
        }

        public Task Invoke(HttpContext context)
        {
            // context.Response.WriteAsync("Files returned");
            Console.WriteLine("Files middleware Invoked");
            Console.WriteLine($"Inside Files middleware {_options?.Default?? "value is null"}");  
            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }

    }
}