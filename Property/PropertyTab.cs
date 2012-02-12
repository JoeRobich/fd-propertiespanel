using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PropertiesPanel.Helpers;

namespace PropertiesPanel.Property
{
    public class PropertyTab
    {
        #region Static Properties

        private static PropertyTab _propertiesTab = new PropertyTab(ResourceHelper.GetString("PropertiesPanel.Label.Properties"), ResourceHelper.GetImage("Properties"));
        private static PropertyTab _eventsTab = new PropertyTab(ResourceHelper.GetString("PropertiesPanel.Label.Events"), ResourceHelper.GetImage("Event"));

        public static PropertyTab PropertiesTab
        {
            get { return _propertiesTab; }
        }

        public static PropertyTab EventsTab
        {
            get { return _eventsTab; }
        }

        #endregion

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
