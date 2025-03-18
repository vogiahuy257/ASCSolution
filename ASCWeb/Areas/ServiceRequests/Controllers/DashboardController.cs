using ASCWeb.Configuration;
using ASCWeb.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASCWeb.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    public class DashboardController : BaseController
    {
        private IOptions<ApplicationSettings> _setting;

        public DashboardController(IOptions<ApplicationSettings> setting)
        {
            _setting = setting;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
