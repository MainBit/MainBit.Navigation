using System;
using System.Collections.Generic;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Services;
using Orchard.UI.Navigation;
using Orchard.Utility.Extensions;
using Orchard.Core.Title.Models;
using MainBit.Navigation.Models;

namespace MainBit.Navigation.Services
{
    public class GoodMenuService : IGoodMenuService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;
        private readonly ISignals _signals;
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly INavigationManager _navigationManager;

        public GoodMenuService(
            ICacheManager cacheManager,
            IClock clock,
            ISignals signals,
            IContentManager contentManager,
            IWorkContextAccessor workContextAccessor,
            INavigationManager navigationManager)
        {
            _cacheManager = cacheManager;
            _clock = clock;
            _signals = signals;
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
            _navigationManager = navigationManager;
        }

        public Menu GetMenu(int menuId)
        {
            var key = string.Format("MainBit.Navigation.Menu-{0}", menuId);
            var resultModel = _cacheManager.Get(key, ctx => {
                ctx.Monitor(_clock.When(TimeSpan.FromDays(365)));
                ctx.Monitor(_signals.When(key));

                var menu = _contentManager.Get(menuId, VersionOptions.Published, new QueryHints().ExpandRecords<TitlePartRecord>());
                
                if(menu == null) {
                    ctx.Monitor(_clock.When(TimeSpan.FromDays(-1)));
                    return null;
                }

                var model = new Menu() {
                    Name = menu.As<TitlePart>().Title.HtmlClassify(),
                    Items = _navigationManager.BuildMenu(menu)
                };

                return model;
            });

            return resultModel;
        }

        public Menu GetCloneMenu(int menuId)
        {
            var menu = GetMenu(menuId);
            var menuClone = new Menu()
            {
                Name = menu.Name,
                Items = CloneMenuItems(menu.Items)
            };
            return menuClone;
        }

        private IEnumerable<MenuItem> CloneMenuItems(IEnumerable<MenuItem> menuItems)
        {
            var cloneMenuItems = new List<MenuItem>();
            foreach (var menuItem in menuItems)
            {
                var cloneMenuItem = new MenuItem() {
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
                cloneMenuItems.Add(cloneMenuItem);
            }
            return cloneMenuItems;
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