using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropertiesPanel.Property;
using System.Windows.Forms;

namespace PropertiesPanel.Files
{
    public class FilesProvider : PropertyProviderBase
    {
        private ListView _filesList = null;

        public FilesProvider()
            : base("Files Provider")
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

        }

        private void UnhookEvents()
        {

        }

        private void BuildItems()
        {

        }
    }
}
