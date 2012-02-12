using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PropertiesPanel
{
    public class PropertyItemBase : IPropertyItem, ICustomTypeDescriptor
    {
        protected Dictionary<IPropertyTab, List<IProperty>> _tabPropertyMap = new Dictionary<IPropertyTab, List<IProperty>>(); 
        protected List<IProperty> _properties = new List<IProperty>();
        protected string _name = string.Empty;
        protected string _typeName = string.Empty;

        protected void OnPropertyChanged(IProperty property)
        {
            if (PropertyChanged != null)
                PropertyChanged(property);
        }

        #region IPropertyItem Members

        public event ValueChangedHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public IEnumerable<IProperty> Properties
        {
            get { return _properties; }
        }

        public IEnumerable<IProperty> GetTabProperties(IPropertyTab tab)
        {
            return _tabPropertyMap[tab];
        }

        #endregion

        #region ICustomTypeDescriptor Members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        private PropertyDescriptorCollection _propertyCache;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return ((ICustomTypeDescriptor)this).GetProperties();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            if (_propertyCache != null)
                return _propertyCache;

            List<PropertyPropertyDescriptor> properties = new List<PropertyPropertyDescriptor>();
            foreach (var property in this.Properties)
            {
                properties.Add(new PropertyPropertyDescriptor(property));
            }
            _propertyCache = new PropertyDescriptorCollection(properties.ToArray());

            return _propertyCache;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    class PropertyPropertyDescriptor : PropertyDescriptor
    {
        IProperty _property;

        public PropertyPropertyDescriptor(IProperty property)
            : base(property.DisplayName, null)
        {
            _property = property;
        }

        #region PropertyDescriptor Members

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return typeof(PropertyItemBase); }
        }

        public override object GetValue(object component)
        {
            return _property.Value;
        }

        public override bool IsReadOnly
        {
            get { return _property.ReadOnly; }
        }

        public override Type PropertyType
        {
            get { return _property.ValueType; }
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            _property.Value = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        #endregion

    }
}
