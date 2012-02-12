using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginCore.Managers;
using PluginCore;
using System.Windows.Forms;
using ProjectManager.Controls.TreeView;
using PropertiesPanel.Property;

namespace PropertiesPanel.Project
{
    class ProjectProvider : PropertyProvider
    {
        private ProjectTreeView _projectTree = null;

        public ProjectProvider()
            : base("ProjectProvider")
        {

        }

        protected override void OnActivating(DockPanelControl panel)
        {
            ProjectManager.PluginUI projectPanel = panel as ProjectManager.PluginUI;
            _projectTree = projectPanel.Tree;

            BuildItems();
            HookEvents();
        }

        protected override void OnDeactivating()
        {
            UnhookEvents();
            ClearItems();
            _projectTree = null;
        }

        private void HookEvents()
        {
            _projectTree.AfterSelect += new TreeViewEventHandler(_projectTree_AfterSelect);
        }

        private void UnhookEvents()
        {
            _projectTree.AfterSelect -= new TreeViewEventHandler(_projectTree_AfterSelect);
        }

        private void BuildItems()
        {
            ClearItems();

            ProjectItem item = new ProjectItem(_projectTree.Project, _projectTree.SelectedNode);
            AddItem(item);

            OnItemsChanged();
            SelectedItem = item;
        }

        void _projectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BuildItems();
        }
    }
}