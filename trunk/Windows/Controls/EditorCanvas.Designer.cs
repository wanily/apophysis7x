namespace Xyrus.Apophysis.Windows.Controls
{
	partial class EditorCanvas
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// EditorCanvas
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Black;
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "EditorCanvas";
			this.Size = new System.Drawing.Size(175, 173);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnCanvasPaint);
			this.Resize += new System.EventHandler(this.OnCanvasResized);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
