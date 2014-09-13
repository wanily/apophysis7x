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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.mToolbar = new System.Windows.Forms.ToolStrip();
			this.ResetAllIteratorsButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.AddIteratorButton = new System.Windows.Forms.ToolStripButton();
			this.DuplicateIteratorButton = new System.Windows.Forms.ToolStripButton();
			this.RemoveIteratorButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.UndoButton = new System.Windows.Forms.ToolStripButton();
			this.RedoButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.IteratorResetButton = new System.Windows.Forms.ToolStripButton();
			this.IteratorResetOriginButton = new System.Windows.Forms.ToolStripButton();
			this.IteratorResetAngleButton = new System.Windows.Forms.ToolStripButton();
			this.IteratorResetScaleButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.Rotate90CcwButton = new System.Windows.Forms.ToolStripButton();
			this.Rotate90CwButton = new System.Windows.Forms.ToolStripButton();
			this.FlipAllHorizontalButton = new System.Windows.Forms.ToolStripButton();
			this.FlipAllVerticalButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.ToggleRulersButton = new System.Windows.Forms.ToolStripButton();
			this.ToggleVariationPreviewButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.TogglePostMatrixButton = new System.Windows.Forms.ToolStripButton();
			this.AddFinalIteratorButton = new System.Windows.Forms.ToolStripButton();
			this.mToolbarSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.IteratorConvertToRegularButton = new System.Windows.Forms.ToolStripButton();
			this.IteratorConvertToFinalButton = new System.Windows.Forms.ToolStripButton();
			this.IteratorCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.mSettings = new Xyrus.Apophysis.Windows.Controls.EditorSettings();
			this.mSidebarSplitter = new System.Windows.Forms.SplitContainer();
			this.mPreviewPanel = new System.Windows.Forms.Panel();
			this.PreviewPicture = new System.Windows.Forms.PictureBox();
			this.mPreviewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.LowQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MediumQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HighQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mPictureBevel = new System.Windows.Forms.Label();
			this.Tabs = new System.Windows.Forms.TabControl();
			this.VariationsTab = new System.Windows.Forms.TabPage();
			this.ClearVariationsButton = new System.Windows.Forms.Button();
			this.HideUnusedVariationsCheckBox = new System.Windows.Forms.CheckBox();
			this.VariationsGrid = new Xyrus.Apophysis.Windows.Controls.DragGrid();
			this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.VariablesTab = new System.Windows.Forms.TabPage();
			this.HideUnrelatedVariablesCheckBox = new System.Windows.Forms.CheckBox();
			this.VariablesGrid = new Xyrus.Apophysis.Windows.Controls.DragGrid();
			this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PointTab = new System.Windows.Forms.TabPage();
			this.mIteratorControlsGroupBox = new System.Windows.Forms.GroupBox();
			this.IteratorScaleRatio = new System.Windows.Forms.ComboBox();
			this.IteratorScaleUp = new System.Windows.Forms.Button();
			this.IteratorScaleDown = new System.Windows.Forms.Button();
			this.IteratorMoveOffset = new System.Windows.Forms.ComboBox();
			this.IteratorMoveLeft = new System.Windows.Forms.Button();
			this.IteratorMoveDown = new System.Windows.Forms.Button();
			this.IteratorMoveRight = new System.Windows.Forms.Button();
			this.IteratorMoveUp = new System.Windows.Forms.Button();
			this.IteratorSnapAngle = new System.Windows.Forms.ComboBox();
			this.IteratorRotateCW = new System.Windows.Forms.Button();
			this.IteratorRotateCCW = new System.Windows.Forms.Button();
			this.IteratorRotate90CW = new System.Windows.Forms.Button();
			this.IteratorRotate90CCW = new System.Windows.Forms.Button();
			this.mPointCoordGroupBox = new System.Windows.Forms.GroupBox();
			this.IteratorResetPointY = new System.Windows.Forms.Button();
			this.IteratorResetPointO = new System.Windows.Forms.Button();
			this.IteratorResetPointX = new System.Windows.Forms.Button();
			this.IteratorPointOyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPointOxTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPointYyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPointYxTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPointXyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPointXxTextBox = new System.Windows.Forms.TextBox();
			this.VectorTab = new System.Windows.Forms.TabPage();
			this.mIteratorPostAffineGroupBox = new System.Windows.Forms.GroupBox();
			this.IteratorResetPostAffine = new System.Windows.Forms.Button();
			this.IteratorResetPostAffineY = new System.Windows.Forms.Button();
			this.IteratorResetPostAffineO = new System.Windows.Forms.Button();
			this.IteratorResetPostAffineX = new System.Windows.Forms.Button();
			this.IteratorPostAffineOyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPostAffineOxTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPostAffineYyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPostAffineYxTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPostAffineXyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPostAffineXxTextBox = new System.Windows.Forms.TextBox();
			this.mPreAffineGroupBox = new System.Windows.Forms.GroupBox();
			this.IteratorResetPreAffine = new System.Windows.Forms.Button();
			this.IteratorResetPreAffineY = new System.Windows.Forms.Button();
			this.IteratorResetPreAffineO = new System.Windows.Forms.Button();
			this.IteratorResetPreAffineX = new System.Windows.Forms.Button();
			this.IteratorPreAffineOyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPreAffineOxTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPreAffineYyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPreAffineYxTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPreAffineXyTextBox = new System.Windows.Forms.TextBox();
			this.IteratorPreAffineXxTextBox = new System.Windows.Forms.TextBox();
			this.ColorTab = new System.Windows.Forms.TabPage();
			this.mPreviewGroupBox = new System.Windows.Forms.GroupBox();
			this.mPreviewDensityLabel = new System.Windows.Forms.Label();
			this.mPreviewRangeLabel = new System.Windows.Forms.Label();
			this.PreviewDensityTrackBar = new System.Windows.Forms.TrackBar();
			this.PreviewRangeTrackBar = new System.Windows.Forms.TrackBar();
			this.mIteratorColorGroupBox = new System.Windows.Forms.GroupBox();
			this.PaletteSelectComboBox = new Xyrus.Apophysis.Windows.Controls.PaletteSelectComboBox();
			this.IteratorDirectColorTextBox = new System.Windows.Forms.TextBox();
			this.IteratorDirectColorDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorOpacityTextBox = new System.Windows.Forms.TextBox();
			this.IteratorOpacityDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorColorSpeedTextBox = new System.Windows.Forms.TextBox();
			this.IteratorColorSpeedDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorColorScrollBar = new System.Windows.Forms.HScrollBar();
			this.IteratorColorTextBox = new System.Windows.Forms.TextBox();
			this.IteratorColorDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.mIteratorPropertyPanel = new System.Windows.Forms.Panel();
			this.IteratorWeightTextBox = new System.Windows.Forms.TextBox();
			this.IteratorWeightDragPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.IteratorNameTextBox = new System.Windows.Forms.TextBox();
			this.mIteratorNameLabel = new System.Windows.Forms.Label();
			this.IteratorSelectionComboBox = new Xyrus.Apophysis.Windows.Controls.IteratorSelectComboBox();
			this.mIteratorSelectLabel = new System.Windows.Forms.Label();
			this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.Panel2.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.mToolbar.SuspendLayout();
			this.mSidebarSplitter.Panel1.SuspendLayout();
			this.mSidebarSplitter.Panel2.SuspendLayout();
			this.mSidebarSplitter.SuspendLayout();
			this.mPreviewPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
			this.mPreviewContextMenu.SuspendLayout();
			this.Tabs.SuspendLayout();
			this.VariationsTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.VariationsGrid)).BeginInit();
			this.VariablesTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.VariablesGrid)).BeginInit();
			this.PointTab.SuspendLayout();
			this.mIteratorControlsGroupBox.SuspendLayout();
			this.mPointCoordGroupBox.SuspendLayout();
			this.VectorTab.SuspendLayout();
			this.mIteratorPostAffineGroupBox.SuspendLayout();
			this.mPreAffineGroupBox.SuspendLayout();
			this.ColorTab.SuspendLayout();
			this.mPreviewGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewDensityTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PreviewRangeTrackBar)).BeginInit();
			this.mIteratorColorGroupBox.SuspendLayout();
			this.mIteratorPropertyPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mRootSplitter
			// 
			this.mRootSplitter.BackColor = System.Drawing.SystemColors.Control;
			this.mRootSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mRootSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.mRootSplitter.IsSplitterFixed = true;
			this.mRootSplitter.Location = new System.Drawing.Point(0, 0);
			this.mRootSplitter.Name = "mRootSplitter";
			// 
			// mRootSplitter.Panel1
			// 
			this.mRootSplitter.Panel1.Controls.Add(this.mToolbar);
			this.mRootSplitter.Panel1.Controls.Add(this.IteratorCanvas);
			// 
			// mRootSplitter.Panel2
			// 
			this.mRootSplitter.Panel2.Controls.Add(this.mSidebarSplitter);
			this.mRootSplitter.Size = new System.Drawing.Size(1014, 661);
			this.mRootSplitter.SplitterDistance = 711;
			this.mRootSplitter.TabIndex = 3;
			// 
			// mToolbar
			// 
			this.mToolbar.AllowMerge = false;
			this.mToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResetAllIteratorsButton,
            this.mToolbarSeparator1,
            this.AddIteratorButton,
            this.DuplicateIteratorButton,
            this.RemoveIteratorButton,
            this.mToolbarSeparator2,
            this.UndoButton,
            this.RedoButton,
            this.mToolbarSeparator3,
            this.IteratorResetButton,
            this.IteratorResetOriginButton,
            this.IteratorResetAngleButton,
            this.IteratorResetScaleButton,
            this.mToolbarSeparator4,
            this.Rotate90CcwButton,
            this.Rotate90CwButton,
            this.FlipAllHorizontalButton,
            this.FlipAllVerticalButton,
            this.mToolbarSeparator5,
            this.ToggleRulersButton,
            this.ToggleVariationPreviewButton,
            this.mToolbarSeparator6,
            this.TogglePostMatrixButton,
            this.AddFinalIteratorButton,
            this.mToolbarSeparator7,
            this.IteratorConvertToRegularButton,
            this.IteratorConvertToFinalButton});
			this.mToolbar.Location = new System.Drawing.Point(0, 0);
			this.mToolbar.Name = "mToolbar";
			this.mToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.mToolbar.Size = new System.Drawing.Size(711, 25);
			this.mToolbar.TabIndex = 2;
			// 
			// ResetAllIteratorsButton
			// 
			this.ResetAllIteratorsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ResetAllIteratorsButton.Image = ((System.Drawing.Image)(resources.GetObject("ResetAllIteratorsButton.Image")));
			this.ResetAllIteratorsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ResetAllIteratorsButton.Name = "ResetAllIteratorsButton";
			this.ResetAllIteratorsButton.Size = new System.Drawing.Size(23, 22);
			this.ResetAllIteratorsButton.Text = "Reset all transforms";
			// 
			// mToolbarSeparator1
			// 
			this.mToolbarSeparator1.Name = "mToolbarSeparator1";
			this.mToolbarSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// AddIteratorButton
			// 
			this.AddIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AddIteratorButton.Image = ((System.Drawing.Image)(resources.GetObject("AddIteratorButton.Image")));
			this.AddIteratorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddIteratorButton.Name = "AddIteratorButton";
			this.AddIteratorButton.Size = new System.Drawing.Size(23, 22);
			this.AddIteratorButton.Text = "Add transform";
			// 
			// DuplicateIteratorButton
			// 
			this.DuplicateIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.DuplicateIteratorButton.Image = ((System.Drawing.Image)(resources.GetObject("DuplicateIteratorButton.Image")));
			this.DuplicateIteratorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DuplicateIteratorButton.Name = "DuplicateIteratorButton";
			this.DuplicateIteratorButton.Size = new System.Drawing.Size(23, 22);
			this.DuplicateIteratorButton.Text = "Duplicate transform";
			// 
			// RemoveIteratorButton
			// 
			this.RemoveIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RemoveIteratorButton.Image = ((System.Drawing.Image)(resources.GetObject("RemoveIteratorButton.Image")));
			this.RemoveIteratorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveIteratorButton.Name = "RemoveIteratorButton";
			this.RemoveIteratorButton.Size = new System.Drawing.Size(23, 22);
			this.RemoveIteratorButton.Text = "Remove transform";
			// 
			// mToolbarSeparator2
			// 
			this.mToolbarSeparator2.Name = "mToolbarSeparator2";
			this.mToolbarSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// UndoButton
			// 
			this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UndoButton.Image = ((System.Drawing.Image)(resources.GetObject("UndoButton.Image")));
			this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UndoButton.Name = "UndoButton";
			this.UndoButton.Size = new System.Drawing.Size(23, 22);
			this.UndoButton.Text = "Undo";
			// 
			// RedoButton
			// 
			this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RedoButton.Image = ((System.Drawing.Image)(resources.GetObject("RedoButton.Image")));
			this.RedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RedoButton.Name = "RedoButton";
			this.RedoButton.Size = new System.Drawing.Size(23, 22);
			this.RedoButton.Text = "Redo";
			// 
			// mToolbarSeparator3
			// 
			this.mToolbarSeparator3.Name = "mToolbarSeparator3";
			this.mToolbarSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// IteratorResetButton
			// 
			this.IteratorResetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.IteratorResetButton.Image = ((System.Drawing.Image)(resources.GetObject("IteratorResetButton.Image")));
			this.IteratorResetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.IteratorResetButton.Name = "IteratorResetButton";
			this.IteratorResetButton.Size = new System.Drawing.Size(23, 22);
			this.IteratorResetButton.Text = "Reset transform";
			// 
			// IteratorResetOriginButton
			// 
			this.IteratorResetOriginButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.IteratorResetOriginButton.Image = ((System.Drawing.Image)(resources.GetObject("IteratorResetOriginButton.Image")));
			this.IteratorResetOriginButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.IteratorResetOriginButton.Name = "IteratorResetOriginButton";
			this.IteratorResetOriginButton.Size = new System.Drawing.Size(23, 22);
			this.IteratorResetOriginButton.Text = "Reset transform origin";
			// 
			// IteratorResetAngleButton
			// 
			this.IteratorResetAngleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.IteratorResetAngleButton.Image = ((System.Drawing.Image)(resources.GetObject("IteratorResetAngleButton.Image")));
			this.IteratorResetAngleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.IteratorResetAngleButton.Name = "IteratorResetAngleButton";
			this.IteratorResetAngleButton.Size = new System.Drawing.Size(23, 22);
			this.IteratorResetAngleButton.Text = "Reset transform angle";
			// 
			// IteratorResetScaleButton
			// 
			this.IteratorResetScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.IteratorResetScaleButton.Image = ((System.Drawing.Image)(resources.GetObject("IteratorResetScaleButton.Image")));
			this.IteratorResetScaleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.IteratorResetScaleButton.Name = "IteratorResetScaleButton";
			this.IteratorResetScaleButton.Size = new System.Drawing.Size(23, 22);
			this.IteratorResetScaleButton.Text = "Reset transform scale";
			// 
			// mToolbarSeparator4
			// 
			this.mToolbarSeparator4.Name = "mToolbarSeparator4";
			this.mToolbarSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// Rotate90CcwButton
			// 
			this.Rotate90CcwButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Rotate90CcwButton.Image = ((System.Drawing.Image)(resources.GetObject("Rotate90CcwButton.Image")));
			this.Rotate90CcwButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Rotate90CcwButton.Name = "Rotate90CcwButton";
			this.Rotate90CcwButton.Size = new System.Drawing.Size(23, 22);
			this.Rotate90CcwButton.Text = "Rotate world 90° counter-clockwise";
			// 
			// Rotate90CwButton
			// 
			this.Rotate90CwButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Rotate90CwButton.Image = ((System.Drawing.Image)(resources.GetObject("Rotate90CwButton.Image")));
			this.Rotate90CwButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Rotate90CwButton.Name = "Rotate90CwButton";
			this.Rotate90CwButton.Size = new System.Drawing.Size(23, 22);
			this.Rotate90CwButton.Text = "Rotate world 90° clockwise";
			// 
			// FlipAllHorizontalButton
			// 
			this.FlipAllHorizontalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.FlipAllHorizontalButton.Image = ((System.Drawing.Image)(resources.GetObject("FlipAllHorizontalButton.Image")));
			this.FlipAllHorizontalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.FlipAllHorizontalButton.Name = "FlipAllHorizontalButton";
			this.FlipAllHorizontalButton.Size = new System.Drawing.Size(23, 22);
			this.FlipAllHorizontalButton.Text = "Flip all transforms horizontally";
			// 
			// FlipAllVerticalButton
			// 
			this.FlipAllVerticalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.FlipAllVerticalButton.Image = ((System.Drawing.Image)(resources.GetObject("FlipAllVerticalButton.Image")));
			this.FlipAllVerticalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.FlipAllVerticalButton.Name = "FlipAllVerticalButton";
			this.FlipAllVerticalButton.Size = new System.Drawing.Size(23, 22);
			this.FlipAllVerticalButton.Text = "Flip all transforms vertically";
			// 
			// mToolbarSeparator5
			// 
			this.mToolbarSeparator5.Name = "mToolbarSeparator5";
			this.mToolbarSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// ToggleRulersButton
			// 
			this.ToggleRulersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToggleRulersButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleRulersButton.Image")));
			this.ToggleRulersButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToggleRulersButton.Name = "ToggleRulersButton";
			this.ToggleRulersButton.Size = new System.Drawing.Size(23, 22);
			this.ToggleRulersButton.Text = "Show ruler";
			// 
			// ToggleVariationPreviewButton
			// 
			this.ToggleVariationPreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToggleVariationPreviewButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleVariationPreviewButton.Image")));
			this.ToggleVariationPreviewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToggleVariationPreviewButton.Name = "ToggleVariationPreviewButton";
			this.ToggleVariationPreviewButton.Size = new System.Drawing.Size(23, 22);
			this.ToggleVariationPreviewButton.Text = "Show variation preview";
			// 
			// mToolbarSeparator6
			// 
			this.mToolbarSeparator6.Name = "mToolbarSeparator6";
			this.mToolbarSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// TogglePostMatrixButton
			// 
			this.TogglePostMatrixButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TogglePostMatrixButton.Image = ((System.Drawing.Image)(resources.GetObject("TogglePostMatrixButton.Image")));
			this.TogglePostMatrixButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TogglePostMatrixButton.Name = "TogglePostMatrixButton";
			this.TogglePostMatrixButton.Size = new System.Drawing.Size(23, 22);
			this.TogglePostMatrixButton.Text = "Enable / edit post-transform";
			// 
			// AddFinalIteratorButton
			// 
			this.AddFinalIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AddFinalIteratorButton.Image = ((System.Drawing.Image)(resources.GetObject("AddFinalIteratorButton.Image")));
			this.AddFinalIteratorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddFinalIteratorButton.Name = "AddFinalIteratorButton";
			this.AddFinalIteratorButton.Size = new System.Drawing.Size(23, 22);
			this.AddFinalIteratorButton.Text = "Add final transform";
			// 
			// mToolbarSeparator7
			// 
			this.mToolbarSeparator7.Name = "mToolbarSeparator7";
			this.mToolbarSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// IteratorConvertToRegularButton
			// 
			this.IteratorConvertToRegularButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.IteratorConvertToRegularButton.Image = ((System.Drawing.Image)(resources.GetObject("IteratorConvertToRegularButton.Image")));
			this.IteratorConvertToRegularButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.IteratorConvertToRegularButton.Name = "IteratorConvertToRegularButton";
			this.IteratorConvertToRegularButton.Size = new System.Drawing.Size(23, 22);
			this.IteratorConvertToRegularButton.Text = "Convert to regular transform";
			// 
			// IteratorConvertToFinalButton
			// 
			this.IteratorConvertToFinalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.IteratorConvertToFinalButton.Image = ((System.Drawing.Image)(resources.GetObject("IteratorConvertToFinalButton.Image")));
			this.IteratorConvertToFinalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.IteratorConvertToFinalButton.Name = "IteratorConvertToFinalButton";
			this.IteratorConvertToFinalButton.Size = new System.Drawing.Size(23, 22);
			this.IteratorConvertToFinalButton.Text = "Convert to final transform";
			// 
			// IteratorCanvas
			// 
			this.IteratorCanvas.ActiveMatrix = Xyrus.Apophysis.Windows.Controls.IteratorMatrix.PreAffine;
			this.IteratorCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorCanvas.BackColor = System.Drawing.Color.Black;
			this.IteratorCanvas.BackdropColor = System.Drawing.Color.Transparent;
			this.IteratorCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorCanvas.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorCanvas.ForeColor = System.Drawing.Color.White;
			this.IteratorCanvas.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.IteratorCanvas.GridZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.IteratorCanvas.HighlightOrigin = true;
			this.IteratorCanvas.Iterators = null;
			this.IteratorCanvas.Location = new System.Drawing.Point(0, 24);
			this.IteratorCanvas.Name = "IteratorCanvas";
			this.IteratorCanvas.PreviewApplyPostTransform = false;
			this.IteratorCanvas.PreviewDensity = 1D;
			this.IteratorCanvas.PreviewRange = 1D;
			this.IteratorCanvas.ReferenceColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.IteratorCanvas.RulerBackdropColor = System.Drawing.Color.Transparent;
			this.IteratorCanvas.RulerBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.IteratorCanvas.RulerGridLineColor = System.Drawing.Color.Silver;
			this.IteratorCanvas.Settings = this.mSettings;
			this.IteratorCanvas.ShowRuler = true;
			this.IteratorCanvas.Size = new System.Drawing.Size(711, 637);
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
			// mSidebarSplitter
			// 
			this.mSidebarSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSidebarSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.mSidebarSplitter.IsSplitterFixed = true;
			this.mSidebarSplitter.Location = new System.Drawing.Point(0, 0);
			this.mSidebarSplitter.Name = "mSidebarSplitter";
			this.mSidebarSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// mSidebarSplitter.Panel1
			// 
			this.mSidebarSplitter.Panel1.Controls.Add(this.mPreviewPanel);
			this.mSidebarSplitter.Panel1MinSize = 100;
			// 
			// mSidebarSplitter.Panel2
			// 
			this.mSidebarSplitter.Panel2.Controls.Add(this.Tabs);
			this.mSidebarSplitter.Panel2.Controls.Add(this.mIteratorPropertyPanel);
			this.mSidebarSplitter.Size = new System.Drawing.Size(299, 661);
			this.mSidebarSplitter.SplitterDistance = 178;
			this.mSidebarSplitter.TabIndex = 0;
			// 
			// mPreviewPanel
			// 
			this.mPreviewPanel.Controls.Add(this.PreviewPicture);
			this.mPreviewPanel.Controls.Add(this.mPictureBevel);
			this.mPreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPreviewPanel.Location = new System.Drawing.Point(0, 0);
			this.mPreviewPanel.Name = "mPreviewPanel";
			this.mPreviewPanel.Size = new System.Drawing.Size(299, 178);
			this.mPreviewPanel.TabIndex = 0;
			// 
			// PreviewPicture
			// 
			this.PreviewPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewPicture.BackColor = System.Drawing.SystemColors.Control;
			this.PreviewPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.PreviewPicture.ContextMenuStrip = this.mPreviewContextMenu;
			this.PreviewPicture.Location = new System.Drawing.Point(4, 10);
			this.PreviewPicture.Name = "PreviewPicture";
			this.PreviewPicture.Size = new System.Drawing.Size(287, 162);
			this.PreviewPicture.TabIndex = 1;
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
			this.mPictureBevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mPictureBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBevel.Location = new System.Drawing.Point(3, 9);
			this.mPictureBevel.Name = "mPictureBevel";
			this.mPictureBevel.Size = new System.Drawing.Size(289, 165);
			this.mPictureBevel.TabIndex = 0;
			// 
			// Tabs
			// 
			this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Tabs.Controls.Add(this.VariationsTab);
			this.Tabs.Controls.Add(this.VariablesTab);
			this.Tabs.Controls.Add(this.PointTab);
			this.Tabs.Controls.Add(this.VectorTab);
			this.Tabs.Controls.Add(this.ColorTab);
			this.Tabs.Location = new System.Drawing.Point(3, 75);
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			this.Tabs.Size = new System.Drawing.Size(293, 401);
			this.Tabs.TabIndex = 1;
			// 
			// VariationsTab
			// 
			this.VariationsTab.Controls.Add(this.ClearVariationsButton);
			this.VariationsTab.Controls.Add(this.HideUnusedVariationsCheckBox);
			this.VariationsTab.Controls.Add(this.VariationsGrid);
			this.VariationsTab.Location = new System.Drawing.Point(4, 22);
			this.VariationsTab.Name = "VariationsTab";
			this.VariationsTab.Padding = new System.Windows.Forms.Padding(3);
			this.VariationsTab.Size = new System.Drawing.Size(285, 375);
			this.VariationsTab.TabIndex = 3;
			this.VariationsTab.Text = "Variations";
			this.VariationsTab.UseVisualStyleBackColor = true;
			// 
			// ClearVariationsButton
			// 
			this.ClearVariationsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ClearVariationsButton.Location = new System.Drawing.Point(3, 351);
			this.ClearVariationsButton.Name = "ClearVariationsButton";
			this.ClearVariationsButton.Size = new System.Drawing.Size(279, 25);
			this.ClearVariationsButton.TabIndex = 2;
			this.ClearVariationsButton.Text = "Clear";
			this.ClearVariationsButton.UseVisualStyleBackColor = true;
			// 
			// HideUnusedVariationsCheckBox
			// 
			this.HideUnusedVariationsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.HideUnusedVariationsCheckBox.AutoSize = true;
			this.HideUnusedVariationsCheckBox.Location = new System.Drawing.Point(3, 329);
			this.HideUnusedVariationsCheckBox.Name = "HideUnusedVariationsCheckBox";
			this.HideUnusedVariationsCheckBox.Size = new System.Drawing.Size(134, 17);
			this.HideUnusedVariationsCheckBox.TabIndex = 1;
			this.HideUnusedVariationsCheckBox.Text = "Hide unused variations";
			this.HideUnusedVariationsCheckBox.UseVisualStyleBackColor = true;
			// 
			// VariationsGrid
			// 
			this.VariationsGrid.AllowUserToAddRows = false;
			this.VariationsGrid.AllowUserToDeleteRows = false;
			this.VariationsGrid.AllowUserToOrderColumns = true;
			this.VariationsGrid.AllowUserToResizeRows = false;
			this.VariationsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VariationsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.VariationsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.VariationsGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.VariationsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.VariationsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.VariationsGrid.EnableHeadersVisualStyles = false;
			this.VariationsGrid.GridColor = System.Drawing.SystemColors.ControlLight;
			this.VariationsGrid.Location = new System.Drawing.Point(3, 3);
			this.VariationsGrid.MultiSelect = false;
			this.VariationsGrid.Name = "VariationsGrid";
			this.VariationsGrid.ResetMode = Xyrus.Apophysis.Windows.Controls.DragGridResetMode.Toggle;
			this.VariationsGrid.RowHeadersVisible = false;
			this.VariationsGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.VariationsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.VariationsGrid.ShowCellErrors = false;
			this.VariationsGrid.ShowCellToolTips = false;
			this.VariationsGrid.ShowEditingIcon = false;
			this.VariationsGrid.ShowRowErrors = false;
			this.VariationsGrid.Size = new System.Drawing.Size(279, 316);
			this.VariationsGrid.TabIndex = 0;
			// 
			// dataGridViewTextBoxColumn23
			// 
			this.dataGridViewTextBoxColumn23.HeaderText = "Name";
			this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
			this.dataGridViewTextBoxColumn23.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn24
			// 
			this.dataGridViewTextBoxColumn24.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn24.HeaderText = "Value";
			this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
			// 
			// VariablesTab
			// 
			this.VariablesTab.Controls.Add(this.HideUnrelatedVariablesCheckBox);
			this.VariablesTab.Controls.Add(this.VariablesGrid);
			this.VariablesTab.Location = new System.Drawing.Point(4, 22);
			this.VariablesTab.Name = "VariablesTab";
			this.VariablesTab.Padding = new System.Windows.Forms.Padding(3);
			this.VariablesTab.Size = new System.Drawing.Size(285, 375);
			this.VariablesTab.TabIndex = 4;
			this.VariablesTab.Text = "Variables";
			this.VariablesTab.UseVisualStyleBackColor = true;
			// 
			// HideUnrelatedVariablesCheckBox
			// 
			this.HideUnrelatedVariablesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.HideUnrelatedVariablesCheckBox.AutoSize = true;
			this.HideUnrelatedVariablesCheckBox.Checked = true;
			this.HideUnrelatedVariablesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HideUnrelatedVariablesCheckBox.Location = new System.Drawing.Point(3, 356);
			this.HideUnrelatedVariablesCheckBox.Name = "HideUnrelatedVariablesCheckBox";
			this.HideUnrelatedVariablesCheckBox.Size = new System.Drawing.Size(202, 17);
			this.HideUnrelatedVariablesCheckBox.TabIndex = 3;
			this.HideUnrelatedVariablesCheckBox.Text = "Hide variables from unused variations";
			this.HideUnrelatedVariablesCheckBox.UseVisualStyleBackColor = true;
			// 
			// VariablesGrid
			// 
			this.VariablesGrid.AllowUserToAddRows = false;
			this.VariablesGrid.AllowUserToDeleteRows = false;
			this.VariablesGrid.AllowUserToOrderColumns = true;
			this.VariablesGrid.AllowUserToResizeRows = false;
			this.VariablesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VariablesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.VariablesGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.VariablesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.VariablesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.VariablesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.VariablesGrid.EnableHeadersVisualStyles = false;
			this.VariablesGrid.GridColor = System.Drawing.SystemColors.ControlLight;
			this.VariablesGrid.Location = new System.Drawing.Point(3, 3);
			this.VariablesGrid.MultiSelect = false;
			this.VariablesGrid.Name = "VariablesGrid";
			this.VariablesGrid.ResetMode = Xyrus.Apophysis.Windows.Controls.DragGridResetMode.Override;
			this.VariablesGrid.RowHeadersVisible = false;
			this.VariablesGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.VariablesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.VariablesGrid.ShowCellErrors = false;
			this.VariablesGrid.ShowCellToolTips = false;
			this.VariablesGrid.ShowEditingIcon = false;
			this.VariablesGrid.ShowRowErrors = false;
			this.VariablesGrid.Size = new System.Drawing.Size(279, 342);
			this.VariablesGrid.TabIndex = 1;
			// 
			// dataGridViewTextBoxColumn25
			// 
			this.dataGridViewTextBoxColumn25.HeaderText = "Name";
			this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
			this.dataGridViewTextBoxColumn25.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn26
			// 
			this.dataGridViewTextBoxColumn26.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn26.HeaderText = "Value";
			this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
			// 
			// PointTab
			// 
			this.PointTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.PointTab.Controls.Add(this.mIteratorControlsGroupBox);
			this.PointTab.Controls.Add(this.mPointCoordGroupBox);
			this.PointTab.Location = new System.Drawing.Point(4, 22);
			this.PointTab.Name = "PointTab";
			this.PointTab.Padding = new System.Windows.Forms.Padding(3);
			this.PointTab.Size = new System.Drawing.Size(285, 375);
			this.PointTab.TabIndex = 1;
			this.PointTab.Text = "Triangle";
			// 
			// mIteratorControlsGroupBox
			// 
			this.mIteratorControlsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorScaleRatio);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorScaleUp);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorScaleDown);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorMoveOffset);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorMoveLeft);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorMoveDown);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorMoveRight);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorMoveUp);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorSnapAngle);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorRotateCW);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorRotateCCW);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorRotate90CW);
			this.mIteratorControlsGroupBox.Controls.Add(this.IteratorRotate90CCW);
			this.mIteratorControlsGroupBox.Location = new System.Drawing.Point(6, 116);
			this.mIteratorControlsGroupBox.Name = "mIteratorControlsGroupBox";
			this.mIteratorControlsGroupBox.Size = new System.Drawing.Size(273, 118);
			this.mIteratorControlsGroupBox.TabIndex = 1;
			this.mIteratorControlsGroupBox.TabStop = false;
			// 
			// IteratorScaleRatio
			// 
			this.IteratorScaleRatio.FormattingEnabled = true;
			this.IteratorScaleRatio.Location = new System.Drawing.Point(71, 81);
			this.IteratorScaleRatio.Name = "IteratorScaleRatio";
			this.IteratorScaleRatio.Size = new System.Drawing.Size(131, 21);
			this.IteratorScaleRatio.TabIndex = 12;
			// 
			// IteratorScaleUp
			// 
			this.IteratorScaleUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorScaleUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorScaleUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorScaleUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorScaleUp.Image = ((System.Drawing.Image)(resources.GetObject("IteratorScaleUp.Image")));
			this.IteratorScaleUp.Location = new System.Drawing.Point(208, 79);
			this.IteratorScaleUp.Name = "IteratorScaleUp";
			this.IteratorScaleUp.Size = new System.Drawing.Size(24, 24);
			this.IteratorScaleUp.TabIndex = 11;
			this.IteratorScaleUp.UseVisualStyleBackColor = true;
			// 
			// IteratorScaleDown
			// 
			this.IteratorScaleDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorScaleDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorScaleDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorScaleDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorScaleDown.Image = ((System.Drawing.Image)(resources.GetObject("IteratorScaleDown.Image")));
			this.IteratorScaleDown.Location = new System.Drawing.Point(41, 79);
			this.IteratorScaleDown.Name = "IteratorScaleDown";
			this.IteratorScaleDown.Size = new System.Drawing.Size(24, 24);
			this.IteratorScaleDown.TabIndex = 10;
			this.IteratorScaleDown.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveOffset
			// 
			this.IteratorMoveOffset.FormattingEnabled = true;
			this.IteratorMoveOffset.Location = new System.Drawing.Point(71, 51);
			this.IteratorMoveOffset.Name = "IteratorMoveOffset";
			this.IteratorMoveOffset.Size = new System.Drawing.Size(131, 21);
			this.IteratorMoveOffset.TabIndex = 9;
			// 
			// IteratorMoveLeft
			// 
			this.IteratorMoveLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveLeft.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveLeft.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorMoveLeft.Image = ((System.Drawing.Image)(resources.GetObject("IteratorMoveLeft.Image")));
			this.IteratorMoveLeft.Location = new System.Drawing.Point(208, 49);
			this.IteratorMoveLeft.Name = "IteratorMoveLeft";
			this.IteratorMoveLeft.Size = new System.Drawing.Size(24, 24);
			this.IteratorMoveLeft.TabIndex = 8;
			this.IteratorMoveLeft.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveDown
			// 
			this.IteratorMoveDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("IteratorMoveDown.Image")));
			this.IteratorMoveDown.Location = new System.Drawing.Point(41, 49);
			this.IteratorMoveDown.Name = "IteratorMoveDown";
			this.IteratorMoveDown.Size = new System.Drawing.Size(24, 24);
			this.IteratorMoveDown.TabIndex = 7;
			this.IteratorMoveDown.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveRight
			// 
			this.IteratorMoveRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveRight.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveRight.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorMoveRight.Image = ((System.Drawing.Image)(resources.GetObject("IteratorMoveRight.Image")));
			this.IteratorMoveRight.Location = new System.Drawing.Point(238, 49);
			this.IteratorMoveRight.Name = "IteratorMoveRight";
			this.IteratorMoveRight.Size = new System.Drawing.Size(24, 24);
			this.IteratorMoveRight.TabIndex = 6;
			this.IteratorMoveRight.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveUp
			// 
			this.IteratorMoveUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("IteratorMoveUp.Image")));
			this.IteratorMoveUp.Location = new System.Drawing.Point(11, 49);
			this.IteratorMoveUp.Name = "IteratorMoveUp";
			this.IteratorMoveUp.Size = new System.Drawing.Size(24, 24);
			this.IteratorMoveUp.TabIndex = 5;
			this.IteratorMoveUp.UseVisualStyleBackColor = true;
			// 
			// IteratorSnapAngle
			// 
			this.IteratorSnapAngle.FormattingEnabled = true;
			this.IteratorSnapAngle.Location = new System.Drawing.Point(71, 21);
			this.IteratorSnapAngle.Name = "IteratorSnapAngle";
			this.IteratorSnapAngle.Size = new System.Drawing.Size(131, 21);
			this.IteratorSnapAngle.TabIndex = 4;
			this.IteratorSnapAngle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorRotateCW
			// 
			this.IteratorRotateCW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotateCW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotateCW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorRotateCW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorRotateCW.Image = ((System.Drawing.Image)(resources.GetObject("IteratorRotateCW.Image")));
			this.IteratorRotateCW.Location = new System.Drawing.Point(208, 19);
			this.IteratorRotateCW.Name = "IteratorRotateCW";
			this.IteratorRotateCW.Size = new System.Drawing.Size(24, 24);
			this.IteratorRotateCW.TabIndex = 3;
			this.IteratorRotateCW.UseVisualStyleBackColor = true;
			// 
			// IteratorRotateCCW
			// 
			this.IteratorRotateCCW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotateCCW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotateCCW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorRotateCCW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorRotateCCW.Image = ((System.Drawing.Image)(resources.GetObject("IteratorRotateCCW.Image")));
			this.IteratorRotateCCW.Location = new System.Drawing.Point(41, 19);
			this.IteratorRotateCCW.Name = "IteratorRotateCCW";
			this.IteratorRotateCCW.Size = new System.Drawing.Size(24, 24);
			this.IteratorRotateCCW.TabIndex = 2;
			this.IteratorRotateCCW.UseVisualStyleBackColor = true;
			// 
			// IteratorRotate90CW
			// 
			this.IteratorRotate90CW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotate90CW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotate90CW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorRotate90CW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorRotate90CW.Image = ((System.Drawing.Image)(resources.GetObject("IteratorRotate90CW.Image")));
			this.IteratorRotate90CW.Location = new System.Drawing.Point(238, 19);
			this.IteratorRotate90CW.Name = "IteratorRotate90CW";
			this.IteratorRotate90CW.Size = new System.Drawing.Size(24, 24);
			this.IteratorRotate90CW.TabIndex = 1;
			this.IteratorRotate90CW.UseVisualStyleBackColor = true;
			// 
			// IteratorRotate90CCW
			// 
			this.IteratorRotate90CCW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotate90CCW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotate90CCW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorRotate90CCW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorRotate90CCW.Image = ((System.Drawing.Image)(resources.GetObject("IteratorRotate90CCW.Image")));
			this.IteratorRotate90CCW.Location = new System.Drawing.Point(11, 19);
			this.IteratorRotate90CCW.Name = "IteratorRotate90CCW";
			this.IteratorRotate90CCW.Size = new System.Drawing.Size(24, 24);
			this.IteratorRotate90CCW.TabIndex = 0;
			this.IteratorRotate90CCW.UseVisualStyleBackColor = true;
			// 
			// mPointCoordGroupBox
			// 
			this.mPointCoordGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mPointCoordGroupBox.Controls.Add(this.IteratorResetPointY);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorResetPointO);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorResetPointX);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointOyTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointOxTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointYyTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointYxTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointXyTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointXxTextBox);
			this.mPointCoordGroupBox.Location = new System.Drawing.Point(6, 6);
			this.mPointCoordGroupBox.Name = "mPointCoordGroupBox";
			this.mPointCoordGroupBox.Size = new System.Drawing.Size(273, 104);
			this.mPointCoordGroupBox.TabIndex = 0;
			this.mPointCoordGroupBox.TabStop = false;
			// 
			// IteratorResetPointY
			// 
			this.IteratorResetPointY.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPointY.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPointY.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPointY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPointY.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPointY.Location = new System.Drawing.Point(11, 44);
			this.IteratorResetPointY.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPointY.Name = "IteratorResetPointY";
			this.IteratorResetPointY.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPointY.TabIndex = 15;
			this.IteratorResetPointY.Text = "Y";
			this.IteratorResetPointY.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPointO
			// 
			this.IteratorResetPointO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPointO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPointO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPointO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPointO.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPointO.Location = new System.Drawing.Point(11, 70);
			this.IteratorResetPointO.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPointO.Name = "IteratorResetPointO";
			this.IteratorResetPointO.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPointO.TabIndex = 14;
			this.IteratorResetPointO.Text = "O";
			this.IteratorResetPointO.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPointX
			// 
			this.IteratorResetPointX.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPointX.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPointX.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPointX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPointX.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPointX.Location = new System.Drawing.Point(11, 19);
			this.IteratorResetPointX.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPointX.Name = "IteratorResetPointX";
			this.IteratorResetPointX.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPointX.TabIndex = 11;
			this.IteratorResetPointX.Text = "X";
			this.IteratorResetPointX.UseVisualStyleBackColor = true;
			// 
			// IteratorPointOyTextBox
			// 
			this.IteratorPointOyTextBox.Location = new System.Drawing.Point(151, 70);
			this.IteratorPointOyTextBox.Name = "IteratorPointOyTextBox";
			this.IteratorPointOyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPointOyTextBox.TabIndex = 8;
			this.IteratorPointOyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointOxTextBox
			// 
			this.IteratorPointOxTextBox.Location = new System.Drawing.Point(34, 70);
			this.IteratorPointOxTextBox.Name = "IteratorPointOxTextBox";
			this.IteratorPointOxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPointOxTextBox.TabIndex = 7;
			this.IteratorPointOxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointYyTextBox
			// 
			this.IteratorPointYyTextBox.Location = new System.Drawing.Point(151, 44);
			this.IteratorPointYyTextBox.Name = "IteratorPointYyTextBox";
			this.IteratorPointYyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPointYyTextBox.TabIndex = 6;
			this.IteratorPointYyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointYxTextBox
			// 
			this.IteratorPointYxTextBox.Location = new System.Drawing.Point(34, 44);
			this.IteratorPointYxTextBox.Name = "IteratorPointYxTextBox";
			this.IteratorPointYxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPointYxTextBox.TabIndex = 5;
			this.IteratorPointYxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointXyTextBox
			// 
			this.IteratorPointXyTextBox.Location = new System.Drawing.Point(151, 19);
			this.IteratorPointXyTextBox.Name = "IteratorPointXyTextBox";
			this.IteratorPointXyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPointXyTextBox.TabIndex = 4;
			this.IteratorPointXyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointXxTextBox
			// 
			this.IteratorPointXxTextBox.Location = new System.Drawing.Point(34, 19);
			this.IteratorPointXxTextBox.Name = "IteratorPointXxTextBox";
			this.IteratorPointXxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPointXxTextBox.TabIndex = 3;
			this.IteratorPointXxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// VectorTab
			// 
			this.VectorTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.VectorTab.Controls.Add(this.mIteratorPostAffineGroupBox);
			this.VectorTab.Controls.Add(this.mPreAffineGroupBox);
			this.VectorTab.Location = new System.Drawing.Point(4, 22);
			this.VectorTab.Name = "VectorTab";
			this.VectorTab.Padding = new System.Windows.Forms.Padding(3);
			this.VectorTab.Size = new System.Drawing.Size(285, 375);
			this.VectorTab.TabIndex = 2;
			this.VectorTab.Text = "Transform";
			// 
			// mIteratorPostAffineGroupBox
			// 
			this.mIteratorPostAffineGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorResetPostAffine);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorResetPostAffineY);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorResetPostAffineO);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorResetPostAffineX);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorPostAffineOyTextBox);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorPostAffineOxTextBox);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorPostAffineYyTextBox);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorPostAffineYxTextBox);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorPostAffineXyTextBox);
			this.mIteratorPostAffineGroupBox.Controls.Add(this.IteratorPostAffineXxTextBox);
			this.mIteratorPostAffineGroupBox.Location = new System.Drawing.Point(6, 148);
			this.mIteratorPostAffineGroupBox.Name = "mIteratorPostAffineGroupBox";
			this.mIteratorPostAffineGroupBox.Size = new System.Drawing.Size(273, 137);
			this.mIteratorPostAffineGroupBox.TabIndex = 17;
			this.mIteratorPostAffineGroupBox.TabStop = false;
			// 
			// IteratorResetPostAffine
			// 
			this.IteratorResetPostAffine.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffine.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffine.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPostAffine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPostAffine.Location = new System.Drawing.Point(11, 97);
			this.IteratorResetPostAffine.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPostAffine.Name = "IteratorResetPostAffine";
			this.IteratorResetPostAffine.Size = new System.Drawing.Size(251, 29);
			this.IteratorResetPostAffine.TabIndex = 16;
			this.IteratorResetPostAffine.Text = "Reset post transform";
			this.IteratorResetPostAffine.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPostAffineY
			// 
			this.IteratorResetPostAffineY.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffineY.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffineY.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPostAffineY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPostAffineY.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPostAffineY.Location = new System.Drawing.Point(11, 44);
			this.IteratorResetPostAffineY.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPostAffineY.Name = "IteratorResetPostAffineY";
			this.IteratorResetPostAffineY.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPostAffineY.TabIndex = 15;
			this.IteratorResetPostAffineY.Text = "Y";
			this.IteratorResetPostAffineY.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPostAffineO
			// 
			this.IteratorResetPostAffineO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffineO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffineO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPostAffineO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPostAffineO.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPostAffineO.Location = new System.Drawing.Point(11, 70);
			this.IteratorResetPostAffineO.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPostAffineO.Name = "IteratorResetPostAffineO";
			this.IteratorResetPostAffineO.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPostAffineO.TabIndex = 14;
			this.IteratorResetPostAffineO.Text = "O";
			this.IteratorResetPostAffineO.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPostAffineX
			// 
			this.IteratorResetPostAffineX.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffineX.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffineX.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPostAffineX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPostAffineX.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPostAffineX.Location = new System.Drawing.Point(11, 19);
			this.IteratorResetPostAffineX.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPostAffineX.Name = "IteratorResetPostAffineX";
			this.IteratorResetPostAffineX.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPostAffineX.TabIndex = 11;
			this.IteratorResetPostAffineX.Text = "X";
			this.IteratorResetPostAffineX.UseVisualStyleBackColor = true;
			// 
			// IteratorPostAffineOyTextBox
			// 
			this.IteratorPostAffineOyTextBox.Location = new System.Drawing.Point(151, 70);
			this.IteratorPostAffineOyTextBox.Name = "IteratorPostAffineOyTextBox";
			this.IteratorPostAffineOyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPostAffineOyTextBox.TabIndex = 8;
			// 
			// IteratorPostAffineOxTextBox
			// 
			this.IteratorPostAffineOxTextBox.Location = new System.Drawing.Point(34, 70);
			this.IteratorPostAffineOxTextBox.Name = "IteratorPostAffineOxTextBox";
			this.IteratorPostAffineOxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPostAffineOxTextBox.TabIndex = 7;
			// 
			// IteratorPostAffineYyTextBox
			// 
			this.IteratorPostAffineYyTextBox.Location = new System.Drawing.Point(151, 44);
			this.IteratorPostAffineYyTextBox.Name = "IteratorPostAffineYyTextBox";
			this.IteratorPostAffineYyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPostAffineYyTextBox.TabIndex = 6;
			// 
			// IteratorPostAffineYxTextBox
			// 
			this.IteratorPostAffineYxTextBox.Location = new System.Drawing.Point(34, 44);
			this.IteratorPostAffineYxTextBox.Name = "IteratorPostAffineYxTextBox";
			this.IteratorPostAffineYxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPostAffineYxTextBox.TabIndex = 5;
			// 
			// IteratorPostAffineXyTextBox
			// 
			this.IteratorPostAffineXyTextBox.Location = new System.Drawing.Point(151, 19);
			this.IteratorPostAffineXyTextBox.Name = "IteratorPostAffineXyTextBox";
			this.IteratorPostAffineXyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPostAffineXyTextBox.TabIndex = 4;
			// 
			// IteratorPostAffineXxTextBox
			// 
			this.IteratorPostAffineXxTextBox.Location = new System.Drawing.Point(34, 19);
			this.IteratorPostAffineXxTextBox.Name = "IteratorPostAffineXxTextBox";
			this.IteratorPostAffineXxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPostAffineXxTextBox.TabIndex = 3;
			// 
			// mPreAffineGroupBox
			// 
			this.mPreAffineGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mPreAffineGroupBox.Controls.Add(this.IteratorResetPreAffine);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorResetPreAffineY);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorResetPreAffineO);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorResetPreAffineX);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorPreAffineOyTextBox);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorPreAffineOxTextBox);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorPreAffineYyTextBox);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorPreAffineYxTextBox);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorPreAffineXyTextBox);
			this.mPreAffineGroupBox.Controls.Add(this.IteratorPreAffineXxTextBox);
			this.mPreAffineGroupBox.Location = new System.Drawing.Point(6, 6);
			this.mPreAffineGroupBox.Name = "mPreAffineGroupBox";
			this.mPreAffineGroupBox.Size = new System.Drawing.Size(273, 136);
			this.mPreAffineGroupBox.TabIndex = 1;
			this.mPreAffineGroupBox.TabStop = false;
			// 
			// IteratorResetPreAffine
			// 
			this.IteratorResetPreAffine.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffine.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffine.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPreAffine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPreAffine.Location = new System.Drawing.Point(11, 97);
			this.IteratorResetPreAffine.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPreAffine.Name = "IteratorResetPreAffine";
			this.IteratorResetPreAffine.Size = new System.Drawing.Size(251, 29);
			this.IteratorResetPreAffine.TabIndex = 16;
			this.IteratorResetPreAffine.Text = "Reset transform";
			this.IteratorResetPreAffine.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPreAffineY
			// 
			this.IteratorResetPreAffineY.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffineY.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffineY.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPreAffineY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPreAffineY.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPreAffineY.Location = new System.Drawing.Point(11, 44);
			this.IteratorResetPreAffineY.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPreAffineY.Name = "IteratorResetPreAffineY";
			this.IteratorResetPreAffineY.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPreAffineY.TabIndex = 15;
			this.IteratorResetPreAffineY.Text = "Y";
			this.IteratorResetPreAffineY.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPreAffineO
			// 
			this.IteratorResetPreAffineO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffineO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffineO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPreAffineO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPreAffineO.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPreAffineO.Location = new System.Drawing.Point(11, 70);
			this.IteratorResetPreAffineO.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPreAffineO.Name = "IteratorResetPreAffineO";
			this.IteratorResetPreAffineO.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPreAffineO.TabIndex = 14;
			this.IteratorResetPreAffineO.Text = "O";
			this.IteratorResetPreAffineO.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPreAffineX
			// 
			this.IteratorResetPreAffineX.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffineX.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffineX.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			this.IteratorResetPreAffineX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.IteratorResetPreAffineX.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IteratorResetPreAffineX.Location = new System.Drawing.Point(11, 19);
			this.IteratorResetPreAffineX.Margin = new System.Windows.Forms.Padding(0);
			this.IteratorResetPreAffineX.Name = "IteratorResetPreAffineX";
			this.IteratorResetPreAffineX.Size = new System.Drawing.Size(20, 20);
			this.IteratorResetPreAffineX.TabIndex = 11;
			this.IteratorResetPreAffineX.Text = "X";
			this.IteratorResetPreAffineX.UseVisualStyleBackColor = true;
			// 
			// IteratorPreAffineOyTextBox
			// 
			this.IteratorPreAffineOyTextBox.Location = new System.Drawing.Point(151, 70);
			this.IteratorPreAffineOyTextBox.Name = "IteratorPreAffineOyTextBox";
			this.IteratorPreAffineOyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPreAffineOyTextBox.TabIndex = 8;
			this.IteratorPreAffineOyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineOxTextBox
			// 
			this.IteratorPreAffineOxTextBox.Location = new System.Drawing.Point(34, 70);
			this.IteratorPreAffineOxTextBox.Name = "IteratorPreAffineOxTextBox";
			this.IteratorPreAffineOxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPreAffineOxTextBox.TabIndex = 7;
			this.IteratorPreAffineOxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineYyTextBox
			// 
			this.IteratorPreAffineYyTextBox.Location = new System.Drawing.Point(151, 44);
			this.IteratorPreAffineYyTextBox.Name = "IteratorPreAffineYyTextBox";
			this.IteratorPreAffineYyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPreAffineYyTextBox.TabIndex = 6;
			this.IteratorPreAffineYyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineYxTextBox
			// 
			this.IteratorPreAffineYxTextBox.Location = new System.Drawing.Point(34, 44);
			this.IteratorPreAffineYxTextBox.Name = "IteratorPreAffineYxTextBox";
			this.IteratorPreAffineYxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPreAffineYxTextBox.TabIndex = 5;
			this.IteratorPreAffineYxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineXyTextBox
			// 
			this.IteratorPreAffineXyTextBox.Location = new System.Drawing.Point(151, 19);
			this.IteratorPreAffineXyTextBox.Name = "IteratorPreAffineXyTextBox";
			this.IteratorPreAffineXyTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPreAffineXyTextBox.TabIndex = 4;
			this.IteratorPreAffineXyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineXxTextBox
			// 
			this.IteratorPreAffineXxTextBox.Location = new System.Drawing.Point(34, 19);
			this.IteratorPreAffineXxTextBox.Name = "IteratorPreAffineXxTextBox";
			this.IteratorPreAffineXxTextBox.Size = new System.Drawing.Size(111, 20);
			this.IteratorPreAffineXxTextBox.TabIndex = 3;
			this.IteratorPreAffineXxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// ColorTab
			// 
			this.ColorTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ColorTab.Controls.Add(this.mPreviewGroupBox);
			this.ColorTab.Controls.Add(this.mIteratorColorGroupBox);
			this.ColorTab.Location = new System.Drawing.Point(4, 22);
			this.ColorTab.Name = "ColorTab";
			this.ColorTab.Padding = new System.Windows.Forms.Padding(3);
			this.ColorTab.Size = new System.Drawing.Size(285, 375);
			this.ColorTab.TabIndex = 0;
			this.ColorTab.Text = "Color";
			// 
			// mPreviewGroupBox
			// 
			this.mPreviewGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mPreviewGroupBox.Controls.Add(this.mPreviewDensityLabel);
			this.mPreviewGroupBox.Controls.Add(this.mPreviewRangeLabel);
			this.mPreviewGroupBox.Controls.Add(this.PreviewDensityTrackBar);
			this.mPreviewGroupBox.Controls.Add(this.PreviewRangeTrackBar);
			this.mPreviewGroupBox.Location = new System.Drawing.Point(6, 200);
			this.mPreviewGroupBox.Name = "mPreviewGroupBox";
			this.mPreviewGroupBox.Size = new System.Drawing.Size(273, 127);
			this.mPreviewGroupBox.TabIndex = 1;
			this.mPreviewGroupBox.TabStop = false;
			this.mPreviewGroupBox.Text = "Variation preview";
			// 
			// mPreviewDensityLabel
			// 
			this.mPreviewDensityLabel.AutoSize = true;
			this.mPreviewDensityLabel.Location = new System.Drawing.Point(10, 71);
			this.mPreviewDensityLabel.Name = "mPreviewDensityLabel";
			this.mPreviewDensityLabel.Size = new System.Drawing.Size(45, 13);
			this.mPreviewDensityLabel.TabIndex = 3;
			this.mPreviewDensityLabel.Text = "Density:";
			// 
			// mPreviewRangeLabel
			// 
			this.mPreviewRangeLabel.AutoSize = true;
			this.mPreviewRangeLabel.Location = new System.Drawing.Point(9, 33);
			this.mPreviewRangeLabel.Name = "mPreviewRangeLabel";
			this.mPreviewRangeLabel.Size = new System.Drawing.Size(42, 13);
			this.mPreviewRangeLabel.TabIndex = 2;
			this.mPreviewRangeLabel.Text = "Range:";
			// 
			// PreviewDensityTrackBar
			// 
			this.PreviewDensityTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewDensityTrackBar.Location = new System.Drawing.Point(57, 67);
			this.PreviewDensityTrackBar.Maximum = 4;
			this.PreviewDensityTrackBar.Minimum = 1;
			this.PreviewDensityTrackBar.Name = "PreviewDensityTrackBar";
			this.PreviewDensityTrackBar.Size = new System.Drawing.Size(210, 45);
			this.PreviewDensityTrackBar.TabIndex = 1;
			this.PreviewDensityTrackBar.Value = 1;
			// 
			// PreviewRangeTrackBar
			// 
			this.PreviewRangeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PreviewRangeTrackBar.Location = new System.Drawing.Point(57, 29);
			this.PreviewRangeTrackBar.Maximum = 5;
			this.PreviewRangeTrackBar.Minimum = 1;
			this.PreviewRangeTrackBar.Name = "PreviewRangeTrackBar";
			this.PreviewRangeTrackBar.Size = new System.Drawing.Size(210, 45);
			this.PreviewRangeTrackBar.TabIndex = 0;
			this.PreviewRangeTrackBar.Value = 1;
			// 
			// mIteratorColorGroupBox
			// 
			this.mIteratorColorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorColorGroupBox.Controls.Add(this.PaletteSelectComboBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorDirectColorTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorDirectColorDragPanel);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorOpacityTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorOpacityDragPanel);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorSpeedTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorSpeedDragPanel);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorScrollBar);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorTextBox);
			this.mIteratorColorGroupBox.Controls.Add(this.IteratorColorDragPanel);
			this.mIteratorColorGroupBox.Location = new System.Drawing.Point(6, 6);
			this.mIteratorColorGroupBox.Name = "mIteratorColorGroupBox";
			this.mIteratorColorGroupBox.Size = new System.Drawing.Size(273, 177);
			this.mIteratorColorGroupBox.TabIndex = 0;
			this.mIteratorColorGroupBox.TabStop = false;
			// 
			// PaletteSelectComboBox
			// 
			this.PaletteSelectComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.PaletteSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PaletteSelectComboBox.FormattingEnabled = true;
			this.PaletteSelectComboBox.ItemHeight = 15;
			this.PaletteSelectComboBox.Location = new System.Drawing.Point(11, 62);
			this.PaletteSelectComboBox.Name = "PaletteSelectComboBox";
			this.PaletteSelectComboBox.Size = new System.Drawing.Size(251, 21);
			this.PaletteSelectComboBox.TabIndex = 1;
			// 
			// IteratorDirectColorTextBox
			// 
			this.IteratorDirectColorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorDirectColorTextBox.Location = new System.Drawing.Point(117, 142);
			this.IteratorDirectColorTextBox.Name = "IteratorDirectColorTextBox";
			this.IteratorDirectColorTextBox.Size = new System.Drawing.Size(143, 20);
			this.IteratorDirectColorTextBox.TabIndex = 15;
			this.IteratorDirectColorTextBox.Text = "1.000";
			// 
			// IteratorDirectColorDragPanel
			// 
			this.IteratorDirectColorDragPanel.BackColor = System.Drawing.SystemColors.Window;
			this.IteratorDirectColorDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorDirectColorDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorDirectColorDragPanel.Default = 1D;
			this.IteratorDirectColorDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.IteratorDirectColorDragPanel.Location = new System.Drawing.Point(11, 142);
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
			// IteratorOpacityTextBox
			// 
			this.IteratorOpacityTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorOpacityTextBox.Location = new System.Drawing.Point(117, 119);
			this.IteratorOpacityTextBox.Name = "IteratorOpacityTextBox";
			this.IteratorOpacityTextBox.Size = new System.Drawing.Size(143, 20);
			this.IteratorOpacityTextBox.TabIndex = 13;
			this.IteratorOpacityTextBox.Text = "1.000";
			// 
			// IteratorOpacityDragPanel
			// 
			this.IteratorOpacityDragPanel.BackColor = System.Drawing.SystemColors.Window;
			this.IteratorOpacityDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorOpacityDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorOpacityDragPanel.Default = 1D;
			this.IteratorOpacityDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.IteratorOpacityDragPanel.Location = new System.Drawing.Point(11, 119);
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
			// IteratorColorSpeedTextBox
			// 
			this.IteratorColorSpeedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorColorSpeedTextBox.Location = new System.Drawing.Point(117, 96);
			this.IteratorColorSpeedTextBox.Name = "IteratorColorSpeedTextBox";
			this.IteratorColorSpeedTextBox.Size = new System.Drawing.Size(143, 20);
			this.IteratorColorSpeedTextBox.TabIndex = 11;
			this.IteratorColorSpeedTextBox.Text = "0.000";
			// 
			// IteratorColorSpeedDragPanel
			// 
			this.IteratorColorSpeedDragPanel.BackColor = System.Drawing.SystemColors.Window;
			this.IteratorColorSpeedDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorColorSpeedDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorColorSpeedDragPanel.Default = 0D;
			this.IteratorColorSpeedDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.IteratorColorSpeedDragPanel.Location = new System.Drawing.Point(11, 96);
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
			// IteratorColorScrollBar
			// 
			this.IteratorColorScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorColorScrollBar.LargeChange = 1;
			this.IteratorColorScrollBar.Location = new System.Drawing.Point(11, 42);
			this.IteratorColorScrollBar.Maximum = 1000;
			this.IteratorColorScrollBar.Name = "IteratorColorScrollBar";
			this.IteratorColorScrollBar.Size = new System.Drawing.Size(251, 17);
			this.IteratorColorScrollBar.TabIndex = 9;
			// 
			// IteratorColorTextBox
			// 
			this.IteratorColorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorColorTextBox.Location = new System.Drawing.Point(58, 19);
			this.IteratorColorTextBox.Name = "IteratorColorTextBox";
			this.IteratorColorTextBox.Size = new System.Drawing.Size(204, 20);
			this.IteratorColorTextBox.TabIndex = 7;
			this.IteratorColorTextBox.Text = "0.000";
			// 
			// IteratorColorDragPanel
			// 
			this.IteratorColorDragPanel.BackColor = System.Drawing.Color.Black;
			this.IteratorColorDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorColorDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorColorDragPanel.Default = 0D;
			this.IteratorColorDragPanel.Location = new System.Drawing.Point(11, 19);
			this.IteratorColorDragPanel.Maximum = 1D;
			this.IteratorColorDragPanel.Minimum = 0D;
			this.IteratorColorDragPanel.Name = "IteratorColorDragPanel";
			this.IteratorColorDragPanel.Size = new System.Drawing.Size(43, 21);
			this.IteratorColorDragPanel.TabIndex = 6;
			this.IteratorColorDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorColorDragPanel.TextBox = this.IteratorColorTextBox;
			this.IteratorColorDragPanel.Value = 0D;
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
			this.mIteratorPropertyPanel.Size = new System.Drawing.Size(299, 69);
			this.mIteratorPropertyPanel.TabIndex = 0;
			// 
			// IteratorWeightTextBox
			// 
			this.IteratorWeightTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorWeightTextBox.Location = new System.Drawing.Point(113, 47);
			this.IteratorWeightTextBox.Name = "IteratorWeightTextBox";
			this.IteratorWeightTextBox.Size = new System.Drawing.Size(179, 20);
			this.IteratorWeightTextBox.TabIndex = 5;
			this.IteratorWeightTextBox.Text = "0.500";
			// 
			// IteratorWeightDragPanel
			// 
			this.IteratorWeightDragPanel.BackColor = System.Drawing.SystemColors.Control;
			this.IteratorWeightDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorWeightDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorWeightDragPanel.Default = 0.5D;
			this.IteratorWeightDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.IteratorWeightDragPanel.Location = new System.Drawing.Point(3, 47);
			this.IteratorWeightDragPanel.Maximum = 1000D;
			this.IteratorWeightDragPanel.Minimum = 0.001D;
			this.IteratorWeightDragPanel.Name = "IteratorWeightDragPanel";
			this.IteratorWeightDragPanel.Size = new System.Drawing.Size(110, 21);
			this.IteratorWeightDragPanel.TabIndex = 4;
			this.IteratorWeightDragPanel.Text = "Weight:";
			this.IteratorWeightDragPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IteratorWeightDragPanel.TextBox = this.IteratorWeightTextBox;
			this.IteratorWeightDragPanel.Value = 0.5D;
			// 
			// IteratorNameTextBox
			// 
			this.IteratorNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IteratorNameTextBox.Location = new System.Drawing.Point(113, 24);
			this.IteratorNameTextBox.Name = "IteratorNameTextBox";
			this.IteratorNameTextBox.Size = new System.Drawing.Size(179, 20);
			this.IteratorNameTextBox.TabIndex = 3;
			// 
			// mIteratorNameLabel
			// 
			this.mIteratorNameLabel.BackColor = System.Drawing.SystemColors.Control;
			this.mIteratorNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mIteratorNameLabel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.mIteratorNameLabel.Location = new System.Drawing.Point(3, 24);
			this.mIteratorNameLabel.Name = "mIteratorNameLabel";
			this.mIteratorNameLabel.Size = new System.Drawing.Size(110, 21);
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
			this.IteratorSelectionComboBox.Flame = null;
			this.IteratorSelectionComboBox.ForeColor = System.Drawing.Color.White;
			this.IteratorSelectionComboBox.FormattingEnabled = true;
			this.IteratorSelectionComboBox.ItemHeight = 15;
			this.IteratorSelectionComboBox.Location = new System.Drawing.Point(113, 0);
			this.IteratorSelectionComboBox.MaxDropDownItems = 20;
			this.IteratorSelectionComboBox.Name = "IteratorSelectionComboBox";
			this.IteratorSelectionComboBox.Size = new System.Drawing.Size(179, 21);
			this.IteratorSelectionComboBox.TabIndex = 1;
			// 
			// mIteratorSelectLabel
			// 
			this.mIteratorSelectLabel.BackColor = System.Drawing.SystemColors.Control;
			this.mIteratorSelectLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorSelectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mIteratorSelectLabel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.mIteratorSelectLabel.Location = new System.Drawing.Point(3, 0);
			this.mIteratorSelectLabel.Name = "mIteratorSelectLabel";
			this.mIteratorSelectLabel.Size = new System.Drawing.Size(110, 22);
			this.mIteratorSelectLabel.TabIndex = 0;
			this.mIteratorSelectLabel.Text = "Transform:";
			this.mIteratorSelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dataGridViewTextBoxColumn15
			// 
			this.dataGridViewTextBoxColumn15.HeaderText = "Name";
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn16
			// 
			this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn16.HeaderText = "Value";
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			// 
			// dataGridViewTextBoxColumn17
			// 
			this.dataGridViewTextBoxColumn17.HeaderText = "Name";
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn18
			// 
			this.dataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn18.HeaderText = "Value";
			this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			// 
			// mToolbarImages
			// 
			this.mToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mToolbarImages.ImageStream")));
			this.mToolbarImages.TransparentColor = System.Drawing.Color.Fuchsia;
			this.mToolbarImages.Images.SetKeyName(0, "ResetAll.bmp");
			this.mToolbarImages.Images.SetKeyName(1, "NewIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(2, "DuplicateIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(3, "RemoveIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(4, "Undo.bmp");
			this.mToolbarImages.Images.SetKeyName(5, "Redo.bmp");
			this.mToolbarImages.Images.SetKeyName(6, "CopyIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(7, "PasteIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(8, "SelectMode.bmp");
			this.mToolbarImages.Images.SetKeyName(9, "MoveMode.bmp");
			this.mToolbarImages.Images.SetKeyName(10, "RotateMode.bmp");
			this.mToolbarImages.Images.SetKeyName(11, "ScaleMode.bmp");
			this.mToolbarImages.Images.SetKeyName(12, "ToggleWorldPivot.bmp");
			this.mToolbarImages.Images.SetKeyName(13, "FlipAllHorizontal.bmp");
			this.mToolbarImages.Images.SetKeyName(14, "FlipAllVertical.bmp");
			this.mToolbarImages.Images.SetKeyName(15, "ToggleVariationPrevie.bmp");
			this.mToolbarImages.Images.SetKeyName(16, "TogglePostMatrix.bmp");
			this.mToolbarImages.Images.SetKeyName(17, "ToggleFinalIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(18, "LinkIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(19, "LockTransformAxes.bmp");
			this.mToolbarImages.Images.SetKeyName(20, "ResetIterator.bmp");
			this.mToolbarImages.Images.SetKeyName(21, "ResetIteratorOrigin.bmp");
			this.mToolbarImages.Images.SetKeyName(22, "ResetIteratorAngle.bmp");
			this.mToolbarImages.Images.SetKeyName(23, "ResetIteratorScale.bmp");
			this.mToolbarImages.Images.SetKeyName(24, "ToggleAutoZoom.bmp");
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Name";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.HeaderText = "Value";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.HeaderText = "Name";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.HeaderText = "Value";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.HeaderText = "Name";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn6.HeaderText = "Value";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.HeaderText = "Name";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn8.HeaderText = "Value";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.HeaderText = "Name";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn10.HeaderText = "Value";
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			// 
			// dataGridViewTextBoxColumn11
			// 
			this.dataGridViewTextBoxColumn11.HeaderText = "Name";
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn12
			// 
			this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn12.HeaderText = "Value";
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			// 
			// dataGridViewTextBoxColumn13
			// 
			this.dataGridViewTextBoxColumn13.HeaderText = "Name";
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn14
			// 
			this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn14.HeaderText = "Value";
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			// 
			// dataGridViewTextBoxColumn19
			// 
			this.dataGridViewTextBoxColumn19.HeaderText = "Name";
			this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			this.dataGridViewTextBoxColumn19.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn20
			// 
			this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn20.HeaderText = "Value";
			this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			// 
			// dataGridViewTextBoxColumn21
			// 
			this.dataGridViewTextBoxColumn21.HeaderText = "Name";
			this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
			this.dataGridViewTextBoxColumn21.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn22
			// 
			this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn22.HeaderText = "Value";
			this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1014, 661);
			this.Controls.Add(this.mRootSplitter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(800, 700);
			this.Name = "Editor";
			this.Text = "Editor";
			this.mRootSplitter.Panel1.ResumeLayout(false);
			this.mRootSplitter.Panel1.PerformLayout();
			this.mRootSplitter.Panel2.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.mToolbar.ResumeLayout(false);
			this.mToolbar.PerformLayout();
			this.mSidebarSplitter.Panel1.ResumeLayout(false);
			this.mSidebarSplitter.Panel2.ResumeLayout(false);
			this.mSidebarSplitter.ResumeLayout(false);
			this.mPreviewPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).EndInit();
			this.mPreviewContextMenu.ResumeLayout(false);
			this.Tabs.ResumeLayout(false);
			this.VariationsTab.ResumeLayout(false);
			this.VariationsTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.VariationsGrid)).EndInit();
			this.VariablesTab.ResumeLayout(false);
			this.VariablesTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.VariablesGrid)).EndInit();
			this.PointTab.ResumeLayout(false);
			this.mIteratorControlsGroupBox.ResumeLayout(false);
			this.mPointCoordGroupBox.ResumeLayout(false);
			this.mPointCoordGroupBox.PerformLayout();
			this.VectorTab.ResumeLayout(false);
			this.mIteratorPostAffineGroupBox.ResumeLayout(false);
			this.mIteratorPostAffineGroupBox.PerformLayout();
			this.mPreAffineGroupBox.ResumeLayout(false);
			this.mPreAffineGroupBox.PerformLayout();
			this.ColorTab.ResumeLayout(false);
			this.mPreviewGroupBox.ResumeLayout(false);
			this.mPreviewGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewDensityTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PreviewRangeTrackBar)).EndInit();
			this.mIteratorColorGroupBox.ResumeLayout(false);
			this.mIteratorColorGroupBox.PerformLayout();
			this.mIteratorPropertyPanel.ResumeLayout(false);
			this.mIteratorPropertyPanel.PerformLayout();
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
		public Xyrus.Apophysis.Windows.Controls.IteratorSelectComboBox IteratorSelectionComboBox;
		public System.Windows.Forms.TextBox IteratorNameTextBox;
		public DragPanel IteratorWeightDragPanel;
		public System.Windows.Forms.TextBox IteratorWeightTextBox;
		public EditorCanvas IteratorCanvas;
		private System.Windows.Forms.GroupBox mIteratorColorGroupBox;
		public System.Windows.Forms.TextBox IteratorColorTextBox;
		public DragPanel IteratorColorDragPanel;
		public System.Windows.Forms.TextBox IteratorColorSpeedTextBox;
		public DragPanel IteratorColorSpeedDragPanel;
		public System.Windows.Forms.TextBox IteratorOpacityTextBox;
		public DragPanel IteratorOpacityDragPanel;
		public System.Windows.Forms.TextBox IteratorDirectColorTextBox;
		public DragPanel IteratorDirectColorDragPanel;
		public System.Windows.Forms.HScrollBar IteratorColorScrollBar;
		private System.Windows.Forms.GroupBox mPointCoordGroupBox;
		private System.Windows.Forms.GroupBox mIteratorControlsGroupBox;
		public System.Windows.Forms.Button IteratorRotate90CCW;
		public System.Windows.Forms.Button IteratorRotate90CW;
		public System.Windows.Forms.Button IteratorRotateCCW;
		public System.Windows.Forms.Button IteratorRotateCW;
		public System.Windows.Forms.ComboBox IteratorSnapAngle;
		public System.Windows.Forms.TextBox IteratorPointXyTextBox;
		public System.Windows.Forms.TextBox IteratorPointXxTextBox;
		public System.Windows.Forms.TextBox IteratorPointYyTextBox;
		public System.Windows.Forms.TextBox IteratorPointYxTextBox;
		public System.Windows.Forms.TextBox IteratorPointOyTextBox;
		public System.Windows.Forms.TextBox IteratorPointOxTextBox;
		public System.Windows.Forms.TabControl Tabs;
		public System.Windows.Forms.TabPage PointTab;
		public System.Windows.Forms.TabPage ColorTab;
		public System.Windows.Forms.Button IteratorMoveLeft;
		public System.Windows.Forms.Button IteratorMoveDown;
		public System.Windows.Forms.Button IteratorMoveRight;
		public System.Windows.Forms.Button IteratorMoveUp;
		public System.Windows.Forms.ComboBox IteratorMoveOffset;
		public System.Windows.Forms.ComboBox IteratorScaleRatio;
		public System.Windows.Forms.Button IteratorScaleUp;
		public System.Windows.Forms.Button IteratorScaleDown;
		public System.Windows.Forms.Button IteratorResetPointX;
		public System.Windows.Forms.Button IteratorResetPointO;
		public System.Windows.Forms.Button IteratorResetPointY;
		private System.Windows.Forms.GroupBox mPreAffineGroupBox;
		public System.Windows.Forms.Button IteratorResetPreAffineY;
		public System.Windows.Forms.Button IteratorResetPreAffineO;
		public System.Windows.Forms.Button IteratorResetPreAffineX;
		public System.Windows.Forms.TextBox IteratorPreAffineOyTextBox;
		public System.Windows.Forms.TextBox IteratorPreAffineOxTextBox;
		public System.Windows.Forms.TextBox IteratorPreAffineYyTextBox;
		public System.Windows.Forms.TextBox IteratorPreAffineYxTextBox;
		public System.Windows.Forms.TextBox IteratorPreAffineXyTextBox;
		public System.Windows.Forms.TextBox IteratorPreAffineXxTextBox;
		public System.Windows.Forms.Button IteratorResetPreAffine;
		public System.Windows.Forms.TabPage VectorTab;
		private System.Windows.Forms.GroupBox mIteratorPostAffineGroupBox;
		public System.Windows.Forms.Button IteratorResetPostAffine;
		public System.Windows.Forms.Button IteratorResetPostAffineY;
		public System.Windows.Forms.Button IteratorResetPostAffineO;
		public System.Windows.Forms.Button IteratorResetPostAffineX;
		public System.Windows.Forms.TextBox IteratorPostAffineOyTextBox;
		public System.Windows.Forms.TextBox IteratorPostAffineOxTextBox;
		public System.Windows.Forms.TextBox IteratorPostAffineYyTextBox;
		public System.Windows.Forms.TextBox IteratorPostAffineYxTextBox;
		public System.Windows.Forms.TextBox IteratorPostAffineXyTextBox;
		public System.Windows.Forms.TextBox IteratorPostAffineXxTextBox;
		private System.Windows.Forms.ToolStrip mToolbar;
		private System.Windows.Forms.ImageList mToolbarImages;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator2;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator1;
		public System.Windows.Forms.ToolStripButton AddIteratorButton;
		public System.Windows.Forms.ToolStripButton DuplicateIteratorButton;
		public System.Windows.Forms.ToolStripButton RemoveIteratorButton;
		public System.Windows.Forms.ToolStripButton ResetAllIteratorsButton;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator3;
		public System.Windows.Forms.ToolStripButton UndoButton;
		public System.Windows.Forms.ToolStripButton RedoButton;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator4;
		public System.Windows.Forms.ToolStripButton IteratorResetButton;
		public System.Windows.Forms.ToolStripButton IteratorResetOriginButton;
		public System.Windows.Forms.ToolStripButton IteratorResetAngleButton;
		public System.Windows.Forms.ToolStripButton IteratorResetScaleButton;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator5;
		public System.Windows.Forms.ToolStripButton Rotate90CcwButton;
		public System.Windows.Forms.ToolStripButton Rotate90CwButton;
		public System.Windows.Forms.ToolStripButton FlipAllHorizontalButton;
		public System.Windows.Forms.ToolStripButton FlipAllVerticalButton;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator6;
		public System.Windows.Forms.ToolStripButton ToggleRulersButton;
		public System.Windows.Forms.ToolStripButton ToggleVariationPreviewButton;
		public System.Windows.Forms.ToolStripButton TogglePostMatrixButton;
		public System.Windows.Forms.ToolStripButton AddFinalIteratorButton;
		private System.Windows.Forms.ToolStripSeparator mToolbarSeparator7;
		public System.Windows.Forms.ToolStripButton IteratorConvertToRegularButton;
		public System.Windows.Forms.ToolStripButton IteratorConvertToFinalButton;
		public PaletteSelectComboBox PaletteSelectComboBox;
		public System.Windows.Forms.TabPage VariationsTab;
		public System.Windows.Forms.TabPage VariablesTab;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		public DragGrid VariationsGrid;
		public System.Windows.Forms.CheckBox HideUnusedVariationsCheckBox;
		public System.Windows.Forms.Button ClearVariationsButton;
		public DragGrid VariablesGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		public System.Windows.Forms.CheckBox HideUnrelatedVariablesCheckBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private System.Windows.Forms.GroupBox mPreviewGroupBox;
		private System.Windows.Forms.Label mPreviewDensityLabel;
		private System.Windows.Forms.Label mPreviewRangeLabel;
		public System.Windows.Forms.TrackBar PreviewRangeTrackBar;
		public System.Windows.Forms.TrackBar PreviewDensityTrackBar;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
		public System.Windows.Forms.PictureBox PreviewPicture;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
		private System.Windows.Forms.ContextMenuStrip mPreviewContextMenu;
		public System.Windows.Forms.ToolStripMenuItem LowQualityMenuItem;
		public System.Windows.Forms.ToolStripMenuItem MediumQualityMenuItem;
		public System.Windows.Forms.ToolStripMenuItem HighQualityMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
	}
}

