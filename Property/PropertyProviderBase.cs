using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertiesPanel.Property
{
    public abstract class PropertyProviderBase : IPropertyProvider
    {
        private string _name = string.Empty;
        private IPropertyItem _selectedItem = null;
        private List<IPropertyItem> _items = new List<IPropertyItem>();
        private List<PropertyTab> _tabs = new List<PropertyTab>();
        private List<PropertyAction> _actions = new List<PropertyAction>();

        public PropertyProviderBase(string name)
        {
            _name = name;
        }

        protected abstract void OnActivating(System.Windows.Forms.DockPanelControl panel);
        protected abstract void OnDeactivating();


        protected void AddItem(IPropertyItem item)
        {
            _items.Add(item);
            OnItemsChanged();
        }

        protected void ClearItems()
        {
            _items.Clear();
            SelectedItem = null;
            OnItemsChanged();
        }

        protected void AddTab(PropertyTab tab)
        {
            _tabs.Add(tab);
        }

        protected void ClearTabs()
        {
            _tabs.Clear();
        }

        protected void AddAction(PropertyAction action)
        {
            _actions.Add(action);
        }

        protected void ClearActions()
        {
            _actions.Clear();
        }

        private void OnActivated()
        {
            if (Activated != null)
                Activated(this);
        }

        private void OnDeactivated()
        {
            if (Deactivated != null)
                Deactivated(this);
        }

        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
                ItemsChanged(this);
        }

        private void OnSelectedChanged()
        {
            if (SelectionChanged != null)
                SelectionChanged(this, SelectedItem);
        }

        #region IPropertyProvider Members

        public event ProviderActivatedHandler Activated;

        public event ProviderDeactivatedHandler Deactivated;

        public event ItemsChangesHandler ItemsChanged;

        public event SelectionChangedHandler SelectionChanged;

        public string Name
        {
            get { return _name; }
        }

        public IPropertyItem SelectedItem
        {
            get { return _selectedItem; }
            protected set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnSelectedChanged();
                }
            }
        }

        public IEnumerable<IPropertyItem> Items
        {
            get { return _items; }
        }

        public IEnumerable<PropertyTab> Tabs
        {
            get { return _tabs; }
        }

        public IEnumerable<PropertyAction> Actions
        {
            get { return _actions; }
        }

        public void Activate(System.Windows.Forms.DockPanelControl panel)
        {
            OnActivating(panel);
            OnActivated();
        }

        public void Deactivate()
        {
            OnDeactivating();
            OnDeactivated();
        }

        #endregion
    }
}
