using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.Core.Navigation.Models;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Caching;
using MainBit.Navigation.Services;

namespace MainBit.Core.Navigation.Handlers
{
    [UsedImplicitly]
    public class MenuPartHandler : ContentHandler {
        private readonly ICacheMenuItemService _cacheMenuItemService;

        public MenuPartHandler(ICacheMenuItemService cacheMenuItemService)
        {
            _cacheMenuItemService = cacheMenuItemService;

            OnCreated<MenuPart>((context, part) => ResetCache(part));
            OnUpdated<MenuPart>((context, part) => ResetCache(part));
            OnRemoved<MenuPart>((context, part) => ResetCache(part));
        }

        protected void ResetCache(MenuPart part)
        {
            _cacheMenuItemService.ResetCache(part.Record.MenuId);
        }
    }
}