using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Services;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Navigation.Services
{
    public class CacheMenuItemService : ICacheMenuItemService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;
        private readonly ISignals _signals;

        public CacheMenuItemService(
            ICacheManager cacheManager,
            IClock clock,
            ISignals signals)
        {
            _cacheManager = cacheManager;
            _clock = clock;
            _signals = signals;
        }

        public IEnumerable<MenuItem> GetMenuItems(IContent menu, Func<IContent, IEnumerable<MenuItem>> buildMenu)
        {
            var key = GetCacheKey(menu.Id);
            return _cacheManager.Get(key, ctx =>
            {

                //ctx.Monitor(_clock.When(TimeSpan.FromDays(365)));
                ctx.Monitor(_signals.When(key));
                return buildMenu(menu);
            });
        }

        public IEnumerable<MenuItem> CloneMenuItems(IEnumerable<MenuItem> menuItems)
        {
            var clonedMenuItems = new List<MenuItem>();
            foreach (var menuItem in menuItems)
            {
                var clonedMenuItem = new MenuItem()
                {
                    Text = menuItem.Text,
                    IdHint = menuItem.IdHint,
                    Url = menuItem.Url,
                    Href = menuItem.Href,
                    Position = menuItem.Position,
                    LinkToFirstChild = menuItem.LinkToFirstChild,
                    LocalNav = menuItem.LocalNav,
                    Culture = menuItem.Culture,
                    Selected = menuItem.Selected,
                    RouteValues = menuItem.RouteValues,
                    Items = CloneMenuItems(menuItem.Items),
                    Permissions = menuItem.Permissions,
                    Content = menuItem.Content,
                    Classes = new List<string>(menuItem.Classes)
                };
                clonedMenuItems.Add(clonedMenuItem);
            }
            return clonedMenuItems;
        }

        public void ResetCache(int menuId)
        {
            _signals.Trigger(GetCacheKey(menuId));
        }

        public string GetCacheKey(int menuId)
        {
            return string.Format("MainBit.Navigation.Menu-{0}", menuId);
        }

    }
}