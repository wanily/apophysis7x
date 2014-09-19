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
			this.Content = new System.Windows.Forms.TextBox();
			this.mContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// Content
			// 
			this.Content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Content.ContextMenuStrip = this.mContextMenu;
			this.Content.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Content.Location = new System.Drawing.Point(6, 6);
			this.Content.Multiline = true;
			this.Content.Name = "Content";
			this.Content.ReadOnly = true;
			this.Content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.Content.Size = new System.Drawing.Size(534, 492);
			this.Content.TabIndex = 0;
			// 
			// mContextMenu
			// 
			this.mContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearMenuItem});
			this.mContextMenu.Name = "mContextMenu";
			this.mContextMenu.Size = new System.Drawing.Size(102, 26);
			// 
			// ClearMenuItem
			// 
			this.ClearMenuItem.Name = "ClearMenuItem";
			this.ClearMenuItem.Size = new System.Drawing.Size(152, 22);
			this.ClearMenuItem.Text = "&Clear";
			// 
			// Messages
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 504);
			this.Controls.Add(this.Content);
			this.MinimumSize = new System.Drawing.Size(200, 200);
			this.Name = "Messages";
			this.Text = "Messages";
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