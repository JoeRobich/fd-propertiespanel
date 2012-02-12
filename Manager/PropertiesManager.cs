using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PropertiesPanel.Manager
{
    public delegate void ProviderChangedHandler(IPropertyProvider provider);

    public class PropertiesManager
    {
        private static Dictionary<Type, IPropertyProvider> _providerMap = new Dictionary<Type, IPropertyProvider>();
        private static IPropertyProvider _activeProvider = null;

        #region Public Events and Methods

        public static event ProviderChangedHandler ActiveProviderChanged;

        public static IPropertyProvider ActiveProvider
        {
            get
            {
                return _activeProvider;
            }
        }

        public static void RegisterProvider(DockPanelControl panel, IPropertyProvider provider)
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

        internal static void RegisterProviderByType(Type panelType, IPropertyProvider provider)
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
            IPropertyProvider provider = GetProvider(panelType);

            if (_activeProvider == provider || provider == null)
                return;

            if (_activeProvider != null)
                _activeProvider.Deactivate();

            _activeProvider = provider;

            if (_activeProvider != null)
                _activeProvider.Activate(panel);

            OnActiveProviderChanged(_activeProvider);
        }

        internal static IPropertyProvider GetProvider(Type panelType)
        {
            if (!_providerMap.ContainsKey(panelType))
                return null;

            return _providerMap[panelType];
        }

        internal static void OnActiveProviderChanged(IPropertyProvider provider)
        {
            if (ActiveProviderChanged != null)
                ActiveProviderChanged(provider);
        }

        #endregion
    }
}
