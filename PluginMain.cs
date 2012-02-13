using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using PluginCore.Localization;
using PluginCore.Utilities;
using PluginCore.Managers;
using PluginCore.Helpers;
using PluginCore;
using ASCompletion.Context;
using ASCompletion.Model;
using PropertiesPanel.Controls;
using PropertiesPanel.Helpers;
using WeifenLuo.WinFormsUI.Docking;
using PropertiesPanel.Manager;
using PropertiesPanel.Outline;
using PropertiesPanel.Project;
using PropertiesPanel.Files;

namespace PropertiesPanel
{
    public class PluginMain : IPlugin
    {
        private const int API = 1;
        private const string NAME = "PropertiesPanel";
        private const string GUID = "B44CB843-5869-48a9-BD82-0D9AF5A9042F";
        private const string HELP = "www.flashdevelop.org/community/";
        private const string DESCRIPTION = "Provides a panel for viewing/editing the properties of selected items from other panels.";
        private const string AUTHOR = "Joey Robichaud";

        private Image _propertiesImage;
        private static Controls.PropertiesPanel _propertiesPanel;
        private DockContent _propertiesContent;
        private string _settingFilename = "";
        private Settings _settings;

        #region Required Properties

        /// <summary>
        /// Api level of the plugin
        /// </summary>
        public Int32 Api
        {
            get { return 1; }
        }

        /// <summary>
        /// Name of the plugin
        /// </summary> 
        public String Name
        {
            get { return NAME; }
        }

        /// <summary>
        /// GUID of the plugin
        /// </summary>
        public String Guid
        {
            get { return GUID; }
        }

        /// <summary>
        /// Author of the plugin
        /// </summary> 
        public String Author
        {
            get { return AUTHOR; }
        }

        /// <summary>
        /// Description of the plugin
        /// </summary> 
        public String Description
        {
            get { return DESCRIPTION; }
        }

        /// <summary>
        /// Web address for help
        /// </summary> 
        public String Help
        {
            get { return HELP; }
        }

        /// <summary>
        /// Object that contains the settings
        /// </summary>
        [Browsable(false)]
        public Object Settings
        {
            get { return this._settings; }
        }

        #endregion

        #region Required Methods

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public void Initialize()
        {
            this.InitBasics();
            this.LoadSettings();
            this.AddEventHandlers();
            this.CreateMenuItems();
            this.CreatePluginPanel();
            this.RegisterProviders();
        }

        /// <summary>
        /// Disposes the plugin
        /// </summary>
        public void Dispose()
        {
            this.SaveSettings();
            _propertiesPanel.Dispose();
        }

        /// <summary>
        /// Handles the incoming events
        /// </summary>
        public void HandleEvent(Object sender, NotifyEvent e, HandlingPriority prority)
        {
            
        }

        #endregion

        public static Controls.PropertiesPanel PropertiesPanel
        {
            get { return _propertiesPanel; }
        }

        /// <summary>
        /// Opens the plugin panel if closed
        /// </summary>
        public void ShowProperties(Object sender, System.EventArgs e)
        {
            _propertiesContent.Show();
        }

        void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            Control activeContent = PluginBase.MainForm.DockPanel.ActiveContent as Control;
            if (activeContent != null && 
                activeContent.Controls.Count > 0 && 
                activeContent.Controls[0] is DockPanelControl)
            {
                DockPanelControl panel = activeContent.Controls[0] as DockPanelControl;
                PropertiesManager.ActivateProvider(panel);
            }
        }

        void RegisterProviders()
        {
            PropertiesManager.RegisterProviderByType(typeof(ASCompletion.PluginUI), new OutlineProvider());
            PropertiesManager.RegisterProviderByType(typeof(ProjectManager.PluginUI), new ProjectProvider());
            PropertiesManager.RegisterProviderByType(typeof(FileExplorer.PluginUI), new FilesProvider());
        }

        #region Custom Methods

        /// <summary>
        /// Initializes important variables
        /// </summary>
        public void InitBasics()
        {
            String dataPath = Path.Combine(PathHelper.DataDir, NAME);
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
            this._settingFilename = Path.Combine(dataPath, "Settings.fdb");

            //Pull the member icons from the resources;
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ASCompletion.PluginUI));
            ImageList icons = new ImageList();
            icons.ImageStream = ((ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
            _propertiesImage = icons.Images["Property.png"];
        }

        /// <summary>
        /// Adds the required event handlers
        /// </summary> 
        public void AddEventHandlers()
        {
            // Set events you want to listen (combine as flags)
            EventManager.AddEventHandler(this, EventType.UIStarted | EventType.ApplySettings);
            PluginBase.MainForm.DockPanel.ActiveContentChanged += new EventHandler(DockPanel_ActiveContentChanged);
        }

        /// <summary>
        /// Creates a plugin panel for the plugin
        /// </summary>
        public void CreatePluginPanel()
        {
            _propertiesPanel = new Controls.PropertiesPanel();
            _propertiesPanel.Text = ResourceHelper.GetString("PropertiesPanel.Title.PropertiesPanel");
            this._propertiesContent = PluginBase.MainForm.CreateDockablePanel(_propertiesPanel, GUID, _propertiesImage, DockState.DockRightAutoHide);
        }

        /// <summary>
        /// Adds shortcuts for manipulating the navigation bar
        /// </summary>
        public void CreateMenuItems()
        {
            ToolStripMenuItem viewMenu = (ToolStripMenuItem)PluginBase.MainForm.FindMenuItem("ViewMenu");
            ToolStripMenuItem viewItem = new ToolStripMenuItem(ResourceHelper.GetString("PropertiesPanel.Label.ShowProperties"), _propertiesImage, new EventHandler(ShowProperties), null);
            PluginBase.MainForm.RegisterShortcutItem("PropertiesPanel.ShowProperties", viewItem);
            viewMenu.DropDownItems.Add(viewItem);
        }

        /// <summary>
        /// Loads the plugin settings
        /// </summary>
        public void LoadSettings()
        {
            this._settings = new Settings();
            if (!File.Exists(this._settingFilename)) this.SaveSettings();
            else
            {
                Object obj = ObjectSerializer.Deserialize(this._settingFilename, this._settings);
                this._settings = (Settings)obj;
            }
        }

        /// <summary>
        /// Saves the plugin settings
        /// </summary>
        public void SaveSettings()
        {
            ObjectSerializer.Serialize(this._settingFilename, this._settings);
        }

        #endregion
    }
}