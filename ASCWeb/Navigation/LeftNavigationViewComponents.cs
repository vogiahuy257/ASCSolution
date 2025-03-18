using ASCWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASCWeb.Navigation
{
    [ViewComponent(Name = "ASCWeb.Navigation.LeftNavigation")]
    public class LeftNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(NavigationMenu menu)
        {
            menu.MenuItems = menu.MenuItems.OrderBy(p => p.Sequence).ToList();
            return View(menu);
        }
    }
}
