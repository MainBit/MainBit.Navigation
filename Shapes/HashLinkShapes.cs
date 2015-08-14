using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Descriptors.ResourceBindingStrategy;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Settings;
using Orchard.UI;
using Orchard.UI.Resources;
using Orchard.UI.Zones;
using Orchard.Utility.Extensions;
using Orchard;
using Orchard.Widgets.Models;
using Orchard.Environment.Extensions;
using Orchard.Logging;

namespace MainBit.Navigation.Shapes
{
    [OrchardFeature("MainBit.Navigation.HashLinks")]
    public class HashLinkShapes : IShapeTableProvider
    {
        private readonly Work<IHttpContextAccessor> _httpContextAccessor;

        public HashLinkShapes(
            Work<IHttpContextAccessor> httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ILogger Logger { get; set; }

        public void Discover(ShapeTableBuilder builder) {

            builder.Describe("MenuItemLink")
                .OnDisplaying(displaying =>
                {
                    string href = displaying.Shape.Href;
                    if (href.IndexOf('#') > 0)
                    {
                        var segments = href.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        if (segments.Length == 2)
                        {
                            var request = _httpContextAccessor.Value.Current().Request;
                            
                            var linkUrl = segments[0];
                            if (linkUrl.StartsWith("//")) // linkUrl.StartsWith("~/") - this should be changed by Orchard.Navigation Module
                            {
                                linkUrl = linkUrl.Substring(1);
                            }
                            if (linkUrl.StartsWith("/"))
                            {
                                var baseUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath.TrimEnd('/');
                                linkUrl = baseUrl + linkUrl;
                            }

                            if (linkUrl.TrimEnd('/').Equals(request.Url.AbsoluteUri.TrimEnd('/'), StringComparison.InvariantCultureIgnoreCase))
                            {
                                displaying.Shape.Href = '#' + segments[1];
                            }
                        }
                    }
                });
        }
    }
}
