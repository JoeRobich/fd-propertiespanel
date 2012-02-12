namespace PropertiesPanel.Controls
{
    partial class PropertiesPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesPanel));
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.itemsStrip = new System.Windows.Forms.ToolStrip();
            this.itemsComboBox = new System.Windows.Forms.ToolStripSpringComboBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.categorizeButton = new System.Windows.Forms.ToolStripButton();
            this.alphabetizeButton = new System.Windows.Forms.ToolStripButton();
            this.sortSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesButton = new System.Windows.Forms.ToolStripButton();
            this.eventsButton = new System.Windows.Forms.ToolStripButton();
            this.stripContainer = new System.Windows.Forms.ToolStripContainer();
            this.itemsStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.stripContainer.ContentPanel.SuspendLayout();
            this.stripContainer.TopToolStripPanel.SuspendLayout();
            this.stripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(222, 249);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // itemsStrip
            // 
            this.itemsStrip.AutoSize = false;
            this.itemsStrip.CanOverflow = false;
            this.itemsStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.itemsStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.itemsStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemsComboBox});
            this.itemsStrip.Location = new System.Drawing.Point(0, 0);
            this.itemsStrip.Name = "itemsStrip";
            this.itemsStrip.Padding = new System.Windows.Forms.Padding(0);
            this.itemsStrip.Size = new System.Drawing.Size(222, 27);
            this.itemsStrip.Stretch = true;
            this.itemsStrip.TabIndex = 1;
            // 
            // itemsComboBox
            // 
            this.itemsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemsComboBox.Name = "itemsComboBox";
            this.itemsComboBox.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.itemsComboBox.Size = new System.Drawing.Size(189, 27);
            // 
            // toolStrip
            // 
            this.toolStrip.CanOverflow = false;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.categorizeButton,
            this.alphabetizeButton,
            this.sortSeparator,
            this.propertiesButton,
            this.eventsButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 27);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(222, 25);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 2;
            // 
            // categorizeButton
            // 
            this.categorizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.categorizeButton.Image = ((System.Drawing.Image)(resources.GetObject("categorizeButton.Image")));
            this.categorizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.categorizeButton.Name = "categorizeButton";
            this.categorizeButton.Size = new System.Drawing.Size(23, 22);
            this.categorizeButton.Text = "Categorize";
            // 
            // alphabetizeButton
            // 
            this.alphabetizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.alphabetizeButton.Image = ((System.Drawing.Image)(resources.GetObject("alphabetizeButton.Image")));
            this.alphabetizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.alphabetizeButton.Name = "alphabetizeButton";
            this.alphabetizeButton.Size = new System.Drawing.Size(23, 22);
            this.alphabetizeButton.Text = "Alphabetize";
            // 
            // sortSeparator
            // 
            this.sortSeparator.Name = "sortSeparator";
            this.sortSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // propertiesButton
            // 
            this.propertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.propertiesButton.Image = ((System.Drawing.Image)(resources.GetObject("propertiesButton.Image")));
            this.propertiesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.propertiesButton.Name = "propertiesButton";
            this.propertiesButton.Size = new System.Drawing.Size(23, 22);
            this.propertiesButton.Text = "Properties";
            // 
            // eventsButton
            // 
            this.eventsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.eventsButton.Image = ((System.Drawing.Image)(resources.GetObject("eventsButton.Image")));
            this.eventsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.eventsButton.Name = "eventsButton";
            this.eventsButton.Size = new System.Drawing.Size(23, 22);
            this.eventsButton.Text = "Events";
            // 
            // stripContainer
            // 
            this.stripContainer.BottomToolStripPanelVisible = false;
            // 
            // stripContainer.ContentPanel
            // 
            this.stripContainer.ContentPanel.AutoScroll = true;
            this.stripContainer.ContentPanel.Controls.Add(this.propertyGrid);
            this.stripContainer.ContentPanel.Size = new System.Drawing.Size(222, 249);
            this.stripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stripContainer.LeftToolStripPanelVisible = false;
            this.stripContainer.Location = new System.Drawing.Point(1, 0);
            this.stripContainer.Name = "stripContainer";
            this.stripContainer.RightToolStripPanelVisible = false;
            this.stripContainer.Size = new System.Drawing.Size(222, 301);
            this.stripContainer.TabIndex = 3;
            this.stripContainer.Text = "toolStripContainer1";
            // 
            // stripContainer.TopToolStripPanel
            // 
            this.stripContainer.TopToolStripPanel.Controls.Add(this.itemsStrip);
            this.stripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // PropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stripContainer);
            this.Name = "PropertiesPanel";
            this.Size = new System.Drawing.Size(224, 301);
            this.itemsStrip.ResumeLayout(false);
            this.itemsStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.stripContainer.ContentPanel.ResumeLayout(false);
            this.stripContainer.TopToolStripPanel.ResumeLayout(false);
            this.stripContainer.TopToolStripPanel.PerformLayout();
            this.stripContainer.ResumeLayout(false);
            this.stripContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStrip itemsStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripContainer stripContainer;
        private System.Windows.Forms.ToolStripSpringComboBox itemsComboBox;
        private System.Windows.Forms.ToolStripButton categorizeButton;
        private System.Windows.Forms.ToolStripButton alphabetizeButton;
        private System.Windows.Forms.ToolStripSeparator sortSeparator;
        private System.Windows.Forms.ToolStripButton propertiesButton;
        private System.Windows.Forms.ToolStripButton eventsButton;
    }
}
