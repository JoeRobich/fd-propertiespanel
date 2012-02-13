using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PropertiesPanel.Property;
using PropertiesPanel.Helpers;
using System.IO;
using PluginCore.Managers;
using PropertiesPanel.Manager;

namespace PropertiesPanel.Files
{
    public class FilesItem : PropertyItem
    {
        private ListViewItem _listItem = null;
        private Property.Property _nameProperty = null;

        public FilesItem(ListViewItem listItem)
        {
            _listItem = listItem;
            _name = _listItem.Text;

            if (_listItem.SubItems[1].Text == "-")
                BuildFolderProperties();
            else
                BuildFileProperties();
        }

        public ListViewItem ListItem
        {
            get { return _listItem; }
        }

        public void Refresh()
        {
            Name = ListItem.Text;
            _nameProperty.Value = Name;
            ((FilesProvider)PropertiesManager.ActiveProvider).FilesListView.Refresh();
            PluginMain.PropertiesPanel.RefreshControls();
        }

        void nameProperty_ValueChanged(PropertiesPanel.Property.Property property)
        {
            if (Name == (string)property.Value)
                return;

            try
            {
                String file = ListItem.Tag.ToString();
                FileInfo info = new FileInfo(file);
                String path = info.Directory + Path.DirectorySeparatorChar.ToString();
                if (File.Exists(file))
                {
                    File.Move(path + Name, path + property.Value);
                    DocumentManager.MoveDocuments(path + Name, path + property.Value);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Move(path + Name, path + property.Value);
                    DocumentManager.MoveDocuments(path + Name, path + property.Value);
                }
                ListItem.Text = (string)property.Value;
                Refresh();
            }
            catch (Exception ex)
            {
                ErrorManager.ShowError(ex);
            }
        }

        private void BuildFolderProperties()
        {
            _typeName = ResourceHelper.GetString("PropertiesPanel.Label.FolderProperties");

            string[] modifiedParts = _listItem.SubItems[3].Text.Split(' ');
            string modifiedDate = modifiedParts[0];
            string modifiedTime = string.Format("{0} {1}", modifiedParts[1], modifiedParts[2]);

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), false);
            property.ValueChanged += new ValueChangedHandler(nameProperty_ValueChanged);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Path"), _listItem.Tag, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.ModifiedDate"), modifiedDate, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.ModifiedTime"), modifiedTime, typeof(string), true);
            _properties.Add(property);
        }

        private void BuildFileProperties()
        {
            _typeName = ResourceHelper.GetString("PropertiesPanel.Label.FileProperties");

            string[] modifiedParts = _listItem.SubItems[3].Text.Split(' ');
            string modifiedDate = modifiedParts[0];
            string modifiedTime = string.Format("{0} {1}", modifiedParts[1], modifiedParts[2]);

            Property.Property property;
            _nameProperty = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), false);
            _nameProperty.ValueChanged += new ValueChangedHandler(nameProperty_ValueChanged);
            _properties.Add(_nameProperty);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Path"), _listItem.Tag, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Size"), _listItem.SubItems[1].Text, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Type"), _listItem.SubItems[2].Text, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.ModifiedDate"), modifiedDate, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.ModifiedTime"), modifiedTime, typeof(string), true);
            _properties.Add(property);
        }
    }
}
