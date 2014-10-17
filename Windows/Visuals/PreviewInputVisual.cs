using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Windows.Input;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class PreviewInputVisual : ControlVisual<PictureBox>
	{
		private CameraInputOperation mOperation;
		private Size mImageSize;
		private bool mFitFrame;
		private Color mGuideColor;

		public PreviewInputVisual([NotNull] PictureBox control) : base(control)
		{
		}
		protected override void DisposeOverride(bool disposing)
		{
			mOperation = null;
		}

		internal CameraData EditData { get; set; }

		public CameraInputOperation Operation
		{
			get { return mOperation; }
			set
			{
				mOperation = value;
				if (value != null)
					InvalidateControl();
			}
		}
		public Size ImageSize
		{
			get { return mImageSize; }
			set { mImageSize = value; }
		}
		public bool FitFrame
		{
			get { return mFitFrame; }
			set { mFitFrame = value; }
		}
		public Color GuideColor
		{
			get { return mGuideColor; }
			set { mGuideColor = value; }
		}

		protected override void RegisterEvents(PictureBox control)
		{
		}
		protected override void UnregisterEvents(PictureBox control)
		{
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			if (EditData == null || mImageSize.Width <= 0 || mImageSize.Height <= 0)
				return;

			var fractalSize = FitFrame ? AttachedControl.ClientSize : mImageSize.FitToFrame(AttachedControl.ClientSize);
			var fractalRect = new Rectangle(new Point(AttachedControl.ClientSize.Width / 2 - fractalSize.Width / 2, AttachedControl.ClientSize.Height / 2 - fractalSize.Height / 2), fractalSize);

			var x0 = new Vector2(fractalRect.Left, fractalRect.Top);
			var x1 = new Vector2(fractalRect.Right, fractalRect.Top);
			var x2 = new Vector2(fractalRect.Right, fractalRect.Bottom);
			var x3 = new Vector2(fractalRect.Left, fractalRect.Bottom);

			var pan = mOperation as PanOperation;
			if (pan != null)
			{
				var offset = System.Math.Pow(2, EditData.Zoom) * EditData.Scale * (pan.OldOffset - pan.NewOffset);

				x0 += offset;
				x1 += offset;
				x2 += offset;
				x3 += offset;

				DrawRectangle(graphics, x0, x1, x2, x3);
			}

			var rotate = mOperation as RotateCanvasOperation;
			if (rotate != null)
			{
				var o = new Vector2(AttachedControl.ClientSize.Width/2.0, AttachedControl.ClientSize.Height/2.0);

				var cos = System.Math.Cos(rotate.NewAngle - rotate.OldAngle);
				var sin = System.Math.Sin(rotate.NewAngle - rotate.OldAngle);

				x0 = new Vector2((x0.X - o.X) * cos + (x0.Y - o.Y) * sin + o.X, (x0.Y - o.Y) * cos - (x0.X - o.X) * sin + o.Y);
				x1 = new Vector2((x1.X - o.X) * cos + (x1.Y - o.Y) * sin + o.X, (x1.Y - o.Y) * cos - (x1.X - o.X) * sin + o.Y);
				x2 = new Vector2((x2.X - o.X) * cos + (x2.Y - o.Y) * sin + o.X, (x2.Y - o.Y) * cos - (x2.X - o.X) * sin + o.Y);
				x3 = new Vector2((x3.X - o.X) * cos + (x3.Y - o.Y) * sin + o.X, (x3.Y - o.Y) * cos - (x3.X - o.X) * sin + o.Y);

				DrawRectangle(graphics, x0, x1, x2, x3);
			}

			var zoom = mOperation as ZoomOperation;
			if (zoom != null)
			{
				const int ps = 30;

				x0 = new Vector2(zoom.InnerRect.Left, zoom.InnerRect.Top);
				x1 = new Vector2(zoom.InnerRect.Right, zoom.InnerRect.Top);
				x2 = new Vector2(zoom.InnerRect.Right, zoom.InnerRect.Bottom);
				x3 = new Vector2(zoom.InnerRect.Left, zoom.InnerRect.Bottom);

				DrawRectangle(graphics, x0, x1, x2, x3);

				x0 = new Vector2(zoom.OuterRect.Left, zoom.OuterRect.Top);
				x1 = new Vector2(zoom.OuterRect.Right, zoom.OuterRect.Top);
				x2 = new Vector2(zoom.OuterRect.Right, zoom.OuterRect.Bottom);
				x3 = new Vector2(zoom.OuterRect.Left, zoom.OuterRect.Bottom);

				var dx = zoom.OuterRect.Left < zoom.OuterRect.Right ? 1 : -1;
				var dy = zoom.OuterRect.Top < zoom.OuterRect.Bottom ? 1 : -1;

				var xx0 = x0 + new Vector2(ps, 0) * dx;
				var xy0 = x0 + new Vector2(0, ps) * dy;

				var xx1 = x1 - new Vector2(ps, 0) * dx;
				var xy1 = x1 + new Vector2(0, ps) * dy;

				var xx2 = x2 - new Vector2(ps, 0) * dx;
				var xy2 = x2 - new Vector2(0, ps) * dy;

				var xx3 = x3 + new Vector2(ps, 0) * dx;
				var xy3 = x3 - new Vector2(0, ps) * dy;

				using (var pen = new Pen(GuideColor, 1.0f))
				{
					graphics.DrawLine(pen, x0.ToPoint(), xx0.ToPoint());
					graphics.DrawLine(pen, x0.ToPoint(), xy0.ToPoint());

					graphics.DrawLine(pen, x1.ToPoint(), xx1.ToPoint());
					graphics.DrawLine(pen, x1.ToPoint(), xy1.ToPoint());

					graphics.DrawLine(pen, x2.ToPoint(), xx2.ToPoint());
					graphics.DrawLine(pen, x2.ToPoint(), xy2.ToPoint());

					graphics.DrawLine(pen, x3.ToPoint(), xx3.ToPoint());
					graphics.DrawLine(pen, x3.ToPoint(), xy3.ToPoint());
				}
			}
		}

		private void DrawRectangle(Graphics graphics, Vector2 x0, Vector2 x1, Vector2 x2, Vector2 x3)
		{
			using (var pen = new Pen(GuideColor, 1.0f))
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