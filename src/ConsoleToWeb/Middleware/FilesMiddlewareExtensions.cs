using Microsoft.AspNetCore.Builder;

namespace ConsoleToWeb.Middleware
{
    public static class FilesMiddlewareExtensions
    {
        public static IApplicationBuilder UseFilesMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<FilesMiddleware>();
        }
    }
}
