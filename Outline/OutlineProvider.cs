using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginCore.Managers;
using PluginCore;
using System.Windows.Forms;
using PropertiesPanel.Property;

namespace PropertiesPanel.Outline
{
    public class OutlineProvider : PropertyProvider
    {
        private TreeView _outlineTree = null;

        public OutlineProvider()
            : base("OutlineProvider")
        {

        }

        protected override void OnActivating(DockPanelControl panel)
        {
            ASCompletion.PluginUI outlinePanel = panel as ASCompletion.PluginUI;
            _outlineTree = outlinePanel.OutlineTree;

            BuildItems();
            HookEvents();
        }

        protected override void OnDeactivating()
        {
            UnhookEvents();
            ClearItems();
            _outlineTree = null;
        }

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

            OutlineItem item = AddItems(_outlineTree.Nodes);
            SelectedItem = item;
        }

        private OutlineItem AddItems(TreeNodeCollection nodes)
        {
            OutlineItem selectedItem = null;

            foreach (TreeNode node in nodes)
            {
                OutlineItem item = new OutlineItem(node);

                AddItem(item);

                if (_outlineTree.SelectedNode == node)
                    selectedItem = item;

                item = AddItems(node.Nodes);

                if (item != null)
                    selectedItem = item;
            }

            return selectedItem;
        }

        void _outlineTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BuildItems();
        }
    }
}