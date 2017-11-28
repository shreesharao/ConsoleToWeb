using System;
using Microsoft.Extensions.Logging;

namespace ConsoleToWeb.CustomLoggingProvider
{
    public class FileProvider : ILoggerProvider   //not completed
    {
        public ILogger CreateLogger(string categoryName)
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            return loggerFactory.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
