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

        public ListView FilesListView
        {
            get { return _filesList; }
        }

        protected override void OnActivating(System.Windows.Forms.DockPanelControl panel)
        {
            FileExplorer.PluginUI filesPanel = panel as FileExplorer.PluginUI;
            _filesList = filesPanel.Controls[0] as ListView;

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
            _filesList = null;
        }

        private void HookEvents()
        {
            _filesList.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(_filesList_ItemSelectionChanged);
            _filesList.AfterLabelEdit += new LabelEditEventHandler(_filesList_AfterLabelEdit);
        }

        private void UnhookEvents()
        {
            _filesList.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(_filesList_ItemSelectionChanged);
            _filesList.AfterLabelEdit -= new LabelEditEventHandler(_filesList_AfterLabelEdit);
        }

        private void BuildItems()
        {
            ClearItems();

            List<PropertyItem> items = new List<PropertyItem>();
            foreach (ListViewItem listItem in _filesList.SelectedItems)
            {
                FilesItem item = new FilesItem(listItem);
                items.Add(item);
            }

            AddItems(items);
            AddSelectedItems(items);
        }

        void _filesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            BuildItems();
        }

        void _filesList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            foreach (FilesItem selectedItem in SelectedItems)
            {
                selectedItem.ListItem.Text = e.Label;
                selectedItem.Refresh();
            }
        }
    }
}
