using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PropertiesPanel.Property
{
    public delegate void ValueChangedHandler(Property property);

    public class Property
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

        #region Constructors

        public Property()
        {

        }

        public Property(string displayName, object value, Type valueType)
            : this(displayName, string.Empty, string.Empty, value, valueType, false)
        {

        }

        public Property(string displayName, object value, Type valueType, bool readOnly)
            : this(displayName, string.Empty, string.Empty, value, valueType, readOnly)
        {

        }

        public Property(string displayName, string category, object value, Type valueType, bool readOnly)
            : this(displayName, category, string.Empty, value, valueType, readOnly)
        {

        }

        public Property(string displayName, string category, string description, object value, Type valueType, bool readOnly)
        {
            _displayName = displayName;
            _category = category;
            _description = description;
            _value = value;
            _valueType = valueType;
            _readOnly = readOnly;

            if (_readOnly)
                _defaultValue = _value;
        }

        #endregion

        protected void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this);
        }

        #region Property Members

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
