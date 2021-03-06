﻿namespace DevLib.Configuration
{
    partial class WinFormConfigEditor
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (this.propertyGridUserControl != null)
                {
                    this.propertyGridUserControl.Dispose();
                }

                if (this._openConfigFileDialog != null)
                {
                    this._openConfigFileDialog.Dispose();
                }

                if (this._openPluginFileDialog != null)
                {
                    this._openPluginFileDialog.Dispose();
                }

                if (this._saveConfigFileDialog != null)
                {
                    this._saveConfigFileDialog.Dispose();
                }

                if (this._configEditorPluginList != null)
                {
                    this._configEditorPluginList.Clear();
                    this._configEditorPluginList = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpenPlugin = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBoxConfigEditorPlugin = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.propertyGridUserControl = new DevLib.Configuration.PropertyGridUserControl();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.CanOverflow = false;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripButtonNew,
            this.toolStripSeparator2,
            this.toolStripButtonOpen,
            this.toolStripSeparator3,
            this.toolStripButtonSave,
            this.toolStripSeparator4,
            this.toolStripButtonSaveAs,
            this.toolStripSeparator5,
            this.toolStripButtonOpenPlugin,
            this.toolStripComboBoxConfigEditorPlugin,
            this.toolStripSeparator6});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(624, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonNew.Text = "New";
            this.toolStripButtonNew.Click += new System.EventHandler(this.OnToolStripButtonNewClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonOpen.Text = "Open...";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.OnToolStripButtonOpenClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.OnToolStripButtonSaveClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonSaveAs.Text = "Save As...";
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.OnToolStripButtonSaveAsClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonOpenPlugin
            // 
            this.toolStripButtonOpenPlugin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOpenPlugin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenPlugin.Name = "toolStripButtonOpenPlugin";
            this.toolStripButtonOpenPlugin.Size = new System.Drawing.Size(86, 22);
            this.toolStripButtonOpenPlugin.Text = "Open Plugin...";
            this.toolStripButtonOpenPlugin.Click += new System.EventHandler(this.OnToolStripButtonOpenPluginClick);
            // 
            // toolStripComboBoxConfigEditorPlugin
            // 
            this.toolStripComboBoxConfigEditorPlugin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxConfigEditorPlugin.Name = "toolStripComboBoxConfigEditorPlugin";
            this.toolStripComboBoxConfigEditorPlugin.Size = new System.Drawing.Size(210, 25);
            this.toolStripComboBoxConfigEditorPlugin.SelectedIndexChanged += new System.EventHandler(this.OnToolStripComboBoxConfigEditorPluginSelectedIndexChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // propertyGridUserControl
            // 
            this.propertyGridUserControl.ConfigObject = null;
            this.propertyGridUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridUserControl.Location = new System.Drawing.Point(0, 25);
            this.propertyGridUserControl.Name = "propertyGridUserControl";
            this.propertyGridUserControl.Size = new System.Drawing.Size(624, 417);
            this.propertyGridUserControl.TabIndex = 1;
            this.propertyGridUserControl.PropertyValueChanged += new System.EventHandler(this.OnPropertyGridUserControlPropertyValueChanged);
            // 
            // WinFormConfigEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.propertyGridUserControl);
            this.Controls.Add(this.toolStrip);
            this.Name = "WinFormConfigEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration Editor";
            this.SizeChanged += new System.EventHandler(this.OnWinFormConfigEditorSizeChanged);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxConfigEditorPlugin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenPlugin;
        private PropertyGridUserControl propertyGridUserControl;

    }
}