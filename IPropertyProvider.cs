using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PropertiesPanel
{
    public delegate void SelectionChangedHandler(IPropertyProvider provider, IPropertyItem selectedItem);
    public delegate void ItemsChangesHandler(IPropertyProvider provider);
    public delegate void ProviderActivatedHandler(IPropertyProvider provider);
    public delegate void ProviderDeactivatedHandler(IPropertyProvider provider);

    public interface IPropertyProvider
    {
        event ProviderActivatedHandler Activated;
        event ProviderDeactivatedHandler Deactivated;
        event ItemsChangesHandler ItemsChanged;
        event SelectionChangedHandler SelectionChanged;

        string Name { get; }
        IPropertyItem SelectedItem { get; }
        IEnumerable<IPropertyItem> Items { get; }
        IEnumerable<IPropertyTab> Tabs { get; }

        void Activate(DockPanelControl panel);
        void Deactivate();
    }
}
