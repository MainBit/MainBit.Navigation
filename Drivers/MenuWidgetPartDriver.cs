using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Navigation.ViewModels;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.Utility.Extensions;
using Orchard;
using MainBit.Navigation.Services;
using MainBit.Common.Services;
using System.Web.Mvc;
using Orchard.Mvc.Html;
using System.Collections;

namespace MainBit.Navigation.Drivers {
    public class MenuWidgetPartDriver : ContentPartDriver<MenuWidgetPart> {
        private readonly IContentManager _contentManager;
        private readonly INavigationManager _navigationManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IMenuService _menuService;
        private readonly IGoodMenuService _goodMenuService;
        private readonly ICurrentContentAccessor _currentContentAccessor;

        public MenuWidgetPartDriver(
            IContentManager contentManager,
            INavigationManager navigationManager,
            IWorkContextAccessor workContextAccessor,
            IMenuService menuService,
            IGoodMenuService goodMenuService,
            ICurrentContentAccessor currentContentAccessor)
        {
            _contentManager = contentManager;
            _navigationManager = navigationManager;
            _workContextAccessor = workContextAccessor;
            _menuService = menuService;
            _goodMenuService = goodMenuService;
            _currentContentAccessor = currentContentAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix {
            get {
                return "MenuWidget";
            }
        }

        protected override DriverResult Display(MenuWidgetPart part, string displayType, dynamic shapeHelper) {
            return ContentShape( "Parts_MenuWidget", () => {
                var menu = _goodMenuService.GetCloneMenu(part.MenuContentItemId);

                if(menu == null) {
                    return null;
                }
                
                //var routeData = _workContextAccessor.GetContext().HttpContext.Request.RequestContext.RouteData;
                var urlHelper = new UrlHelper(_workContextAccessor.GetContext().HttpContext.Request.RequestContext);
                string cleanUrl;
                RouteValueDictionary routeValueDictionary;
                if (_currentContentAccessor.CurrentContentItem != null)
                {
                    var contentItemMetadata = _contentManager.GetItemMetadata(_currentContentAccessor.CurrentContentItem);
                    routeValueDictionary = contentItemMetadata.DisplayRouteValues;
                    cleanUrl = urlHelper.ItemDisplayUrl(_currentContentAccessor.CurrentContentItem);
                    //cleanUrl = urlHelper.Action(Convert.ToString(contentItemMetadata.DisplayRouteValues["action"]), contentItemMetadata.DisplayRouteValues);
                }
                else
                {
                    routeValueDictionary = _workContextAccessor.GetContext().HttpContext.Request.RequestContext.RouteData.Values;
                    cleanUrl = urlHelper.Action(Convert.ToString(routeValueDictionary["action"]), routeValueDictionary);
                }
                
                
                
                IEnumerable<MenuItem> menuItems = menu.Items.ToList();
                var selectedPath = MainBit.Navigation.Helpers.NavigationHelper.SetSelectedPath(menuItems, routeValueDictionary, cleanUrl);
                                          
                dynamic menuShape = shapeHelper.Menu();

                if (part.Breadcrumb) {
                    if (selectedPath == null)
                    {
                        return null;
                        //selectedPath = new Stack<MenuItem>();
                    }
                   
                    menuItems = selectedPath;
                    foreach (var menuItem in menuItems) {
                        menuItem.Items = Enumerable.Empty<MenuItem>();
                    }

                    // apply level limits to breadcrumb
                    menuItems = menuItems.Skip(part.StartLevel - 1);

                    if (part.Levels > 0) {
                        menuItems = menuItems.Take(part.Levels);
                    }

                    var result = new List<MenuItem>(menuItems);

                    // inject the home page
                    if (part.AddHomePage) {
                        result.Insert(0, new MenuItem {
                            Href = _navigationManager.GetUrl("~/", null),
                            Text = T("Home")
                        });
                    }

                    // inject the current page
                    if (!part.AddCurrentPage && menuItems.Last().Href == cleanUrl) {
                        result.RemoveAt(result.Count - 1);
                    } else if(part.AddCurrentPage && menuItems.Last().Href != cleanUrl)
                    {
                        //надо добавить текущую страницу
                    }

                    // prevent the home page to be added as the home page and the current page
                    if (result.Count == 2 && String.Equals(result[0].Href, result[1].Href, StringComparison.OrdinalIgnoreCase)) {
                        result.RemoveAt(1);
                    }

                    menuItems = result;

                    menuShape = shapeHelper.Breadcrumb();
                }
                else {
                    var topLevelItems = menuItems.ToList();

                    // apply start level by pushing children as top level items. When the start level is
                    // greater than 1 (ie. below the top level), only menu items along the selected path
                    // will be displayed.
                    for (var i = 0; topLevelItems.Any() && i < part.StartLevel - 1; i++)
                    {
                        var temp = new List<MenuItem>();
                        // should the menu be filtered on the currently displayed page ?
                        if (part.ShowFullMenu)
                        {
                            foreach (var menuItem in topLevelItems)
                            {
                                temp.AddRange(menuItem.Items);
                            }
                        }
                        else if (selectedPath != null)
                        {
                            topLevelItems = topLevelItems.Intersect(selectedPath.Where(x => x.Selected)).ToList();
                            foreach (var menuItem in topLevelItems)
                            {
                                temp.AddRange(menuItem.Items);
                            }
                        }
                        topLevelItems = temp;
                    }

                    // limit the number of levels to display (down from and including the start level)
                    if (part.Levels > 0)
                    {
                        var current = topLevelItems.ToList();
                        for (var i = 1; current.Any() && i < part.Levels; i++)
                        {
                            var temp = new List<MenuItem>();
                            foreach (var menuItem in current)
                            {
                                temp.AddRange(menuItem.Items);
                            }
                            current = temp;
                        }
                        // cut the sub-levels beneath any menu items that are at the lowest level being displayed
                        foreach (var menuItem in current)
                        {
                            menuItem.Items = Enumerable.Empty<MenuItem>();
                        }
                    }
                    menuItems = topLevelItems;
                }


                menuShape.MenuName(menu.Name);
                NavigationHelper.PopulateMenu(shapeHelper, menuShape, menuShape, menuItems);

                return shapeHelper.Parts_MenuWidget(Menu: menuShape);
            });
        }
    }
}
