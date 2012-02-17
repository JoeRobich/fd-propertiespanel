using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PropertiesPanel.Property
{
    public abstract class PropertyAction : PropertyTab
    {
        public PropertyAction(string name, Image icon)
            : base(name, icon)
        {
        }

        public abstract bool IsEnabled(PropertyProvider provider, IEnumerable<PropertyItem> selectedItems);
        public abstract void OnActionClicked(PropertyProvider provider, IEnumerable<PropertyItem> selectedItems);
    }
}
