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
			this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mToolbar = new System.Windows.Forms.ToolStrip();
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.BatchListView = new System.Windows.Forms.ListView();
			this.BatchListNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mPreviewBevel = new System.Windows.Forms.Label();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.mMainMenu.SuspendLayout();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.Panel2.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// mMainMenu
			// 
			this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
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
            this.ExitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// ExitToolStripMenuItem
			// 
			this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
			this.ExitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
			this.ExitToolStripMenuItem.Text = "E&xit";
			// 
			// mToolbar
			// 
			this.mToolbar.AllowMerge = false;
			this.mToolbar.GripMargin = new System.Windows.Forms.Padding(0);
			this.mToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mToolbar.Location = new System.Drawing.Point(0, 24);
			this.mToolbar.Name = "mToolbar";
			this.mToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.mToolbar.Size = new System.Drawing.Size(1146, 25);
			this.mToolbar.TabIndex = 1;
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
		public System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
		private System.Windows.Forms.SplitContainer mRootSplitter;
		public System.Windows.Forms.ListView BatchListView;
		private System.Windows.Forms.ColumnHeader BatchListNameColumn;
		private System.Windows.Forms.Label mPreviewBevel;
		private System.Windows.Forms.StatusStrip StatusBar;
	}
}