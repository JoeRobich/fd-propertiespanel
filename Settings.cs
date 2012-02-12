using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using PropertiesPanel.Localization;

namespace PropertiesPanel
{
    public delegate void SettingsChangesEvent();

    [Serializable]
    public class Settings
    {
        [field: NonSerialized]
        public event SettingsChangesEvent OnSettingsChanged;

        private const bool DEFAULT_SHOW_IMPORTED_CLASSES = false;

        private bool _showImportedClasses = DEFAULT_SHOW_IMPORTED_CLASSES;

        [LocalizedCategory("PropertiesPanel.Category.Visibility")]
        [LocalizedDisplayName("PropertiesPanel.Label.ShowImportedClasses")]
        [LocalizedDescription("PropertiesPanel.Description.ShowImportedClasses")]
        [DefaultValue(DEFAULT_SHOW_IMPORTED_CLASSES)]
        public bool ShowImportedClasses
        {
            get { return _showImportedClasses; }
            set
            {
                if (_showImportedClasses != value)
                {
                    _showImportedClasses = value;
                    FireChanged();
                }
            }
        }

        private void FireChanged()
        {
            if (OnSettingsChanged != null) OnSettingsChanged();
        }
    }
}
