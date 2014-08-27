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
			this.mStatusbar = new System.Windows.Forms.StatusStrip();
			this.mStatusXPositionLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mStatusYPositionLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mStatusStringLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.mCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.mStatusbar.SuspendLayout();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// mStatusbar
			// 
			this.mStatusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mStatusXPositionLabel,
            this.mStatusYPositionLabel,
            this.mStatusStringLabel});
			this.mStatusbar.Location = new System.Drawing.Point(0, 601);
			this.mStatusbar.Name = "mStatusbar";
			this.mStatusbar.Size = new System.Drawing.Size(1043, 24);
			this.mStatusbar.SizingGrip = false;
			this.mStatusbar.TabIndex = 2;
			this.mStatusbar.Text = "statusStrip1";
			// 
			// mStatusXPositionLabel
			// 
			this.mStatusXPositionLabel.AutoSize = false;
			this.mStatusXPositionLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.mStatusXPositionLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
			this.mStatusXPositionLabel.Name = "mStatusXPositionLabel";
			this.mStatusXPositionLabel.Size = new System.Drawing.Size(60, 19);
			this.mStatusXPositionLabel.Text = "X: 0";
			this.mStatusXPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mStatusYPositionLabel
			// 
			this.mStatusYPositionLabel.AutoSize = false;
			this.mStatusYPositionLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
			this.mStatusYPositionLabel.Name = "mStatusYPositionLabel";
			this.mStatusYPositionLabel.Size = new System.Drawing.Size(60, 19);
			this.mStatusYPositionLabel.Text = "Y: 0";
			this.mStatusYPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mStatusStringLabel
			// 
			this.mStatusStringLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.mStatusStringLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
			this.mStatusStringLabel.Name = "mStatusStringLabel";
			this.mStatusStringLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.mStatusStringLabel.Size = new System.Drawing.Size(9, 19);
			this.mStatusStringLabel.Visible = false;
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
			this.mRootSplitter.Size = new System.Drawing.Size(1043, 601);
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
			this.mCanvas.GridZeroLineColor = System.Drawing.Color.Gray;
			this.mCanvas.HighlightOrigin = false;
			this.mCanvas.Location = new System.Drawing.Point(0, 0);
			this.mCanvas.Name = "mCanvas";
			this.mCanvas.ReferenceColor = System.Drawing.Color.Gray;
			this.mCanvas.RulerBackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.RulerBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mCanvas.RulerGridLineColor = System.Drawing.Color.Silver;
			this.mCanvas.ShowRuler = true;
			this.mCanvas.Size = new System.Drawing.Size(1043, 601);
			this.mCanvas.TabIndex = 1;
			this.mCanvas.Transforms = null;
			this.mCanvas.TransformUpdated += new Xyrus.Apophysis.Windows.Input.TransformUpdatedEventHandler(this.OnTransformUpdatedFromCanvas);
			this.mCanvas.TransformHit += new Xyrus.Apophysis.Windows.Input.TransformHitEventHandler(this.OnTransformHitOnCanvas);
			this.mCanvas.TransformHitCleared += new System.EventHandler(this.OnTransformHitClearedOnCanvas);
			this.mCanvas.BeginEdit += new System.EventHandler(this.OnCanvasBeginEdit);
			this.mCanvas.EndEdit += new System.EventHandler(this.OnCanvasEndEdit);
			this.mCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnCanvasMouseMove);
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1043, 625);
			this.Controls.Add(this.mRootSplitter);
			this.Controls.Add(this.mStatusbar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 39);
			this.Name = "Editor";
			this.Text = "xw_prototype_2014-337";
			this.mStatusbar.ResumeLayout(false);
			this.mStatusbar.PerformLayout();
			this.mRootSplitter.Panel1.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip mStatusbar;
		private System.Windows.Forms.ToolStripStatusLabel mStatusStringLabel;
		private System.Windows.Forms.SplitContainer mRootSplitter;
		private Controls.EditorCanvas mCanvas;
		private System.Windows.Forms.ToolStripStatusLabel mStatusXPositionLabel;
		private System.Windows.Forms.ToolStripStatusLabel mStatusYPositionLabel;
	}
}

