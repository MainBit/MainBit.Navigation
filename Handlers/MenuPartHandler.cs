using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.Core.Navigation.Models;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Caching;
using MainBit.Navigation.Services;

namespace Orchard.Core.Navigation.Handlers {
    [UsedImplicitly]
    public class MenuPartHandler : ContentHandler {
        private readonly IGoodMenuService _goodMenuService;

        public MenuPartHandler(IGoodMenuService goodMenuService)
        {
            _goodMenuService = goodMenuService;

            OnCreated<MenuPart>((context, part) => ResetCache(part));
            OnUpdated<MenuPart>((context, part) => ResetCache(part));
            OnRemoved<MenuPart>((context, part) => ResetCache(part));
        }

        protected void ResetCache(MenuPart part)
        {
            _goodMenuService.ResetCache(part.Record.MenuId);
        }
    }
}