using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		private readonly InputController mInputController;
		private bool mShowGuidelines;

		public Main()
		{
			InitializeComponent();
			mInputController = new InputController();
			PreviewPicture.Paint += OnPreviewPaint;
		}

		private void OnPreviewPaint(object sender, PaintEventArgs e)
		{
			if (ShowGuidelines)
			{
				var s = sender as PictureBox;
				if (s == null)
					return;

				Point p1, p2;

				p1 = new Point(s.Width / 2, 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.White, p1, p2);

				p1 = new Point(0, s.Height / 2);
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.White, p1, p2);

				p1 = new Point(s.Width / 2, 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.White, p1, p2);

				p1 = new Point(0, s.Height / 2);
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.White, p1, p2);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void OnBatchListResized(object sender, System.EventArgs e)
		{
			UpdateBatchListColumnSize();
		}
		private void OnWindowLoaded(object sender, System.EventArgs e)
		{
			UpdateBatchListColumnSize();
		}

		public bool ShowGuidelines
		{
			get { return mShowGuidelines; }
			set
			{
				mShowGuidelines = value;
				PreviewPicture.Refresh();
			}
		}

		internal void UpdateBatchListColumnSize()
		{
			BatchListView.Columns[0].Width = BatchListView.ClientSize.Width - 3;
		}

		private void OnDensityKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputController.HandleKeyPressForIntegerTextBox(e);
		}
	}
}
