using JetBrains.Annotations;
using MainBit.Navigation.Services;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Title.Models;

namespace MainBit.Core.Navigation.Handlers {
    [UsedImplicitly]
    public class MenuHandler : ContentHandler {
        private readonly IGoodMenuService _goodMenuService;

        public MenuHandler(IGoodMenuService goodMenuService)
        {
            _goodMenuService = goodMenuService;
        }

        protected override void Updated(UpdateContentContext context)
        {
            ResetCache(context.ContentItem);
        }
        protected override void Removed(RemoveContentContext context)
        {
            ResetCache(context.ContentItem);
        }

        protected void ResetCache(ContentItem contentItem)
        {
            if (contentItem.ContentType != "Menu") {
                return;
            }
            _goodMenuService.ResetCache(contentItem.Id);
        }
    }
}