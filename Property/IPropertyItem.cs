using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertiesPanel.Property
{
    public interface IPropertyItem
    {
        event ValueChangedHandler PropertyChanged;
        
        string Name { get; }
        string TypeName { get; }
        IEnumerable<IProperty> Properties { get; }
        IEnumerable<IProperty> GetTabProperties(PropertyTab tab);
    }
}
