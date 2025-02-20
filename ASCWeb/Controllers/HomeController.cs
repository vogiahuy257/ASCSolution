using System.Diagnostics;
using ASCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ASCWeb.Configuration;
using Microsoft.Extensions.Options;

namespace ASCWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IOptions<ApplicationSettings> _settings;

        public HomeController(ILogger<HomeController> logger,IOptions<ApplicationSettings> _settings)
        {
            _logger = logger;
            this._settings = _settings;
        }

        public IActionResult Index()
        {
            ViewBag.Title = _settings.Value.ApplicationTille;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
