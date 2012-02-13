using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginCore.Managers;
using PluginCore;
using System.Windows.Forms;
using PropertiesPanel.Property;
using ASCompletion.Context;

namespace PropertiesPanel.Outline
{
    public class OutlineProvider : PropertyProvider
    {
        private TreeView _outlineTree = null;

        public OutlineProvider()
            : base("OutlineProvider")
        {
            PropertyAction navigateAction = new PropertyAction("Goto Declaration", PluginBase.MainForm.FindImage("99|9|3|-3"));
            navigateAction.ActionFired += new ActionFiredHandler(navigateAction_ActionFired);
            AddAction(navigateAction);
        }

        public TreeView OutlineTreeView
        {
            get { return _outlineTree; }
        }

        void navigateAction_ActionFired(PropertyProvider provider, PropertyItem item)
        {
            OutlineItem outlineItem = (OutlineItem)item;
            outlineItem.NavigateTo();
        }

        protected override void OnActivating(DockPanelControl panel)
        {
            ASCompletion.PluginUI outlinePanel = panel as ASCompletion.PluginUI;
            _outlineTree = outlinePanel.OutlineTree;

            BuildItems();
            HookEvents();
        }

        protected override void OnRefresh()
        {
            BuildItems();
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

            List<PropertyItem> items = new List<PropertyItem>();           
            PropertyItem selectedItem = BuildItems(_outlineTree.Nodes, items);

            AddItems(items);
            AddSelectedItem(selectedItem);
        }

        private OutlineItem BuildItems(TreeNodeCollection nodes, List<PropertyItem> items)
        {
            OutlineItem selectedItem = null;

            foreach (TreeNode node in nodes)
            {
                OutlineItem item = new OutlineItem(node);

                items.Add(item);

                if (_outlineTree.SelectedNode == node)
                    selectedItem = item;

                item = BuildItems(node.Nodes, items);

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