using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PropertiesPanel.Property
{
    public delegate void ActionFiredHandler(IPropertyProvider provider, IPropertyItem item);

    public class PropertyAction : PropertyTab
    {
        public event ActionFiredHandler ActionFired;

        public PropertyAction(string name, Image icon)
            : base(name, icon)
        {
        }

        internal void OnActionFired(IPropertyProvider provider, IPropertyItem item)
        {
            if (ActionFired != null)
                ActionFired(provider, item);
        }
    }
}
