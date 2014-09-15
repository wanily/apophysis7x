using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		private readonly InputController mInputController;
		private bool mShowGuidelines;
		private bool mShowTransparency;

		public Main()
		{
			InitializeComponent();
			mInputController = new InputController();
			PreviewPicture.Paint += OnPreviewPaint;
		}

		private void OnPreviewPaint(object sender, PaintEventArgs e)
		{
			var s = sender as PictureBox;
				if (s == null)
					return;

			if (ShowTransparency)
			{
				var tilesX = (s.ClientSize.Width + 10) / 10;
				var tilesY = (s.ClientSize.Height + 10) / 10;

				if (tilesX % 2 != 0) tilesX++;
				if (tilesY % 2 != 0) tilesY++;

				using (var brushA = new SolidBrush(Color.White))
				using (var brushB = new SolidBrush(Color.LightGray))
				{
					int ii = 0, jj;
					for (int i = 0; i < tilesX; i ++)
					{
						jj = 0;
						for (int j = 0; j <= tilesY; j ++)
						{
							var brush = ((i+j) % 2 != 0) ? brushA : brushB;

							e.Graphics.FillRectangle(brush, new Rectangle(new Point(ii, jj), new Size(10,10)));

							jj+=10;
						}
						ii+=10;
					}

				}
			}
			else
			{
				using (var brush = new SolidBrush(s.BackColor))
				{
					e.Graphics.FillRectangle(brush, new Rectangle(new Point(), s.ClientSize));
				}
			}

			if (s.Image != null)
			{
				e.Graphics.DrawImage(s.Image, new Rectangle(s.ClientSize.Width / 2 - s.Image.Width / 2, s.ClientSize.Height / 2 - s.Image.Height / 2, s.Image.Width, s.Image.Height));
			}

			if (ShowGuidelines)
			{
				Point p1, p2;
				const double ratio = 0.61803399;

				p1 = new Point(s.Width / 2, 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.White, p1, p2);

				p1 = new Point(0, s.Height / 2);
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.White, p1, p2);

				p1 = new Point(s.Width / 3, 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.Red, p1, p2);

				p1 = new Point(0, s.Height / 3);
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.Red, p1, p2);

				p1 = new Point(2 * s.Width / 3, 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.Red, p1, p2);

				p1 = new Point(0, 2 * s.Height / 3);
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.Red, p1, p2);

				p1 = new Point((int)(ratio * s.Width), 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.Green, p1, p2);

				p1 = new Point(0, (int)(ratio * s.Height));
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.Green, p1, p2);

				p1 = new Point(s.Width - (int)(ratio * s.Width), 0);
				p2 = new Point(p1.X, s.Height);
				e.Graphics.DrawLine(Pens.Green, p1, p2);

				p1 = new Point(0, s.Height - (int)(ratio * s.Height));
				p2 = new Point(s.Width, p1.Y);
				e.Graphics.DrawLine(Pens.Green, p1, p2);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
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
		public bool ShowTransparency
		{
			get { return mShowTransparency; }
			set
			{
				mShowTransparency = value;
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
