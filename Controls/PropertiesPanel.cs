using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginCore;
using PropertiesPanel.Helpers;
using PropertiesPanel.Manager;

namespace PropertiesPanel.Controls
{
    public partial class PropertiesPanel : DockPanelControl
    {
        private IPropertyProvider _provider = null;
        private Font _boldFont = null;

        public PropertiesPanel()
        {
            InitializeComponent();
            InitializeToolStrip();
            InitializeLayout();
            HookEvents();
        }

        private void InitializeToolStrip()
        {
            categorizeButton.Text = ResourceHelper.GetString("PropertiesPanel.Label.Categorized");
            categorizeButton.Image = ResourceHelper.GetImage("SortCategorized");
            alphabetizeButton.Text = ResourceHelper.GetString("PropertiesPanel.Label.Alphabetical");
            alphabetizeButton.Image = ResourceHelper.GetImage("SortAlphabetical");
            propertiesButton.Text = ResourceHelper.GetString("PropertiesPanel.Label.Properties");
            propertiesButton.Image = ResourceHelper.GetImage("Properties");
            eventsButton.Text = ResourceHelper.GetString("PropertiesPanel.Label.Events");
            eventsButton.Image = ResourceHelper.GetImage("Event");
        }

        /// <summary>
        /// Initializes the custom rendering
        /// </summary>
        private void InitializeLayout()
        {
            this.toolStrip.Renderer = new DockPanelStripRenderer();
            this.itemsStrip.Renderer = new DockPanelStripRenderer();
            this.itemsComboBox.FlatStyle = PluginBase.Settings.ComboBoxFlatStyle;
        }

        private void HookEvents()
        {
            PropertiesManager.ActiveProviderChanged += new ProviderChangedHandler(PropertiesManager_ActiveProviderChanged);
            this.itemsComboBox.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.itemsComboBox.ComboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
        }

        void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (_boldFont == null)
                _boldFont = new Font(e.Font, FontStyle.Bold);

            if (e.Index == -1)
                return;

            IPropertyItem item = (IPropertyItem)itemsComboBox.Items[e.Index];
            float nameLength = e.Graphics.MeasureString(item.Name, _boldFont).Width;

            e.DrawBackground();

            Brush brush = new SolidBrush(e.ForeColor);
            e.Graphics.DrawString(item.Name, _boldFont, brush, e.Bounds.Left, e.Bounds.Top);
            e.Graphics.DrawString(item.TypeName, e.Font, brush, e.Bounds.Left + nameLength, e.Bounds.Top);         
        }

        void HookProvider()
        {
            if (_provider != null)
            {
                _provider.ItemsChanged += new ItemsChangesHandler(_provider_ItemsChanged);
                _provider.SelectionChanged += new SelectionChangedHandler(_provider_SelectionChanged);
            }
        }

        void UnhookProvider()
        {
            if (_provider != null)
            {
                _provider.ItemsChanged -= new ItemsChangesHandler(_provider_ItemsChanged);
                _provider.SelectionChanged -= new SelectionChangedHandler(_provider_SelectionChanged);
            }
        }

        void RefreshItems()
        {
            UpdateItems();
            UpdateSelectedItem();
        }

        void UpdateItems()
        {
            itemsComboBox.Items.Clear();
            itemsComboBox.Items.AddRange(_provider.Items.ToArray());
        }

        void UpdateSelectedItem()
        {
            itemsComboBox.SelectedItem = _provider.SelectedItem;
            propertyGrid.SelectedObject = itemsComboBox.SelectedItem;
        }

        void PropertiesManager_ActiveProviderChanged(IPropertyProvider provider)
        {
            UnhookProvider();
            _provider = provider;
            HookProvider();
            RefreshItems();
        }

        void _provider_SelectionChanged(IPropertyProvider provider, IPropertyItem selectedItem)
        {
            UpdateSelectedItem();
        }

        void _provider_ItemsChanged(IPropertyProvider provider)
        {
            UpdateItems();
        }
    }
}
