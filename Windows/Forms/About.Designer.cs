namespace Xyrus.Apophysis.Windows.Forms
{
	partial class About
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
			this.mBannerPanel = new System.Windows.Forms.Panel();
			this.mBannerPicture = new System.Windows.Forms.PictureBox();
			this.mClientPanel = new System.Windows.Forms.Panel();
			this.mInfoLabel = new System.Windows.Forms.Label();
			this.mCopyright = new System.Windows.Forms.Label();
			this.mVersionLabel = new System.Windows.Forms.Label();
			this.mCreditsPanel = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.mFlamesCreditsHeader = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.mDeveloperHeader = new System.Windows.Forms.Label();
			this.mOriginalCreditsLabel = new System.Windows.Forms.Label();
			this.mCreditsHeader = new System.Windows.Forms.Label();
			this.mCloseButton = new System.Windows.Forms.Button();
			this.mBannerPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mBannerPicture)).BeginInit();
			this.mClientPanel.SuspendLayout();
			this.mCreditsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mBannerPanel
			// 
			this.mBannerPanel.Controls.Add(this.mBannerPicture);
			resources.ApplyResources(this.mBannerPanel, "mBannerPanel");
			this.mBannerPanel.Name = "mBannerPanel";
			// 
			// mBannerPicture
			// 
			resources.ApplyResources(this.mBannerPicture, "mBannerPicture");
			this.mBannerPicture.Name = "mBannerPicture";
			this.mBannerPicture.TabStop = false;
			// 
			// mClientPanel
			// 
			resources.ApplyResources(this.mClientPanel, "mClientPanel");
			this.mClientPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mClientPanel.Controls.Add(this.mInfoLabel);
			this.mClientPanel.Controls.Add(this.mCopyright);
			this.mClientPanel.Controls.Add(this.mVersionLabel);
			this.mClientPanel.Name = "mClientPanel";
			// 
			// mInfoLabel
			// 
			resources.ApplyResources(this.mInfoLabel, "mInfoLabel");
			this.mInfoLabel.Name = "mInfoLabel";
			// 
			// mCopyright
			// 
			resources.ApplyResources(this.mCopyright, "mCopyright");
			this.mCopyright.Name = "mCopyright";
			// 
			// mVersionLabel
			// 
			resources.ApplyResources(this.mVersionLabel, "mVersionLabel");
			this.mVersionLabel.Name = "mVersionLabel";
			// 
			// mCreditsPanel
			// 
			resources.ApplyResources(this.mCreditsPanel, "mCreditsPanel");
			this.mCreditsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mCreditsPanel.Controls.Add(this.label2);
			this.mCreditsPanel.Controls.Add(this.mFlamesCreditsHeader);
			this.mCreditsPanel.Controls.Add(this.label1);
			this.mCreditsPanel.Controls.Add(this.mDeveloperHeader);
			this.mCreditsPanel.Controls.Add(this.mOriginalCreditsLabel);
			this.mCreditsPanel.Controls.Add(this.mCreditsHeader);
			this.mCreditsPanel.Name = "mCreditsPanel";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// mFlamesCreditsHeader
			// 
			resources.ApplyResources(this.mFlamesCreditsHeader, "mFlamesCreditsHeader");
			this.mFlamesCreditsHeader.Name = "mFlamesCreditsHeader";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// mDeveloperHeader
			// 
			resources.ApplyResources(this.mDeveloperHeader, "mDeveloperHeader");
			this.mDeveloperHeader.Name = "mDeveloperHeader";
			// 
			// mOriginalCreditsLabel
			// 
			resources.ApplyResources(this.mOriginalCreditsLabel, "mOriginalCreditsLabel");
			this.mOriginalCreditsLabel.Name = "mOriginalCreditsLabel";
			// 
			// mCreditsHeader
			// 
			resources.ApplyResources(this.mCreditsHeader, "mCreditsHeader");
			this.mCreditsHeader.Name = "mCreditsHeader";
			// 
			// mCloseButton
			// 
			resources.ApplyResources(this.mCloseButton, "mCloseButton");
			this.mCloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.mCloseButton.Name = "mCloseButton";
			this.mCloseButton.UseVisualStyleBackColor = true;
			this.mCloseButton.Click += new System.EventHandler(this.OnCloseClick);
			// 
			// About
			// 
			this.AcceptButton = this.mCloseButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.mCloseButton;
			this.Controls.Add(this.mCloseButton);
			this.Controls.Add(this.mCreditsPanel);
			this.Controls.Add(this.mClientPanel);
			this.Controls.Add(this.mBannerPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Load += new System.EventHandler(this.OnLoad);
			this.mBannerPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mBannerPicture)).EndInit();
			this.mClientPanel.ResumeLayout(false);
			this.mCreditsPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel mBannerPanel;
		private System.Windows.Forms.PictureBox mBannerPicture;
		private System.Windows.Forms.Panel mClientPanel;
		private System.Windows.Forms.Label mVersionLabel;
		private System.Windows.Forms.Label mCopyright;
		private System.Windows.Forms.Label mInfoLabel;
		private System.Windows.Forms.Panel mCreditsPanel;
		private System.Windows.Forms.Label mCreditsHeader;
		private System.Windows.Forms.Label mOriginalCreditsLabel;
		private System.Windows.Forms.Label mDeveloperHeader;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label mFlamesCreditsHeader;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button mCloseButton;
	}
}