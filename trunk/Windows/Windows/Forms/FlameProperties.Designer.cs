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
			this.RotationScrollBar = new System.Windows.Forms.HScrollBar();
			this.RotationTextBox = new System.Windows.Forms.TextBox();
			this.YPositionScrollBar = new System.Windows.Forms.HScrollBar();
			this.YPositionTextBox = new System.Windows.Forms.TextBox();
			this.XPositionScrollBar = new System.Windows.Forms.HScrollBar();
			this.XPositionTextBox = new System.Windows.Forms.TextBox();
			this.ZoomScrollBar = new System.Windows.Forms.HScrollBar();
			this.ZoomTextBox = new System.Windows.Forms.TextBox();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mImagingTab = new System.Windows.Forms.TabPage();
			this.GammaThresholdTextBox = new System.Windows.Forms.TextBox();
			this.mBackgroundLabel = new System.Windows.Forms.Label();
			this.VibrancyScrollBar = new System.Windows.Forms.HScrollBar();
			this.VibrancyTextBox = new System.Windows.Forms.TextBox();
			this.BrightnessScrollBar = new System.Windows.Forms.HScrollBar();
			this.BrightnessTextBox = new System.Windows.Forms.TextBox();
			this.GammaScrollBar = new System.Windows.Forms.HScrollBar();
			this.GammaTextBox = new System.Windows.Forms.TextBox();
			this.mTabImages = new System.Windows.Forms.ImageList(this.components);
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
			this.RotationDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.YPositionDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.XPositionDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.ZoomDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.GammaThresholdDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.VibrancyDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.BrightnessDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.GammaDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.BackgroundPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
			this.mPreviewContextMenu.SuspendLayout();
			this.mToolbar.SuspendLayout();
			this.mCameraTab.SuspendLayout();
			this.mTabs.SuspendLayout();
			this.mImagingTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// PreviewPicture
			// 
			this.PreviewPicture.BackColor = System.Drawing.SystemColors.Control;
			this.PreviewPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.PreviewPicture.ContextMenuStrip = this.mPreviewContextMenu;
			this.PreviewPicture.Location = new System.Drawing.Point(5, 6);
			this.PreviewPicture.Name = "PreviewPicture";
			this.PreviewPicture.Size = new System.Drawing.Size(255, 141);
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
			this.mPictureBevel.Size = new System.Drawing.Size(257, 143);
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
			this.mToolbar.Location = new System.Drawing.Point(456, 6);
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
			this.mCameraTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mCameraTab.Controls.Add(this.RotationScrollBar);
			this.mCameraTab.Controls.Add(this.RotationTextBox);
			this.mCameraTab.Controls.Add(this.RotationDragPanel);
			this.mCameraTab.Controls.Add(this.YPositionScrollBar);
			this.mCameraTab.Controls.Add(this.YPositionTextBox);
			this.mCameraTab.Controls.Add(this.YPositionDragPanel);
			this.mCameraTab.Controls.Add(this.XPositionScrollBar);
			this.mCameraTab.Controls.Add(this.XPositionTextBox);
			this.mCameraTab.Controls.Add(this.XPositionDragPanel);
			this.mCameraTab.Controls.Add(this.ZoomScrollBar);
			this.mCameraTab.Controls.Add(this.ZoomTextBox);
			this.mCameraTab.Controls.Add(this.ZoomDragPanel);
			this.mCameraTab.ImageIndex = 0;
			this.mCameraTab.Location = new System.Drawing.Point(4, 23);
			this.mCameraTab.Name = "mCameraTab";
			this.mCameraTab.Padding = new System.Windows.Forms.Padding(3);
			this.mCameraTab.Size = new System.Drawing.Size(492, 132);
			this.mCameraTab.TabIndex = 0;
			this.mCameraTab.Text = "Camera";
			// 
			// RotationScrollBar
			// 
			this.RotationScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RotationScrollBar.LargeChange = 1;
			this.RotationScrollBar.Location = new System.Drawing.Point(134, 102);
			this.RotationScrollBar.Maximum = 360;
			this.RotationScrollBar.Minimum = -360;
			this.RotationScrollBar.Name = "RotationScrollBar";
			this.RotationScrollBar.Size = new System.Drawing.Size(284, 21);
			this.RotationScrollBar.TabIndex = 22;
			// 
			// RotationTextBox
			// 
			this.RotationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RotationTextBox.Location = new System.Drawing.Point(421, 103);
			this.RotationTextBox.Name = "RotationTextBox";
			this.RotationTextBox.Size = new System.Drawing.Size(64, 20);
			this.RotationTextBox.TabIndex = 21;
			this.RotationTextBox.Text = "0.000";
			// 
			// YPositionScrollBar
			// 
			this.YPositionScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.YPositionScrollBar.LargeChange = 1;
			this.YPositionScrollBar.Location = new System.Drawing.Point(134, 72);
			this.YPositionScrollBar.Maximum = 10000;
			this.YPositionScrollBar.Minimum = -10000;
			this.YPositionScrollBar.Name = "YPositionScrollBar";
			this.YPositionScrollBar.Size = new System.Drawing.Size(284, 21);
			this.YPositionScrollBar.TabIndex = 19;
			// 
			// YPositionTextBox
			// 
			this.YPositionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.YPositionTextBox.Location = new System.Drawing.Point(421, 73);
			this.YPositionTextBox.Name = "YPositionTextBox";
			this.YPositionTextBox.Size = new System.Drawing.Size(64, 20);
			this.YPositionTextBox.TabIndex = 18;
			this.YPositionTextBox.Text = "0.000";
			// 
			// XPositionScrollBar
			// 
			this.XPositionScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.XPositionScrollBar.LargeChange = 1;
			this.XPositionScrollBar.Location = new System.Drawing.Point(134, 41);
			this.XPositionScrollBar.Maximum = 10000;
			this.XPositionScrollBar.Minimum = -10000;
			this.XPositionScrollBar.Name = "XPositionScrollBar";
			this.XPositionScrollBar.Size = new System.Drawing.Size(284, 21);
			this.XPositionScrollBar.TabIndex = 16;
			// 
			// XPositionTextBox
			// 
			this.XPositionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.XPositionTextBox.Location = new System.Drawing.Point(421, 42);
			this.XPositionTextBox.Name = "XPositionTextBox";
			this.XPositionTextBox.Size = new System.Drawing.Size(64, 20);
			this.XPositionTextBox.TabIndex = 15;
			this.XPositionTextBox.Text = "0.000";
			// 
			// ZoomScrollBar
			// 
			this.ZoomScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ZoomScrollBar.LargeChange = 1;
			this.ZoomScrollBar.Location = new System.Drawing.Point(134, 10);
			this.ZoomScrollBar.Maximum = 3000;
			this.ZoomScrollBar.Minimum = -3000;
			this.ZoomScrollBar.Name = "ZoomScrollBar";
			this.ZoomScrollBar.Size = new System.Drawing.Size(284, 21);
			this.ZoomScrollBar.TabIndex = 13;
			// 
			// ZoomTextBox
			// 
			this.ZoomTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ZoomTextBox.Location = new System.Drawing.Point(421, 11);
			this.ZoomTextBox.Name = "ZoomTextBox";
			this.ZoomTextBox.Size = new System.Drawing.Size(64, 20);
			this.ZoomTextBox.TabIndex = 12;
			this.ZoomTextBox.Text = "0.000";
			// 
			// mTabs
			// 
			this.mTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mTabs.Controls.Add(this.mCameraTab);
			this.mTabs.Controls.Add(this.mImagingTab);
			this.mTabs.ImageList = this.mTabImages;
			this.mTabs.Location = new System.Drawing.Point(5, 153);
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			this.mTabs.Size = new System.Drawing.Size(498, 159);
			this.mTabs.TabIndex = 6;
			// 
			// mImagingTab
			// 
			this.mImagingTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.mImagingTab.Controls.Add(this.BackgroundPictureBox);
			this.mImagingTab.Controls.Add(this.GammaThresholdTextBox);
			this.mImagingTab.Controls.Add(this.GammaThresholdDragPanel);
			this.mImagingTab.Controls.Add(this.mBackgroundLabel);
			this.mImagingTab.Controls.Add(this.VibrancyScrollBar);
			this.mImagingTab.Controls.Add(this.VibrancyTextBox);
			this.mImagingTab.Controls.Add(this.VibrancyDragPanel);
			this.mImagingTab.Controls.Add(this.BrightnessScrollBar);
			this.mImagingTab.Controls.Add(this.BrightnessTextBox);
			this.mImagingTab.Controls.Add(this.BrightnessDragPanel);
			this.mImagingTab.Controls.Add(this.GammaScrollBar);
			this.mImagingTab.Controls.Add(this.GammaTextBox);
			this.mImagingTab.Controls.Add(this.GammaDragPanel);
			this.mImagingTab.ImageIndex = 1;
			this.mImagingTab.Location = new System.Drawing.Point(4, 23);
			this.mImagingTab.Name = "mImagingTab";
			this.mImagingTab.Padding = new System.Windows.Forms.Padding(3);
			this.mImagingTab.Size = new System.Drawing.Size(490, 132);
			this.mImagingTab.TabIndex = 1;
			this.mImagingTab.Text = "Rendering";
			// 
			// GammaThresholdTextBox
			// 
			this.GammaThresholdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.GammaThresholdTextBox.Location = new System.Drawing.Point(419, 103);
			this.GammaThresholdTextBox.Name = "GammaThresholdTextBox";
			this.GammaThresholdTextBox.Size = new System.Drawing.Size(64, 20);
			this.GammaThresholdTextBox.TabIndex = 31;
			this.GammaThresholdTextBox.Text = "0.001";
			// 
			// mBackgroundLabel
			// 
			this.mBackgroundLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mBackgroundLabel.Location = new System.Drawing.Point(5, 102);
			this.mBackgroundLabel.Name = "mBackgroundLabel";
			this.mBackgroundLabel.Size = new System.Drawing.Size(126, 21);
			this.mBackgroundLabel.TabIndex = 29;
			this.mBackgroundLabel.Text = "Background";
			this.mBackgroundLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// VibrancyScrollBar
			// 
			this.VibrancyScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VibrancyScrollBar.LargeChange = 1;
			this.VibrancyScrollBar.Location = new System.Drawing.Point(134, 72);
			this.VibrancyScrollBar.Maximum = 3000;
			this.VibrancyScrollBar.Name = "VibrancyScrollBar";
			this.VibrancyScrollBar.Size = new System.Drawing.Size(282, 21);
			this.VibrancyScrollBar.TabIndex = 28;
			this.VibrancyScrollBar.Value = 1000;
			// 
			// VibrancyTextBox
			// 
			this.VibrancyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.VibrancyTextBox.Location = new System.Drawing.Point(419, 73);
			this.VibrancyTextBox.Name = "VibrancyTextBox";
			this.VibrancyTextBox.Size = new System.Drawing.Size(64, 20);
			this.VibrancyTextBox.TabIndex = 27;
			this.VibrancyTextBox.Text = "1.000";
			// 
			// BrightnessScrollBar
			// 
			this.BrightnessScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BrightnessScrollBar.LargeChange = 1;
			this.BrightnessScrollBar.Location = new System.Drawing.Point(134, 41);
			this.BrightnessScrollBar.Maximum = 16000;
			this.BrightnessScrollBar.Minimum = 1;
			this.BrightnessScrollBar.Name = "BrightnessScrollBar";
			this.BrightnessScrollBar.Size = new System.Drawing.Size(282, 21);
			this.BrightnessScrollBar.TabIndex = 25;
			this.BrightnessScrollBar.Value = 4000;
			// 
			// BrightnessTextBox
			// 
			this.BrightnessTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrightnessTextBox.Location = new System.Drawing.Point(419, 42);
			this.BrightnessTextBox.Name = "BrightnessTextBox";
			this.BrightnessTextBox.Size = new System.Drawing.Size(64, 20);
			this.BrightnessTextBox.TabIndex = 24;
			this.BrightnessTextBox.Text = "4.000";
			// 
			// GammaScrollBar
			// 
			this.GammaScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GammaScrollBar.LargeChange = 1;
			this.GammaScrollBar.Location = new System.Drawing.Point(134, 10);
			this.GammaScrollBar.Maximum = 5000;
			this.GammaScrollBar.Minimum = 1000;
			this.GammaScrollBar.Name = "GammaScrollBar";
			this.GammaScrollBar.Size = new System.Drawing.Size(282, 21);
			this.GammaScrollBar.TabIndex = 22;
			this.GammaScrollBar.Value = 4000;
			// 
			// GammaTextBox
			// 
			this.GammaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.GammaTextBox.Location = new System.Drawing.Point(419, 11);
			this.GammaTextBox.Name = "GammaTextBox";
			this.GammaTextBox.Size = new System.Drawing.Size(64, 20);
			this.GammaTextBox.TabIndex = 21;
			this.GammaTextBox.Text = "4.000";
			// 
			// mTabImages
			// 
			this.mTabImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mTabImages.ImageStream")));
			this.mTabImages.TransparentColor = System.Drawing.Color.Magenta;
			this.mTabImages.Images.SetKeyName(0, "FlameProperties.bmp");
			this.mTabImages.Images.SetKeyName(1, "RenderFlame.bmp");
			this.mTabImages.Images.SetKeyName(2, "PaletteEditor.bmp");
			this.mTabImages.Images.SetKeyName(3, "CanvasProperties.bmp");
			// 
			// DepthBlurTextBox
			// 
			this.DepthBlurTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DepthBlurTextBox.Location = new System.Drawing.Point(388, 6);
			this.DepthBlurTextBox.Name = "DepthBlurTextBox";
			this.DepthBlurTextBox.Size = new System.Drawing.Size(70, 20);
			this.DepthBlurTextBox.TabIndex = 8;
			this.DepthBlurTextBox.Text = "0.000";
			this.DepthBlurTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// PitchTextBox
			// 
			this.PitchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PitchTextBox.Location = new System.Drawing.Point(388, 30);
			this.PitchTextBox.Name = "PitchTextBox";
			this.PitchTextBox.Size = new System.Drawing.Size(115, 20);
			this.PitchTextBox.TabIndex = 10;
			this.PitchTextBox.Text = "0.000";
			// 
			// YawTextBox
			// 
			this.YawTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.YawTextBox.Location = new System.Drawing.Point(388, 54);
			this.YawTextBox.Name = "YawTextBox";
			this.YawTextBox.Size = new System.Drawing.Size(115, 20);
			this.YawTextBox.TabIndex = 12;
			this.YawTextBox.Text = "0.000";
			// 
			// HeightTextBox
			// 
			this.HeightTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.HeightTextBox.Location = new System.Drawing.Point(388, 78);
			this.HeightTextBox.Name = "HeightTextBox";
			this.HeightTextBox.Size = new System.Drawing.Size(115, 20);
			this.HeightTextBox.TabIndex = 14;
			this.HeightTextBox.Text = "0.000";
			// 
			// PerspectiveTextBox
			// 
			this.PerspectiveTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PerspectiveTextBox.Location = new System.Drawing.Point(388, 102);
			this.PerspectiveTextBox.Name = "PerspectiveTextBox";
			this.PerspectiveTextBox.Size = new System.Drawing.Size(115, 20);
			this.PerspectiveTextBox.TabIndex = 16;
			this.PerspectiveTextBox.Text = "0.000";
			// 
			// ScaleTextBox
			// 
			this.ScaleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ScaleTextBox.Location = new System.Drawing.Point(388, 127);
			this.ScaleTextBox.Name = "ScaleTextBox";
			this.ScaleTextBox.Size = new System.Drawing.Size(115, 20);
			this.ScaleTextBox.TabIndex = 18;
			this.ScaleTextBox.Text = "25.000";
			// 
			// ScaleDragPanel
			// 
			this.ScaleDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ScaleDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ScaleDragPanel.Default = 25D;
			this.ScaleDragPanel.DragStepping = 10D;
			this.ScaleDragPanel.Location = new System.Drawing.Point(267, 127);
			this.ScaleDragPanel.Maximum = 1000D;
			this.ScaleDragPanel.Minimum = 0.1D;
			this.ScaleDragPanel.Name = "ScaleDragPanel";
			this.ScaleDragPanel.Size = new System.Drawing.Size(125, 21);
			this.ScaleDragPanel.TabIndex = 17;
			this.ScaleDragPanel.Text = "Scale";
			this.ScaleDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ScaleDragPanel.TextBox = this.ScaleTextBox;
			this.ScaleDragPanel.Value = 25D;
			// 
			// PerspectiveDragPanel
			// 
			this.PerspectiveDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PerspectiveDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PerspectiveDragPanel.Default = 0D;
			this.PerspectiveDragPanel.DragStepping = 0.1D;
			this.PerspectiveDragPanel.Location = new System.Drawing.Point(267, 102);
			this.PerspectiveDragPanel.Maximum = 1.7976931348623157E+308D;
			this.PerspectiveDragPanel.Minimum = -1.7976931348623157E+308D;
			this.PerspectiveDragPanel.Name = "PerspectiveDragPanel";
			this.PerspectiveDragPanel.Size = new System.Drawing.Size(125, 21);
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
			this.HeightDragPanel.Location = new System.Drawing.Point(267, 78);
			this.HeightDragPanel.Maximum = 1.7976931348623157E+308D;
			this.HeightDragPanel.Minimum = -1.7976931348623157E+308D;
			this.HeightDragPanel.Name = "HeightDragPanel";
			this.HeightDragPanel.Size = new System.Drawing.Size(125, 21);
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
			this.YawDragPanel.Location = new System.Drawing.Point(267, 54);
			this.YawDragPanel.Maximum = 1.7976931348623157E+308D;
			this.YawDragPanel.Minimum = -1.7976931348623157E+308D;
			this.YawDragPanel.Name = "YawDragPanel";
			this.YawDragPanel.Size = new System.Drawing.Size(125, 21);
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
			this.PitchDragPanel.Location = new System.Drawing.Point(267, 30);
			this.PitchDragPanel.Maximum = 1.7976931348623157E+308D;
			this.PitchDragPanel.Minimum = -1.7976931348623157E+308D;
			this.PitchDragPanel.Name = "PitchDragPanel";
			this.PitchDragPanel.Size = new System.Drawing.Size(125, 21);
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
			this.DepthBlurDragPanel.Location = new System.Drawing.Point(267, 6);
			this.DepthBlurDragPanel.Maximum = 1.7976931348623157E+308D;
			this.DepthBlurDragPanel.Minimum = 0D;
			this.DepthBlurDragPanel.Name = "DepthBlurDragPanel";
			this.DepthBlurDragPanel.Size = new System.Drawing.Size(125, 21);
			this.DepthBlurDragPanel.TabIndex = 7;
			this.DepthBlurDragPanel.Text = "Depth blur";
			this.DepthBlurDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.DepthBlurDragPanel.TextBox = this.DepthBlurTextBox;
			this.DepthBlurDragPanel.Value = 0D;
			// 
			// RotationDragPanel
			// 
			this.RotationDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.RotationDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RotationDragPanel.Default = 0D;
			this.RotationDragPanel.DragStepping = 5D;
			this.RotationDragPanel.Location = new System.Drawing.Point(5, 102);
			this.RotationDragPanel.Maximum = 360D;
			this.RotationDragPanel.Minimum = -360D;
			this.RotationDragPanel.Name = "RotationDragPanel";
			this.RotationDragPanel.Size = new System.Drawing.Size(126, 21);
			this.RotationDragPanel.TabIndex = 20;
			this.RotationDragPanel.Text = "Rotation";
			this.RotationDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.RotationDragPanel.TextBox = this.RotationTextBox;
			this.RotationDragPanel.Value = 0D;
			// 
			// YPositionDragPanel
			// 
			this.YPositionDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.YPositionDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.YPositionDragPanel.Default = 0D;
			this.YPositionDragPanel.DragStepping = 0.1D;
			this.YPositionDragPanel.Location = new System.Drawing.Point(5, 73);
			this.YPositionDragPanel.Maximum = 10D;
			this.YPositionDragPanel.Minimum = -10D;
			this.YPositionDragPanel.Name = "YPositionDragPanel";
			this.YPositionDragPanel.Size = new System.Drawing.Size(126, 21);
			this.YPositionDragPanel.TabIndex = 17;
			this.YPositionDragPanel.Text = "Y-Position";
			this.YPositionDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.YPositionDragPanel.TextBox = this.YPositionTextBox;
			this.YPositionDragPanel.Value = 0D;
			// 
			// XPositionDragPanel
			// 
			this.XPositionDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.XPositionDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.XPositionDragPanel.Default = 0D;
			this.XPositionDragPanel.DragStepping = 0.1D;
			this.XPositionDragPanel.Location = new System.Drawing.Point(5, 41);
			this.XPositionDragPanel.Maximum = 10D;
			this.XPositionDragPanel.Minimum = -10D;
			this.XPositionDragPanel.Name = "XPositionDragPanel";
			this.XPositionDragPanel.Size = new System.Drawing.Size(126, 21);
			this.XPositionDragPanel.TabIndex = 14;
			this.XPositionDragPanel.Text = "X-Position";
			this.XPositionDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.XPositionDragPanel.TextBox = this.XPositionTextBox;
			this.XPositionDragPanel.Value = 0D;
			// 
			// ZoomDragPanel
			// 
			this.ZoomDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ZoomDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ZoomDragPanel.Default = 0D;
			this.ZoomDragPanel.DragStepping = 0.1D;
			this.ZoomDragPanel.Location = new System.Drawing.Point(5, 11);
			this.ZoomDragPanel.Maximum = 3D;
			this.ZoomDragPanel.Minimum = -3D;
			this.ZoomDragPanel.Name = "ZoomDragPanel";
			this.ZoomDragPanel.Size = new System.Drawing.Size(126, 21);
			this.ZoomDragPanel.TabIndex = 11;
			this.ZoomDragPanel.Text = "Zoom";
			this.ZoomDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ZoomDragPanel.TextBox = this.ZoomTextBox;
			this.ZoomDragPanel.Value = 0D;
			// 
			// GammaThresholdDragPanel
			// 
			this.GammaThresholdDragPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.GammaThresholdDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.GammaThresholdDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.GammaThresholdDragPanel.Default = 0.001D;
			this.GammaThresholdDragPanel.DragStepping = 0.1D;
			this.GammaThresholdDragPanel.Location = new System.Drawing.Point(290, 103);
			this.GammaThresholdDragPanel.Maximum = 1.7976931348623157E+308D;
			this.GammaThresholdDragPanel.Minimum = -1.7976931348623157E+308D;
			this.GammaThresholdDragPanel.Name = "GammaThresholdDragPanel";
			this.GammaThresholdDragPanel.Size = new System.Drawing.Size(126, 21);
			this.GammaThresholdDragPanel.TabIndex = 30;
			this.GammaThresholdDragPanel.Text = "Gamma threshold";
			this.GammaThresholdDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.GammaThresholdDragPanel.TextBox = this.GammaThresholdTextBox;
			this.GammaThresholdDragPanel.Value = 0.001D;
			// 
			// VibrancyDragPanel
			// 
			this.VibrancyDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.VibrancyDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.VibrancyDragPanel.Default = 1D;
			this.VibrancyDragPanel.DragStepping = 0.1D;
			this.VibrancyDragPanel.Location = new System.Drawing.Point(5, 73);
			this.VibrancyDragPanel.Maximum = 3D;
			this.VibrancyDragPanel.Minimum = 0D;
			this.VibrancyDragPanel.Name = "VibrancyDragPanel";
			this.VibrancyDragPanel.Size = new System.Drawing.Size(126, 21);
			this.VibrancyDragPanel.TabIndex = 26;
			this.VibrancyDragPanel.Text = "Vibrancy";
			this.VibrancyDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.VibrancyDragPanel.TextBox = this.VibrancyTextBox;
			this.VibrancyDragPanel.Value = 1D;
			// 
			// BrightnessDragPanel
			// 
			this.BrightnessDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.BrightnessDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BrightnessDragPanel.Default = 4D;
			this.BrightnessDragPanel.DragStepping = 0.1D;
			this.BrightnessDragPanel.Location = new System.Drawing.Point(5, 41);
			this.BrightnessDragPanel.Maximum = 16D;
			this.BrightnessDragPanel.Minimum = 0.001D;
			this.BrightnessDragPanel.Name = "BrightnessDragPanel";
			this.BrightnessDragPanel.Size = new System.Drawing.Size(126, 21);
			this.BrightnessDragPanel.TabIndex = 23;
			this.BrightnessDragPanel.Text = "Brightness";
			this.BrightnessDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BrightnessDragPanel.TextBox = this.BrightnessTextBox;
			this.BrightnessDragPanel.Value = 4D;
			// 
			// GammaDragPanel
			// 
			this.GammaDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.GammaDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.GammaDragPanel.Default = 4D;
			this.GammaDragPanel.DragStepping = 0.1D;
			this.GammaDragPanel.Location = new System.Drawing.Point(5, 11);
			this.GammaDragPanel.Maximum = 5D;
			this.GammaDragPanel.Minimum = 1D;
			this.GammaDragPanel.Name = "GammaDragPanel";
			this.GammaDragPanel.Size = new System.Drawing.Size(126, 21);
			this.GammaDragPanel.TabIndex = 20;
			this.GammaDragPanel.Text = "Gamma";
			this.GammaDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.GammaDragPanel.TextBox = this.GammaTextBox;
			this.GammaDragPanel.Value = 4D;
			// 
			// BackgroundPictureBox
			// 
			this.BackgroundPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BackgroundPictureBox.BackColor = System.Drawing.Color.Black;
			this.BackgroundPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.BackgroundPictureBox.Location = new System.Drawing.Point(134, 102);
			this.BackgroundPictureBox.Name = "BackgroundPictureBox";
			this.BackgroundPictureBox.Size = new System.Drawing.Size(150, 22);
			this.BackgroundPictureBox.TabIndex = 32;
			this.BackgroundPictureBox.TabStop = false;
			// 
			// FlameProperties
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(508, 317);
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
			this.Text = "Adjustment";
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).EndInit();
			this.mPreviewContextMenu.ResumeLayout(false);
			this.mToolbar.ResumeLayout(false);
			this.mToolbar.PerformLayout();
			this.mCameraTab.ResumeLayout(false);
			this.mCameraTab.PerformLayout();
			this.mTabs.ResumeLayout(false);
			this.mImagingTab.ResumeLayout(false);
			this.mImagingTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).EndInit();
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
		public System.Windows.Forms.TextBox ZoomTextBox;
		public Controls.DragPanel ZoomDragPanel;
		public System.Windows.Forms.HScrollBar ZoomScrollBar;
		public System.Windows.Forms.HScrollBar XPositionScrollBar;
		public System.Windows.Forms.TextBox XPositionTextBox;
		public Controls.DragPanel XPositionDragPanel;
		public System.Windows.Forms.HScrollBar YPositionScrollBar;
		public System.Windows.Forms.TextBox YPositionTextBox;
		public Controls.DragPanel YPositionDragPanel;
		public System.Windows.Forms.HScrollBar RotationScrollBar;
		public System.Windows.Forms.TextBox RotationTextBox;
		public Controls.DragPanel RotationDragPanel;
		private System.Windows.Forms.ImageList mTabImages;
		private System.Windows.Forms.TabPage mImagingTab;
		public System.Windows.Forms.HScrollBar VibrancyScrollBar;
		public System.Windows.Forms.TextBox VibrancyTextBox;
		public Controls.DragPanel VibrancyDragPanel;
		public System.Windows.Forms.HScrollBar BrightnessScrollBar;
		public System.Windows.Forms.TextBox BrightnessTextBox;
		public Controls.DragPanel BrightnessDragPanel;
		public System.Windows.Forms.HScrollBar GammaScrollBar;
		public System.Windows.Forms.TextBox GammaTextBox;
		public Controls.DragPanel GammaDragPanel;
		private System.Windows.Forms.Label mBackgroundLabel;
		public System.Windows.Forms.TextBox GammaThresholdTextBox;
		public Controls.DragPanel GammaThresholdDragPanel;
		public System.Windows.Forms.PictureBox BackgroundPictureBox;
	}
}