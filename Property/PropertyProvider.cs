using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertiesPanel.Property
{
    public delegate void SelectionChangedHandler(PropertyProvider provider);
    public delegate void ItemsChangesHandler(PropertyProvider provider);
    public delegate void ProviderActivatedHandler(PropertyProvider provider);
    public delegate void ProviderDeactivatedHandler(PropertyProvider provider);

    public abstract class PropertyProvider 
    {
        private string _name = string.Empty;
        private List<PropertyItem> _selectedItems = new List<PropertyItem>();
        private PropertyTab _selectedTab = null;
        private List<PropertyItem> _items = new List<PropertyItem>();
        private List<PropertyTab> _tabs = new List<PropertyTab>();
        private List<PropertyAction> _actions = new List<PropertyAction>();

        public PropertyProvider(string name)
        {
            _name = name;
        }

        protected abstract void OnActivating(System.Windows.Forms.DockPanelControl panel);
        protected abstract void OnRefresh();
        protected abstract void OnDeactivating();

        protected void AddItem(PropertyItem item)
        {
            AddItems(new PropertyItem[] { item });
        }

        protected void AddItems(IEnumerable<PropertyItem> items)
        {
            foreach (PropertyItem item in items)
            {
                if (_items.Contains(item))
                    continue;

                _items.Add(item);
            }
            OnItemsChanged();
        }

        protected void ClearItems()
        {
            ClearSelectedItems();
            _items.Clear();
            OnItemsChanged();
        }

        protected void AddSelectedItem(PropertyItem item)
        {
            AddSelectedItems(new PropertyItem[] { item });
        }

        protected void AddSelectedItems(IEnumerable<PropertyItem> items)
        {
            foreach (PropertyItem item in items)
            {
                if (!_items.Contains(item) || _selectedItems.Contains(item))
                    continue;

                _selectedItems.Add(item);
            }
            OnSelectedChanged();
        }

        protected void ClearSelectedItems()
        {
            _selectedItems.Clear();
            OnSelectedChanged();
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
                SelectionChanged(this);
        }

        #region PropertyProvider Members

        public event ProviderActivatedHandler Activated;
        public event ProviderDeactivatedHandler Deactivated;
        public event ItemsChangesHandler ItemsChanged;
        public event SelectionChangedHandler SelectionChanged;

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<PropertyItem> SelectedItems
        {
            get { return _selectedItems; }
        }

        public PropertyTab SelectedTab
        {
            get { return _selectedTab; }
            internal set
            {
                if (_selectedTab != value && _tabs.Count > 0)
                {
                    _selectedTab = value;
                    OnSelectedChanged();
                }
            }
        }

        public IEnumerable<PropertyItem> Items
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

        internal void Activate(System.Windows.Forms.DockPanelControl panel)
        {
            OnActivating(panel);
            OnActivated();
        }

        public void Refresh()
        {
            OnRefresh();
        }

        internal void Deactivate()
        {
            OnDeactivating();
            OnDeactivated();
        }

        #endregion
    }
}
