using System.Drawing;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class PreviewGuidelinesVisual : ControlVisual<PictureBox>
	{
		private bool mVisible;

		public PreviewGuidelinesVisual([NotNull] PictureBox control) : base(control)
		{
		}

		public bool Visible
		{
			get { return mVisible; }
			set
			{
				mVisible = value;
				InvalidateControl();
			}
		}

		protected override void RegisterEvents(PictureBox control)
		{
		}
		protected override void UnregisterEvents(PictureBox control)
		{
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			if (!Visible) 
				return;

			var s = AttachedControl;

			Point p1, p2;
			const double ratio = 0.61803399;

			p1 = new Point(s.Width / 2, 0);
			p2 = new Point(p1.X, s.Height);
			graphics.DrawLine(Pens.White, p1, p2);

			p1 = new Point(0, s.Height / 2);
			p2 = new Point(s.Width, p1.Y);
			graphics.DrawLine(Pens.White, p1, p2);

			p1 = new Point(s.Width / 3, 0);
			p2 = new Point(p1.X, s.Height);
			graphics.DrawLine(Pens.Red, p1, p2);

			p1 = new Point(0, s.Height / 3);
			p2 = new Point(s.Width, p1.Y);
			graphics.DrawLine(Pens.Red, p1, p2);

			p1 = new Point(2 * s.Width / 3, 0);
			p2 = new Point(p1.X, s.Height);
			graphics.DrawLine(Pens.Red, p1, p2);

			p1 = new Point(0, 2 * s.Height / 3);
			p2 = new Point(s.Width, p1.Y);
			graphics.DrawLine(Pens.Red, p1, p2);

			p1 = new Point((int)(ratio * s.Width), 0);
			p2 = new Point(p1.X, s.Height);
			graphics.DrawLine(Pens.LightGreen, p1, p2);

			p1 = new Point(0, (int)(ratio * s.Height));
			p2 = new Point(s.Width, p1.Y);
			graphics.DrawLine(Pens.LightGreen, p1, p2);

			p1 = new Point(s.Width - (int)(ratio * s.Width), 0);
			p2 = new Point(p1.X, s.Height);
			graphics.DrawLine(Pens.LightGreen, p1, p2);

			p1 = new Point(0, s.Height - (int)(ratio * s.Height));
			p2 = new Point(s.Width, p1.Y);
			graphics.DrawLine(Pens.LightGreen, p1, p2);
		}
	}
}