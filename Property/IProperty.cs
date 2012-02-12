using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PropertiesPanel.Property
{
    public delegate void ValueChangedHandler(IProperty property);

    public interface IProperty
    {
        event ValueChangedHandler ValueChanged;

        string DisplayName { get; }
        string Description { get; }
        string Category { get; }
        bool Browsable { get; }
        bool ReadOnly { get; }
        object DefaultValue { get; }
        bool RestrictToList { get; }
        object[] ListValues { get; }
        object Value { get; set; }
        Type ValueType { get; }
    }
}
