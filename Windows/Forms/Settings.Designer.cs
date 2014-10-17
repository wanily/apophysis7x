namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Settings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
			this.CancelButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mCommonPage = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.OldVariationStyleCheckBox = new System.Windows.Forms.CheckBox();
			this.NamePrefixTextBox = new System.Windows.Forms.TextBox();
			this.mNamePrefixLabel = new System.Windows.Forms.Label();
			this.mRestartNoticeLabel = new System.Windows.Forms.Label();
			this.mAutosaveGroupBox = new System.Windows.Forms.GroupBox();
			this.AutosaveEnabledCheckBox = new System.Windows.Forms.CheckBox();
			this.AutosaveThresholdComboBox = new System.Windows.Forms.ComboBox();
			this.mAutosaveThesholdLabel = new System.Windows.Forms.Label();
			this.AutosavePathButton = new System.Windows.Forms.Button();
			this.mButtonImages = new System.Windows.Forms.ImageList(this.components);
			this.AutosavePathTextBox = new System.Windows.Forms.TextBox();
			this.mAutosavePathLabel = new System.Windows.Forms.Label();
			this.ConfirmStopRendering = new System.Windows.Forms.CheckBox();
			this.CameraEditUseScaleCheckBox = new System.Windows.Forms.CheckBox();
			this.ConfirmDeleteCheckBox = new System.Windows.Forms.CheckBox();
			this.WarnOnMissingPluginsCheckBox = new System.Windows.Forms.CheckBox();
			this.ThreadsComboBox = new System.Windows.Forms.ComboBox();
			this.mThreadsLabel = new System.Windows.Forms.Label();
			this.PngTransparencyComboBox = new System.Windows.Forms.ComboBox();
			this.mPngTransparencyLabel = new System.Windows.Forms.Label();
			this.JpegQualityComboBox = new System.Windows.Forms.ComboBox();
			this.mJpegQualityLabel = new System.Windows.Forms.Label();
			this.PluginPathButton = new System.Windows.Forms.Button();
			this.PluginPathTextBox = new System.Windows.Forms.TextBox();
			this.mPluginPathLabel = new System.Windows.Forms.Label();
			this.mViewPage = new System.Windows.Forms.TabPage();
			this.mPreviewPage = new System.Windows.Forms.TabPage();
			this.mTabs.SuspendLayout();
			this.mCommonPage.SuspendLayout();
			this.mAutosaveGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// CancelButton
			// 
			resources.ApplyResources(this.CancelButton, "CancelButton");
			this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.UseVisualStyleBackColor = true;
			// 
			// OkButton
			// 
			resources.ApplyResources(this.OkButton, "OkButton");
			this.OkButton.Name = "OkButton";
			this.OkButton.UseVisualStyleBackColor = true;
			// 
			// mTabs
			// 
			this.mTabs.Controls.Add(this.mCommonPage);
			this.mTabs.Controls.Add(this.mViewPage);
			this.mTabs.Controls.Add(this.mPreviewPage);
			resources.ApplyResources(this.mTabs, "mTabs");
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			// 
			// mCommonPage
			// 
			this.mCommonPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mCommonPage.Controls.Add(this.label1);
			this.mCommonPage.Controls.Add(this.OldVariationStyleCheckBox);
			this.mCommonPage.Controls.Add(this.NamePrefixTextBox);
			this.mCommonPage.Controls.Add(this.mNamePrefixLabel);
			this.mCommonPage.Controls.Add(this.mRestartNoticeLabel);
			this.mCommonPage.Controls.Add(this.mAutosaveGroupBox);
			this.mCommonPage.Controls.Add(this.ConfirmStopRendering);
			this.mCommonPage.Controls.Add(this.CameraEditUseScaleCheckBox);
			this.mCommonPage.Controls.Add(this.ConfirmDeleteCheckBox);
			this.mCommonPage.Controls.Add(this.WarnOnMissingPluginsCheckBox);
			this.mCommonPage.Controls.Add(this.ThreadsComboBox);
			this.mCommonPage.Controls.Add(this.mThreadsLabel);
			this.mCommonPage.Controls.Add(this.PngTransparencyComboBox);
			this.mCommonPage.Controls.Add(this.mPngTransparencyLabel);
			this.mCommonPage.Controls.Add(this.JpegQualityComboBox);
			this.mCommonPage.Controls.Add(this.mJpegQualityLabel);
			this.mCommonPage.Controls.Add(this.PluginPathButton);
			this.mCommonPage.Controls.Add(this.PluginPathTextBox);
			this.mCommonPage.Controls.Add(this.mPluginPathLabel);
			resources.ApplyResources(this.mCommonPage, "mCommonPage");
			this.mCommonPage.Name = "mCommonPage";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// OldVariationStyleCheckBox
			// 
			resources.ApplyResources(this.OldVariationStyleCheckBox, "OldVariationStyleCheckBox");
			this.OldVariationStyleCheckBox.Name = "OldVariationStyleCheckBox";
			this.OldVariationStyleCheckBox.UseVisualStyleBackColor = true;
			// 
			// NamePrefixTextBox
			// 
			resources.ApplyResources(this.NamePrefixTextBox, "NamePrefixTextBox");
			this.NamePrefixTextBox.Name = "NamePrefixTextBox";
			// 
			// mNamePrefixLabel
			// 
			this.mNamePrefixLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mNamePrefixLabel, "mNamePrefixLabel");
			this.mNamePrefixLabel.Name = "mNamePrefixLabel";
			// 
			// mRestartNoticeLabel
			// 
			resources.ApplyResources(this.mRestartNoticeLabel, "mRestartNoticeLabel");
			this.mRestartNoticeLabel.Name = "mRestartNoticeLabel";
			// 
			// mAutosaveGroupBox
			// 
			resources.ApplyResources(this.mAutosaveGroupBox, "mAutosaveGroupBox");
			this.mAutosaveGroupBox.Controls.Add(this.AutosaveEnabledCheckBox);
			this.mAutosaveGroupBox.Controls.Add(this.AutosaveThresholdComboBox);
			this.mAutosaveGroupBox.Controls.Add(this.mAutosaveThesholdLabel);
			this.mAutosaveGroupBox.Controls.Add(this.AutosavePathButton);
			this.mAutosaveGroupBox.Controls.Add(this.AutosavePathTextBox);
			this.mAutosaveGroupBox.Controls.Add(this.mAutosavePathLabel);
			this.mAutosaveGroupBox.Name = "mAutosaveGroupBox";
			this.mAutosaveGroupBox.TabStop = false;
			// 
			// AutosaveEnabledCheckBox
			// 
			resources.ApplyResources(this.AutosaveEnabledCheckBox, "AutosaveEnabledCheckBox");
			this.AutosaveEnabledCheckBox.Name = "AutosaveEnabledCheckBox";
			this.AutosaveEnabledCheckBox.UseVisualStyleBackColor = true;
			// 
			// AutosaveThresholdComboBox
			// 
			this.AutosaveThresholdComboBox.FormattingEnabled = true;
			this.AutosaveThresholdComboBox.Items.AddRange(new object[] {
            resources.GetString("AutosaveThresholdComboBox.Items"),
            resources.GetString("AutosaveThresholdComboBox.Items1"),
            resources.GetString("AutosaveThresholdComboBox.Items2"),
            resources.GetString("AutosaveThresholdComboBox.Items3"),
            resources.GetString("AutosaveThresholdComboBox.Items4")});
			resources.ApplyResources(this.AutosaveThresholdComboBox, "AutosaveThresholdComboBox");
			this.AutosaveThresholdComboBox.Name = "AutosaveThresholdComboBox";
			this.AutosaveThresholdComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// mAutosaveThesholdLabel
			// 
			this.mAutosaveThesholdLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mAutosaveThesholdLabel, "mAutosaveThesholdLabel");
			this.mAutosaveThesholdLabel.Name = "mAutosaveThesholdLabel";
			// 
			// AutosavePathButton
			// 
			resources.ApplyResources(this.AutosavePathButton, "AutosavePathButton");
			this.AutosavePathButton.ImageList = this.mButtonImages;
			this.AutosavePathButton.Name = "AutosavePathButton";
			this.AutosavePathButton.UseVisualStyleBackColor = true;
			// 
			// mButtonImages
			// 
			this.mButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mButtonImages.ImageStream")));
			this.mButtonImages.TransparentColor = System.Drawing.Color.Fuchsia;
			this.mButtonImages.Images.SetKeyName(0, "OpenBatch.bmp");
			// 
			// AutosavePathTextBox
			// 
			resources.ApplyResources(this.AutosavePathTextBox, "AutosavePathTextBox");
			this.AutosavePathTextBox.Name = "AutosavePathTextBox";
			this.AutosavePathTextBox.ReadOnly = true;
			// 
			// mAutosavePathLabel
			// 
			this.mAutosavePathLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mAutosavePathLabel, "mAutosavePathLabel");
			this.mAutosavePathLabel.Name = "mAutosavePathLabel";
			// 
			// ConfirmStopRendering
			// 
			resources.ApplyResources(this.ConfirmStopRendering, "ConfirmStopRendering");
			this.ConfirmStopRendering.Name = "ConfirmStopRendering";
			this.ConfirmStopRendering.UseVisualStyleBackColor = true;
			// 
			// CameraEditUseScaleCheckBox
			// 
			resources.ApplyResources(this.CameraEditUseScaleCheckBox, "CameraEditUseScaleCheckBox");
			this.CameraEditUseScaleCheckBox.Name = "CameraEditUseScaleCheckBox";
			this.CameraEditUseScaleCheckBox.UseVisualStyleBackColor = true;
			// 
			// ConfirmDeleteCheckBox
			// 
			resources.ApplyResources(this.ConfirmDeleteCheckBox, "ConfirmDeleteCheckBox");
			this.ConfirmDeleteCheckBox.Name = "ConfirmDeleteCheckBox";
			this.ConfirmDeleteCheckBox.UseVisualStyleBackColor = true;
			// 
			// WarnOnMissingPluginsCheckBox
			// 
			resources.ApplyResources(this.WarnOnMissingPluginsCheckBox, "WarnOnMissingPluginsCheckBox");
			this.WarnOnMissingPluginsCheckBox.Name = "WarnOnMissingPluginsCheckBox";
			this.WarnOnMissingPluginsCheckBox.UseVisualStyleBackColor = true;
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
			// PngTransparencyComboBox
			// 
			this.PngTransparencyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PngTransparencyComboBox.FormattingEnabled = true;
			this.PngTransparencyComboBox.Items.AddRange(new object[] {
            resources.GetString("PngTransparencyComboBox.Items"),
            resources.GetString("PngTransparencyComboBox.Items1")});
			resources.ApplyResources(this.PngTransparencyComboBox, "PngTransparencyComboBox");
			this.PngTransparencyComboBox.Name = "PngTransparencyComboBox";
			// 
			// mPngTransparencyLabel
			// 
			this.mPngTransparencyLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mPngTransparencyLabel, "mPngTransparencyLabel");
			this.mPngTransparencyLabel.Name = "mPngTransparencyLabel";
			// 
			// JpegQualityComboBox
			// 
			this.JpegQualityComboBox.FormattingEnabled = true;
			this.JpegQualityComboBox.Items.AddRange(new object[] {
            resources.GetString("JpegQualityComboBox.Items"),
            resources.GetString("JpegQualityComboBox.Items1"),
            resources.GetString("JpegQualityComboBox.Items2"),
            resources.GetString("JpegQualityComboBox.Items3")});
			resources.ApplyResources(this.JpegQualityComboBox, "JpegQualityComboBox");
			this.JpegQualityComboBox.Name = "JpegQualityComboBox";
			this.JpegQualityComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// mJpegQualityLabel
			// 
			this.mJpegQualityLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mJpegQualityLabel, "mJpegQualityLabel");
			this.mJpegQualityLabel.Name = "mJpegQualityLabel";
			// 
			// PluginPathButton
			// 
			resources.ApplyResources(this.PluginPathButton, "PluginPathButton");
			this.PluginPathButton.ImageList = this.mButtonImages;
			this.PluginPathButton.Name = "PluginPathButton";
			this.PluginPathButton.UseVisualStyleBackColor = true;
			// 
			// PluginPathTextBox
			// 
			resources.ApplyResources(this.PluginPathTextBox, "PluginPathTextBox");
			this.PluginPathTextBox.Name = "PluginPathTextBox";
			this.PluginPathTextBox.ReadOnly = true;
			// 
			// mPluginPathLabel
			// 
			this.mPluginPathLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mPluginPathLabel, "mPluginPathLabel");
			this.mPluginPathLabel.Name = "mPluginPathLabel";
			// 
			// mViewPage
			// 
			this.mViewPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
			resources.ApplyResources(this.mViewPage, "mViewPage");
			this.mViewPage.Name = "mViewPage";
			// 
			// mPreviewPage
			// 
			this.mPreviewPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
			resources.ApplyResources(this.mPreviewPage, "mPreviewPage");
			this.mPreviewPage.Name = "mPreviewPage";
			// 
			// Settings
			// 
			this.AcceptButton = this.OkButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mTabs);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.CancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Settings";
			this.mTabs.ResumeLayout(false);
			this.mCommonPage.ResumeLayout(false);
			this.mCommonPage.PerformLayout();
			this.mAutosaveGroupBox.ResumeLayout(false);
			this.mAutosaveGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Button CancelButton;
		public System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mCommonPage;
		private System.Windows.Forms.TabPage mViewPage;
		private System.Windows.Forms.TabPage mPreviewPage;
		private System.Windows.Forms.Label mPluginPathLabel;
		public System.Windows.Forms.TextBox PluginPathTextBox;
		public System.Windows.Forms.Button PluginPathButton;
		private System.Windows.Forms.ImageList mButtonImages;
		private System.Windows.Forms.Label mJpegQualityLabel;
		public System.Windows.Forms.ComboBox JpegQualityComboBox;
		public System.Windows.Forms.ComboBox PngTransparencyComboBox;
		private System.Windows.Forms.Label mPngTransparencyLabel;
		public System.Windows.Forms.ComboBox ThreadsComboBox;
		private System.Windows.Forms.Label mThreadsLabel;
		public System.Windows.Forms.CheckBox WarnOnMissingPluginsCheckBox;
		public System.Windows.Forms.CheckBox ConfirmDeleteCheckBox;
		public System.Windows.Forms.CheckBox CameraEditUseScaleCheckBox;
		public System.Windows.Forms.CheckBox ConfirmStopRendering;
		private System.Windows.Forms.GroupBox mAutosaveGroupBox;
		public System.Windows.Forms.Button AutosavePathButton;
		public System.Windows.Forms.TextBox AutosavePathTextBox;
		private System.Windows.Forms.Label mAutosavePathLabel;
		public System.Windows.Forms.ComboBox AutosaveThresholdComboBox;
		private System.Windows.Forms.Label mAutosaveThesholdLabel;
		public System.Windows.Forms.CheckBox AutosaveEnabledCheckBox;
		public System.Windows.Forms.TextBox NamePrefixTextBox;
		private System.Windows.Forms.Label mNamePrefixLabel;
		public System.Windows.Forms.CheckBox OldVariationStyleCheckBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label mRestartNoticeLabel;
	}
}