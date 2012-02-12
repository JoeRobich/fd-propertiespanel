﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCompletion.Model;
using ASCompletion.Context;

namespace PropertiesPanel.Outline
{
    public class OutlineItem: PropertyItemBase
    {
        private TreeNode _node = null;

        public OutlineItem(TreeNode node)
        {
            _node = node;
            InitializeItem();
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
                        BuildClassProperties((ClassModel)importModel);
                    else
                        BuildImportProperties(importModel);
                }
                else if (tag == "class")
                {
                   ClassModel classModel = null;
                   classModel = FindClass(Name);
                   BuildClassProperties(classModel);
               }
                else
                {
                    _typeName = tag;
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
                if (importModel.Type == name)
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
            _typeName = "Member Properties";

            OutlineProperty property;
            property = new OutlineProperty("Member Name", string.Empty, null, Name, typeof(string));
            _properties.Add(property);
            property = new OutlineProperty("Member Type", string.Empty, null, model.Type, typeof(string));
            _properties.Add(property);
        }

        private void BuildImportProperties(MemberModel model)
        {
            if (model == null)
                return;

            _name = model.Name;
            _typeName = "Class Properties";

            OutlineProperty property;
            property = new OutlineProperty("Class Name", string.Empty, null, Name, typeof(string));
            _properties.Add(property);
            property = new OutlineProperty("Class Namespace", string.Empty, null, model.Type.Substring(0, model.Type.Length - model.Name.Length - 1), typeof(string));
            _properties.Add(property);
        }

        private void BuildClassProperties(ClassModel model)
        {
            if (model == null)
                return;

            _name = model.Name;
            _typeName = "Class Properties";

            OutlineProperty property;
            property = new OutlineProperty("Class Name", string.Empty, null, Name, typeof(string));
            _properties.Add(property);
            property = new OutlineProperty("Class Namespace", string.Empty, null, model.QualifiedName.Substring(0, model.QualifiedName.Length - model.Name.Length - 1), typeof(string));
            _properties.Add(property);
        }

        private void BuildFileProperties()
        {
            _typeName = "File Properties";

            OutlineProperty property;
            property = new OutlineProperty("File Name", string.Empty, null, Name, typeof(string));
            _properties.Add(property);
        }

        private void BuildFolderProperties()
        {
            _typeName = "Folder Properties";

            OutlineProperty property;
            property = new OutlineProperty("Folder Name", string.Empty, null, Name, typeof(string));
            _properties.Add(property);
        }
    }
}
