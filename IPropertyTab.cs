using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PropertiesPanel
{
    public interface IPropertyTab
    {
        string Name { get; }
        Image Icon { get; }
    }
}
