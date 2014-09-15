using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class PreviewInputVisual : ControlVisual<PictureBox>
	{
		private CameraInputOperation mOperation;

		public PreviewInputVisual([NotNull] PictureBox control) : base(control)
		{
		}
		protected override void DisposeOverride(bool disposing)
		{
			mOperation = null;
		}

		public CameraInputOperation Operation
		{
			get { return mOperation; }
			set
			{
				mOperation = value;
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
			var rotate = mOperation as RotateCanvasOperation;
			if (rotate != null)
			{
				var x0 = new Vector2(0, 0);
				var x1 = new Vector2(AttachedControl.ClientSize.Width, 0);
				var x2 = new Vector2(AttachedControl.ClientSize.Width, AttachedControl.ClientSize.Height);
				var x3 = new Vector2(0, AttachedControl.ClientSize.Height);

				var o = new Vector2(AttachedControl.ClientSize.Width/2.0, AttachedControl.ClientSize.Height/2.0);

				x0 = x0.Rotate(rotate.Angle, o);
				x1 = x1.Rotate(rotate.Angle, o);
				x2 = x2.Rotate(rotate.Angle, o);
				x3 = x3.Rotate(rotate.Angle, o);

				using (var pen = new Pen(Color.White, 1.0f))
				{
					pen.DashPattern = new[] { 6.0f, 4.0f };

					graphics.DrawLine(pen, x0.ToPoint(), x1.ToPoint());
					graphics.DrawLine(pen, x1.ToPoint(), x2.ToPoint());
					graphics.DrawLine(pen, x2.ToPoint(), x3.ToPoint());
					graphics.DrawLine(pen, x3.ToPoint(), x0.ToPoint());
				}
			}
		}
	}
}