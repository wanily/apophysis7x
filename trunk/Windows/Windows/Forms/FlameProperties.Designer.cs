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
			this.mPreviewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.LowQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MediumQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HighQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mPictureBevel = new System.Windows.Forms.Label();
			this.mToolbar = new System.Windows.Forms.ToolStrip();
			this.UndoButton = new System.Windows.Forms.ToolStripButton();
			this.RedoButton = new System.Windows.Forms.ToolStripButton();
			this.mCameraTab = new System.Windows.Forms.TabPage();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.DepthBlurTextBox = new System.Windows.Forms.TextBox();
			this.PitchTextBox = new System.Windows.Forms.TextBox();
			this.YawTextBox = new System.Windows.Forms.TextBox();
			this.HeightTextBox = new System.Windows.Forms.TextBox();
			this.PerspectiveTextBox = new System.Windows.Forms.TextBox();
			this.ScaleTextBox = new System.Windows.Forms.TextBox();
			this.ScaleDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.PerspectiveDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.HeightDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.YawDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.PitchDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.DepthBlurDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
			this.mPreviewContextMenu.SuspendLayout();
			this.mToolbar.SuspendLayout();
			this.mTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// PreviewPicture
			// 
			this.PreviewPicture.BackColor = System.Drawing.SystemColors.Control;
			this.PreviewPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.PreviewPicture.ContextMenuStrip = this.mPreviewContextMenu;
			this.PreviewPicture.Location = new System.Drawing.Point(5, 6);
			this.PreviewPicture.Name = "PreviewPicture";
			this.PreviewPicture.Size = new System.Drawing.Size(264, 138);
			this.PreviewPicture.TabIndex = 3;
			this.PreviewPicture.TabStop = false;
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
			// mPictureBevel
			// 
			this.mPictureBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBevel.Location = new System.Drawing.Point(4, 5);
			this.mPictureBevel.Name = "mPictureBevel";
			this.mPictureBevel.Size = new System.Drawing.Size(266, 140);
			this.mPictureBevel.TabIndex = 2;
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
			this.mToolbar.Location = new System.Drawing.Point(449, 6);
			this.mToolbar.Name = "mToolbar";
			this.mToolbar.Padding = new System.Windows.Forms.Padding(0);
			this.mToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.mToolbar.Size = new System.Drawing.Size(47, 21);
			this.mToolbar.TabIndex = 5;
			// 
			// UndoButton
			// 
			this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UndoButton.Image = ((System.Drawing.Image)(resources.GetObject("UndoButton.Image")));
			this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UndoButton.Name = "UndoButton";
			this.UndoButton.Size = new System.Drawing.Size(23, 18);
			this.UndoButton.Text = "Undo";
			// 
			// RedoButton
			// 
			this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RedoButton.Image = ((System.Drawing.Image)(resources.GetObject("RedoButton.Image")));
			this.RedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RedoButton.Name = "RedoButton";
			this.RedoButton.Size = new System.Drawing.Size(23, 18);
			this.RedoButton.Text = "Redo";
			// 
			// mCameraTab
			// 
			this.mCameraTab.Location = new System.Drawing.Point(4, 22);
			this.mCameraTab.Name = "mCameraTab";
			this.mCameraTab.Padding = new System.Windows.Forms.Padding(3);
			this.mCameraTab.Size = new System.Drawing.Size(483, 165);
			this.mCameraTab.TabIndex = 0;
			this.mCameraTab.Text = "Camera";
			this.mCameraTab.UseVisualStyleBackColor = true;
			// 
			// mTabs
			// 
			this.mTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mTabs.Controls.Add(this.mCameraTab);
			this.mTabs.Location = new System.Drawing.Point(5, 150);
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			this.mTabs.Size = new System.Drawing.Size(491, 191);
			this.mTabs.TabIndex = 6;
			// 
			// DepthBlurTextBox
			// 
			this.DepthBlurTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DepthBlurTextBox.Location = new System.Drawing.Point(388, 6);
			this.DepthBlurTextBox.Name = "DepthBlurTextBox";
			this.DepthBlurTextBox.Size = new System.Drawing.Size(63, 20);
			this.DepthBlurTextBox.TabIndex = 8;
			this.DepthBlurTextBox.Text = "0.000";
			this.DepthBlurTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// PitchTextBox
			// 
			this.PitchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PitchTextBox.Location = new System.Drawing.Point(388, 33);
			this.PitchTextBox.Name = "PitchTextBox";
			this.PitchTextBox.Size = new System.Drawing.Size(108, 20);
			this.PitchTextBox.TabIndex = 10;
			this.PitchTextBox.Text = "0.000";
			// 
			// YawTextBox
			// 
			this.YawTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.YawTextBox.Location = new System.Drawing.Point(388, 54);
			this.YawTextBox.Name = "YawTextBox";
			this.YawTextBox.Size = new System.Drawing.Size(108, 20);
			this.YawTextBox.TabIndex = 12;
			this.YawTextBox.Text = "0.000";
			// 
			// HeightTextBox
			// 
			this.HeightTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.HeightTextBox.Location = new System.Drawing.Point(388, 75);
			this.HeightTextBox.Name = "HeightTextBox";
			this.HeightTextBox.Size = new System.Drawing.Size(108, 20);
			this.HeightTextBox.TabIndex = 14;
			this.HeightTextBox.Text = "0.000";
			// 
			// PerspectiveTextBox
			// 
			this.PerspectiveTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PerspectiveTextBox.Location = new System.Drawing.Point(388, 96);
			this.PerspectiveTextBox.Name = "PerspectiveTextBox";
			this.PerspectiveTextBox.Size = new System.Drawing.Size(108, 20);
			this.PerspectiveTextBox.TabIndex = 16;
			this.PerspectiveTextBox.Text = "0.000";
			// 
			// ScaleTextBox
			// 
			this.ScaleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ScaleTextBox.Location = new System.Drawing.Point(388, 124);
			this.ScaleTextBox.Name = "ScaleTextBox";
			this.ScaleTextBox.Size = new System.Drawing.Size(108, 20);
			this.ScaleTextBox.TabIndex = 18;
			this.ScaleTextBox.Text = "0.100";
			// 
			// ScaleDragPanel
			// 
			this.ScaleDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ScaleDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ScaleDragPanel.Default = 0D;
			this.ScaleDragPanel.DragStepping = 10D;
			this.ScaleDragPanel.Location = new System.Drawing.Point(276, 124);
			this.ScaleDragPanel.Maximum = 1000D;
			this.ScaleDragPanel.Minimum = 0.1D;
			this.ScaleDragPanel.Name = "ScaleDragPanel";
			this.ScaleDragPanel.Size = new System.Drawing.Size(116, 21);
			this.ScaleDragPanel.TabIndex = 17;
			this.ScaleDragPanel.Text = "Scale";
			this.ScaleDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ScaleDragPanel.TextBox = this.ScaleTextBox;
			this.ScaleDragPanel.Value = 0.1D;
			// 
			// PerspectiveDragPanel
			// 
			this.PerspectiveDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PerspectiveDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PerspectiveDragPanel.Default = 0D;
			this.PerspectiveDragPanel.DragStepping = 0.1D;
			this.PerspectiveDragPanel.Location = new System.Drawing.Point(276, 96);
			this.PerspectiveDragPanel.Maximum = 1.7976931348623157E+308D;
			this.PerspectiveDragPanel.Minimum = -1.7976931348623157E+308D;
			this.PerspectiveDragPanel.Name = "PerspectiveDragPanel";
			this.PerspectiveDragPanel.Size = new System.Drawing.Size(116, 21);
			this.PerspectiveDragPanel.TabIndex = 15;
			this.PerspectiveDragPanel.Text = "Perspective";
			this.PerspectiveDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.PerspectiveDragPanel.TextBox = this.PerspectiveTextBox;
			this.PerspectiveDragPanel.Value = 0D;
			// 
			// HeightDragPanel
			// 
			this.HeightDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.HeightDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.HeightDragPanel.Default = 0D;
			this.HeightDragPanel.DragStepping = 0.1D;
			this.HeightDragPanel.Location = new System.Drawing.Point(276, 75);
			this.HeightDragPanel.Maximum = 1.7976931348623157E+308D;
			this.HeightDragPanel.Minimum = -1.7976931348623157E+308D;
			this.HeightDragPanel.Name = "HeightDragPanel";
			this.HeightDragPanel.Size = new System.Drawing.Size(116, 21);
			this.HeightDragPanel.TabIndex = 13;
			this.HeightDragPanel.Text = "Height";
			this.HeightDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.HeightDragPanel.TextBox = this.HeightTextBox;
			this.HeightDragPanel.Value = 0D;
			// 
			// YawDragPanel
			// 
			this.YawDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.YawDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.YawDragPanel.Default = 0D;
			this.YawDragPanel.DragStepping = 5D;
			this.YawDragPanel.Location = new System.Drawing.Point(276, 54);
			this.YawDragPanel.Maximum = 1.7976931348623157E+308D;
			this.YawDragPanel.Minimum = -1.7976931348623157E+308D;
			this.YawDragPanel.Name = "YawDragPanel";
			this.YawDragPanel.Size = new System.Drawing.Size(116, 21);
			this.YawDragPanel.TabIndex = 11;
			this.YawDragPanel.Text = "Yaw";
			this.YawDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.YawDragPanel.TextBox = this.YawTextBox;
			this.YawDragPanel.Value = 0D;
			// 
			// PitchDragPanel
			// 
			this.PitchDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PitchDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PitchDragPanel.Default = 0D;
			this.PitchDragPanel.DragStepping = 5D;
			this.PitchDragPanel.Location = new System.Drawing.Point(276, 33);
			this.PitchDragPanel.Maximum = 1.7976931348623157E+308D;
			this.PitchDragPanel.Minimum = -1.7976931348623157E+308D;
			this.PitchDragPanel.Name = "PitchDragPanel";
			this.PitchDragPanel.Size = new System.Drawing.Size(116, 21);
			this.PitchDragPanel.TabIndex = 9;
			this.PitchDragPanel.Text = "Pitch";
			this.PitchDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.PitchDragPanel.TextBox = this.PitchTextBox;
			this.PitchDragPanel.Value = 0D;
			// 
			// DepthBlurDragPanel
			// 
			this.DepthBlurDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.DepthBlurDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DepthBlurDragPanel.Default = 0D;
			this.DepthBlurDragPanel.DragStepping = 0.01D;
			this.DepthBlurDragPanel.Location = new System.Drawing.Point(276, 6);
			this.DepthBlurDragPanel.Maximum = 1.7976931348623157E+308D;
			this.DepthBlurDragPanel.Minimum = 0D;
			this.DepthBlurDragPanel.Name = "DepthBlurDragPanel";
			this.DepthBlurDragPanel.Size = new System.Drawing.Size(116, 21);
			this.DepthBlurDragPanel.TabIndex = 7;
			this.DepthBlurDragPanel.Text = "Depth blur";
			this.DepthBlurDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DepthBlurDragPanel.TextBox = this.DepthBlurTextBox;
			this.DepthBlurDragPanel.Value = 0D;
			// 
			// FlameProperties
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(501, 346);
			this.Controls.Add(this.ScaleTextBox);
			this.Controls.Add(this.ScaleDragPanel);
			this.Controls.Add(this.PerspectiveTextBox);
			this.Controls.Add(this.PerspectiveDragPanel);
			this.Controls.Add(this.HeightTextBox);
			this.Controls.Add(this.HeightDragPanel);
			this.Controls.Add(this.YawTextBox);
			this.Controls.Add(this.YawDragPanel);
			this.Controls.Add(this.PitchTextBox);
			this.Controls.Add(this.PitchDragPanel);
			this.Controls.Add(this.DepthBlurTextBox);
			this.Controls.Add(this.DepthBlurDragPanel);
			this.Controls.Add(this.mTabs);
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
			this.mTabs.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private System.Windows.Forms.TabPage mCameraTab;
		private System.Windows.Forms.TabControl mTabs;
		public Controls.DragPanel DepthBlurDragPanel;
		public System.Windows.Forms.TextBox DepthBlurTextBox;
		public System.Windows.Forms.TextBox PitchTextBox;
		public Controls.DragPanel PitchDragPanel;
		public System.Windows.Forms.TextBox YawTextBox;
		public Controls.DragPanel YawDragPanel;
		public System.Windows.Forms.TextBox HeightTextBox;
		public Controls.DragPanel HeightDragPanel;
		public System.Windows.Forms.TextBox PerspectiveTextBox;
		public Controls.DragPanel PerspectiveDragPanel;
		public System.Windows.Forms.TextBox ScaleTextBox;
		public Controls.DragPanel ScaleDragPanel;
	}
}