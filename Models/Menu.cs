using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Navigation.Models
{
    public class Menu
    {
        public string Name { get; set; }
        public IEnumerable<MenuItem> Items { get; set; }
    }
}