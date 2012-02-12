using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PropertiesPanel.Property;

namespace PropertiesPanel.Manager
{
    public delegate void ProviderChangedHandler(PropertyProvider provider);

    public class PropertiesManager
    {
        private static Dictionary<Type, PropertyProvider> _providerMap = new Dictionary<Type, PropertyProvider>();
        private static PropertyProvider _activeProvider = null;

        #region Public Events and Methods

        public static event ProviderChangedHandler ActiveProviderChanged;

        public static PropertyProvider ActiveProvider
        {
            get
            {
                return _activeProvider;
            }
        }

        public static void RegisterProvider(DockPanelControl panel, PropertyProvider provider)
        {
            Type panelType = panel.GetType();
            RegisterProviderByType(panelType, provider);
        }

        public static void UnregisterProvider(DockPanelControl panel)
        {
            Type panelType = panel.GetType();
            UnregisterProviderByType(panelType);
        }

        #endregion

        #region Internal Methods

        internal static void RegisterProviderByType(Type panelType, PropertyProvider provider)
        {
            _providerMap[panelType] = provider;
        }

        internal static void UnregisterProviderByType(Type panelType)
        {
            if (!_providerMap.ContainsKey(panelType))
                return;

            _providerMap.Remove(panelType);
        }

        internal static void ActivateProvider(DockPanelControl panel)
        {
            Type panelType = panel.GetType();
            PropertyProvider provider = GetProvider(panelType);

            if (_activeProvider == provider || provider == null)
                return;

            if (_activeProvider != null)
                _activeProvider.Deactivate();

            _activeProvider = provider;

            if (_activeProvider != null)
                _activeProvider.Activate(panel);

            OnActiveProviderChanged(_activeProvider);
        }

        internal static PropertyProvider GetProvider(Type panelType)
        {
            if (!_providerMap.ContainsKey(panelType))
                return null;

            return _providerMap[panelType];
        }

        internal static void OnActiveProviderChanged(PropertyProvider provider)
        {
            if (ActiveProviderChanged != null)
                ActiveProviderChanged(provider);
        }

        #endregion
    }
}
