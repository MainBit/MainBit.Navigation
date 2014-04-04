using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MainBit.Navigation.Helpers
{
    public class NavigationHelper
    {
        public static Stack<MenuItem> SetSelectedPath(IEnumerable<MenuItem> menuItems, RouteValueDictionary currentRouteData, string url)
        {
            if (menuItems == null)
                return null;

            foreach (MenuItem menuItem in menuItems)
            {
                Stack<MenuItem> selectedPath = SetSelectedPath(menuItem.Items, currentRouteData, url);
                if (selectedPath != null)
                {
                    menuItem.Selected = true;
                    selectedPath.Push(menuItem);
                    return selectedPath;
                }

                if (RouteMatches(menuItem.RouteValues, currentRouteData) || UrlMatches(menuItem.Href, url))
                {
                    menuItem.Selected = true;

                    selectedPath = new Stack<MenuItem>();
                    selectedPath.Push(menuItem);
                    return selectedPath;
                }
            }

            return null;
        }

        public static bool RouteMatches(RouteValueDictionary itemValues, RouteValueDictionary requestValues)
        {
            if (itemValues == null && requestValues == null)
            {
                return true;
            }
            if (itemValues == null || requestValues == null)
            {
                return false;
            }
            if (itemValues.Keys.Any(key => requestValues.ContainsKey(key) == false))
            {
                return false;
            }
            return itemValues.Keys.All(key => string.Equals(Convert.ToString(itemValues[key]), Convert.ToString(requestValues[key]), StringComparison.OrdinalIgnoreCase));
        }

        public static bool UrlMatches(string itemUrl, string requestUrl)
        {
            //if (itemUrl == requestUrl || requestUrl.StartsWith(itemUrl + "/"))
            if(requestUrl.StartsWith(itemUrl))
            {
                return true;
            }

            return false;
        }
    }
}