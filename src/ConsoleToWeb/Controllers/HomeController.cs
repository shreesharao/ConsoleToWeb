using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ConsoleToWeb.Options;

namespace ConsoleToWeb.Controllers
{
    public class HomeController : Controller
    {
        private LogOption _option = null;
        public HomeController(IOptionsSnapshot<LogOption> optionsSnapshot)
        {
            Console.WriteLine($"{nameof(HomeController)} reached");
            _option = optionsSnapshot.Value;
        }
        public IActionResult Index()
        {
            Console.WriteLine($"Inside Controller constructor: { _option.Default??"value is null"}");
            return View();
        }
    }
}