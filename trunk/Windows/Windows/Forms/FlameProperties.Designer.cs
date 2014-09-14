namespace Xyrus.Apophysis.Windows.Forms
{
	partial class FlameProperties
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlameProperties));
			this.PreviewPicture = new System.Windows.Forms.PictureBox();
			this.mPictureBevel = new System.Windows.Forms.Label();
			this.mPreviewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.LowQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MediumQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HighQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mToolbar = new System.Windows.Forms.ToolStrip();
			this.UndoButton = new System.Windows.Forms.ToolStripButton();
			this.RedoButton = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
			this.mPreviewContextMenu.SuspendLayout();
			this.mToolbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// PreviewPicture
			// 
			this.PreviewPicture.BackColor = System.Drawing.SystemColors.Control;
			this.PreviewPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.PreviewPicture.ContextMenuStrip = this.mPreviewContextMenu;
			this.PreviewPicture.Location = new System.Drawing.Point(12, 12);
			this.PreviewPicture.Name = "PreviewPicture";
			this.PreviewPicture.Size = new System.Drawing.Size(287, 162);
			this.PreviewPicture.TabIndex = 3;
			this.PreviewPicture.TabStop = false;
			// 
			// mPictureBevel
			// 
			this.mPictureBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBevel.Location = new System.Drawing.Point(11, 11);
			this.mPictureBevel.Name = "mPictureBevel";
			this.mPictureBevel.Size = new System.Drawing.Size(289, 165);
			this.mPictureBevel.TabIndex = 2;
			// 
			// mPreviewContextMenu
			// 
			this.mPreviewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LowQualityMenuItem,
            this.MediumQualityMenuItem,
            this.HighQualityMenuItem});
			this.mPreviewContextMenu.Name = "mPreviewContextMenu";
			this.mPreviewContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.mPreviewContextMenu.Size = new System.Drawing.Size(159, 70);
			// 
			// LowQualityMenuItem
			// 
			this.LowQualityMenuItem.Name = "LowQualityMenuItem";
			this.LowQualityMenuItem.Size = new System.Drawing.Size(158, 22);
			this.LowQualityMenuItem.Text = "&Low quality";
			// 
			// MediumQualityMenuItem
			// 
			this.MediumQualityMenuItem.Name = "MediumQualityMenuItem";
			this.MediumQualityMenuItem.Size = new System.Drawing.Size(158, 22);
			this.MediumQualityMenuItem.Text = "&Medium quality";
			// 
			// HighQualityMenuItem
			// 
			this.HighQualityMenuItem.Name = "HighQualityMenuItem";
			this.HighQualityMenuItem.Size = new System.Drawing.Size(158, 22);
			this.HighQualityMenuItem.Text = "&High quality";
			// 
			// mToolbar
			// 
			this.mToolbar.AllowMerge = false;
			this.mToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mToolbar.AutoSize = false;
			this.mToolbar.CanOverflow = false;
			this.mToolbar.Dock = System.Windows.Forms.DockStyle.None;
			this.mToolbar.GripMargin = new System.Windows.Forms.Padding(0);
			this.mToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoButton,
            this.RedoButton});
			this.mToolbar.Location = new System.Drawing.Point(445, 11);
			this.mToolbar.Name = "mToolbar";
			this.mToolbar.Padding = new System.Windows.Forms.Padding(0);
			this.mToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.mToolbar.Size = new System.Drawing.Size(47, 22);
			this.mToolbar.TabIndex = 5;
			// 
			// UndoButton
			// 
			this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UndoButton.Image = ((System.Drawing.Image)(resources.GetObject("UndoButton.Image")));
			this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UndoButton.Name = "UndoButton";
			this.UndoButton.Size = new System.Drawing.Size(23, 19);
			this.UndoButton.Text = "Undo";
			// 
			// RedoButton
			// 
			this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RedoButton.Image = ((System.Drawing.Image)(resources.GetObject("RedoButton.Image")));
			this.RedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RedoButton.Name = "RedoButton";
			this.RedoButton.Size = new System.Drawing.Size(23, 19);
			this.RedoButton.Text = "Redo";
			// 
			// FlameProperties
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(501, 346);
			this.Controls.Add(this.mToolbar);
			this.Controls.Add(this.PreviewPicture);
			this.Controls.Add(this.mPictureBevel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FlameProperties";
			this.ShowIcon = false;
			this.Text = "Adjustment";
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).EndInit();
			this.mPreviewContextMenu.ResumeLayout(false);
			this.mToolbar.ResumeLayout(false);
			this.mToolbar.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.PictureBox PreviewPicture;
		private System.Windows.Forms.Label mPictureBevel;
		private System.Windows.Forms.ContextMenuStrip mPreviewContextMenu;
		public System.Windows.Forms.ToolStripMenuItem LowQualityMenuItem;
		public System.Windows.Forms.ToolStripMenuItem MediumQualityMenuItem;
		public System.Windows.Forms.ToolStripMenuItem HighQualityMenuItem;
		private System.Windows.Forms.ToolStrip mToolbar;
		public System.Windows.Forms.ToolStripButton UndoButton;
		public System.Windows.Forms.ToolStripButton RedoButton;
	}
}