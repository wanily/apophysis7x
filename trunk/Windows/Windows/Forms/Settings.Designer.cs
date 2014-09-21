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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
			this.CancelButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mCommonPage = new System.Windows.Forms.TabPage();
			this.mEditorPage = new System.Windows.Forms.TabPage();
			this.mViewPage = new System.Windows.Forms.TabPage();
			this.mPreviewPage = new System.Windows.Forms.TabPage();
			this.mAutosavePage = new System.Windows.Forms.TabPage();
			this.mTabs.SuspendLayout();
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
			this.mTabs.Controls.Add(this.mEditorPage);
			this.mTabs.Controls.Add(this.mViewPage);
			this.mTabs.Controls.Add(this.mPreviewPage);
			this.mTabs.Controls.Add(this.mAutosavePage);
			resources.ApplyResources(this.mTabs, "mTabs");
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			// 
			// mCommonPage
			// 
			this.mCommonPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
			resources.ApplyResources(this.mCommonPage, "mCommonPage");
			this.mCommonPage.Name = "mCommonPage";
			// 
			// mEditorPage
			// 
			this.mEditorPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
			resources.ApplyResources(this.mEditorPage, "mEditorPage");
			this.mEditorPage.Name = "mEditorPage";
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
			// mAutosavePage
			// 
			this.mAutosavePage.BackColor = System.Drawing.SystemColors.ControlLightLight;
			resources.ApplyResources(this.mAutosavePage, "mAutosavePage");
			this.mAutosavePage.Name = "mAutosavePage";
			// 
			// Settings
			// 
			this.AcceptButton = this.OkButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelButton;
			this.Controls.Add(this.mTabs);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.CancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Settings";
			this.mTabs.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Button CancelButton;
		public System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mCommonPage;
		private System.Windows.Forms.TabPage mEditorPage;
		private System.Windows.Forms.TabPage mViewPage;
		private System.Windows.Forms.TabPage mPreviewPage;
		private System.Windows.Forms.TabPage mAutosavePage;
	}
}