using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PropertiesPanel
{
    public class PropertyBase : IProperty
    {
        protected string _displayName = string.Empty;
        protected string _description = string.Empty;
        protected string _category = string.Empty;
        protected bool _browsable = true;
        protected bool _readOnly = false;
        protected object _defaultValue = null;
        protected bool _restrictToList = false;
        protected object[] _listValues = null;
        protected object _value = null;
        protected Type _valueType = null;

        public PropertyBase(string displayName, string description, string category, object value, Type valueType)
        {
            _displayName = displayName;
            _description = description;
            _category = category;
            _value = value;
            _valueType = valueType;
        }

        protected void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        #region IProperty Members

        public event ValueChangedHandler ValueChanged;

        public string DisplayName
        {
            get { return _displayName; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Category
        {
            get { return _category; }
        }

        public bool Browsable
        {
            get { return _browsable; }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public bool RestrictToList
        {
            get { return _restrictToList; }
        }

        public object[] ListValues
        {
            get { return _listValues; }
        }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnValueChanged();
                }
            }
        }

        public Type ValueType
        {
            get { return _valueType; }
        }

        #endregion
    }
}
