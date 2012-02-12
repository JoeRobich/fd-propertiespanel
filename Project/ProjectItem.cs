using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProjectManager.Controls.TreeView;
using PropertiesPanel.Property;

namespace PropertiesPanel.Project
{
    class ProjectItem : PropertyItemBase
    {
        private ProjectManager.Projects.Project _project;
        private TreeNode _node = null;

        public ProjectItem(ProjectManager.Projects.Project project, TreeNode node)
        {
            _project = project;
            _node = node;
            InitializeItem();
        }

        private void InitializeItem()
        {
            _name = _node.Text;

            if (_node is FileNode)
                BuildFileProperties((FileNode)_node);
            else if (_node is ProjectNode)
                BuildProjectProperties((ProjectNode)_node);
            else if (_node is DirectoryNode)
                BuildFolderProperties((DirectoryNode)_node);
            else
                BuildGenericProperties(_node);
        }

        private void BuildFileProperties(FileNode node)
        {
            _typeName = "File Properties";

            Property.Property property;
            property = new Property.Property("File Name", Name, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property("Full Path", node.BackingPath, typeof(string), true);
            _properties.Add(property);
        }

        private void BuildFolderProperties(DirectoryNode node)
        {
            _typeName = "Folder Properties";

            Property.Property property;
            property = new Property.Property("Folder Name", Name, typeof(string), true);
            _properties.Add(property);
        }

        private void BuildProjectProperties(ProjectNode node)
        {
            _typeName = "Project Properties";

            Property.Property property;
            property = new Property.Property("Project Name", _project.Name, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property("Project Path", _project.ProjectPath, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property("Project Language", _project.Language, typeof(string), true);
            _properties.Add(property);
        }

        private void BuildGenericProperties(TreeNode node)
        {
            string type = node.GetType().Name.Substring(0, node.GetType().Name.Length - 4);

            _typeName = type + " Properties";

            Property.Property property;
            property = new Property.Property(type + " Name", Name, typeof(string), true);
            _properties.Add(property);
        }
    }
}
