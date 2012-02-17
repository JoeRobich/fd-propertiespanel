using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCompletion.Model;
using ASCompletion.Context;
using PropertiesPanel.Property;
using PropertiesPanel.Helpers;
using ASCompletion.Completion;
using CodeRefactor.Commands;
using CodeRefactor.Provider;

namespace PropertiesPanel.Outline
{
    public class OutlineItem: PropertyItem
    {
        private TreeNode _node = null;

        public OutlineItem(TreeNode node)
        {
            _node = node;
            InitializeItem();
        }

        public TreeNode Node
        {
            get { return _node; }
        }

        public void NavigateTo()
        {
            ASContext.Context.OnSelectOutlineNode(Node);
        }

        void nameProperty_ValueChanged(PropertiesPanel.Property.Property property)
        {
            NavigateTo();
            Rename rename = new Rename(RefactoringHelper.GetDefaultRefactorTarget(), true, (string)property.Value);
            rename.Execute();
        }

        private void InitializeItem()
        {
            _name = _node.Text;

            if (_node.Tag != null)
            {
                string tag = _node.Tag.ToString();
                if (tag.Contains('@'))
                {
                    MemberModel memberModel = null;
                    string[] parts = tag.Split('@');
                    memberModel = FindMember(parts[0], int.Parse(parts[1]));
                    BuildMemberProperties(memberModel);
                }
                else if (tag == "import")
                {
                    MemberModel importModel = null;
                    importModel = FindImport(Name);
                    if (importModel is ClassModel)
                        BuildClassProperties((ClassModel)importModel, true);
                    else
                        BuildImportProperties(importModel);
                }
                else if (tag == "class")
                {
                   ClassModel classModel = null;
                   classModel = FindClass(Name);
                   BuildClassProperties(classModel, false);
                }
                else
                {
                    _type = tag;
                }
            }
            else
            {
                if (_name.Contains('.'))
                    BuildFileProperties();
                else
                    BuildFolderProperties();
            }
        }

        private MemberModel FindMember(string name, int lineFrom)
        {
            if (!ASContext.HasContext)
                return null;

            foreach (MemberModel memberModel in ASContext.Context.CurrentModel.Members)
                if (memberModel.Name == name && memberModel.LineFrom == lineFrom)
                    return memberModel;

            foreach (ClassModel classModel in ASContext.Context.CurrentModel.Classes)
                foreach (MemberModel memberModel in classModel.Members)
                    if (memberModel.Name == name && memberModel.LineFrom == lineFrom)
                        return memberModel;

            return null;   
        }

        private MemberModel FindImport(string name)
        {
            if (!ASContext.HasContext)
                return null;

            foreach (MemberModel importModel in ASContext.Context.CurrentModel.Imports)
                if (importModel.Type == name || importModel.Type.EndsWith(name))
                    return importModel;

            foreach (ClassModel classModel in ASContext.Context.CurrentModel.Classes)
            {
                classModel.ResolveExtends();
                ClassModel extendsModel = classModel.Extends;
                while (extendsModel.Type != "Object" && extendsModel != ClassModel.VoidClass)
                {
                    if (extendsModel.Type == name)
                        return extendsModel;

                    extendsModel = extendsModel.Extends;
                }
            }

            return null;
        }

        private ClassModel FindClass(string name)
        {
            if (!ASContext.HasContext)
                return null;

            foreach (ClassModel classModel in ASContext.Context.CurrentModel.Classes)
                if (classModel.Name == name)
                    return classModel;

            return null;
        }

