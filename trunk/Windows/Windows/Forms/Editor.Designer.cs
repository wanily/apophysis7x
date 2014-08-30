using Xyrus.Apophysis.Windows.Controls;

namespace Xyrus.Apophysis.Windows.Forms
{
	partial class Editor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.mRootSplitter = new System.Windows.Forms.SplitContainer();
			this.mSidebarSplitter = new System.Windows.Forms.SplitContainer();
			this.mPreviewPanel = new System.Windows.Forms.Panel();
			this.mPictureBevel = new System.Windows.Forms.Label();
			this.mIteratorPropertyPanel = new System.Windows.Forms.Panel();
			this.mIteratorWeight = new System.Windows.Forms.TextBox();
			this.mIteratorName = new System.Windows.Forms.TextBox();
			this.mIteratorNameLabel = new System.Windows.Forms.Label();
			this.mIteratorSelect = new System.Windows.Forms.ComboBox();
			this.mIteratorSelectLabel = new System.Windows.Forms.Label();
			this.mCanvas = new Xyrus.Apophysis.Windows.Controls.EditorCanvas();
			this.mSettings = new Xyrus.Apophysis.Windows.Controls.EditorSettings();
			this.mIteratorWeightLabel = new Xyrus.Apophysis.Windows.Controls.DragPanel();
			this.mTabs = new System.Windows.Forms.TabControl();
			this.mColorTab = new System.Windows.Forms.TabPage();
			this.mRootSplitter.Panel1.SuspendLayout();
			this.mRootSplitter.Panel2.SuspendLayout();
			this.mRootSplitter.SuspendLayout();
			this.mSidebarSplitter.Panel1.SuspendLayout();
			this.mSidebarSplitter.Panel2.SuspendLayout();
			this.mSidebarSplitter.SuspendLayout();
			this.mPreviewPanel.SuspendLayout();
			this.mIteratorPropertyPanel.SuspendLayout();
			this.mTabs.SuspendLayout();
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
			// 
			// mRootSplitter.Panel2
			// 
			this.mRootSplitter.Panel2.Controls.Add(this.mSidebarSplitter);
			this.mRootSplitter.Size = new System.Drawing.Size(1043, 625);
			this.mRootSplitter.SplitterDistance = 740;
			this.mRootSplitter.TabIndex = 3;
			// 
			// mSidebarSplitter
			// 
			this.mSidebarSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mSidebarSplitter.Location = new System.Drawing.Point(0, 0);
			this.mSidebarSplitter.Name = "mSidebarSplitter";
			this.mSidebarSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// mSidebarSplitter.Panel1
			// 
			this.mSidebarSplitter.Panel1.Controls.Add(this.mPreviewPanel);
			// 
			// mSidebarSplitter.Panel2
			// 
			this.mSidebarSplitter.Panel2.Controls.Add(this.mTabs);
			this.mSidebarSplitter.Panel2.Controls.Add(this.mIteratorPropertyPanel);
			this.mSidebarSplitter.Size = new System.Drawing.Size(299, 625);
			this.mSidebarSplitter.SplitterDistance = 187;
			this.mSidebarSplitter.TabIndex = 0;
			// 
			// mPreviewPanel
			// 
			this.mPreviewPanel.Controls.Add(this.mPictureBevel);
			this.mPreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPreviewPanel.Location = new System.Drawing.Point(0, 0);
			this.mPreviewPanel.Name = "mPreviewPanel";
			this.mPreviewPanel.Size = new System.Drawing.Size(299, 187);
			this.mPreviewPanel.TabIndex = 0;
			// 
			// mPictureBevel
			// 
			this.mPictureBevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBevel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mPictureBevel.Location = new System.Drawing.Point(0, 0);
			this.mPictureBevel.Name = "mPictureBevel";
			this.mPictureBevel.Size = new System.Drawing.Size(299, 187);
			this.mPictureBevel.TabIndex = 0;
			// 
			// mIteratorPropertyPanel
			// 
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorWeight);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorWeightLabel);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorName);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorNameLabel);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorSelect);
			this.mIteratorPropertyPanel.Controls.Add(this.mIteratorSelectLabel);
			this.mIteratorPropertyPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.mIteratorPropertyPanel.Location = new System.Drawing.Point(0, 0);
			this.mIteratorPropertyPanel.Name = "mIteratorPropertyPanel";
			this.mIteratorPropertyPanel.Size = new System.Drawing.Size(299, 86);
			this.mIteratorPropertyPanel.TabIndex = 0;
			// 
			// mIteratorWeight
			// 
			this.mIteratorWeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorWeight.Location = new System.Drawing.Point(120, 53);
			this.mIteratorWeight.Name = "mIteratorWeight";
			this.mIteratorWeight.Size = new System.Drawing.Size(168, 20);
			this.mIteratorWeight.TabIndex = 5;
			this.mIteratorWeight.Text = "0.000";
			// 
			// mIteratorName
			// 
			this.mIteratorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorName.Location = new System.Drawing.Point(120, 32);
			this.mIteratorName.Name = "mIteratorName";
			this.mIteratorName.Size = new System.Drawing.Size(168, 20);
			this.mIteratorName.TabIndex = 3;
			this.mIteratorName.TextChanged += new System.EventHandler(this.OnIteratorNameChanged);
			// 
			// mIteratorNameLabel
			// 
			this.mIteratorNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mIteratorNameLabel.Location = new System.Drawing.Point(14, 32);
			this.mIteratorNameLabel.Name = "mIteratorNameLabel";
			this.mIteratorNameLabel.Size = new System.Drawing.Size(107, 20);
			this.mIteratorNameLabel.TabIndex = 2;
			this.mIteratorNameLabel.Text = "Name:";
			this.mIteratorNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mIteratorSelect
			// 
			this.mIteratorSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mIteratorSelect.BackColor = System.Drawing.Color.Black;
			this.mIteratorSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.mIteratorSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mIteratorSelect.ForeColor = System.Drawing.Color.White;
			this.mIteratorSelect.FormattingEnabled = true;
			this.mIteratorSelect.ItemHeight = 15;
			this.mIteratorSelect.Location = new System.Drawing.Point(120, 10);
			this.mIteratorSelect.MaxDropDownItems = 20;
			this.mIteratorSelect.Name = "mIteratorSelect";
			this.mIteratorSelect.Size = new System.Drawing.Size(168, 21);
			this.mIteratorSelect.TabIndex = 1;
			this.mIteratorSelect.SelectedIndexChanged += new System.EventHandler(this.OnIteratorSelectedFromCombo);
			// 
			// mIteratorSelectLabel
			// 
			this.mIteratorSelectLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorSelectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mIteratorSelectLabel.Location = new System.Drawing.Point(14, 10);
			this.mIteratorSelectLabel.Name = "mIteratorSelectLabel";
			this.mIteratorSelectLabel.Size = new System.Drawing.Size(107, 21);
			this.mIteratorSelectLabel.TabIndex = 0;
			this.mIteratorSelectLabel.Text = "Transform:";
			this.mIteratorSelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mCanvas
			// 
			this.mCanvas.ActiveMatrix = Xyrus.Apophysis.Windows.Controls.IteratorMatrix.PreAffine;
			this.mCanvas.BackColor = System.Drawing.Color.Black;
			this.mCanvas.BackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mCanvas.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mCanvas.ForeColor = System.Drawing.Color.White;
			this.mCanvas.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mCanvas.GridZeroLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.mCanvas.HighlightOrigin = true;
			this.mCanvas.Iterators = null;
			this.mCanvas.Location = new System.Drawing.Point(0, 0);
			this.mCanvas.Name = "mCanvas";
			this.mCanvas.ReferenceColor = System.Drawing.Color.Gray;
			this.mCanvas.RulerBackdropColor = System.Drawing.Color.Transparent;
			this.mCanvas.RulerBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mCanvas.RulerGridLineColor = System.Drawing.Color.Silver;
			this.mCanvas.Settings = this.mSettings;
			this.mCanvas.ShowRuler = true;
			this.mCanvas.Size = new System.Drawing.Size(740, 625);
			this.mCanvas.TabIndex = 1;
			this.mCanvas.SelectionChanged += new System.EventHandler(this.OnIteratorSelectedByCanvas);
			// 
			// mSettings
			// 
			this.mSettings.AngleSnap = 15D;
			this.mSettings.LockAxes = false;
			this.mSettings.MoveAmount = 0.1D;
			this.mSettings.ScaleSnap = 125D;
			this.mSettings.ShowVariationPreview = false;
			this.mSettings.ZoomAutomatically = false;
			// 
			// mIteratorWeightLabel
			// 
			this.mIteratorWeightLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mIteratorWeightLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.mIteratorWeightLabel.DisplayCulture = new System.Globalization.CultureInfo("");
			this.mIteratorWeightLabel.Location = new System.Drawing.Point(14, 53);
			this.mIteratorWeightLabel.Name = "mIteratorWeightLabel";
			this.mIteratorWeightLabel.Size = new System.Drawing.Size(107, 20);
			this.mIteratorWeightLabel.TabIndex = 4;
			this.mIteratorWeightLabel.Text = "Weight:";
			this.mIteratorWeightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.mIteratorWeightLabel.TextBox = this.mIteratorWeight;
			this.mIteratorWeightLabel.Value = 0D;
			this.mIteratorWeightLabel.ValueChanged += new System.EventHandler(this.OnIteratorWeightChanged);
			// 
			// mTabs
			// 
			this.mTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mTabs.Controls.Add(this.mColorTab);
			this.mTabs.Location = new System.Drawing.Point(4, 90);
			this.mTabs.Name = "mTabs";
			this.mTabs.SelectedIndex = 0;
			this.mTabs.Size = new System.Drawing.Size(293, 341);
			this.mTabs.TabIndex = 1;
			// 
			// mColorTab
			// 
			this.mColorTab.Location = new System.Drawing.Point(4, 22);
			this.mColorTab.Name = "mColorTab";
			this.mColorTab.Padding = new System.Windows.Forms.Padding(3);
			this.mColorTab.Size = new System.Drawing.Size(285, 315);
			this.mColorTab.TabIndex = 0;
			this.mColorTab.Text = "Color";
			this.mColorTab.UseVisualStyleBackColor = true;
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
			this.mRootSplitter.Panel2.ResumeLayout(false);
			this.mRootSplitter.ResumeLayout(false);
			this.mSidebarSplitter.Panel1.ResumeLayout(false);
			this.mSidebarSplitter.Panel2.ResumeLayout(false);
			this.mSidebarSplitter.ResumeLayout(false);
			this.mPreviewPanel.ResumeLayout(false);
			this.mIteratorPropertyPanel.ResumeLayout(false);
			this.mIteratorPropertyPanel.PerformLayout();
			this.mTabs.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer mRootSplitter;
		private Controls.EditorCanvas mCanvas;
		private EditorSettings mSettings;
		private System.Windows.Forms.SplitContainer mSidebarSplitter;
		private System.Windows.Forms.Panel mPreviewPanel;
		private System.Windows.Forms.Panel mIteratorPropertyPanel;
		private System.Windows.Forms.ComboBox mIteratorSelect;
		private System.Windows.Forms.Label mIteratorSelectLabel;
		private System.Windows.Forms.Label mIteratorNameLabel;
		private System.Windows.Forms.TextBox mIteratorName;
		private DragPanel mIteratorWeightLabel;
		private System.Windows.Forms.TextBox mIteratorWeight;
		private System.Windows.Forms.Label mPictureBevel;
		private System.Windows.Forms.TabControl mTabs;
		private System.Windows.Forms.TabPage mColorTab;
	}
}

