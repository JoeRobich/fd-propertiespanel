using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PropertiesPanel.Property;
using PropertiesPanel.Helpers;

namespace PropertiesPanel.Files
{
    public class FilesItem : PropertyItem
    {
        private ListViewItem _listItem = null;

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

        private void BuildFolderProperties()
        {
            _typeName = ResourceHelper.GetString("PropertiesPanel.Label.FolderProperties");

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Path"), _listItem.Tag, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.LastModifiedOn"), _listItem.SubItems[3].Text, typeof(string), true);
            _properties.Add(property);
        }

        private void BuildFileProperties()
        {
            _typeName = ResourceHelper.GetString("PropertiesPanel.Label.FileProperties");

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Path"), _listItem.Tag, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Size"), _listItem.SubItems[1].Text, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Type"), _listItem.SubItems[2].Text, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.LastModifiedOn"), _listItem.SubItems[3].Text, typeof(string), true);
            _properties.Add(property);
        }
    }
}
