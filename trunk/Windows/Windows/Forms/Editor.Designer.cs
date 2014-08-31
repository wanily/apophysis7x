using Xyrus.Apophysis.Windows.Controls;

namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Editor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.mSidebarSplitter = new System.Windows.Forms.SplitContainer();
			this.mPreviewPanel = new System.Windows.Forms.Panel();
			this.mPictureBevel = new System.Windows.Forms.Label();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mColorTab = new System.Windows.Forms.TabPage();
			this.mIteratorPropertyPanel = new System.Windows.Forms.Panel();
			this.IteratorWeightTextBox = new System.Windows.Forms.TextBox();
			this.IteratorNameTextBox = new System.Windows.Forms.TextBox();
			this.mIteratorNameLabel = new System.Windows.Forms.Label();
			this.IteratorSelectionComboBox = new System.Windows.Forms.ComboBox();
			this.mIteratorSelectLabel = new System.Windows.Forms.Label();
			this.mIteratorColorGroupBox = new System.Windows.Forms.GroupBox();
			this.IteratorColorTextBox = new System.Windows.Forms.TextBox();
			this.mPalettePictureBox = new System.Windows.Forms.PictureBox();
			this.IteratorColorScrollBar = new System.Windows.Forms.HScrollBar();
			this.IteratorColorSpeedTextBox = new System.Windows.Forms.TextBox();
			this.IteratorOpacityTextBox = new System.Windows.Forms.TextBox();
			this.IteratorDirectColorTextBox = new System.Windows.Forms.TextBox();
			this.IteratorIsExclusiveCheckBox = new System.Windows.Forms.CheckBox();
			this.IteratorCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.mSettings = new Xyrus.Apophysis.Windows.Controls.EditorSettings();
			this.IteratorDirectColorDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorOpacityDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorColorSpeedDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorColorDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorWeightDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.Panel2.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.mSidebarSplitter.Panel1.SuspendLayout();
			this.mSidebarSplitter.Panel2.SuspendLayout();
			this.mSidebarSplitter.SuspendLayout();
			this.mPreviewPanel.SuspendLayout();
			this.mTabs.SuspendLayout();
			this.mColorTab.SuspendLayout();
			this.mIteratorPropertyPanel.SuspendLayout();
			this.mIteratorColorGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mPalettePictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// mRootSplitter
			// 
			this.mRootSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mRootSplitter.Location = new System.Drawing.Point(0, 0);
			this.mRootSplitter.Name = "mRootSplitter";
			// 
			// mRootSplitter.Panel1
			// 
			this.mRootSplitter.Panel1.Controls.Add(this.IteratorCanvas);
			// 
			// mRootSplitter.Panel2
			// 
			this.mRootSplitter.Panel2.Controls.Add(this.mSidebarSplitter);
			this.mRootSplitter.Size = new System.Drawing.Size(1043, 765);
			this.mRootSplitter.SplitterDistance = 740;
			this.mRootSplitter.TabIndex = 3;
			// 
			// mSidebarSplitter
			// 
			this.mSidebarSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSidebarSplitter.Location = new System.Drawing.Point(0, 0);
			this.mSidebarSplitter.Name = "mSidebarSplitter";
			this.mSidebarSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// mSidebarSplitter.Panel1
			// 
			this.mSidebarSplitter.Panel1.Controls.Add(this.mPreviewPanel);
			// 
			// mSidebarSplitter.Panel2
			// 
			this.mSidebarSplitter.Panel2.Controls.Add(this.mTabs);
			this.mSidebarSplitter.Panel2.Controls.Add(this.mIteratorPropertyPanel);
			this.mSidebarSplitter.Size = new System.Drawing.Size(299, 765);
			this.mSidebarSplitter.SplitterDistance = 182;
			this.mSidebarSplitter.TabIndex = 0;
			// 
			// mPreviewPanel
			// 
			this.mPreviewPanel.Controls.Add(this.mPictureBevel);
			this.mPreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPreviewPanel.Location = new System.Drawing.Point(0, 0);
			this.mPreviewPanel.Name = "mPreviewPanel";
			this.mPreviewPanel.Size = new System.Drawing.Size(299, 182);
			this.mPreviewPanel.TabIndex = 0;
			// 
			// mPictureBevel
			// 
			this.mPictureBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBevel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPictureBevel.Location = new System.Drawing.Point(0, 0);
			this.mPictureBevel.Name = "mPictureBevel";
			this.mPictureBevel.Size = new System.Drawing.Size(299, 182);
			this.mPictureBevel.TabIndex = 0;
			// 
			// mTabs
			// 
			this.mTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mTabs.Controls.Add(this.mColorTab);
			this.mTabs.Location = new System.Drawing.Point(4, 90);
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			this.mTabs.Size = new System.Drawing.Size(293, 486);
			this.mTabs.TabIndex = 1;
			// 
			// mColorTab
			// 
			this.mColorTab.Controls.Add(this.mIteratorColorGroupBox);
			this.mColorTab.Location = new System.Drawing.Point(4, 22);
			this.mColorTab.Name = "mColorTab";
			this.mColorTab.Padding = new System.Windows.Forms.Padding(3);
			this.mColorTab.Size = new System.Drawing.Size(285, 460);
			this.mColorTab.TabIndex = 0;
			this.mColorTab.Text = "Color";
			this.mColorTab.UseVisualStyleBackColor = true;
			// 
			// mIteratorPropertyPanel
			// 
			this.mIteratorPropertyPanel.Controls.Add(this.IteratorWeightTextBox);
			this.mIteratorPropertyPanel.Controls.Add(this.IteratorWeightDragPanel);
			this.mIteratorPropertyPanel.Controls.Add(this.IteratorNameTextBox);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorNameLabel);
			this.mIteratorPropertyPanel.Controls.Add(this.IteratorSelectionComboBox);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorSelectLabel);
			this.mIteratorPropertyPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.mIteratorPropertyPanel.Location = new System.Drawing.Point(0, 0);
			this.mIteratorPropertyPanel.Name = "mIteratorPropertyPanel";
			this.mIteratorPropertyPanel.Size = new System.Drawing.Size(299, 86);
			this.mIteratorPropertyPanel.TabIndex = 0;
			// 
			// IteratorWeightTextBox
			// 
			this.IteratorWeightTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorWeightTextBox.Location = new System.Drawing.Point(120, 53);
			this.IteratorWeightTextBox.Name = "IteratorWeightTextBox";
			this.IteratorWeightTextBox.Size = new System.Drawing.Size(168, 20);
			this.IteratorWeightTextBox.TabIndex = 5;
			this.IteratorWeightTextBox.Text = "0.500";
			// 
			// IteratorNameTextBox
			// 
			this.IteratorNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorNameTextBox.Location = new System.Drawing.Point(120, 32);
			this.IteratorNameTextBox.Name = "IteratorNameTextBox";
			this.IteratorNameTextBox.Size = new System.Drawing.Size(168, 20);
			this.IteratorNameTextBox.TabIndex = 3;
			// 
			// mIteratorNameLabel
			// 
			this.mIteratorNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mIteratorNameLabel.Location = new System.Drawing.Point(14, 32);
			this.mIteratorNameLabel.Name = "mIteratorNameLabel";
			this.mIteratorNameLabel.Size = new System.Drawing.Size(107, 21);
			this.mIteratorNameLabel.TabIndex = 2;
			this.mIteratorNameLabel.Text = "Name:";
			this.mIteratorNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// IteratorSelectionComboBox
			// 
			this.IteratorSelectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorSelectionComboBox.BackColor = System.Drawing.Color.Black;
			this.IteratorSelectionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.IteratorSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.IteratorSelectionComboBox.ForeColor = System.Drawing.Color.White;
			this.IteratorSelectionComboBox.FormattingEnabled = true;
			this.IteratorSelectionComboBox.ItemHeight = 15;
			this.IteratorSelectionComboBox.Location = new System.Drawing.Point(120, 10);
			this.IteratorSelectionComboBox.MaxDropDownItems = 20;
			this.IteratorSelectionComboBox.Name = "IteratorSelectionComboBox";
			this.IteratorSelectionComboBox.Size = new System.Drawing.Size(168, 21);
			this.IteratorSelectionComboBox.TabIndex = 1;
			// 
			// mIteratorSelectLabel
			// 
			this.mIteratorSelectLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorSelectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mIteratorSelectLabel.Location = new System.Drawing.Point(14, 10);
			this.mIteratorSelectLabel.Name = "mIteratorSelectLabel";
			this.mIteratorSelectLabel.Size = new System.Drawing.Size(107, 22);
			this.mIteratorSelectLabel.TabIndex = 0;
			this.mIteratorSelectLabel.Text = "Transform:";
			this.mIteratorSelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mIteratorColorGroupBox
			// 
			this.mIteratorColorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorIsExclusiveCheckBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorDirectColorTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorDirectColorDragPanel);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorOpacityTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorOpacityDragPanel);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorSpeedTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorSpeedDragPanel);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorScrollBar);
			this.mIteratorColorGroupBox.Controls.Add(this.mPalettePictureBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorDragPanel);
			this.mIteratorColorGroupBox.Location = new System.Drawing.Point(6, 6);
			this.mIteratorColorGroupBox.Name = "mIteratorColorGroupBox";
			this.mIteratorColorGroupBox.Size = new System.Drawing.Size(273, 187);
			this.mIteratorColorGroupBox.TabIndex = 0;
			this.mIteratorColorGroupBox.TabStop = false;
			this.mIteratorColorGroupBox.Text = "Transform color";
			// 
			// IteratorColorTextBox
			// 
			this.IteratorColorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorColorTextBox.Location = new System.Drawing.Point(58, 19);
			this.IteratorColorTextBox.Name = "IteratorColorTextBox";
			this.IteratorColorTextBox.Size = new System.Drawing.Size(209, 20);
			this.IteratorColorTextBox.TabIndex = 7;
			this.IteratorColorTextBox.Text = "0.000";
			// 
			// mPalettePictureBox
			// 
			this.mPalettePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mPalettePictureBox.BackColor = System.Drawing.Color.Black;
			this.mPalettePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPalettePictureBox.Location = new System.Drawing.Point(6, 39);
			this.mPalettePictureBox.Name = "mPalettePictureBox";
			this.mPalettePictureBox.Size = new System.Drawing.Size(262, 20);
			this.mPalettePictureBox.TabIndex = 8;
			this.mPalettePictureBox.TabStop = false;
			// 
			// IteratorColorScrollBar
			// 
			this.IteratorColorScrollBar.LargeChange = 1;
			this.IteratorColorScrollBar.Location = new System.Drawing.Point(6, 59);
			this.IteratorColorScrollBar.Maximum = 1000;
			this.IteratorColorScrollBar.Name = "IteratorColorScrollBar";
			this.IteratorColorScrollBar.Size = new System.Drawing.Size(261, 17);
			this.IteratorColorScrollBar.TabIndex = 9;
			// 
			// IteratorColorSpeedTextBox
			// 
			this.IteratorColorSpeedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorColorSpeedTextBox.Location = new System.Drawing.Point(112, 88);
			this.IteratorColorSpeedTextBox.Name = "IteratorColorSpeedTextBox";
			this.IteratorColorSpeedTextBox.Size = new System.Drawing.Size(155, 20);
			this.IteratorColorSpeedTextBox.TabIndex = 11;
			this.IteratorColorSpeedTextBox.Text = "0.000";
			// 
			// IteratorOpacityTextBox
			// 
			this.IteratorOpacityTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorOpacityTextBox.Location = new System.Drawing.Point(112, 109);
			this.IteratorOpacityTextBox.Name = "IteratorOpacityTextBox";
			this.IteratorOpacityTextBox.Size = new System.Drawing.Size(155, 20);
			this.IteratorOpacityTextBox.TabIndex = 13;
			this.IteratorOpacityTextBox.Text = "1.000";
			// 
			// IteratorDirectColorTextBox
			// 
			this.IteratorDirectColorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorDirectColorTextBox.Location = new System.Drawing.Point(112, 130);
			this.IteratorDirectColorTextBox.Name = "IteratorDirectColorTextBox";
			this.IteratorDirectColorTextBox.Size = new System.Drawing.Size(155, 20);
			this.IteratorDirectColorTextBox.TabIndex = 15;
			this.IteratorDirectColorTextBox.Text = "1.000";
			// 
			// IteratorIsExclusiveCheckBox
			// 
			this.IteratorIsExclusiveCheckBox.AutoSize = true;
			this.IteratorIsExclusiveCheckBox.Location = new System.Drawing.Point(6, 160);
			this.IteratorIsExclusiveCheckBox.Name = "IteratorIsExclusiveCheckBox";
			this.IteratorIsExclusiveCheckBox.Size = new System.Drawing.Size(47, 17);
			this.IteratorIsExclusiveCheckBox.TabIndex = 16;
			this.IteratorIsExclusiveCheckBox.Text = "Solo";
			this.IteratorIsExclusiveCheckBox.UseVisualStyleBackColor = true;
			// 
			// IteratorCanvas
			// 
			this.IteratorCanvas.ActiveMatrix = Xyrus.Apophysis.Windows.Controls.IteratorMatrix.PreAffine;
			this.IteratorCanvas.BackColor = System.Drawing.Color.Black;
			this.IteratorCanvas.BackdropColor = System.Drawing.Color.Transparent;
			this.IteratorCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.IteratorCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.IteratorCanvas.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorCanvas.ForeColor = System.Drawing.Color.White;
			this.IteratorCanvas.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.IteratorCanvas.GridZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.IteratorCanvas.HighlightOrigin = true;
			this.IteratorCanvas.Iterators = null;
			this.IteratorCanvas.Location = new System.Drawing.Point(0, 0);
			this.IteratorCanvas.Name = "IteratorCanvas";
			this.IteratorCanvas.ReferenceColor = System.Drawing.Color.Gray;
			this.IteratorCanvas.RulerBackdropColor = System.Drawing.Color.Transparent;
			this.IteratorCanvas.RulerBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.IteratorCanvas.RulerGridLineColor = System.Drawing.Color.Silver;
			this.IteratorCanvas.Settings = this.mSettings;
			this.IteratorCanvas.ShowRuler = true;
			this.IteratorCanvas.Size = new System.Drawing.Size(740, 765);
			this.IteratorCanvas.TabIndex = 1;
			// 
			// mSettings
			// 
			this.mSettings.AngleSnap = 15D;
			this.mSettings.LockAxes = false;
			this.mSettings.MoveAmount = 0.1D;
			this.mSettings.ScaleSnap = 125D;
			this.mSettings.ShowVariationPreview = false;
			this.mSettings.ZoomAutomatically = false;
			// 
			// IteratorDirectColorDragPanel
			// 
			this.IteratorDirectColorDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorDirectColorDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorDirectColorDragPanel.Default = 1D;
			this.IteratorDirectColorDragPanel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.IteratorDirectColorDragPanel.Location = new System.Drawing.Point(6, 130);
			this.IteratorDirectColorDragPanel.Maximum = 1D;
			this.IteratorDirectColorDragPanel.Minimum = 0D;
			this.IteratorDirectColorDragPanel.Name = "IteratorDirectColorDragPanel";
			this.IteratorDirectColorDragPanel.Size = new System.Drawing.Size(107, 21);
			this.IteratorDirectColorDragPanel.TabIndex = 14;
			this.IteratorDirectColorDragPanel.Text = "Direct color:";
			this.IteratorDirectColorDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorDirectColorDragPanel.TextBox = this.IteratorDirectColorTextBox;
			this.IteratorDirectColorDragPanel.Value = 1D;
			// 
			// IteratorOpacityDragPanel
			// 
			this.IteratorOpacityDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorOpacityDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorOpacityDragPanel.Default = 1D;
			this.IteratorOpacityDragPanel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.IteratorOpacityDragPanel.Location = new System.Drawing.Point(6, 109);
			this.IteratorOpacityDragPanel.Maximum = 1D;
			this.IteratorOpacityDragPanel.Minimum = 0D;
			this.IteratorOpacityDragPanel.Name = "IteratorOpacityDragPanel";
			this.IteratorOpacityDragPanel.Size = new System.Drawing.Size(107, 21);
			this.IteratorOpacityDragPanel.TabIndex = 12;
			this.IteratorOpacityDragPanel.Text = "Opacity:";
			this.IteratorOpacityDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorOpacityDragPanel.TextBox = this.IteratorOpacityTextBox;
			this.IteratorOpacityDragPanel.Value = 1D;
			// 
			// IteratorColorSpeedDragPanel
			// 
			this.IteratorColorSpeedDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorColorSpeedDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorColorSpeedDragPanel.Default = 0D;
			this.IteratorColorSpeedDragPanel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.IteratorColorSpeedDragPanel.Location = new System.Drawing.Point(6, 88);
			this.IteratorColorSpeedDragPanel.Maximum = 1D;
			this.IteratorColorSpeedDragPanel.Minimum = -1D;
			this.IteratorColorSpeedDragPanel.Name = "IteratorColorSpeedDragPanel";
			this.IteratorColorSpeedDragPanel.Size = new System.Drawing.Size(107, 21);
			this.IteratorColorSpeedDragPanel.TabIndex = 10;
			this.IteratorColorSpeedDragPanel.Text = "Color speed:";
			this.IteratorColorSpeedDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorColorSpeedDragPanel.TextBox = this.IteratorColorSpeedTextBox;
			this.IteratorColorSpeedDragPanel.Value = 0D;
			// 
			// IteratorColorDragPanel
			// 
			this.IteratorColorDragPanel.BackColor = System.Drawing.Color.Black;
			this.IteratorColorDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorColorDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorColorDragPanel.Default = 0D;
			this.IteratorColorDragPanel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.IteratorColorDragPanel.Location = new System.Drawing.Point(6, 19);
			this.IteratorColorDragPanel.Maximum = 1D;
			this.IteratorColorDragPanel.Minimum = 0D;
			this.IteratorColorDragPanel.Name = "IteratorColorDragPanel";
			this.IteratorColorDragPanel.Size = new System.Drawing.Size(56, 21);
			this.IteratorColorDragPanel.TabIndex = 6;
			this.IteratorColorDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorColorDragPanel.TextBox = this.IteratorColorTextBox;
			this.IteratorColorDragPanel.Value = 0D;
			// 
			// IteratorWeightDragPanel
			// 
			this.IteratorWeightDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorWeightDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorWeightDragPanel.Default = 0.5D;
			this.IteratorWeightDragPanel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.IteratorWeightDragPanel.Location = new System.Drawing.Point(14, 53);
			this.IteratorWeightDragPanel.Maximum = 1000D;
			this.IteratorWeightDragPanel.Minimum = 0.001D;
			this.IteratorWeightDragPanel.Name = "IteratorWeightDragPanel";
			this.IteratorWeightDragPanel.Size = new System.Drawing.Size(107, 21);
			this.IteratorWeightDragPanel.TabIndex = 4;
			this.IteratorWeightDragPanel.Text = "Weight:";
			this.IteratorWeightDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorWeightDragPanel.TextBox = this.IteratorWeightTextBox;
			this.IteratorWeightDragPanel.Value = 0.5D;
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1043, 765);
			this.Controls.Add(this.mRootSplitter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 39);
			this.Name = "Editor";
			this.Text = "xw_prototype_2014-337";
			this.mRootSplitter.Panel1.ResumeLayout(false);
			this.mRootSplitter.Panel2.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.mSidebarSplitter.Panel1.ResumeLayout(false);
			this.mSidebarSplitter.Panel2.ResumeLayout(false);
			this.mSidebarSplitter.ResumeLayout(false);
			this.mPreviewPanel.ResumeLayout(false);
			this.mTabs.ResumeLayout(false);
			this.mColorTab.ResumeLayout(false);
			this.mIteratorPropertyPanel.ResumeLayout(false);
			this.mIteratorPropertyPanel.PerformLayout();
			this.mIteratorColorGroupBox.ResumeLayout(false);
			this.mIteratorColorGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mPalettePictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer mRootSplitter;
		private EditorSettings mSettings;
		private System.Windows.Forms.SplitContainer mSidebarSplitter;
		private System.Windows.Forms.Panel mPreviewPanel;
		private System.Windows.Forms.Panel mIteratorPropertyPanel;
		private System.Windows.Forms.Label mIteratorSelectLabel;
		private System.Windows.Forms.Label mIteratorNameLabel;
		private System.Windows.Forms.Label mPictureBevel;
		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mColorTab;
		public System.Windows.Forms.ComboBox IteratorSelectionComboBox;
		public System.Windows.Forms.TextBox IteratorNameTextBox;
		public DragPanel IteratorWeightDragPanel;
		public System.Windows.Forms.TextBox IteratorWeightTextBox;
		public EditorCanvas IteratorCanvas;
		private System.Windows.Forms.GroupBox mIteratorColorGroupBox;
		public System.Windows.Forms.TextBox IteratorColorTextBox;
		public DragPanel IteratorColorDragPanel;
		private System.Windows.Forms.PictureBox mPalettePictureBox;
		public System.Windows.Forms.TextBox IteratorColorSpeedTextBox;
		public DragPanel IteratorColorSpeedDragPanel;
		public System.Windows.Forms.TextBox IteratorOpacityTextBox;
		public DragPanel IteratorOpacityDragPanel;
		public System.Windows.Forms.TextBox IteratorDirectColorTextBox;
		public DragPanel IteratorDirectColorDragPanel;
		public System.Windows.Forms.CheckBox IteratorIsExclusiveCheckBox;
		public System.Windows.Forms.HScrollBar IteratorColorScrollBar;
	}
}

