using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PropertiesPanel.Manager;

namespace PropertiesPanel.Property
{
    public class PropertyItem : ICustomTypeDescriptor
    {
        protected Dictionary<PropertyTab, List<Property>> _tabPropertyMap = new Dictionary<PropertyTab, List<Property>>();
        protected List<Property> _properties = new List<Property>();
        protected string _name = string.Empty;
        protected string _typeName = string.Empty;

        protected void OnPropertyChanged(Property property)
        {
            if (PropertyChanged != null)
                PropertyChanged(property);
        }

        #region PropertyItem Members

        public event ValueChangedHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            protected set
            {
                _name = value;
            }
        }

        public string TypeName
        {
            get { return _typeName; }
            protected set
            {
                _typeName = value;
            }
        }

        public IEnumerable<Property> Properties
        {
            get { return _properties; }
        }

        public IEnumerable<Property> GetTabProperties(PropertyTab tab)
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
        private Dictionary<PropertyTab, PropertyDescriptorCollection> _tabPropertyCache = new Dictionary<PropertyTab, PropertyDescriptorCollection>();

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return ((ICustomTypeDescriptor)this).GetProperties();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            PropertyTab selectedTab = PropertiesManager.ActiveProvider.SelectedTab;

            if (selectedTab == null)
            {
                if (_propertyCache == null)
                {
                    List<PropertyPropertyDescriptor> properties = new List<PropertyPropertyDescriptor>();
                    foreach (var property in this.Properties)
                    {
                        properties.Add(new PropertyPropertyDescriptor(property));
                    }
                    _propertyCache = new PropertyDescriptorCollection(properties.ToArray());
                }
                return _propertyCache;
            }
            else
            {
                if (!_tabPropertyCache.ContainsKey(selectedTab))
                {
                    List<PropertyPropertyDescriptor> properties = new List<PropertyPropertyDescriptor>();
                    foreach (var property in this.GetTabProperties(selectedTab))
                    {
                        properties.Add(new PropertyPropertyDescriptor(property));
                    }
                    _tabPropertyCache[selectedTab] = new PropertyDescriptorCollection(properties.ToArray());
                }

                return _tabPropertyCache[selectedTab];
            }
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }

    class PropertyPropertyDescriptor : PropertyDescriptor
    {
        Property _property;

        public PropertyPropertyDescriptor(Property property)
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
            get { return typeof(PropertyItem); }
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
