using System;
using System.Drawing;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Fullscreen : Form
	{
		private int mProgress;
		private string mTimeElapsed;
		private string mTimeRemaining;
		private bool mIsInProgress;

		public Fullscreen()
		{
			InitializeComponent();
		}

		public int Progress
		{
			get { return mProgress; }
			set
			{
				mProgress = value;
				Refresh();
			}
		}
		public string TimeElapsed
		{
			get { return mTimeElapsed; }
			set
			{
				mTimeElapsed = value;
				Refresh();
			}
		}
		public string TimeRemaining
		{
			get { return mTimeRemaining; }
			set
			{
				mTimeRemaining = value;
				Refresh();
			}
		}
		public bool IsInProgress
		{
			get { return mIsInProgress; }
			set
			{
				mIsInProgress = value;
				Refresh();
			}
		}

		public event EventHandler Hidden;

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				Hide();
				if (Hidden != null)
					Hidden(this, new EventArgs());
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!mIsInProgress)
				return;

			using (var foreground = new SolidBrush(ForeColor))
			using (var pen = new Pen(foreground))
			{
				var rect = new Rectangle(10, ClientSize.Height - 30, ClientSize.Width - 20, 20);
				var rectProgress = new RectangleF(12f, ClientSize.Height - 28, (ClientSize.Width - 24)*(float) mProgress/100f, 16);

				e.Graphics.DrawRectangle(pen, rect);
				e.Graphics.FillRectangle(foreground, rectProgress);

				var elapsedString = TimeElapsed;
				var remainingString = TimeRemaining;

				var elapsedSize = e.Graphics.MeasureString(elapsedString, Font);
				var remainingSize = e.Graphics.MeasureString(remainingString, Font);

				e.Graphics.DrawString(elapsedString, Font, foreground, 10, ClientSize.Height - 35 - elapsedSize.Height);
				e.Graphics.DrawString(remainingString, Font, foreground, ClientSize.Width - 10 - remainingSize.Width, ClientSize.Height - 35 - elapsedSize.Height);
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			var screen = Screen.FromHandle(Handle);

			Left = screen.Bounds.Left-1;
			Top = screen.Bounds.Top-1;
			Width = screen.Bounds.Width+2;
			Height = screen.Bounds.Height+2;
		}
	}
}
