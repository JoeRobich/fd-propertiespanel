using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PluginCore;
using PropertiesPanel.Helpers;
using PropertiesPanel.Manager;
using PropertiesPanel.Property;
using System.Diagnostics;

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
            InitializeFonts();
            InitializeToolStrip();
            InitializeLayout();
            HookEvents();
        }

        private void InitializeFonts()
        {
            itemsComboBox.Font = PluginBase.Settings.DefaultFont;
            propertyGrid.Font = PluginBase.Settings.DefaultFont;
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
            e.Graphics.DrawString(item.Type, e.Font, brush, e.Bounds.Left + nameLength, e.Bounds.Top);         
        }

        void HookProvider()
        {
            if (_provider != null)
            {
                _provider.ItemsChanged += new ItemsChangedHandler(_provider_ItemsChanged);
                _provider.SelectionChanged += new SelectionChangedHandler(_provider_SelectionChanged);
            }
        }

        void UnhookProvider()
        {
            if (_provider != null)
            {
                _provider.ItemsChanged -= new ItemsChangedHandler(_provider_ItemsChanged);
                _provider.SelectionChanged -= new SelectionChangedHandler(_provider_SelectionChanged);
            }
        }

        void RefreshItems()
        {
            UpdateItems();
            UpdateSelectedItems();
        }

        void UpdateItems()
        {
            itemsComboBox.Items.Clear();
            List<PropertyItem> itemList = new List<PropertyItem>(_provider.Items);
            itemsComboBox.Items.AddRange(itemList.ToArray());
        }

        void UpdateSelectedItems()
        {
            List<PropertyItem> itemList = new List<PropertyItem>(_provider.SelectedItems);
            PropertyItem[] selectedItems = itemList.ToArray();
            itemsComboBox.SelectedItem = selectedItems.Length == 1 ? selectedItems[0] : null;
            propertyGrid.SelectedObjects = selectedItems;

            UpdateTabActions();
        }

        void UpdateTabActions()
        {
            foreach (ToolStripButton actionButton in _actionButtons)
            {
                PropertyAction action = actionButton.Tag as PropertyAction;
                actionButton.Enabled = action.IsEnabled(_provider, _provider.SelectedItems);
            }
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
            RefreshControls();
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

                toolStrip.Items.Remove(item);
            }

            SelectTab(null);
            _actionButtons.Clear();
        }

        private void AddPropertyTabs()
        {
            List<PropertyTab> tabList = new List<PropertyTab>(_provider.Tabs);
            PropertyTab[] tabs = tabList.ToArray();
            if (tabs.Length == 0)
                return;

            toolStrip.Items.Add(new ToolStripSeparator());

            foreach (PropertyTab tab in tabs)
            {
                ToolStripButton tabButton = new ToolStripButton(tab.Name, tab.Icon);
                tabButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
                tabButton.Tag = tab;
                
                if (_selectedTabButton == null)
                    SelectTab(tabButton);

                HookTabEvents(tabButton);
                toolStrip.Items.Add(tabButton);
            }
        }

        private void AddPropertyActions()
        {
            List<PropertyAction> actionList = new List<PropertyAction>(_provider.Actions);
            PropertyAction[] actions = actionList.ToArray();
            if (actions.Length == 0)
                return;

            toolStrip.Items.Add(new ToolStripSeparator());

            foreach (PropertyAction action in actions)
            {
                ToolStripButton actionButton = new ToolStripButton(action.Name, action.Icon);
                actionButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
                actionButton.Tag = action;

                HookActionEvents(actionButton);
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
            action.OnActionClicked(_provider, _provider.SelectedItems);
        }

        void tabItem_Click(object sender, EventArgs e)
        {
            ToolStripButton tabButton = (ToolStripButton)sender;
            SelectTab(tabButton);
        }

        void _provider_SelectionChanged(PropertyProvider provider)
        {
            UpdateSelectedItems();
        }

        void _provider_ItemsChanged(PropertyProvider provider)
        {
            UpdateItems();
        }

        private void itemsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<PropertyItem> itemList = new List<PropertyItem>(_provider.SelectedItems);
            PropertyItem[] selectedItems = itemList.ToArray();
            PropertyItem selectedItem = (PropertyItem)itemsComboBox.SelectedItem;

            if (selectedItems.Length > 1 && itemsComboBox.Items.Count > 1)
            {
                itemsComboBox.Items.Clear();
                itemsComboBox.Items.Add(selectedItem);
                itemsComboBox.SelectedItem = selectedItem;
            }

            propertyGrid.SelectedObject = selectedItem;
        }

        public void RefreshControls()
        {
            itemsStrip.Refresh();
            toolStrip.Refresh();
            propertyGrid.Refresh();
        }
    }
}
