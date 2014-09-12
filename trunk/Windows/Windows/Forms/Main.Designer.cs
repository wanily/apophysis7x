namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Main
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.mMainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewFlameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenBatchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RestoreAutosaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.SaveFlameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveBatchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.PaletteFromImageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.BrowsePalettesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.RandomBatchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RedoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.CopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mToolbar = new System.Windows.Forms.ToolStrip();
			this.NewFlameButton = new System.Windows.Forms.ToolStripButton();
			this.OpenBatchButton = new System.Windows.Forms.ToolStripButton();
			this.SaveFlameButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.UndoButton = new System.Windows.Forms.ToolStripButton();
			this.RedoButton = new System.Windows.Forms.ToolStripButton();
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.BatchListView = new System.Windows.Forms.ListView();
			this.BatchListNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mPreviewBevel = new System.Windows.Forms.Label();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.RenderFlameButton = new System.Windows.Forms.ToolStripButton();
			this.RenderBatchButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.BatchListViewButton = new System.Windows.Forms.ToolStripButton();
			this.BatchIconViewButton = new System.Windows.Forms.ToolStripButton();
			this.mMainMenu.SuspendLayout();
			this.mToolbar.SuspendLayout();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.Panel2.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// mMainMenu
			// 
			this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.mMainMenu.Location = new System.Drawing.Point(0, 0);
			this.mMainMenu.Name = "mMainMenu";
			this.mMainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.mMainMenu.Size = new System.Drawing.Size(1146, 24);
			this.mMainMenu.TabIndex = 0;
			this.mMainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFlameMenuItem,
            this.OpenBatchMenuItem,
            this.RestoreAutosaveMenuItem,
            this.toolStripMenuItem1,
            this.SaveFlameMenuItem,
            this.SaveBatchMenuItem,
            this.toolStripMenuItem2,
            this.PaletteFromImageMenuItem,
            this.BrowsePalettesMenuItem,
            this.toolStripMenuItem3,
            this.RandomBatchMenuItem,
            this.toolStripMenuItem4,
            this.ExitMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// NewFlameMenuItem
			// 
			this.NewFlameMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("NewFlameMenuItem.Image")));
			this.NewFlameMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.NewFlameMenuItem.Name = "NewFlameMenuItem";
			this.NewFlameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.NewFlameMenuItem.Size = new System.Drawing.Size(249, 22);
			this.NewFlameMenuItem.Text = "&New";
			// 
			// OpenBatchMenuItem
			// 
			this.OpenBatchMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("OpenBatchMenuItem.Image")));
			this.OpenBatchMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.OpenBatchMenuItem.Name = "OpenBatchMenuItem";
			this.OpenBatchMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.OpenBatchMenuItem.Size = new System.Drawing.Size(249, 22);
			this.OpenBatchMenuItem.Text = "&Open...";
			// 
			// RestoreAutosaveMenuItem
			// 
			this.RestoreAutosaveMenuItem.Name = "RestoreAutosaveMenuItem";
			this.RestoreAutosaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.A)));
			this.RestoreAutosaveMenuItem.Size = new System.Drawing.Size(249, 22);
			this.RestoreAutosaveMenuItem.Text = "&Restore last autosave";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(246, 6);
			// 
			// SaveFlameMenuItem
			// 
			this.SaveFlameMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("SaveFlameMenuItem.Image")));
			this.SaveFlameMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.SaveFlameMenuItem.Name = "SaveFlameMenuItem";
			this.SaveFlameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.SaveFlameMenuItem.Size = new System.Drawing.Size(249, 22);
			this.SaveFlameMenuItem.Text = "&Save...";
			// 
			// SaveBatchMenuItem
			// 
			this.SaveBatchMenuItem.Name = "SaveBatchMenuItem";
			this.SaveBatchMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.SaveBatchMenuItem.Size = new System.Drawing.Size(249, 22);
			this.SaveBatchMenuItem.Text = "S&ave all...";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(246, 6);
			// 
			// PaletteFromImageMenuItem
			// 
			this.PaletteFromImageMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("PaletteFromImageMenuItem.Image")));
			this.PaletteFromImageMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.PaletteFromImageMenuItem.Name = "PaletteFromImageMenuItem";
			this.PaletteFromImageMenuItem.Size = new System.Drawing.Size(249, 22);
			this.PaletteFromImageMenuItem.Text = "S&mooth palette...";
			// 
			// BrowsePalettesMenuItem
			// 
			this.BrowsePalettesMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("BrowsePalettesMenuItem.Image")));
			this.BrowsePalettesMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.BrowsePalettesMenuItem.Name = "BrowsePalettesMenuItem";
			this.BrowsePalettesMenuItem.Size = new System.Drawing.Size(249, 22);
			this.BrowsePalettesMenuItem.Text = "&Gradient browser...";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(246, 6);
			// 
			// RandomBatchMenuItem
			// 
			this.RandomBatchMenuItem.Name = "RandomBatchMenuItem";
			this.RandomBatchMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.RandomBatchMenuItem.Size = new System.Drawing.Size(249, 22);
			this.RandomBatchMenuItem.Text = "Random &batch";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(246, 6);
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Name = "ExitMenuItem";
			this.ExitMenuItem.Size = new System.Drawing.Size(249, 22);
			this.ExitMenuItem.Text = "E&xit";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoMenuItem,
            this.RedoMenuItem,
            this.toolStripMenuItem5,
            this.CopyMenuItem,
            this.PasteMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// UndoMenuItem
			// 
			this.UndoMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("UndoMenuItem.Image")));
			this.UndoMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.UndoMenuItem.Name = "UndoMenuItem";
			this.UndoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.UndoMenuItem.Size = new System.Drawing.Size(152, 22);
			this.UndoMenuItem.Text = "&Undo";
			// 
			// RedoMenuItem
			// 
			this.RedoMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RedoMenuItem.Image")));
			this.RedoMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.RedoMenuItem.Name = "RedoMenuItem";
			this.RedoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.RedoMenuItem.Size = new System.Drawing.Size(152, 22);
			this.RedoMenuItem.Text = "&Redo";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(149, 6);
			// 
			// CopyMenuItem
			// 
			this.CopyMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("CopyMenuItem.Image")));
			this.CopyMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.CopyMenuItem.Name = "CopyMenuItem";
			this.CopyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.CopyMenuItem.Size = new System.Drawing.Size(152, 22);
			this.CopyMenuItem.Text = "&Copy";
			// 
			// PasteMenuItem
			// 
			this.PasteMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("PasteMenuItem.Image")));
			this.PasteMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.PasteMenuItem.Name = "PasteMenuItem";
			this.PasteMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.PasteMenuItem.Size = new System.Drawing.Size(152, 22);
			this.PasteMenuItem.Text = "&Paste";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditorMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// EditorMenuItem
			// 
			this.EditorMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("EditorMenuItem.Image")));
			this.EditorMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.EditorMenuItem.Name = "EditorMenuItem";
			this.EditorMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.EditorMenuItem.Size = new System.Drawing.Size(152, 22);
			this.EditorMenuItem.Text = "&Editor";
			// 
			// mToolbar
			// 
			this.mToolbar.AllowMerge = false;
			this.mToolbar.GripMargin = new System.Windows.Forms.Padding(0);
			this.mToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFlameButton,
            this.OpenBatchButton,
            this.SaveFlameButton,
            this.toolStripSeparator2,
            this.RenderFlameButton,
            this.RenderBatchButton,
            this.toolStripSeparator3,
            this.BatchListViewButton,
            this.BatchIconViewButton,
            this.toolStripSeparator1,
            this.UndoButton,
            this.RedoButton});
			this.mToolbar.Location = new System.Drawing.Point(0, 24);
			this.mToolbar.Name = "mToolbar";
			this.mToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.mToolbar.Size = new System.Drawing.Size(1146, 25);
			this.mToolbar.TabIndex = 1;
			// 
			// NewFlameButton
			// 
			this.NewFlameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.NewFlameButton.Image = ((System.Drawing.Image)(resources.GetObject("NewFlameButton.Image")));
			this.NewFlameButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.NewFlameButton.Name = "NewFlameButton";
			this.NewFlameButton.Size = new System.Drawing.Size(23, 22);
			this.NewFlameButton.Text = "toolStripButton1";
			// 
			// OpenBatchButton
			// 
			this.OpenBatchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.OpenBatchButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenBatchButton.Image")));
			this.OpenBatchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.OpenBatchButton.Name = "OpenBatchButton";
			this.OpenBatchButton.Size = new System.Drawing.Size(23, 22);
			this.OpenBatchButton.Text = "toolStripButton1";
			// 
			// SaveFlameButton
			// 
			this.SaveFlameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.SaveFlameButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveFlameButton.Image")));
			this.SaveFlameButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SaveFlameButton.Name = "SaveFlameButton";
			this.SaveFlameButton.Size = new System.Drawing.Size(23, 22);
			this.SaveFlameButton.Text = "toolStripButton1";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// UndoButton
			// 
			this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UndoButton.Image = ((System.Drawing.Image)(resources.GetObject("UndoButton.Image")));
			this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UndoButton.Name = "UndoButton";
			this.UndoButton.Size = new System.Drawing.Size(23, 22);
			this.UndoButton.Text = "toolStripButton1";
			// 
			// RedoButton
			// 
			this.RedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RedoButton.Image = ((System.Drawing.Image)(resources.GetObject("RedoButton.Image")));
			this.RedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RedoButton.Name = "RedoButton";
			this.RedoButton.Size = new System.Drawing.Size(23, 22);
			this.RedoButton.Text = "toolStripButton1";
			// 
			// mRootSplitter
			// 
			this.mRootSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mRootSplitter.Location = new System.Drawing.Point(0, 49);
			this.mRootSplitter.Name = "mRootSplitter";
			// 
			// mRootSplitter.Panel1
			// 
			this.mRootSplitter.Panel1.Controls.Add(this.BatchListView);
			// 
			// mRootSplitter.Panel2
			// 
			this.mRootSplitter.Panel2.Controls.Add(this.mPreviewBevel);
			this.mRootSplitter.Size = new System.Drawing.Size(1146, 649);
			this.mRootSplitter.SplitterDistance = 250;
			this.mRootSplitter.TabIndex = 2;
			// 
			// BatchListView
			// 
			this.BatchListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BatchListView.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.BatchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BatchListNameColumn});
			this.BatchListView.FullRowSelect = true;
			this.BatchListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.BatchListView.LabelEdit = true;
			this.BatchListView.Location = new System.Drawing.Point(3, 3);
			this.BatchListView.MultiSelect = false;
			this.BatchListView.Name = "BatchListView";
			this.BatchListView.ShowGroups = false;
			this.BatchListView.Size = new System.Drawing.Size(247, 620);
			this.BatchListView.TabIndex = 0;
			this.BatchListView.UseCompatibleStateImageBehavior = false;
			this.BatchListView.View = System.Windows.Forms.View.Details;
			this.BatchListView.Resize += new System.EventHandler(this.OnBatchListResized);
			// 
			// BatchListNameColumn
			// 
			this.BatchListNameColumn.Text = "Name";
			// 
			// mPreviewBevel
			// 
			this.mPreviewBevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mPreviewBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPreviewBevel.Location = new System.Drawing.Point(0, 3);
			this.mPreviewBevel.Name = "mPreviewBevel";
			this.mPreviewBevel.Size = new System.Drawing.Size(889, 620);
			this.mPreviewBevel.TabIndex = 0;
			// 
			// StatusBar
			// 
			this.StatusBar.Location = new System.Drawing.Point(0, 676);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.StatusBar.Size = new System.Drawing.Size(1146, 22);
			this.StatusBar.TabIndex = 3;
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// RenderFlameButton
			// 
			this.RenderFlameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RenderFlameButton.Image = ((System.Drawing.Image)(resources.GetObject("RenderFlameButton.Image")));
			this.RenderFlameButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RenderFlameButton.Name = "RenderFlameButton";
			this.RenderFlameButton.Size = new System.Drawing.Size(23, 22);
			this.RenderFlameButton.Text = "toolStripButton1";
			// 
			// RenderBatchButton
			// 
			this.RenderBatchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RenderBatchButton.Image = ((System.Drawing.Image)(resources.GetObject("RenderBatchButton.Image")));
			this.RenderBatchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RenderBatchButton.Name = "RenderBatchButton";
			this.RenderBatchButton.Size = new System.Drawing.Size(23, 22);
			this.RenderBatchButton.Text = "toolStripButton2";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// BatchListViewButton
			// 
			this.BatchListViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BatchListViewButton.Image = ((System.Drawing.Image)(resources.GetObject("BatchListViewButton.Image")));
			this.BatchListViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BatchListViewButton.Name = "BatchListViewButton";
			this.BatchListViewButton.Size = new System.Drawing.Size(23, 22);
			this.BatchListViewButton.Text = "toolStripButton3";
			// 
			// BatchIconViewButton
			// 
			this.BatchIconViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.BatchIconViewButton.Image = ((System.Drawing.Image)(resources.GetObject("BatchIconViewButton.Image")));
			this.BatchIconViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.BatchIconViewButton.Name = "BatchIconViewButton";
			this.BatchIconViewButton.Size = new System.Drawing.Size(23, 22);
			this.BatchIconViewButton.Text = "toolStripButton4";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1146, 698);
			this.Controls.Add(this.StatusBar);
			this.Controls.Add(this.mRootSplitter);
			this.Controls.Add(this.mToolbar);
			this.Controls.Add(this.mMainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mMainMenu;
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "Main";
			this.Text = "Apophysis";
			this.Load += new System.EventHandler(this.OnWindowLoaded);
			this.mMainMenu.ResumeLayout(false);
			this.mMainMenu.PerformLayout();
			this.mToolbar.ResumeLayout(false);
			this.mToolbar.PerformLayout();
			this.mRootSplitter.Panel1.ResumeLayout(false);
			this.mRootSplitter.Panel2.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mMainMenu;
		private System.Windows.Forms.ToolStrip mToolbar;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
		private System.Windows.Forms.SplitContainer mRootSplitter;
		public System.Windows.Forms.ListView BatchListView;
		private System.Windows.Forms.ColumnHeader BatchListNameColumn;
		private System.Windows.Forms.Label mPreviewBevel;
		private System.Windows.Forms.StatusStrip StatusBar;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		public System.Windows.Forms.ToolStripMenuItem UndoMenuItem;
		public System.Windows.Forms.ToolStripMenuItem RedoMenuItem;
		public System.Windows.Forms.ToolStripMenuItem CopyMenuItem;
		public System.Windows.Forms.ToolStripMenuItem PasteMenuItem;
		public System.Windows.Forms.ToolStripMenuItem NewFlameMenuItem;
		public System.Windows.Forms.ToolStripMenuItem OpenBatchMenuItem;
		public System.Windows.Forms.ToolStripMenuItem RestoreAutosaveMenuItem;
		public System.Windows.Forms.ToolStripMenuItem SaveFlameMenuItem;
		public System.Windows.Forms.ToolStripMenuItem SaveBatchMenuItem;
		public System.Windows.Forms.ToolStripMenuItem PaletteFromImageMenuItem;
		public System.Windows.Forms.ToolStripMenuItem BrowsePalettesMenuItem;
		public System.Windows.Forms.ToolStripMenuItem RandomBatchMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem EditorMenuItem;
		public System.Windows.Forms.ToolStripButton NewFlameButton;
		public System.Windows.Forms.ToolStripButton OpenBatchButton;
		public System.Windows.Forms.ToolStripButton SaveFlameButton;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		public System.Windows.Forms.ToolStripButton UndoButton;
		public System.Windows.Forms.ToolStripButton RedoButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		public System.Windows.Forms.ToolStripButton RenderFlameButton;
		public System.Windows.Forms.ToolStripButton RenderBatchButton;
		public System.Windows.Forms.ToolStripButton BatchListViewButton;
		public System.Windows.Forms.ToolStripButton BatchIconViewButton;
	}
}