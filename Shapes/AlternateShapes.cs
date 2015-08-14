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

namespace MainBit.Navigation.Shapes
{
    [OrchardFeature("MainBit.Navigation.Alternates")]
    public class AlternateShapes : IShapeTableProvider {

        public void Discover(ShapeTableBuilder builder) {
            // the root page shape named 'Layout' is wrapped with 'Document'
            // and has an automatic zone creating behavior
            builder.Describe("Parts_MenuWidget")
                .OnDisplaying(displaying => {
                    var contentItem = displaying.Shape.ContentItem as IContent;
                    var widget = contentItem.As<WidgetPart>();
                    displaying.Shape.Menu.ZoneName = widget.Zone;
                    displaying.Shape.Menu.WidgetName = widget.Name;
                });

            builder.Describe("Menu")
                .OnDisplaying(displaying =>
                {
                    string zoneName = displaying.Shape.ZoneName;
                    string widgetName = displaying.Shape.WidgetName;

                    if (!string.IsNullOrWhiteSpace(zoneName))
                    {
                        displaying.Shape.Metadata.Alternates.Add("Menu__Zone__" + EncodeAlternateElement(zoneName));
                    }

                    if (!string.IsNullOrWhiteSpace(widgetName))
                    {
                        displaying.Shape.Metadata.Alternates.Add("Menu__WidgetName__" + EncodeAlternateElement(widgetName));
                    }
                });

            builder.Describe("MenuItem")
                .OnDisplaying(displaying =>
                {
                    string zoneName = displaying.Shape.Menu.ZoneName;
                    string widgetName = displaying.Shape.Menu.WidgetName;

                    if (!string.IsNullOrWhiteSpace(zoneName))
                    {
                        displaying.Shape.Metadata.Alternates.Add("MenuItem__Zone__" + EncodeAlternateElement(zoneName));
                    }

                    if (!string.IsNullOrWhiteSpace(widgetName))
                    {
                        displaying.Shape.Metadata.Alternates.Add("MenuItem__WidgetName__" + EncodeAlternateElement(widgetName));
                    }
                });

            builder.Describe("MenuItemLink")
                .OnDisplaying(displaying =>
                {
                    string zoneName = displaying.Shape.Menu.ZoneName;
                    string widgetName = displaying.Shape.Menu.WidgetName;

                    if (!string.IsNullOrWhiteSpace(zoneName))
                    {
                        displaying.Shape.Metadata.Alternates.Add("MenuItemLink__Zone__" + EncodeAlternateElement(zoneName));
                    }

                    if (!string.IsNullOrWhiteSpace(widgetName))
                    {
                        displaying.Shape.Metadata.Alternates.Add("MenuItemLink__WidgetName__" + EncodeAlternateElement(widgetName));
                    }
                });
        }


        /// <summary>
        /// Encodes dashed and dots so that they don't conflict in filenames 
        /// </summary>
        /// <param name="alternateElement"></param>
        /// <returns></returns>
        private string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace("-", "__").Replace(".", "_");
        }
    }
}
