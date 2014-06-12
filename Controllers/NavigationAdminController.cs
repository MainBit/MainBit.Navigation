using Orchard.UI.Admin;
using System.Web.Mvc;
using MainBit.Navigation.Services;
using System.Collections.Concurrent;

namespace MainBit.Navigation.Controllers
{
    [Admin]
    public class NavigationAdminController : Controller
    {
        private readonly IGoodMenuService _goodMenuService;

        public NavigationAdminController
        (
            IGoodMenuService goodMenuService
        )
        {
            _goodMenuService = goodMenuService;
        }

        public ActionResult Index(int id)
        {
            _goodMenuService.ResetCache(id);
            return View();
        }
    }
}