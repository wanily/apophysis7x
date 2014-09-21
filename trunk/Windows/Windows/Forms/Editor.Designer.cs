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
			this.mPreviewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.LowQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MediumQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HighQualityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
			this.IteratorCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.mSettings = new Xyrus.Apophysis.Windows.Controls.EditorSettings();
			this.mSidebarSplitter = new System.Windows.Forms.SplitContainer();
			this.mPreviewPanel = new System.Windows.Forms.Panel();
			this.PreviewPicture = new System.Windows.Forms.PictureBox();
			this.mPictureBevel = new System.Windows.Forms.Label();
			this.Tabs = new System.Windows.Forms.TabControl();
			this.VariationsTab = new System.Windows.Forms.TabPage();
			this.ClearVariationsButton = new System.Windows.Forms.Button();
			this.HideUnusedVariationsCheckBox = new System.Windows.Forms.CheckBox();
			this.VariationsGrid = new Xyrus.Apophysis.Windows.Controls.DragGrid();
			this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.VariablesTab = new System.Windows.Forms.TabPage();
			this.HideUnrelatedVariablesCheckBox = new System.Windows.Forms.CheckBox();
			this.VariablesGrid = new Xyrus.Apophysis.Windows.Controls.DragGrid();
			this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
			this.ApplyPostTransformToVariationPreviewCheckBox = new System.Windows.Forms.CheckBox();
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
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.Panel2.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.mToolbar.SuspendLayout();
			this.mPreviewContextMenu.SuspendLayout();
			this.mSidebarSplitter.Panel1.SuspendLayout();
			this.mSidebarSplitter.Panel2.SuspendLayout();
			this.mSidebarSplitter.SuspendLayout();
			this.mPreviewPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).BeginInit();
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
			resources.ApplyResources(this.mRootSplitter, "mRootSplitter");
			this.mRootSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
			resources.ApplyResources(this.mToolbar, "mToolbar");
			this.mToolbar.Name = "mToolbar";
			this.mToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			// 
			// ResetAllIteratorsButton
			// 
			this.ResetAllIteratorsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.ResetAllIteratorsButton, "ResetAllIteratorsButton");
			this.ResetAllIteratorsButton.Name = "ResetAllIteratorsButton";
			// 
			// mToolbarSeparator1
			// 
			this.mToolbarSeparator1.Name = "mToolbarSeparator1";
			resources.ApplyResources(this.mToolbarSeparator1, "mToolbarSeparator1");
			// 
			// AddIteratorButton
			// 
			this.AddIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.AddIteratorButton, "AddIteratorButton");
			this.AddIteratorButton.Name = "AddIteratorButton";
			// 
			// DuplicateIteratorButton
			// 
			this.DuplicateIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.DuplicateIteratorButton, "DuplicateIteratorButton");
			this.DuplicateIteratorButton.Name = "DuplicateIteratorButton";
			// 
			// RemoveIteratorButton
			// 
			this.RemoveIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.RemoveIteratorButton, "RemoveIteratorButton");
			this.RemoveIteratorButton.Name = "RemoveIteratorButton";
			// 
			// mToolbarSeparator2
			// 
			this.mToolbarSeparator2.Name = "mToolbarSeparator2";
			resources.ApplyResources(this.mToolbarSeparator2, "mToolbarSeparator2");
			// 
			// UndoButton
			// 
			this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.UndoButton, "UndoButton");
			this.UndoButton.Name = "UndoButton";
			// 
			// RedoButton
			// 
			this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.RedoButton, "RedoButton");
			this.RedoButton.Name = "RedoButton";
			// 
			// mToolbarSeparator3
			// 
			this.mToolbarSeparator3.Name = "mToolbarSeparator3";
			resources.ApplyResources(this.mToolbarSeparator3, "mToolbarSeparator3");
			// 
			// IteratorResetButton
			// 
			this.IteratorResetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.IteratorResetButton, "IteratorResetButton");
			this.IteratorResetButton.Name = "IteratorResetButton";
			// 
			// IteratorResetOriginButton
			// 
			this.IteratorResetOriginButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.IteratorResetOriginButton, "IteratorResetOriginButton");
			this.IteratorResetOriginButton.Name = "IteratorResetOriginButton";
			// 
			// IteratorResetAngleButton
			// 
			this.IteratorResetAngleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.IteratorResetAngleButton, "IteratorResetAngleButton");
			this.IteratorResetAngleButton.Name = "IteratorResetAngleButton";
			// 
			// IteratorResetScaleButton
			// 
			this.IteratorResetScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.IteratorResetScaleButton, "IteratorResetScaleButton");
			this.IteratorResetScaleButton.Name = "IteratorResetScaleButton";
			// 
			// mToolbarSeparator4
			// 
			this.mToolbarSeparator4.Name = "mToolbarSeparator4";
			resources.ApplyResources(this.mToolbarSeparator4, "mToolbarSeparator4");
			// 
			// Rotate90CcwButton
			// 
			this.Rotate90CcwButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.Rotate90CcwButton, "Rotate90CcwButton");
			this.Rotate90CcwButton.Name = "Rotate90CcwButton";
			// 
			// Rotate90CwButton
			// 
			this.Rotate90CwButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.Rotate90CwButton, "Rotate90CwButton");
			this.Rotate90CwButton.Name = "Rotate90CwButton";
			// 
			// FlipAllHorizontalButton
			// 
			this.FlipAllHorizontalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.FlipAllHorizontalButton, "FlipAllHorizontalButton");
			this.FlipAllHorizontalButton.Name = "FlipAllHorizontalButton";
			// 
			// FlipAllVerticalButton
			// 
			this.FlipAllVerticalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.FlipAllVerticalButton, "FlipAllVerticalButton");
			this.FlipAllVerticalButton.Name = "FlipAllVerticalButton";
			// 
			// mToolbarSeparator5
			// 
			this.mToolbarSeparator5.Name = "mToolbarSeparator5";
			resources.ApplyResources(this.mToolbarSeparator5, "mToolbarSeparator5");
			// 
			// ToggleRulersButton
			// 
			this.ToggleRulersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.ToggleRulersButton, "ToggleRulersButton");
			this.ToggleRulersButton.Name = "ToggleRulersButton";
			// 
			// ToggleVariationPreviewButton
			// 
			this.ToggleVariationPreviewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.ToggleVariationPreviewButton, "ToggleVariationPreviewButton");
			this.ToggleVariationPreviewButton.Name = "ToggleVariationPreviewButton";
			// 
			// mToolbarSeparator6
			// 
			this.mToolbarSeparator6.Name = "mToolbarSeparator6";
			resources.ApplyResources(this.mToolbarSeparator6, "mToolbarSeparator6");
			// 
			// TogglePostMatrixButton
			// 
			this.TogglePostMatrixButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.TogglePostMatrixButton, "TogglePostMatrixButton");
			this.TogglePostMatrixButton.Name = "TogglePostMatrixButton";
			// 
			// AddFinalIteratorButton
			// 
			this.AddFinalIteratorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.AddFinalIteratorButton, "AddFinalIteratorButton");
			this.AddFinalIteratorButton.Name = "AddFinalIteratorButton";
			// 
			// mToolbarSeparator7
			// 
			this.mToolbarSeparator7.Name = "mToolbarSeparator7";
			resources.ApplyResources(this.mToolbarSeparator7, "mToolbarSeparator7");
			// 
			// IteratorConvertToRegularButton
			// 
			this.IteratorConvertToRegularButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.IteratorConvertToRegularButton, "IteratorConvertToRegularButton");
			this.IteratorConvertToRegularButton.Name = "IteratorConvertToRegularButton";
			// 
			// IteratorConvertToFinalButton
			// 
			this.IteratorConvertToFinalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.IteratorConvertToFinalButton, "IteratorConvertToFinalButton");
			this.IteratorConvertToFinalButton.Name = "IteratorConvertToFinalButton";
			// 
			// mPreviewContextMenu
			// 
			this.mPreviewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LowQualityMenuItem,
            this.MediumQualityMenuItem,
            this.HighQualityMenuItem});
			this.mPreviewContextMenu.Name = "mPreviewContextMenu";
			this.mPreviewContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			resources.ApplyResources(this.mPreviewContextMenu, "mPreviewContextMenu");
			// 
			// LowQualityMenuItem
			// 
			this.LowQualityMenuItem.Name = "LowQualityMenuItem";
			resources.ApplyResources(this.LowQualityMenuItem, "LowQualityMenuItem");
			// 
			// MediumQualityMenuItem
			// 
			this.MediumQualityMenuItem.Name = "MediumQualityMenuItem";
			resources.ApplyResources(this.MediumQualityMenuItem, "MediumQualityMenuItem");
			// 
			// HighQualityMenuItem
			// 
			this.HighQualityMenuItem.Name = "HighQualityMenuItem";
			resources.ApplyResources(this.HighQualityMenuItem, "HighQualityMenuItem");
			// 
			// dataGridViewTextBoxColumn27
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn27, "dataGridViewTextBoxColumn27");
			this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
			this.dataGridViewTextBoxColumn27.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn28
			// 
			this.dataGridViewTextBoxColumn28.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn28, "dataGridViewTextBoxColumn28");
			this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
			// 
			// dataGridViewTextBoxColumn29
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn29, "dataGridViewTextBoxColumn29");
			this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
			this.dataGridViewTextBoxColumn29.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn30
			// 
			this.dataGridViewTextBoxColumn30.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn30, "dataGridViewTextBoxColumn30");
			this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
			// 
			// dataGridViewTextBoxColumn23
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn23, "dataGridViewTextBoxColumn23");
			this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
			this.dataGridViewTextBoxColumn23.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn24
			// 
			this.dataGridViewTextBoxColumn24.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn24, "dataGridViewTextBoxColumn24");
			this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
			// 
			// dataGridViewTextBoxColumn25
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn25, "dataGridViewTextBoxColumn25");
			this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
			this.dataGridViewTextBoxColumn25.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn26
			// 
			this.dataGridViewTextBoxColumn26.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn26, "dataGridViewTextBoxColumn26");
			this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
			// 
			// dataGridViewTextBoxColumn15
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
			this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			this.dataGridViewTextBoxColumn15.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn16
			// 
			this.dataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
			this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			// 
			// dataGridViewTextBoxColumn17
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn17, "dataGridViewTextBoxColumn17");
			this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			this.dataGridViewTextBoxColumn17.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn18
			// 
			this.dataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn18, "dataGridViewTextBoxColumn18");
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
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			// 
			// dataGridViewTextBoxColumn5
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			// 
			// dataGridViewTextBoxColumn7
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			// 
			// dataGridViewTextBoxColumn9
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			// 
			// dataGridViewTextBoxColumn11
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn12
			// 
			this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			// 
			// dataGridViewTextBoxColumn13
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn14
			// 
			this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			// 
			// dataGridViewTextBoxColumn19
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn19, "dataGridViewTextBoxColumn19");
			this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			this.dataGridViewTextBoxColumn19.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn20
			// 
			this.dataGridViewTextBoxColumn20.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn20, "dataGridViewTextBoxColumn20");
			this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			// 
			// dataGridViewTextBoxColumn21
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn21, "dataGridViewTextBoxColumn21");
			this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
			this.dataGridViewTextBoxColumn21.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn22
			// 
			this.dataGridViewTextBoxColumn22.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn22, "dataGridViewTextBoxColumn22");
			this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
			// 
			// IteratorCanvas
			// 
			this.IteratorCanvas.ActiveMatrix = Xyrus.Apophysis.Windows.Controls.IteratorMatrix.PreAffine;
			resources.ApplyResources(this.IteratorCanvas, "IteratorCanvas");
			this.IteratorCanvas.BackColor = System.Drawing.Color.Black;
			this.IteratorCanvas.BackdropColor = System.Drawing.Color.Transparent;
			this.IteratorCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorCanvas.ForeColor = System.Drawing.Color.White;
			this.IteratorCanvas.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.IteratorCanvas.GridZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.IteratorCanvas.HighlightOrigin = true;
			this.IteratorCanvas.Iterators = null;
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
			resources.ApplyResources(this.mSidebarSplitter, "mSidebarSplitter");
			this.mSidebarSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.mSidebarSplitter.Name = "mSidebarSplitter";
			// 
			// mSidebarSplitter.Panel1
			// 
			this.mSidebarSplitter.Panel1.Controls.Add(this.mPreviewPanel);
			// 
			// mSidebarSplitter.Panel2
			// 
			this.mSidebarSplitter.Panel2.Controls.Add(this.Tabs);
			this.mSidebarSplitter.Panel2.Controls.Add(this.mIteratorPropertyPanel);
			// 
			// mPreviewPanel
			// 
			this.mPreviewPanel.Controls.Add(this.PreviewPicture);
			this.mPreviewPanel.Controls.Add(this.mPictureBevel);
			resources.ApplyResources(this.mPreviewPanel, "mPreviewPanel");
			this.mPreviewPanel.Name = "mPreviewPanel";
			// 
			// PreviewPicture
			// 
			resources.ApplyResources(this.PreviewPicture, "PreviewPicture");
			this.PreviewPicture.BackColor = System.Drawing.SystemColors.Control;
			this.PreviewPicture.ContextMenuStrip = this.mPreviewContextMenu;
			this.PreviewPicture.Name = "PreviewPicture";
			this.PreviewPicture.TabStop = false;
			// 
			// mPictureBevel
			// 
			resources.ApplyResources(this.mPictureBevel, "mPictureBevel");
			this.mPictureBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBevel.Name = "mPictureBevel";
			// 
			// Tabs
			// 
			resources.ApplyResources(this.Tabs, "Tabs");
			this.Tabs.Controls.Add(this.VariationsTab);
			this.Tabs.Controls.Add(this.VariablesTab);
			this.Tabs.Controls.Add(this.PointTab);
			this.Tabs.Controls.Add(this.VectorTab);
			this.Tabs.Controls.Add(this.ColorTab);
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			// 
			// VariationsTab
			// 
			this.VariationsTab.Controls.Add(this.ClearVariationsButton);
			this.VariationsTab.Controls.Add(this.HideUnusedVariationsCheckBox);
			this.VariationsTab.Controls.Add(this.VariationsGrid);
			resources.ApplyResources(this.VariationsTab, "VariationsTab");
			this.VariationsTab.Name = "VariationsTab";
			this.VariationsTab.UseVisualStyleBackColor = true;
			// 
			// ClearVariationsButton
			// 
			resources.ApplyResources(this.ClearVariationsButton, "ClearVariationsButton");
			this.ClearVariationsButton.Name = "ClearVariationsButton";
			this.ClearVariationsButton.UseVisualStyleBackColor = true;
			// 
			// HideUnusedVariationsCheckBox
			// 
			resources.ApplyResources(this.HideUnusedVariationsCheckBox, "HideUnusedVariationsCheckBox");
			this.HideUnusedVariationsCheckBox.Name = "HideUnusedVariationsCheckBox";
			this.HideUnusedVariationsCheckBox.UseVisualStyleBackColor = true;
			// 
			// VariationsGrid
			// 
			this.VariationsGrid.AllowUserToAddRows = false;
			this.VariationsGrid.AllowUserToDeleteRows = false;
			this.VariationsGrid.AllowUserToOrderColumns = true;
			this.VariationsGrid.AllowUserToResizeRows = false;
			resources.ApplyResources(this.VariationsGrid, "VariationsGrid");
			this.VariationsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.VariationsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.VariationsGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.VariationsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.VariationsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.VariationsGrid.EnableHeadersVisualStyles = false;
			this.VariationsGrid.GridColor = System.Drawing.SystemColors.ControlLight;
			this.VariationsGrid.MultiSelect = false;
			this.VariationsGrid.Name = "VariationsGrid";
			this.VariationsGrid.ResetMode = Xyrus.Apophysis.Windows.Controls.DragGridResetMode.Toggle;
			this.VariationsGrid.RowHeadersVisible = false;
			this.VariationsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.VariationsGrid.ShowCellErrors = false;
			this.VariationsGrid.ShowCellToolTips = false;
			this.VariationsGrid.ShowEditingIcon = false;
			this.VariationsGrid.ShowRowErrors = false;
			// 
			// dataGridViewTextBoxColumn31
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn31, "dataGridViewTextBoxColumn31");
			this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
			this.dataGridViewTextBoxColumn31.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn32
			// 
			this.dataGridViewTextBoxColumn32.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn32, "dataGridViewTextBoxColumn32");
			this.dataGridViewTextBoxColumn32.Name = "dataGridViewTextBoxColumn32";
			// 
			// VariablesTab
			// 
			this.VariablesTab.Controls.Add(this.HideUnrelatedVariablesCheckBox);
			this.VariablesTab.Controls.Add(this.VariablesGrid);
			resources.ApplyResources(this.VariablesTab, "VariablesTab");
			this.VariablesTab.Name = "VariablesTab";
			this.VariablesTab.UseVisualStyleBackColor = true;
			// 
			// HideUnrelatedVariablesCheckBox
			// 
			resources.ApplyResources(this.HideUnrelatedVariablesCheckBox, "HideUnrelatedVariablesCheckBox");
			this.HideUnrelatedVariablesCheckBox.Checked = true;
			this.HideUnrelatedVariablesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HideUnrelatedVariablesCheckBox.Name = "HideUnrelatedVariablesCheckBox";
			this.HideUnrelatedVariablesCheckBox.UseVisualStyleBackColor = true;
			// 
			// VariablesGrid
			// 
			this.VariablesGrid.AllowUserToAddRows = false;
			this.VariablesGrid.AllowUserToDeleteRows = false;
			this.VariablesGrid.AllowUserToOrderColumns = true;
			this.VariablesGrid.AllowUserToResizeRows = false;
			resources.ApplyResources(this.VariablesGrid, "VariablesGrid");
			this.VariablesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.VariablesGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.VariablesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.VariablesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.VariablesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.VariablesGrid.EnableHeadersVisualStyles = false;
			this.VariablesGrid.GridColor = System.Drawing.SystemColors.ControlLight;
			this.VariablesGrid.MultiSelect = false;
			this.VariablesGrid.Name = "VariablesGrid";
			this.VariablesGrid.ResetMode = Xyrus.Apophysis.Windows.Controls.DragGridResetMode.Override;
			this.VariablesGrid.RowHeadersVisible = false;
			this.VariablesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.VariablesGrid.ShowCellErrors = false;
			this.VariablesGrid.ShowCellToolTips = false;
			this.VariablesGrid.ShowEditingIcon = false;
			this.VariablesGrid.ShowRowErrors = false;
			// 
			// dataGridViewTextBoxColumn33
			// 
			resources.ApplyResources(this.dataGridViewTextBoxColumn33, "dataGridViewTextBoxColumn33");
			this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
			this.dataGridViewTextBoxColumn33.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn34
			// 
			this.dataGridViewTextBoxColumn34.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn34, "dataGridViewTextBoxColumn34");
			this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
			// 
			// PointTab
			// 
			this.PointTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.PointTab.Controls.Add(this.mIteratorControlsGroupBox);
			this.PointTab.Controls.Add(this.mPointCoordGroupBox);
			resources.ApplyResources(this.PointTab, "PointTab");
			this.PointTab.Name = "PointTab";
			// 
			// mIteratorControlsGroupBox
			// 
			resources.ApplyResources(this.mIteratorControlsGroupBox, "mIteratorControlsGroupBox");
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
			this.mIteratorControlsGroupBox.Name = "mIteratorControlsGroupBox";
			this.mIteratorControlsGroupBox.TabStop = false;
			// 
			// IteratorScaleRatio
			// 
			this.IteratorScaleRatio.FormattingEnabled = true;
			resources.ApplyResources(this.IteratorScaleRatio, "IteratorScaleRatio");
			this.IteratorScaleRatio.Name = "IteratorScaleRatio";
			// 
			// IteratorScaleUp
			// 
			this.IteratorScaleUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorScaleUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorScaleUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorScaleUp, "IteratorScaleUp");
			this.IteratorScaleUp.Name = "IteratorScaleUp";
			this.IteratorScaleUp.UseVisualStyleBackColor = true;
			// 
			// IteratorScaleDown
			// 
			this.IteratorScaleDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorScaleDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorScaleDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorScaleDown, "IteratorScaleDown");
			this.IteratorScaleDown.Name = "IteratorScaleDown";
			this.IteratorScaleDown.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveOffset
			// 
			this.IteratorMoveOffset.FormattingEnabled = true;
			resources.ApplyResources(this.IteratorMoveOffset, "IteratorMoveOffset");
			this.IteratorMoveOffset.Name = "IteratorMoveOffset";
			// 
			// IteratorMoveLeft
			// 
			this.IteratorMoveLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveLeft.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveLeft.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorMoveLeft, "IteratorMoveLeft");
			this.IteratorMoveLeft.Name = "IteratorMoveLeft";
			this.IteratorMoveLeft.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveDown
			// 
			this.IteratorMoveDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveDown.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveDown.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorMoveDown, "IteratorMoveDown");
			this.IteratorMoveDown.Name = "IteratorMoveDown";
			this.IteratorMoveDown.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveRight
			// 
			this.IteratorMoveRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveRight.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveRight.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorMoveRight, "IteratorMoveRight");
			this.IteratorMoveRight.Name = "IteratorMoveRight";
			this.IteratorMoveRight.UseVisualStyleBackColor = true;
			// 
			// IteratorMoveUp
			// 
			this.IteratorMoveUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorMoveUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorMoveUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorMoveUp, "IteratorMoveUp");
			this.IteratorMoveUp.Name = "IteratorMoveUp";
			this.IteratorMoveUp.UseVisualStyleBackColor = true;
			// 
			// IteratorSnapAngle
			// 
			this.IteratorSnapAngle.FormattingEnabled = true;
			resources.ApplyResources(this.IteratorSnapAngle, "IteratorSnapAngle");
			this.IteratorSnapAngle.Name = "IteratorSnapAngle";
			this.IteratorSnapAngle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorRotateCW
			// 
			this.IteratorRotateCW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotateCW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotateCW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorRotateCW, "IteratorRotateCW");
			this.IteratorRotateCW.Name = "IteratorRotateCW";
			this.IteratorRotateCW.UseVisualStyleBackColor = true;
			// 
			// IteratorRotateCCW
			// 
			this.IteratorRotateCCW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotateCCW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotateCCW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorRotateCCW, "IteratorRotateCCW");
			this.IteratorRotateCCW.Name = "IteratorRotateCCW";
			this.IteratorRotateCCW.UseVisualStyleBackColor = true;
			// 
			// IteratorRotate90CW
			// 
			this.IteratorRotate90CW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotate90CW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotate90CW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorRotate90CW, "IteratorRotate90CW");
			this.IteratorRotate90CW.Name = "IteratorRotate90CW";
			this.IteratorRotate90CW.UseVisualStyleBackColor = true;
			// 
			// IteratorRotate90CCW
			// 
			this.IteratorRotate90CCW.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorRotate90CCW.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorRotate90CCW.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorRotate90CCW, "IteratorRotate90CCW");
			this.IteratorRotate90CCW.Name = "IteratorRotate90CCW";
			this.IteratorRotate90CCW.UseVisualStyleBackColor = true;
			// 
			// mPointCoordGroupBox
			// 
			resources.ApplyResources(this.mPointCoordGroupBox, "mPointCoordGroupBox");
			this.mPointCoordGroupBox.Controls.Add(this.IteratorResetPointY);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorResetPointO);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorResetPointX);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointOyTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointOxTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointYyTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointYxTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointXyTextBox);
			this.mPointCoordGroupBox.Controls.Add(this.IteratorPointXxTextBox);
			this.mPointCoordGroupBox.Name = "mPointCoordGroupBox";
			this.mPointCoordGroupBox.TabStop = false;
			// 
			// IteratorResetPointY
			// 
			this.IteratorResetPointY.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPointY.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPointY.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPointY, "IteratorResetPointY");
			this.IteratorResetPointY.Name = "IteratorResetPointY";
			this.IteratorResetPointY.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPointO
			// 
			this.IteratorResetPointO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPointO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPointO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPointO, "IteratorResetPointO");
			this.IteratorResetPointO.Name = "IteratorResetPointO";
			this.IteratorResetPointO.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPointX
			// 
			this.IteratorResetPointX.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPointX.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPointX.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPointX, "IteratorResetPointX");
			this.IteratorResetPointX.Name = "IteratorResetPointX";
			this.IteratorResetPointX.UseVisualStyleBackColor = true;
			// 
			// IteratorPointOyTextBox
			// 
			resources.ApplyResources(this.IteratorPointOyTextBox, "IteratorPointOyTextBox");
			this.IteratorPointOyTextBox.Name = "IteratorPointOyTextBox";
			this.IteratorPointOyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointOxTextBox
			// 
			resources.ApplyResources(this.IteratorPointOxTextBox, "IteratorPointOxTextBox");
			this.IteratorPointOxTextBox.Name = "IteratorPointOxTextBox";
			this.IteratorPointOxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointYyTextBox
			// 
			resources.ApplyResources(this.IteratorPointYyTextBox, "IteratorPointYyTextBox");
			this.IteratorPointYyTextBox.Name = "IteratorPointYyTextBox";
			this.IteratorPointYyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointYxTextBox
			// 
			resources.ApplyResources(this.IteratorPointYxTextBox, "IteratorPointYxTextBox");
			this.IteratorPointYxTextBox.Name = "IteratorPointYxTextBox";
			this.IteratorPointYxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointXyTextBox
			// 
			resources.ApplyResources(this.IteratorPointXyTextBox, "IteratorPointXyTextBox");
			this.IteratorPointXyTextBox.Name = "IteratorPointXyTextBox";
			this.IteratorPointXyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPointXxTextBox
			// 
			resources.ApplyResources(this.IteratorPointXxTextBox, "IteratorPointXxTextBox");
			this.IteratorPointXxTextBox.Name = "IteratorPointXxTextBox";
			this.IteratorPointXxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// VectorTab
			// 
			this.VectorTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.VectorTab.Controls.Add(this.mIteratorPostAffineGroupBox);
			this.VectorTab.Controls.Add(this.mPreAffineGroupBox);
			resources.ApplyResources(this.VectorTab, "VectorTab");
			this.VectorTab.Name = "VectorTab";
			// 
			// mIteratorPostAffineGroupBox
			// 
			resources.ApplyResources(this.mIteratorPostAffineGroupBox, "mIteratorPostAffineGroupBox");
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
			this.mIteratorPostAffineGroupBox.Name = "mIteratorPostAffineGroupBox";
			this.mIteratorPostAffineGroupBox.TabStop = false;
			// 
			// IteratorResetPostAffine
			// 
			this.IteratorResetPostAffine.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffine.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffine.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPostAffine, "IteratorResetPostAffine");
			this.IteratorResetPostAffine.Name = "IteratorResetPostAffine";
			this.IteratorResetPostAffine.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPostAffineY
			// 
			this.IteratorResetPostAffineY.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffineY.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffineY.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPostAffineY, "IteratorResetPostAffineY");
			this.IteratorResetPostAffineY.Name = "IteratorResetPostAffineY";
			this.IteratorResetPostAffineY.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPostAffineO
			// 
			this.IteratorResetPostAffineO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffineO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffineO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPostAffineO, "IteratorResetPostAffineO");
			this.IteratorResetPostAffineO.Name = "IteratorResetPostAffineO";
			this.IteratorResetPostAffineO.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPostAffineX
			// 
			this.IteratorResetPostAffineX.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPostAffineX.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPostAffineX.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPostAffineX, "IteratorResetPostAffineX");
			this.IteratorResetPostAffineX.Name = "IteratorResetPostAffineX";
			this.IteratorResetPostAffineX.UseVisualStyleBackColor = true;
			// 
			// IteratorPostAffineOyTextBox
			// 
			resources.ApplyResources(this.IteratorPostAffineOyTextBox, "IteratorPostAffineOyTextBox");
			this.IteratorPostAffineOyTextBox.Name = "IteratorPostAffineOyTextBox";
			// 
			// IteratorPostAffineOxTextBox
			// 
			resources.ApplyResources(this.IteratorPostAffineOxTextBox, "IteratorPostAffineOxTextBox");
			this.IteratorPostAffineOxTextBox.Name = "IteratorPostAffineOxTextBox";
			// 
			// IteratorPostAffineYyTextBox
			// 
			resources.ApplyResources(this.IteratorPostAffineYyTextBox, "IteratorPostAffineYyTextBox");
			this.IteratorPostAffineYyTextBox.Name = "IteratorPostAffineYyTextBox";
			// 
			// IteratorPostAffineYxTextBox
			// 
			resources.ApplyResources(this.IteratorPostAffineYxTextBox, "IteratorPostAffineYxTextBox");
			this.IteratorPostAffineYxTextBox.Name = "IteratorPostAffineYxTextBox";
			// 
			// IteratorPostAffineXyTextBox
			// 
			resources.ApplyResources(this.IteratorPostAffineXyTextBox, "IteratorPostAffineXyTextBox");
			this.IteratorPostAffineXyTextBox.Name = "IteratorPostAffineXyTextBox";
			// 
			// IteratorPostAffineXxTextBox
			// 
			resources.ApplyResources(this.IteratorPostAffineXxTextBox, "IteratorPostAffineXxTextBox");
			this.IteratorPostAffineXxTextBox.Name = "IteratorPostAffineXxTextBox";
			// 
			// mPreAffineGroupBox
			// 
			resources.ApplyResources(this.mPreAffineGroupBox, "mPreAffineGroupBox");
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
			this.mPreAffineGroupBox.Name = "mPreAffineGroupBox";
			this.mPreAffineGroupBox.TabStop = false;
			// 
			// IteratorResetPreAffine
			// 
			this.IteratorResetPreAffine.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffine.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffine.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPreAffine, "IteratorResetPreAffine");
			this.IteratorResetPreAffine.Name = "IteratorResetPreAffine";
			this.IteratorResetPreAffine.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPreAffineY
			// 
			this.IteratorResetPreAffineY.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffineY.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffineY.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPreAffineY, "IteratorResetPreAffineY");
			this.IteratorResetPreAffineY.Name = "IteratorResetPreAffineY";
			this.IteratorResetPreAffineY.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPreAffineO
			// 
			this.IteratorResetPreAffineO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffineO.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffineO.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPreAffineO, "IteratorResetPreAffineO");
			this.IteratorResetPreAffineO.Name = "IteratorResetPreAffineO";
			this.IteratorResetPreAffineO.UseVisualStyleBackColor = true;
			// 
			// IteratorResetPreAffineX
			// 
			this.IteratorResetPreAffineX.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this.IteratorResetPreAffineX.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.HotTrack;
			this.IteratorResetPreAffineX.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Highlight;
			resources.ApplyResources(this.IteratorResetPreAffineX, "IteratorResetPreAffineX");
			this.IteratorResetPreAffineX.Name = "IteratorResetPreAffineX";
			this.IteratorResetPreAffineX.UseVisualStyleBackColor = true;
			// 
			// IteratorPreAffineOyTextBox
			// 
			resources.ApplyResources(this.IteratorPreAffineOyTextBox, "IteratorPreAffineOyTextBox");
			this.IteratorPreAffineOyTextBox.Name = "IteratorPreAffineOyTextBox";
			this.IteratorPreAffineOyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineOxTextBox
			// 
			resources.ApplyResources(this.IteratorPreAffineOxTextBox, "IteratorPreAffineOxTextBox");
			this.IteratorPreAffineOxTextBox.Name = "IteratorPreAffineOxTextBox";
			this.IteratorPreAffineOxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineYyTextBox
			// 
			resources.ApplyResources(this.IteratorPreAffineYyTextBox, "IteratorPreAffineYyTextBox");
			this.IteratorPreAffineYyTextBox.Name = "IteratorPreAffineYyTextBox";
			this.IteratorPreAffineYyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineYxTextBox
			// 
			resources.ApplyResources(this.IteratorPreAffineYxTextBox, "IteratorPreAffineYxTextBox");
			this.IteratorPreAffineYxTextBox.Name = "IteratorPreAffineYxTextBox";
			this.IteratorPreAffineYxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineXyTextBox
			// 
			resources.ApplyResources(this.IteratorPreAffineXyTextBox, "IteratorPreAffineXyTextBox");
			this.IteratorPreAffineXyTextBox.Name = "IteratorPreAffineXyTextBox";
			this.IteratorPreAffineXyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// IteratorPreAffineXxTextBox
			// 
			resources.ApplyResources(this.IteratorPreAffineXxTextBox, "IteratorPreAffineXxTextBox");
			this.IteratorPreAffineXxTextBox.Name = "IteratorPreAffineXxTextBox";
			this.IteratorPreAffineXxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNumericTextBoxKeyPress);
			// 
			// ColorTab
			// 
			this.ColorTab.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ColorTab.Controls.Add(this.mPreviewGroupBox);
			this.ColorTab.Controls.Add(this.mIteratorColorGroupBox);
			resources.ApplyResources(this.ColorTab, "ColorTab");
			this.ColorTab.Name = "ColorTab";
			// 
			// mPreviewGroupBox
			// 
			resources.ApplyResources(this.mPreviewGroupBox, "mPreviewGroupBox");
			this.mPreviewGroupBox.Controls.Add(this.ApplyPostTransformToVariationPreviewCheckBox);
			this.mPreviewGroupBox.Controls.Add(this.mPreviewDensityLabel);
			this.mPreviewGroupBox.Controls.Add(this.mPreviewRangeLabel);
			this.mPreviewGroupBox.Controls.Add(this.PreviewDensityTrackBar);
			this.mPreviewGroupBox.Controls.Add(this.PreviewRangeTrackBar);
			this.mPreviewGroupBox.Name = "mPreviewGroupBox";
			this.mPreviewGroupBox.TabStop = false;
			// 
			// ApplyPostTransformToVariationPreviewCheckBox
			// 
			resources.ApplyResources(this.ApplyPostTransformToVariationPreviewCheckBox, "ApplyPostTransformToVariationPreviewCheckBox");
			this.ApplyPostTransformToVariationPreviewCheckBox.Name = "ApplyPostTransformToVariationPreviewCheckBox";
			this.ApplyPostTransformToVariationPreviewCheckBox.UseVisualStyleBackColor = true;
			// 
			// mPreviewDensityLabel
			// 
			resources.ApplyResources(this.mPreviewDensityLabel, "mPreviewDensityLabel");
			this.mPreviewDensityLabel.Name = "mPreviewDensityLabel";
			// 
			// mPreviewRangeLabel
			// 
			resources.ApplyResources(this.mPreviewRangeLabel, "mPreviewRangeLabel");
			this.mPreviewRangeLabel.Name = "mPreviewRangeLabel";
			// 
			// PreviewDensityTrackBar
			// 
			resources.ApplyResources(this.PreviewDensityTrackBar, "PreviewDensityTrackBar");
			this.PreviewDensityTrackBar.Maximum = 4;
			this.PreviewDensityTrackBar.Minimum = 1;
			this.PreviewDensityTrackBar.Name = "PreviewDensityTrackBar";
			this.PreviewDensityTrackBar.Value = 1;
			// 
			// PreviewRangeTrackBar
			// 
			resources.ApplyResources(this.PreviewRangeTrackBar, "PreviewRangeTrackBar");
			this.PreviewRangeTrackBar.Maximum = 5;
			this.PreviewRangeTrackBar.Minimum = 1;
			this.PreviewRangeTrackBar.Name = "PreviewRangeTrackBar";
			this.PreviewRangeTrackBar.Value = 1;
			// 
			// mIteratorColorGroupBox
			// 
			resources.ApplyResources(this.mIteratorColorGroupBox, "mIteratorColorGroupBox");
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
			this.mIteratorColorGroupBox.Name = "mIteratorColorGroupBox";
			this.mIteratorColorGroupBox.TabStop = false;
			// 
			// PaletteSelectComboBox
			// 
			this.PaletteSelectComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.PaletteSelectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PaletteSelectComboBox.FormattingEnabled = true;
			resources.ApplyResources(this.PaletteSelectComboBox, "PaletteSelectComboBox");
			this.PaletteSelectComboBox.Name = "PaletteSelectComboBox";
			// 
			// IteratorDirectColorTextBox
			// 
			resources.ApplyResources(this.IteratorDirectColorTextBox, "IteratorDirectColorTextBox");
			this.IteratorDirectColorTextBox.Name = "IteratorDirectColorTextBox";
			// 
			// IteratorDirectColorDragPanel
			// 
			this.IteratorDirectColorDragPanel.BackColor = System.Drawing.SystemColors.Window;
			this.IteratorDirectColorDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorDirectColorDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorDirectColorDragPanel.Default = 1D;
			this.IteratorDirectColorDragPanel.DragStepping = 0.1D;
			this.IteratorDirectColorDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			resources.ApplyResources(this.IteratorDirectColorDragPanel, "IteratorDirectColorDragPanel");
			this.IteratorDirectColorDragPanel.Maximum = 1D;
			this.IteratorDirectColorDragPanel.Minimum = 0D;
			this.IteratorDirectColorDragPanel.Name = "IteratorDirectColorDragPanel";
			this.IteratorDirectColorDragPanel.TextBox = this.IteratorDirectColorTextBox;
			this.IteratorDirectColorDragPanel.Value = 1D;
			// 
			// IteratorOpacityTextBox
			// 
			resources.ApplyResources(this.IteratorOpacityTextBox, "IteratorOpacityTextBox");
			this.IteratorOpacityTextBox.Name = "IteratorOpacityTextBox";
			// 
			// IteratorOpacityDragPanel
			// 
			this.IteratorOpacityDragPanel.BackColor = System.Drawing.SystemColors.Window;
			this.IteratorOpacityDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorOpacityDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorOpacityDragPanel.Default = 1D;
			this.IteratorOpacityDragPanel.DragStepping = 0.1D;
			this.IteratorOpacityDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			resources.ApplyResources(this.IteratorOpacityDragPanel, "IteratorOpacityDragPanel");
			this.IteratorOpacityDragPanel.Maximum = 1D;
			this.IteratorOpacityDragPanel.Minimum = 0D;
			this.IteratorOpacityDragPanel.Name = "IteratorOpacityDragPanel";
			this.IteratorOpacityDragPanel.TextBox = this.IteratorOpacityTextBox;
			this.IteratorOpacityDragPanel.Value = 1D;
			// 
			// IteratorColorSpeedTextBox
			// 
			resources.ApplyResources(this.IteratorColorSpeedTextBox, "IteratorColorSpeedTextBox");
			this.IteratorColorSpeedTextBox.Name = "IteratorColorSpeedTextBox";
			// 
			// IteratorColorSpeedDragPanel
			// 
			this.IteratorColorSpeedDragPanel.BackColor = System.Drawing.SystemColors.Window;
			this.IteratorColorSpeedDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorColorSpeedDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorColorSpeedDragPanel.Default = 0D;
			this.IteratorColorSpeedDragPanel.DragStepping = 0.1D;
			this.IteratorColorSpeedDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			resources.ApplyResources(this.IteratorColorSpeedDragPanel, "IteratorColorSpeedDragPanel");
			this.IteratorColorSpeedDragPanel.Maximum = 1D;
			this.IteratorColorSpeedDragPanel.Minimum = -1D;
			this.IteratorColorSpeedDragPanel.Name = "IteratorColorSpeedDragPanel";
			this.IteratorColorSpeedDragPanel.TextBox = this.IteratorColorSpeedTextBox;
			this.IteratorColorSpeedDragPanel.Value = 0D;
			// 
			// IteratorColorScrollBar
			// 
			resources.ApplyResources(this.IteratorColorScrollBar, "IteratorColorScrollBar");
			this.IteratorColorScrollBar.LargeChange = 1;
			this.IteratorColorScrollBar.Maximum = 1000;
			this.IteratorColorScrollBar.Name = "IteratorColorScrollBar";
			// 
			// IteratorColorTextBox
			// 
			resources.ApplyResources(this.IteratorColorTextBox, "IteratorColorTextBox");
			this.IteratorColorTextBox.Name = "IteratorColorTextBox";
			// 
			// IteratorColorDragPanel
			// 
			this.IteratorColorDragPanel.BackColor = System.Drawing.Color.Black;
			this.IteratorColorDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorColorDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorColorDragPanel.Default = 0D;
			this.IteratorColorDragPanel.DragStepping = 0.1D;
			resources.ApplyResources(this.IteratorColorDragPanel, "IteratorColorDragPanel");
			this.IteratorColorDragPanel.Maximum = 1D;
			this.IteratorColorDragPanel.Minimum = 0D;
			this.IteratorColorDragPanel.Name = "IteratorColorDragPanel";
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
			resources.ApplyResources(this.mIteratorPropertyPanel, "mIteratorPropertyPanel");
			this.mIteratorPropertyPanel.Name = "mIteratorPropertyPanel";
			// 
			// IteratorWeightTextBox
			// 
			resources.ApplyResources(this.IteratorWeightTextBox, "IteratorWeightTextBox");
			this.IteratorWeightTextBox.Name = "IteratorWeightTextBox";
			// 
			// IteratorWeightDragPanel
			// 
			this.IteratorWeightDragPanel.BackColor = System.Drawing.SystemColors.Control;
			this.IteratorWeightDragPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.IteratorWeightDragPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.IteratorWeightDragPanel.Default = 0.5D;
			this.IteratorWeightDragPanel.DragStepping = 0.1D;
			this.IteratorWeightDragPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			resources.ApplyResources(this.IteratorWeightDragPanel, "IteratorWeightDragPanel");
			this.IteratorWeightDragPanel.Maximum = 1000D;
			this.IteratorWeightDragPanel.Minimum = 0.001D;
			this.IteratorWeightDragPanel.Name = "IteratorWeightDragPanel";
			this.IteratorWeightDragPanel.TextBox = this.IteratorWeightTextBox;
			this.IteratorWeightDragPanel.Value = 0.5D;
			// 
			// IteratorNameTextBox
			// 
			resources.ApplyResources(this.IteratorNameTextBox, "IteratorNameTextBox");
			this.IteratorNameTextBox.Name = "IteratorNameTextBox";
			// 
			// mIteratorNameLabel
			// 
			this.mIteratorNameLabel.BackColor = System.Drawing.SystemColors.Control;
			this.mIteratorNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mIteratorNameLabel, "mIteratorNameLabel");
			this.mIteratorNameLabel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.mIteratorNameLabel.Name = "mIteratorNameLabel";
			// 
			// IteratorSelectionComboBox
			// 
			resources.ApplyResources(this.IteratorSelectionComboBox, "IteratorSelectionComboBox");
			this.IteratorSelectionComboBox.BackColor = System.Drawing.Color.Black;
			this.IteratorSelectionComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.IteratorSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.IteratorSelectionComboBox.Flame = null;
			this.IteratorSelectionComboBox.ForeColor = System.Drawing.Color.White;
			this.IteratorSelectionComboBox.FormattingEnabled = true;
			this.IteratorSelectionComboBox.Name = "IteratorSelectionComboBox";
			// 
			// mIteratorSelectLabel
			// 
			this.mIteratorSelectLabel.BackColor = System.Drawing.SystemColors.Control;
			this.mIteratorSelectLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			resources.ApplyResources(this.mIteratorSelectLabel, "mIteratorSelectLabel");
			this.mIteratorSelectLabel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.mIteratorSelectLabel.Name = "mIteratorSelectLabel";
			// 
			// Editor
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mRootSplitter);
			this.Name = "Editor";
			this.mRootSplitter.Panel1.ResumeLayout(false);
			this.mRootSplitter.Panel1.PerformLayout();
			this.mRootSplitter.Panel2.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.mToolbar.ResumeLayout(false);
			this.mToolbar.PerformLayout();
			this.mPreviewContextMenu.ResumeLayout(false);
			this.mSidebarSplitter.Panel1.ResumeLayout(false);
			this.mSidebarSplitter.Panel2.ResumeLayout(false);
			this.mSidebarSplitter.ResumeLayout(false);
			this.mPreviewPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PreviewPicture)).EndInit();
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
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;
		public System.Windows.Forms.CheckBox ApplyPostTransformToVariationPreviewCheckBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;
	}
}

