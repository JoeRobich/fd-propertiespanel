using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertiesPanel.Property;
using System.Windows.Forms;

namespace PropertiesPanel.Files
{
    public class FilesProvider : PropertyProvider
    {
        private ListView _filesList = null;

        public FilesProvider()
            : base("FilesProvider")
        {

        }

        protected override void OnActivating(System.Windows.Forms.DockPanelControl panel)
        {
            FileExplorer.PluginUI filesPanel = panel as FileExplorer.PluginUI;
            _filesList = filesPanel.Controls[0] as ListView;

            BuildItems();
            HookEvents();
        }

        protected override void OnDeactivating()
        {
            UnhookEvents();
            ClearItems();
            _filesList = null;
        }

        private void HookEvents()
        {
            _filesList.SelectedIndexChanged += new EventHandler(_filesList_SelectedIndexChanged);
        }

        private void UnhookEvents()
        {
            _filesList.SelectedIndexChanged -= new EventHandler(_filesList_SelectedIndexChanged);
        }

        private void BuildItems()
        {
            ClearItems();

            if (_filesList.SelectedItems.Count == 0)
                return;

            FilesItem selectedItem = new FilesItem(_filesList.SelectedItems[0]);
            AddItem(selectedItem);
            SelectedItem = selectedItem;
        }

        void _filesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildItems();
        }
    }
}
