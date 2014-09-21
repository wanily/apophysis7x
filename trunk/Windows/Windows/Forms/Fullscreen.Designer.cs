namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Fullscreen
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fullscreen));
			this.SuspendLayout();
			// 
			// Fullscreen
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.CornflowerBlue;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Fullscreen";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion
	}
}