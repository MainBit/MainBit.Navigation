using Orchard.UI.Admin;
using System.Web.Mvc;
using MainBit.Navigation.Services;
using System.Collections.Concurrent;

namespace MainBit.Navigation.Controllers
{
    [Admin]
    public class NavigationAdminController : Controller
    {
        private readonly ICacheMenuItemService _cacheMenuItemService;

        public NavigationAdminController(
            ICacheMenuItemService cacheMenuItemService
            )
        {
            _cacheMenuItemService = cacheMenuItemService;
        }

        public ActionResult Index(int id)
        {
            _cacheMenuItemService.ResetCache(id);
            return View();
        }
    }
}