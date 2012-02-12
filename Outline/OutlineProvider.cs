using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginCore.Managers;
using PluginCore;
using System.Windows.Forms;

namespace PropertiesPanel.Outline
{
    public class OutlineProvider : IPropertyProvider
    {
        private TreeView _outlineTree = null;
        private List<IPropertyItem> _items = new List<IPropertyItem>();
        private IPropertyItem _selectedItem = null;

        private void HookEvents()
        {
            _outlineTree.AfterSelect += new TreeViewEventHandler(_outlineTree_AfterSelect);
        }

        private void UnhookEvents()
        {
            _outlineTree.AfterSelect -= new TreeViewEventHandler(_outlineTree_AfterSelect);
        }

        private void BuildItems()
        {
            ClearItems();

            AddItems(_outlineTree.Nodes);

            if (_selectedItem == null && _items.Count > 0)
                _selectedItem = _items[0];

            OnItemsChanged(this);
            OnSelectionChanged(this, _selectedItem);
        }

        private void ClearItems()
        {
            _items.Clear();
            _selectedItem = null;
        }

        private void AddItems(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                OutlineItem item = new OutlineItem(node);
                _items.Add(item);

                if (_outlineTree.SelectedNode == node)
                    _selectedItem = item;

                AddItems(node.Nodes);
            }
        }

        private void OnItemsChanged(IPropertyProvider provider)
        {
            if (ItemsChanged != null)
                ItemsChanged(provider);
        }

        private void OnSelectionChanged(IPropertyProvider provider, IPropertyItem selectedItem)
        {
            if (SelectionChanged != null)
                SelectionChanged(provider, selectedItem);
        }

        void _outlineTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BuildItems();
        }

        #region IPropertyProvider Members

        public event ProviderActivatedHandler Activated;
        public event ProviderDeactivatedHandler Deactivated;
        public event ItemsChangesHandler ItemsChanged;
        public event SelectionChangedHandler SelectionChanged;

        public string Name
        {
            get { return "Outline Provider"; }
        }

        public IPropertyItem SelectedItem
        {
            get { return _selectedItem; }
        }

        public IEnumerable<IPropertyItem> Items
        {
            get { return _items; }
        }

        public IEnumerable<IPropertyTab> Tabs
        {
            get { return null; }
        }

        public void Activate(System.Windows.Forms.DockPanelControl panel)
        {
            ASCompletion.PluginUI outlinePanel = panel as ASCompletion.PluginUI;
            _outlineTree = outlinePanel.OutlineTree;

            BuildItems();
            HookEvents();
        }

        public void Deactivate()
        {
            UnhookEvents();
            ClearItems();
            _outlineTree = null;
        }

        #endregion
    }
}
