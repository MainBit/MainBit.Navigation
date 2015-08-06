using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using Orchard.Security;
using Orchard.Security.Permissions;
using Orchard.UI;
using Orchard.UI.Navigation;
using Orchard.Utility;
using Orchard;
using Orchard.Core.Navigation.Services;

namespace MainBit.Navigation.Services {
    public class CachedNavigationManager : NavigationManager, INavigationManager
    {
        private readonly ICacheMenuItemService _cacheMenuItemService;

        public CachedNavigationManager(
            IEnumerable<INavigationProvider> navigationProviders, 
            IEnumerable<IMenuProvider> menuProviders,
            IAuthorizationService authorizationService,
            IEnumerable<INavigationFilter> navigationFilters,
            UrlHelper urlHelper, 
            IOrchardServices orchardServices,
            ShellSettings shellSettings,
            ICacheMenuItemService cacheMenuItemService)
            : base(navigationProviders, 
                menuProviders,
                authorizationService,
                navigationFilters,
                urlHelper, 
                orchardServices,
                shellSettings)
        {
            _cacheMenuItemService = cacheMenuItemService;
        }

        public new IEnumerable<MenuItem> BuildMenu(IContent menu) {
            var menuItems = _cacheMenuItemService.GetMenuItems(menu, (m) => base.BuildMenu(m));
            return _cacheMenuItemService.CloneMenuItems(menuItems);
        }
    }
}