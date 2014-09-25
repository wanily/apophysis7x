namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Render
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Render));
			this.Tabs = new System.Windows.Forms.TabControl();
			this.SettingsTab = new System.Windows.Forms.TabPage();
			this.FormatComboBox = new System.Windows.Forms.ComboBox();
			this.FormatLabel = new System.Windows.Forms.Label();
			this.mCompletionGroupBox = new System.Windows.Forms.GroupBox();
			this.ThreadsComboBox = new System.Windows.Forms.ComboBox();
			this.mThreadsLabel = new System.Windows.Forms.Label();
			this.SaveIncompleteRendersCheckBox = new System.Windows.Forms.CheckBox();
			this.SaveParametersCheckBox = new System.Windows.Forms.CheckBox();
			this.PropertiesGroupBox = new System.Windows.Forms.GroupBox();
			this.OversampleTextBox = new System.Windows.Forms.NumericUpDown();
			this.mOversampleLabel = new System.Windows.Forms.Label();
			this.DensityComboBox = new System.Windows.Forms.ComboBox();
			this.mDensitylabel = new System.Windows.Forms.Label();
			this.FilterRadiusTextBox = new System.Windows.Forms.TextBox();
			this.SelectFolderButton = new System.Windows.Forms.Button();
			this.mButtonImages = new System.Windows.Forms.ImageList(this.components);
			this.GoToFolderButton = new System.Windows.Forms.Button();
			this.DestinationTextBox = new System.Windows.Forms.TextBox();
			this.DestinationLabel = new System.Windows.Forms.Label();
			this.SizeGroupBox = new System.Windows.Forms.GroupBox();
			this.mPresetGroupBox = new System.Windows.Forms.GroupBox();
			this.Preset3SaveButton = new System.Windows.Forms.Button();
			this.Preset3SelectButton = new System.Windows.Forms.Button();
			this.Preset2SaveButton = new System.Windows.Forms.Button();
			this.Preset2SelectButton = new System.Windows.Forms.Button();
			this.Preset1SaveButton = new System.Windows.Forms.Button();
			this.Preset1SelectButton = new System.Windows.Forms.Button();
			this.mSizeGroupBox = new System.Windows.Forms.GroupBox();
			this.HeightComboBox = new System.Windows.Forms.ComboBox();
			this.WidthComboBox = new System.Windows.Forms.ComboBox();
			this.MaintainAspectRatioCheckBox = new System.Windows.Forms.CheckBox();
			this.mHeightLabel = new System.Windows.Forms.Label();
			this.mWidthLabel = new System.Windows.Forms.Label();
			this.MessagesTab = new System.Windows.Forms.TabPage();
			this.MessagesTextBox = new System.Windows.Forms.TextBox();
			this.mTabImages = new System.Windows.Forms.ImageList(this.components);
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.mStatusBar = new System.Windows.Forms.StatusStrip();
			this.ElapsedLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.RemainingLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.InfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.CancelButton = new System.Windows.Forms.Button();
			this.PauseButton = new System.Windows.Forms.Button();
			this.StartButton = new System.Windows.Forms.Button();
			this.FilterRadiusDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.Tabs.SuspendLayout();
			this.SettingsTab.SuspendLayout();
			this.mCompletionGroupBox.SuspendLayout();
			this.PropertiesGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.OversampleTextBox)).BeginInit();
			this.SizeGroupBox.SuspendLayout();
			this.mPresetGroupBox.SuspendLayout();
			this.mSizeGroupBox.SuspendLayout();
			this.MessagesTab.SuspendLayout();
			this.mStatusBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// Tabs
			// 
			resources.ApplyResources(this.Tabs, "Tabs");
			this.Tabs.Controls.Add(this.SettingsTab);
			this.Tabs.Controls.Add(this.MessagesTab);
			this.Tabs.ImageList = this.mTabImages;
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			// 
			// SettingsTab
			// 
			this.SettingsTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.SettingsTab.Controls.Add(this.FormatComboBox);
			this.SettingsTab.Controls.Add(this.FormatLabel);
			this.SettingsTab.Controls.Add(this.mCompletionGroupBox);
			this.SettingsTab.Controls.Add(this.PropertiesGroupBox);
			this.SettingsTab.Controls.Add(this.SelectFolderButton);
			this.SettingsTab.Controls.Add(this.GoToFolderButton);
			this.SettingsTab.Controls.Add(this.DestinationTextBox);
			this.SettingsTab.Controls.Add(this.DestinationLabel);
			this.SettingsTab.Controls.Add(this.SizeGroupBox);
			resources.ApplyResources(this.SettingsTab, "SettingsTab");
			this.SettingsTab.Name = "SettingsTab";
			// 
			// FormatComboBox
			// 
			resources.ApplyResources(this.FormatComboBox, "FormatComboBox");
			this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FormatComboBox.FormattingEnabled = true;
			this.FormatComboBox.Items.AddRange(new object[] {
            resources.GetString("FormatComboBox.Items"),
            resources.GetString("FormatComboBox.Items1"),
            resources.GetString("FormatComboBox.Items2"),
            resources.GetString("FormatComboBox.Items3")});
			this.FormatComboBox.Name = "FormatComboBox";
			// 
			// FormatLabel
			// 
			this.FormatLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.FormatLabel, "FormatLabel");
			this.FormatLabel.Name = "FormatLabel";
			// 
			// mCompletionGroupBox
			// 
			resources.ApplyResources(this.mCompletionGroupBox, "mCompletionGroupBox");
			this.mCompletionGroupBox.Controls.Add(this.ThreadsComboBox);
			this.mCompletionGroupBox.Controls.Add(this.mThreadsLabel);
			this.mCompletionGroupBox.Controls.Add(this.SaveIncompleteRendersCheckBox);
			this.mCompletionGroupBox.Controls.Add(this.SaveParametersCheckBox);
			this.mCompletionGroupBox.Name = "mCompletionGroupBox";
			this.mCompletionGroupBox.TabStop = false;
			// 
			// ThreadsComboBox
			// 
			this.ThreadsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ThreadsComboBox.FormattingEnabled = true;
			this.ThreadsComboBox.Items.AddRange(new object[] {
            resources.GetString("ThreadsComboBox.Items"),
            resources.GetString("ThreadsComboBox.Items1"),
            resources.GetString("ThreadsComboBox.Items2"),
            resources.GetString("ThreadsComboBox.Items3"),
            resources.GetString("ThreadsComboBox.Items4"),
            resources.GetString("ThreadsComboBox.Items5"),
            resources.GetString("ThreadsComboBox.Items6"),
            resources.GetString("ThreadsComboBox.Items7"),
            resources.GetString("ThreadsComboBox.Items8"),
            resources.GetString("ThreadsComboBox.Items9"),
            resources.GetString("ThreadsComboBox.Items10"),
            resources.GetString("ThreadsComboBox.Items11"),
            resources.GetString("ThreadsComboBox.Items12")});
			resources.ApplyResources(this.ThreadsComboBox, "ThreadsComboBox");
			this.ThreadsComboBox.Name = "ThreadsComboBox";
			// 
			// mThreadsLabel
			// 
			this.mThreadsLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mThreadsLabel, "mThreadsLabel");
			this.mThreadsLabel.Name = "mThreadsLabel";
			// 
			// SaveIncompleteRendersCheckBox
			// 
			resources.ApplyResources(this.SaveIncompleteRendersCheckBox, "SaveIncompleteRendersCheckBox");
			this.SaveIncompleteRendersCheckBox.Name = "SaveIncompleteRendersCheckBox";
			this.SaveIncompleteRendersCheckBox.UseVisualStyleBackColor = true;
			// 
			// SaveParametersCheckBox
			// 
			resources.ApplyResources(this.SaveParametersCheckBox, "SaveParametersCheckBox");
			this.SaveParametersCheckBox.Name = "SaveParametersCheckBox";
			this.SaveParametersCheckBox.UseVisualStyleBackColor = true;
			// 
			// PropertiesGroupBox
			// 
			this.PropertiesGroupBox.Controls.Add(this.OversampleTextBox);
			this.PropertiesGroupBox.Controls.Add(this.mOversampleLabel);
			this.PropertiesGroupBox.Controls.Add(this.DensityComboBox);
			this.PropertiesGroupBox.Controls.Add(this.mDensitylabel);
			this.PropertiesGroupBox.Controls.Add(this.FilterRadiusTextBox);
			this.PropertiesGroupBox.Controls.Add(this.FilterRadiusDragPanel);
			resources.ApplyResources(this.PropertiesGroupBox, "PropertiesGroupBox");
			this.PropertiesGroupBox.Name = "PropertiesGroupBox";
			this.PropertiesGroupBox.TabStop = false;
			// 
			// OversampleTextBox
			// 
			resources.ApplyResources(this.OversampleTextBox, "OversampleTextBox");
			this.OversampleTextBox.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.OversampleTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.OversampleTextBox.Name = "OversampleTextBox";
			this.OversampleTextBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// mOversampleLabel
			// 
			this.mOversampleLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mOversampleLabel, "mOversampleLabel");
			this.mOversampleLabel.Name = "mOversampleLabel";
			// 
			// DensityComboBox
			// 
			resources.ApplyResources(this.DensityComboBox, "DensityComboBox");
			this.DensityComboBox.FormattingEnabled = true;
			this.DensityComboBox.Items.AddRange(new object[] {
            resources.GetString("DensityComboBox.Items"),
            resources.GetString("DensityComboBox.Items1"),
            resources.GetString("DensityComboBox.Items2"),
            resources.GetString("DensityComboBox.Items3"),
            resources.GetString("DensityComboBox.Items4")});
			this.DensityComboBox.Name = "DensityComboBox";
			// 
			// mDensitylabel
			// 
			this.mDensitylabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mDensitylabel, "mDensitylabel");
			this.mDensitylabel.Name = "mDensitylabel";
			// 
			// FilterRadiusTextBox
			// 
			resources.ApplyResources(this.FilterRadiusTextBox, "FilterRadiusTextBox");
			this.FilterRadiusTextBox.Name = "FilterRadiusTextBox";
			// 
			// SelectFolderButton
			// 
			resources.ApplyResources(this.SelectFolderButton, "SelectFolderButton");
			this.SelectFolderButton.ImageList = this.mButtonImages;
			this.SelectFolderButton.Name = "SelectFolderButton";
			this.SelectFolderButton.UseVisualStyleBackColor = true;
			// 
			// mButtonImages
			// 
			this.mButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mButtonImages.ImageStream")));
			this.mButtonImages.TransparentColor = System.Drawing.Color.Fuchsia;
			this.mButtonImages.Images.SetKeyName(0, "SaveFlame.bmp");
			this.mButtonImages.Images.SetKeyName(1, "OpenBatch.bmp");
			this.mButtonImages.Images.SetKeyName(2, "Go.bmp");
			// 
			// GoToFolderButton
			// 
			resources.ApplyResources(this.GoToFolderButton, "GoToFolderButton");
			this.GoToFolderButton.ImageList = this.mButtonImages;
			this.GoToFolderButton.Name = "GoToFolderButton";
			this.GoToFolderButton.UseVisualStyleBackColor = true;
			// 
			// DestinationTextBox
			// 
			resources.ApplyResources(this.DestinationTextBox, "DestinationTextBox");
			this.DestinationTextBox.Name = "DestinationTextBox";
			this.DestinationTextBox.ReadOnly = true;
			// 
			// DestinationLabel
			// 
			this.DestinationLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.DestinationLabel, "DestinationLabel");
			this.DestinationLabel.Name = "DestinationLabel";
			// 
			// SizeGroupBox
			// 
			resources.ApplyResources(this.SizeGroupBox, "SizeGroupBox");
			this.SizeGroupBox.Controls.Add(this.mPresetGroupBox);
			this.SizeGroupBox.Controls.Add(this.mSizeGroupBox);
			this.SizeGroupBox.Name = "SizeGroupBox";
			this.SizeGroupBox.TabStop = false;
			// 
			// mPresetGroupBox
			// 
			resources.ApplyResources(this.mPresetGroupBox, "mPresetGroupBox");
			this.mPresetGroupBox.Controls.Add(this.Preset3SaveButton);
			this.mPresetGroupBox.Controls.Add(this.Preset3SelectButton);
			this.mPresetGroupBox.Controls.Add(this.Preset2SaveButton);
			this.mPresetGroupBox.Controls.Add(this.Preset2SelectButton);
			this.mPresetGroupBox.Controls.Add(this.Preset1SaveButton);
			this.mPresetGroupBox.Controls.Add(this.Preset1SelectButton);
			this.mPresetGroupBox.Name = "mPresetGroupBox";
			this.mPresetGroupBox.TabStop = false;
			// 
			// Preset3SaveButton
			// 
			resources.ApplyResources(this.Preset3SaveButton, "Preset3SaveButton");
			this.Preset3SaveButton.ImageList = this.mButtonImages;
			this.Preset3SaveButton.Name = "Preset3SaveButton";
			this.Preset3SaveButton.UseVisualStyleBackColor = true;
			// 
			// Preset3SelectButton
			// 
			resources.ApplyResources(this.Preset3SelectButton, "Preset3SelectButton");
			this.Preset3SelectButton.Name = "Preset3SelectButton";
			this.Preset3SelectButton.UseVisualStyleBackColor = true;
			// 
			// Preset2SaveButton
			// 
			resources.ApplyResources(this.Preset2SaveButton, "Preset2SaveButton");
			this.Preset2SaveButton.ImageList = this.mButtonImages;
			this.Preset2SaveButton.Name = "Preset2SaveButton";
			this.Preset2SaveButton.UseVisualStyleBackColor = true;
			// 
			// Preset2SelectButton
			// 
			resources.ApplyResources(this.Preset2SelectButton, "Preset2SelectButton");
			this.Preset2SelectButton.Name = "Preset2SelectButton";
			this.Preset2SelectButton.UseVisualStyleBackColor = true;
			// 
			// Preset1SaveButton
			// 
			resources.ApplyResources(this.Preset1SaveButton, "Preset1SaveButton");
			this.Preset1SaveButton.ImageList = this.mButtonImages;
			this.Preset1SaveButton.Name = "Preset1SaveButton";
			this.Preset1SaveButton.UseVisualStyleBackColor = true;
			// 
			// Preset1SelectButton
			// 
			resources.ApplyResources(this.Preset1SelectButton, "Preset1SelectButton");
			this.Preset1SelectButton.Name = "Preset1SelectButton";
			this.Preset1SelectButton.UseVisualStyleBackColor = true;
			// 
			// mSizeGroupBox
			// 
			resources.ApplyResources(this.mSizeGroupBox, "mSizeGroupBox");
			this.mSizeGroupBox.Controls.Add(this.HeightComboBox);
			this.mSizeGroupBox.Controls.Add(this.WidthComboBox);
			this.mSizeGroupBox.Controls.Add(this.MaintainAspectRatioCheckBox);
			this.mSizeGroupBox.Controls.Add(this.mHeightLabel);
			this.mSizeGroupBox.Controls.Add(this.mWidthLabel);
			this.mSizeGroupBox.Name = "mSizeGroupBox";
			this.mSizeGroupBox.TabStop = false;
			// 
			// HeightComboBox
			// 
			resources.ApplyResources(this.HeightComboBox, "HeightComboBox");
			this.HeightComboBox.FormattingEnabled = true;
			this.HeightComboBox.Items.AddRange(new object[] {
            resources.GetString("HeightComboBox.Items"),
            resources.GetString("HeightComboBox.Items1"),
            resources.GetString("HeightComboBox.Items2"),
            resources.GetString("HeightComboBox.Items3"),
            resources.GetString("HeightComboBox.Items4"),
            resources.GetString("HeightComboBox.Items5"),
            resources.GetString("HeightComboBox.Items6"),
            resources.GetString("HeightComboBox.Items7"),
            resources.GetString("HeightComboBox.Items8"),
            resources.GetString("HeightComboBox.Items9")});
			this.HeightComboBox.Name = "HeightComboBox";
			// 
			// WidthComboBox
			// 
			resources.ApplyResources(this.WidthComboBox, "WidthComboBox");
			this.WidthComboBox.FormattingEnabled = true;
			this.WidthComboBox.Items.AddRange(new object[] {
            resources.GetString("WidthComboBox.Items"),
            resources.GetString("WidthComboBox.Items1"),
            resources.GetString("WidthComboBox.Items2"),
            resources.GetString("WidthComboBox.Items3"),
            resources.GetString("WidthComboBox.Items4"),
            resources.GetString("WidthComboBox.Items5")});
			this.WidthComboBox.Name = "WidthComboBox";
			// 
			// MaintainAspectRatioCheckBox
			// 
			resources.ApplyResources(this.MaintainAspectRatioCheckBox, "MaintainAspectRatioCheckBox");
			this.MaintainAspectRatioCheckBox.Name = "MaintainAspectRatioCheckBox";
			this.MaintainAspectRatioCheckBox.UseVisualStyleBackColor = true;
			// 
			// mHeightLabel
			// 
			this.mHeightLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mHeightLabel, "mHeightLabel");
			this.mHeightLabel.Name = "mHeightLabel";
			// 
			// mWidthLabel
			// 
			this.mWidthLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mWidthLabel, "mWidthLabel");
			this.mWidthLabel.Name = "mWidthLabel";
			// 
			// MessagesTab
			// 
			this.MessagesTab.BackColor = System.Drawing.SystemColors.Control;
			this.MessagesTab.Controls.Add(this.MessagesTextBox);
			resources.ApplyResources(this.MessagesTab, "MessagesTab");
			this.MessagesTab.Name = "MessagesTab";
			// 
			// MessagesTextBox
			// 
			resources.ApplyResources(this.MessagesTextBox, "MessagesTextBox");
			this.MessagesTextBox.Name = "MessagesTextBox";
			this.MessagesTextBox.ReadOnly = true;
			// 
			// mTabImages
			// 
			this.mTabImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mTabImages.ImageStream")));
			this.mTabImages.TransparentColor = System.Drawing.Color.Fuchsia;
			this.mTabImages.Images.SetKeyName(0, "FlameProperties.bmp");
			this.mTabImages.Images.SetKeyName(1, "Messages.bmp");
			// 
			// ProgressBar
			// 
			resources.ApplyResources(this.ProgressBar, "ProgressBar");
			this.ProgressBar.Name = "ProgressBar";
			// 
			// mStatusBar
			// 
			this.mStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ElapsedLabel,
            this.RemainingLabel,
            this.InfoLabel});
			resources.ApplyResources(this.mStatusBar, "mStatusBar");
			this.mStatusBar.Name = "mStatusBar";
			this.mStatusBar.SizingGrip = false;
			// 
			// ElapsedLabel
			// 
			resources.ApplyResources(this.ElapsedLabel, "ElapsedLabel");
			this.ElapsedLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.ElapsedLabel.Name = "ElapsedLabel";
			// 
			// RemainingLabel
			// 
			resources.ApplyResources(this.RemainingLabel, "RemainingLabel");
			this.RemainingLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.RemainingLabel.Name = "RemainingLabel";
			// 
			// InfoLabel
			// 
			resources.ApplyResources(this.InfoLabel, "InfoLabel");
			this.InfoLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Spring = true;
			// 
			// CancelButton
			// 
			resources.ApplyResources(this.CancelButton, "CancelButton");
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.UseVisualStyleBackColor = true;
			// 
			// PauseButton
			// 
			resources.ApplyResources(this.PauseButton, "PauseButton");
			this.PauseButton.Name = "PauseButton";
			this.PauseButton.UseVisualStyleBackColor = true;
			// 
			// StartButton
			// 
			resources.ApplyResources(this.StartButton, "StartButton");
			this.StartButton.Name = "StartButton";
			this.StartButton.UseVisualStyleBackColor = true;
			// 
			// FilterRadiusDragPanel
			// 
			this.FilterRadiusDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.FilterRadiusDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.FilterRadiusDragPanel.Default = 0D;
			this.FilterRadiusDragPanel.DragStepping = 5D;
			resources.ApplyResources(this.FilterRadiusDragPanel, "FilterRadiusDragPanel");
			this.FilterRadiusDragPanel.Maximum = 1.7976931348623157E+308D;
			this.FilterRadiusDragPanel.Minimum = 0D;
			this.FilterRadiusDragPanel.Name = "FilterRadiusDragPanel";
			this.FilterRadiusDragPanel.TextBox = this.FilterRadiusTextBox;
			this.FilterRadiusDragPanel.Value = 0D;
			// 
			// Render
			// 
			this.AcceptButton = this.StartButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.StartButton);
			this.Controls.Add(this.PauseButton);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.mStatusBar);
			this.Controls.Add(this.ProgressBar);
			this.Controls.Add(this.Tabs);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Render";
			this.Tabs.ResumeLayout(false);
			this.SettingsTab.ResumeLayout(false);
			this.SettingsTab.PerformLayout();
			this.mCompletionGroupBox.ResumeLayout(false);
			this.PropertiesGroupBox.ResumeLayout(false);
			this.PropertiesGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.OversampleTextBox)).EndInit();
			this.SizeGroupBox.ResumeLayout(false);
			this.mPresetGroupBox.ResumeLayout(false);
			this.mSizeGroupBox.ResumeLayout(false);
			this.MessagesTab.ResumeLayout(false);
			this.MessagesTab.PerformLayout();
			this.mStatusBar.ResumeLayout(false);
			this.mStatusBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox mPresetGroupBox;
		public System.Windows.Forms.Button Preset3SaveButton;
		public System.Windows.Forms.Button Preset3SelectButton;
		public System.Windows.Forms.Button Preset2SaveButton;
		public System.Windows.Forms.Button Preset2SelectButton;
		public System.Windows.Forms.Button Preset1SaveButton;
		public System.Windows.Forms.Button Preset1SelectButton;
		private System.Windows.Forms.GroupBox mSizeGroupBox;
		public System.Windows.Forms.ComboBox HeightComboBox;
		public System.Windows.Forms.ComboBox WidthComboBox;
		public System.Windows.Forms.CheckBox MaintainAspectRatioCheckBox;
		private System.Windows.Forms.Label mHeightLabel;
		private System.Windows.Forms.Label mWidthLabel;
		private System.Windows.Forms.ImageList mButtonImages;
		public System.Windows.Forms.TextBox DestinationTextBox;
		public System.Windows.Forms.Button GoToFolderButton;
		public System.Windows.Forms.Button SelectFolderButton;
		private System.Windows.Forms.ImageList mTabImages;
		public System.Windows.Forms.TextBox FilterRadiusTextBox;
		public Controls.DragPanel FilterRadiusDragPanel;
		private System.Windows.Forms.Label mDensitylabel;
		public System.Windows.Forms.ComboBox DensityComboBox;
		public System.Windows.Forms.NumericUpDown OversampleTextBox;
		private System.Windows.Forms.Label mOversampleLabel;
		private System.Windows.Forms.GroupBox mCompletionGroupBox;
		public System.Windows.Forms.CheckBox SaveParametersCheckBox;
		public System.Windows.Forms.CheckBox SaveIncompleteRendersCheckBox;
		private System.Windows.Forms.StatusStrip mStatusBar;
		public System.Windows.Forms.ToolStripStatusLabel RemainingLabel;
		public System.Windows.Forms.ToolStripStatusLabel InfoLabel;
		public System.Windows.Forms.ToolStripStatusLabel ElapsedLabel;
		public System.Windows.Forms.Button CancelButton;
		public System.Windows.Forms.Button PauseButton;
		public System.Windows.Forms.Button StartButton;
		public System.Windows.Forms.TextBox MessagesTextBox;
		public System.Windows.Forms.TabControl Tabs;
		public System.Windows.Forms.TabPage SettingsTab;
		public System.Windows.Forms.TabPage MessagesTab;
		public System.Windows.Forms.ProgressBar ProgressBar;
		public System.Windows.Forms.GroupBox SizeGroupBox;
		public System.Windows.Forms.GroupBox PropertiesGroupBox;
		public System.Windows.Forms.Label DestinationLabel;
		public System.Windows.Forms.Label FormatLabel;
		public System.Windows.Forms.ComboBox FormatComboBox;
		public System.Windows.Forms.ComboBox ThreadsComboBox;
		private System.Windows.Forms.Label mThreadsLabel;

	}
}