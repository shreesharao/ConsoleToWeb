using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ConsoleToWeb.Options;
using ConsoleToWeb.Services.Abstractions;

namespace ConsoleToWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly LogOption _option = null;
        private readonly ILogger _log = null;
        private readonly INotifier _notifier = null;

        public HomeController(IOptionsSnapshot<LogOption> optionsSnapshot,ILoggerFactory loggerFactory,INotifier notifier)
        {
            Console.WriteLine($"{nameof(HomeController)} reached");

            _option = optionsSnapshot.Value;
            _log = loggerFactory.CreateLogger(typeof(HomeController));
            _notifier = notifier;
        }
        public IActionResult Index()
        {
            Console.WriteLine($"Inside HomeController constructor: { _option.Default??"value is null"}");
            Console.WriteLine($"Inside HomeController constructor: { _option.Debug.Microsoft ?? "value is null"}");
            _notifier.Notify("message sent to the Console notifier");

            using (_log.BeginScope("This message will be attached"))  //using logging scopes
            {
                _log.LogInformation("logging successful");
            }
            
            return View();
        }
    }
}