namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Messages
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Messages));
			this.Content = new System.Windows.Forms.TextBox();
			this.mContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// Content
			// 
			resources.ApplyResources(this.Content, "Content");
			this.Content.ContextMenuStrip = this.mContextMenu;
			this.Content.Name = "Content";
			this.Content.ReadOnly = true;
			// 
			// mContextMenu
			// 
			this.mContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearMenuItem});
			this.mContextMenu.Name = "mContextMenu";
			resources.ApplyResources(this.mContextMenu, "mContextMenu");
			// 
			// ClearMenuItem
			// 
			this.ClearMenuItem.Name = "ClearMenuItem";
			resources.ApplyResources(this.ClearMenuItem, "ClearMenuItem");
			// 
			// Messages
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Content);
			this.Name = "Messages";
			this.mContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox Content;
		private System.Windows.Forms.ContextMenuStrip mContextMenu;
		public System.Windows.Forms.ToolStripMenuItem ClearMenuItem;

	}
}