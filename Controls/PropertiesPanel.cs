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
using PropertiesPanel.Property;

namespace PropertiesPanel.Controls
{
    public partial class PropertiesPanel : DockPanelControl
    {
        private PropertyProvider _provider = null;
        private Font _boldFont = null;
        private ToolStripButton _selectedTabButton = null;
        private List<ToolStripButton> _actionButtons = new List<ToolStripButton>();

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
            categorizeButton.Checked = true;
            categorizeButton.Click += new EventHandler(categorizeButton_Click);

            alphabetizeButton.Text = ResourceHelper.GetString("PropertiesPanel.Label.Alphabetical");
            alphabetizeButton.Image = ResourceHelper.GetImage("SortAlphabetical");
            alphabetizeButton.Click += new EventHandler(alphabetizeButton_Click);
        }

        void alphabetizeButton_Click(object sender, EventArgs e)
        {
            if (!alphabetizeButton.Checked)
            {
                alphabetizeButton.Checked = true;
                categorizeButton.Checked = false;
                propertyGrid.PropertySort = PropertySort.Alphabetical;
            }
        }

        void categorizeButton_Click(object sender, EventArgs e)
        {
            if (!categorizeButton.Checked)
            {
                alphabetizeButton.Checked = false;
                categorizeButton.Checked = true;
                propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical;
            }
        }

        /// <summary>
        /// Initializes the custom rendering
        /// </summary>
        private void InitializeLayout()
        {
            this.toolStrip.Renderer = new DockPanelStripRenderer(false);
            this.itemsStrip.Renderer = new DockPanelStripRenderer(false);
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

            PropertyItem item = (PropertyItem)itemsComboBox.Items[e.Index];
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

        void PropertiesManager_ActiveProviderChanged(PropertyProvider provider)
        {
            UnhookProvider();
            _provider = provider;
            HookProvider();
            BuildToolbar();
            RefreshItems();
        }

        private void BuildToolbar()
        {
            ClearToolbar();
            AddPropertyTabs();
            AddPropertyActions();
        }

        private void ClearToolbar()
        {
            while (toolStrip.Items.Count > 2)
            {
                ToolStripItem item = toolStrip.Items[2];
                if (item.Tag is PropertyTab)
                    UnhookTabEvents(item);
                else if (item.Tag is PropertyAction)
                    UnhookActionEvents(item);
            }

            SelectTab(null);
            _actionButtons.Clear();
        }

        private void AddPropertyTabs()
        {
            PropertyTab[] tabs = _provider.Tabs.ToArray();
            if (tabs.Length == 0)
                return;

            toolStrip.Items.Add(new ToolStripSeparator());

            foreach (PropertyTab tab in tabs)
            {
                ToolStripButton tabButton = new ToolStripButton(tab.Name, tab.Icon);
                
                if (_selectedTabButton == null)
                    SelectTab(tabButton);

                HookTabEvents(tabButton);
                toolStrip.Items.Add(tabButton);
            }
        }

        private void AddPropertyActions()
        {
            PropertyAction[] actions = _provider.Actions.ToArray();
            if (actions.Length == 0)
                return;

            toolStrip.Items.Add(new ToolStripSeparator());

            foreach (PropertyAction action in actions)
            {
                ToolStripButton actionButton = new ToolStripButton(action.Name, action.Icon);
                HookTabEvents(actionButton);
                toolStrip.Items.Add(actionButton);
                _actionButtons.Add(actionButton);
            }
        }

        private void SelectTab(ToolStripButton tabButton)
        {
            if (_selectedTabButton != null)
                _selectedTabButton.Checked = false;

            _selectedTabButton = tabButton;

            PropertyTab selectedTab = null;
            if (_selectedTabButton != null)
            {
                _selectedTabButton.Checked = true;
                selectedTab = (PropertyTab)_selectedTabButton.Tag;
            }

            _provider.SelectedTab = selectedTab;
        }

        private void HookTabEvents(ToolStripItem tabItem)
        {
            tabItem.Click += new EventHandler(tabItem_Click);
        }

        private void UnhookTabEvents(ToolStripItem tabItem)
        {
            tabItem.Click -= new EventHandler(tabItem_Click);
        }

        private void HookActionEvents(ToolStripItem actionItem)
        {
            actionItem.Click += new EventHandler(actionItem_Click);
        }

        private void UnhookActionEvents(ToolStripItem actionItem)
        {
            actionItem.Click -= new EventHandler(actionItem_Click);
        }

        void actionItem_Click(object sender, EventArgs e)
        {
            ToolStripButton actionButton = (ToolStripButton)sender;
            PropertyAction action = (PropertyAction)actionButton.Tag;
            action.OnActionFired(_provider, _provider.SelectedItem);
        }

        void tabItem_Click(object sender, EventArgs e)
        {
            ToolStripButton tabButton = (ToolStripButton)sender;
            SelectTab(tabButton);
        }

        void _provider_SelectionChanged(PropertyProvider provider, PropertyItem selectedItem)
        {
            UpdateSelectedItem();
        }

        void _provider_ItemsChanged(PropertyProvider provider)
        {
            UpdateItems();
        }
    }
}
