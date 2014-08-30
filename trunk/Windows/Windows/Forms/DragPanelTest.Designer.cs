namespace Xyrus.Apophysis.Windows.Forms
{
	partial class DragPanelTest
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
			this.mPanel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// mPanel
			// 
			this.mPanel.BackColor = System.Drawing.SystemColors.Control;
			this.mPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPanel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.mPanel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.mPanel.Location = new System.Drawing.Point(44, 113);
			this.mPanel.Name = "mPanel";
			this.mPanel.Size = new System.Drawing.Size(89, 20);
			this.mPanel.TabIndex = 0;
			this.mPanel.Text = "Test:";
			this.mPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.mPanel.TextBox = this.textBox1;
			this.mPanel.Value = 0D;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(130, 113);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 1;
			// 
			// DragPanelTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.mPanel);
			this.Name = "DragPanelTest";
			this.Text = "DragPanelTest";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Controls.DragPanel mPanel;
		private System.Windows.Forms.TextBox textBox1;
	}
}