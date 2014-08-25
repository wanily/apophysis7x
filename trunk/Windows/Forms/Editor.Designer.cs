namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Editor
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
			this.mCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.SuspendLayout();
			// 
			// mCanvas
			// 
			this.mCanvas.BackColor = System.Drawing.Color.Black;
			this.mCanvas.BackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mCanvas.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mCanvas.ForeColor = System.Drawing.Color.White;
			this.mCanvas.GridLineColor = System.Drawing.Color.Gray;
			this.mCanvas.GridZeroLineColor = System.Drawing.Color.Gray;
			this.mCanvas.Location = new System.Drawing.Point(0, 0);
			this.mCanvas.Name = "mCanvas";
			this.mCanvas.RulerBackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.RulerBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mCanvas.RulerGridLineColor = System.Drawing.Color.Silver;
			this.mCanvas.ShowRuler = true;
			this.mCanvas.Size = new System.Drawing.Size(874, 516);
			this.mCanvas.TabIndex = 0;
			this.mCanvas.Transforms = null;
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(874, 516);
			this.Controls.Add(this.mCanvas);
			this.Name = "Editor";
			this.Text = "Editor";
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.EditorCanvas mCanvas;
	}
}

