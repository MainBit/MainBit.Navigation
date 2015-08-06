using Orchard;
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
    public interface ICacheMenuItemService : IDependency
    {
        IEnumerable<MenuItem> GetMenuItems(IContent menu, Func<IContent, IEnumerable<MenuItem>> buildMenu);
        IEnumerable<MenuItem> CloneMenuItems(IEnumerable<MenuItem> menuItems);
        void ResetCache(int menuId);
        string GetCacheKey(int menuId);
    }

    
}