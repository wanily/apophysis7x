using Xyrus.Apophysis.Windows.Controls;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.mCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.mSettings = new EditorSettings();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// mRootSplitter
			// 
			this.mRootSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mRootSplitter.Location = new System.Drawing.Point(0, 0);
			this.mRootSplitter.Name = "mRootSplitter";
			// 
			// mRootSplitter.Panel1
			// 
			this.mRootSplitter.Panel1.Controls.Add(this.mCanvas);
			this.mRootSplitter.Panel2Collapsed = true;
			this.mRootSplitter.Size = new System.Drawing.Size(1043, 625);
			this.mRootSplitter.SplitterDistance = 1011;
			this.mRootSplitter.TabIndex = 3;
			// 
			// mCanvas
			// 
			this.mCanvas.BackColor = System.Drawing.Color.Black;
			this.mCanvas.BackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mCanvas.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mCanvas.ForeColor = System.Drawing.Color.White;
			this.mCanvas.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mCanvas.GridZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.mCanvas.HighlightOrigin = true;
			this.mCanvas.Location = new System.Drawing.Point(0, 0);
			this.mCanvas.Name = "mCanvas";
			this.mCanvas.ReferenceColor = System.Drawing.Color.Gray;
			this.mCanvas.RulerBackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.RulerBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mCanvas.RulerGridLineColor = System.Drawing.Color.Silver;
			this.mCanvas.Settings = this.mSettings;
			this.mCanvas.ShowRuler = true;
			this.mCanvas.Size = new System.Drawing.Size(1043, 625);
			this.mCanvas.TabIndex = 1;
			this.mCanvas.Iterators = null;
			// 
			// mSettings
			// 
			this.mSettings.AngleSnap = 15D;
			this.mSettings.MoveAmount = 0.1D;
			this.mSettings.ScaleSnap = 125D;
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1043, 625);
			this.Controls.Add(this.mRootSplitter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 39);
			this.Name = "Editor";
			this.Text = "xw_prototype_2014-337";
			this.mRootSplitter.Panel1.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer mRootSplitter;
		private Controls.EditorCanvas mCanvas;
		private EditorSettings mSettings;
	}
}

