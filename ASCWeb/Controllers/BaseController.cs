using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASCWeb.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

    }
}
