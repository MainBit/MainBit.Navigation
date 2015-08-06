using MainBit.Navigation.Services;
using Orchard.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainBit.Navigation
{
    public class MenuUpdateFilter : FilterProvider, IActionFilter
    {
        private readonly ICacheMenuItemService _cacheMenuItemService;
        private readonly Type ControllerType = typeof(Orchard.Core.Navigation.Controllers.AdminController);
        private readonly string ActionName = "index";


        public MenuUpdateFilter(ICacheMenuItemService cacheMenuItemService)
        {
            _cacheMenuItemService = cacheMenuItemService;
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase)
                || !filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Equals(ControllerType)
                || !filterContext.ActionDescriptor.ActionName.Equals(ActionName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var menuId = filterContext.Controller.ValueProvider.GetValue("menuId");
            if(menuId == null) {
                return;
            }

            _cacheMenuItemService.ResetCache(Convert.ToInt32(menuId.AttemptedValue));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}