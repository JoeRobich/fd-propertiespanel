using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertiesPanel.Outline
{
    public class OutlineProperty : PropertyBase
    {
        public OutlineProperty(string displayName, string description, string category, object value, Type propertyType)
            : base(displayName, description, category, value, propertyType)
        {
            _readOnly = true;
        }
    }
}