        private void BuildMemberProperties(MemberModel model)
        {
            if (model == null)
                return;

            _name = model.Name;

            _type = (model.Flags & FlagType.Constant) > 0 ? ResourceHelper.GetString("PropertiesPanel.Label.ConstantProperties") :
                                (model.Flags & (FlagType.Getter | FlagType.Setter)) > 0 ? ResourceHelper.GetString("PropertiesPanel.Label.PropertyProperties") :
                                (model.Flags & FlagType.Variable) > 0 ? ResourceHelper.GetString("PropertiesPanel.Label.VariableProperties") :
                                (model.Flags & FlagType.Constructor) > 0 ? ResourceHelper.GetString("PropertiesPanel.Label.ConstructorProperties") : ResourceHelper.GetString("PropertiesPanel.Label.FunctionProperties");

            string visibility = (model.Access & Visibility.Internal) > 0 ? "Internal" :
                                (model.Access & Visibility.Private) > 0 ? "Private" :
                                (model.Access & Visibility.Protected) > 0 ? "Protected" :
                                (model.Access & Visibility.Public) > 0 ? "Public" : "Default";

            bool isStatic = (model.Flags & FlagType.Static) > 0;
            bool isOverride = (model.Flags & FlagType.Override) > 0;
            bool isFinal = (model.Flags & FlagType.Function) > 0;

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), false);
            property.ValueChanged += new ValueChangedHandler(nameProperty_ValueChanged);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Type"), model.Type, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Visibility"), visibility, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsStatic"), isStatic, typeof(bool), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsOverride"), isOverride, typeof(bool), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsFinal"), isFinal, typeof(bool), true);
            _properties.Add(property);
        }

        private void BuildImportProperties(MemberModel model)
        {
            if (model == null)
                return;

            _name = model.Name;

            if ((model.Flags & FlagType.Interface) > 0)
                _type = ResourceHelper.GetString("PropertiesPanel.Label.InterfaceProperties");
            else if ((model.Flags & FlagType.Enum) > 0)
                _type = ResourceHelper.GetString("PropertiesPanel.Label.EnumProperties");
            else
                _type = ResourceHelper.GetString("PropertiesPanel.Label.ClassProperties");

            string visibility = (model.Access & Visibility.Internal) > 0 ? "Internal" :
                    (model.Access & Visibility.Private) > 0 ? "Private" :
                    (model.Access & Visibility.Protected) > 0 ? "Protected" :
                    (model.Access & Visibility.Public) > 0 ? "Public" : "Default";

            bool isIntrinsic = (model.Flags & FlagType.Intrinsic) > 0;
            bool isDynamic = (model.Flags & FlagType.Dynamic) > 0;

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Namespace"), model.Type.Substring(0, model.Type.Length - model.Name.Length - 1), typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Visibility"), visibility, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsIntrinsic"), isIntrinsic, typeof(bool), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsDynamic"), isDynamic, typeof(bool), true);
            _properties.Add(property);
        }

        private void BuildClassProperties(ClassModel model, bool isNameReadonly)
        {
            if (model == null)
                return;

            _name = model.Name;

            if ((model.Flags & FlagType.Interface) > 0)
                _type = ResourceHelper.GetString("PropertiesPanel.Label.InterfaceProperties");
            else if ((model.Flags & FlagType.Enum) > 0)
                _type = ResourceHelper.GetString("PropertiesPanel.Label.EnumProperties");
            else
                _type = ResourceHelper.GetString("PropertiesPanel.Label.ClassProperties");

            string visibility = (model.Access & Visibility.Internal) > 0 ? "Internal" :
                    (model.Access & Visibility.Private) > 0 ? "Private" :
                    (model.Access & Visibility.Protected) > 0 ? "Protected" :
                    (model.Access & Visibility.Public) > 0 ? "Public" : "Default";

            bool isIntrinsic = (model.Flags & FlagType.Intrinsic) > 0;
            bool isDynamic = (model.Flags & FlagType.Dynamic) > 0;
            string namespaceName = model.QualifiedName == model.Name ? string.Empty : model.QualifiedName.Substring(0, model.QualifiedName.Length - model.Name.Length - 1);

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), isNameReadonly);
            property.ValueChanged +=new ValueChangedHandler(nameProperty_ValueChanged);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Namespace"), namespaceName, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Visibility"), visibility, typeof(string), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsIntrinsic"), isIntrinsic, typeof(bool), true);
            _properties.Add(property);
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.IsDynamic"), isDynamic, typeof(bool), true);
            _properties.Add(property);
        }

        private void BuildFileProperties()
        {
            _type = ResourceHelper.GetString("PropertiesPanel.Label.FileProperties");

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), true);
            _properties.Add(property);
        }

        private void BuildFolderProperties()
        {
            _type = ResourceHelper.GetString("PropertiesPanel.Label.FolderProperties");

            Property.Property property;
            property = new Property.Property(ResourceHelper.GetString("PropertiesPanel.Label.Name"), Name, typeof(string), true);
            _properties.Add(property);
        }
    }
}
