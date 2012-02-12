using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PropertiesPanel.Property
{
    public class PropertyTab
    {
        private string _name = string.Empty;
        private Image _icon = null;

        public PropertyTab(string name, Image icon)
        {
            _name = name;
            _icon = icon;
        }

        public string Name
        {
            get { return _name; }
        }
        public Image Icon
        {
            get { return _icon; }
        }
    }
}
