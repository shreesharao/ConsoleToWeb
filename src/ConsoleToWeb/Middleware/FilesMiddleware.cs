using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace ConsoleToWeb
{
    public class FilesMiddleware
    {
        private readonly RequestDelegate _next;

        public FilesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            // context.Response.WriteAsync("Files returned");
            System.Console.WriteLine("Files middleware reached");
            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }

    }
}