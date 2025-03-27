using System.Diagnostics;
using ASCWeb.Models;
using Microsoft.AspNetCore.Mvc;
using ASCWeb.Configuration;
using Microsoft.Extensions.Options;
using ASC.Utilities;

namespace ASCWeb.Controllers
{
    public class HomeController : AnonymousController
    {
        private readonly ILogger<HomeController>? _logger;
        private readonly IOptions<ApplicationSettings> _settings;

        public HomeController(IOptions<ApplicationSettings> settings, ILogger<HomeController>? logger = null)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Set Session
            HttpContext.Session.SetSession("Test", _settings.Value);

            // Get Session
            var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");

            // Usage of IOptions
            ViewBag.Title = _settings.Value.ApplicationTitle;

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
