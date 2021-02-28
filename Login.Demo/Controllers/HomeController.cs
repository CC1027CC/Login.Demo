using Login.Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Login.Demo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITest _test;

        public HomeController(ILogger<HomeController> logger, ITest test)
        {
            _logger = logger;
            _test = test;
        }

        public IActionResult Index()
        {
            var result = _test.GetName("老板");
            ViewData["Text"] = result;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public interface ITest
    {
        string GetName(string a);
    }
    public class Test : ITest
    {
        public string GetName(string a)
            => $"您好,{a}";

    }
}
