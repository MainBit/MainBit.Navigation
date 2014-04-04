using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Title.Models;

namespace MainBit.Navigation.Services {
    [UsedImplicitly]
    public class FakeMenuService : Orchard.Core.Navigation.Services.MainMenuService, Orchard.Core.Navigation.Services.IMenuService
    {
        private readonly IContentManager _contentManager;

        public FakeMenuService(IContentManager contentManager)
            : base(contentManager)
        {
            _contentManager = contentManager;
        }

        IContent Orchard.Core.Navigation.Services.IMenuService.GetMenu(int menuId)
        {
            return null;
        }
    }
}