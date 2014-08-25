using System.Drawing;
using Xyrus.Apophysis.Windows.Math;
using Rectangle = Xyrus.Apophysis.Windows.Math.Rectangle;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class GridVisual : CanvasVisual<Grid>
	{
		public GridVisual([NotNull] Grid canvas) : base(canvas)
		{
		}

		protected Rectangle GetWorldBounds(Vector2 snapScale)
		{
			var u = Canvas.CanvasToWorld(new Vector2());
			var v = Canvas.CanvasToWorld(Canvas.Size);

			var c0 = Canvas.Snap(u, snapScale, CanvasSnapBehavior.Floor, CanvasSnapBehavior.Ceil);
			var c1 = Canvas.Snap(v, snapScale, CanvasSnapBehavior.Ceil, CanvasSnapBehavior.Floor);

			return new Rectangle(c0, c1 - c0);
		}
		protected Rectangle GetCanvasBounds(Vector2 snapScale)
		{
			var wb = GetWorldBounds(snapScale);

			var c0 = Canvas.WorldToCanvas(wb.TopLeft);
			var c1 = Canvas.WorldToCanvas(wb.BottomRight);

			return new Rectangle(c0, c1 - c0);
		}

		private void DrawBackground(Graphics g, Vector2 scale, Brush brush)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);
			var invScale = 1.0 / scale;

			for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
			{
				for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
				{
					var xy = new Vector2(x, y);
					var uv = Canvas.CanvasToWorld(xy * invScale);

					var uI = (int)System.Math.Round(uv.X);
					var vI = (int)System.Math.Round(uv.Y);

					if ((uI % 2 == 0 && vI % 2 == 0) || (uI % 2 != 0 && vI % 2 != 0))
					{
						g.FillRectangle(brush, new System.Drawing.Rectangle((int)xy.X, (int)xy.Y, (int)step.X, (int)step.Y));
					}
				}
			}
		}
		private void DrawGridLines(Graphics g, Vector2 scale, Pen pen)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
			{
				g.DrawLine(pen, new Point((int)bounds.TopLeft.X, (int)y), new Point((int)bounds.BottomRight.X, (int)y));
			}

			for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
			{
				g.DrawLine(pen, new Point((int)x, (int)bounds.TopLeft.Y), new Point((int)x, (int)bounds.BottomRight.Y));
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var glc = Color.FromArgb(0xff, GridLineColor.R, GridLineColor.G, GridLineColor.B);
			var glc05 = Color.FromArgb(0x80, GridLineColor.R, GridLineColor.G, GridLineColor.B);
			var glzc = Color.FromArgb(0xff, GridZeroLineColor.R, GridZeroLineColor.G, GridZeroLineColor.B);

			using (var backdropBrush = new SolidBrush(BackdropColor))
			using (var gridlinePen = new Pen(glc, 1.0f))
			using (var gridlinePenHalf = new Pen(glc05, 1.0f))
			using (var gridlinePenZero = new Pen(glzc, 1.0f))
			{
				var scale = new Vector2(Canvas.Scale, Canvas.Scale);

				DrawBackground(graphics, scale, backdropBrush);
				DrawGridLines(graphics, scale, gridlinePen);
				DrawGridLines(graphics, scale * 0.1, gridlinePenHalf);

				var line0 = Canvas.WorldToCanvas(new Vector2());
				var world = new Rectangle(new Vector2(), Canvas.Size);

				if (world.IsOnSurface(new Vector2(line0.X, Canvas.Size.Y / 2.0)))
				{
					var x0 = new Point((int) line0.X, 0);
					var x1 = new Point((int) line0.X, (int)Canvas.Size.Y);

					graphics.DrawLine(gridlinePenZero, x0, x1);
				}

				if (world.IsOnSurface(new Vector2(Canvas.Size.X/2.0, line0.Y)))
				{
					var y0 = new Point(0, (int)line0.Y);
					var y1 = new Point((int)Canvas.Size.X, (int)line0.Y);

					graphics.DrawLine(gridlinePenZero, y0, y1);
				}
			}
		}
	}
}
