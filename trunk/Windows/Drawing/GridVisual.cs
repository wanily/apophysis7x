using System;
using System.Drawing;
using System.Globalization;
using Xyrus.Apophysis.Windows.Math;
using Rectangle = Xyrus.Apophysis.Windows.Math.Rectangle;

namespace Xyrus.Apophysis.Windows.Drawing
{
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

			var line0 = Canvas.WorldToCanvas(new Vector2());
			var world = new Rectangle(new Vector2(), Canvas.Size);

			if (world.IsOnSurface(line0))
			{
				var y0 = new Point(0, (int) line0.Y);
				var y1 = new Point((int)Canvas.Size.X, (int) line0.Y);

				var x0 = new Point((int) line0.X, 0);
				var x1 = new Point((int) line0.X, (int)Canvas.Size.Y);

				using (var zpen = new Pen(pen.Brush, 4.0f))
				{
					g.DrawLine(zpen, x0, x1);
					g.DrawLine(zpen, y0, y1);
				}
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var glc = Color.FromArgb(0xff, GridLineColor.R, GridLineColor.G, GridLineColor.B);
			var glc05 = Color.FromArgb(0x80, GridLineColor.R, GridLineColor.G, GridLineColor.B);

			using (var backdropBrush = new SolidBrush(BackdropColor))
			using (var gridlinePen = new Pen(glc, 1.0f))
			using (var gridlinePenHalf = new Pen(glc05, 1.0f))
			{
				var scale = new Vector2(Canvas.Scale, Canvas.Scale);

				DrawBackground(graphics, scale, backdropBrush);
				DrawGridLines(graphics, scale, gridlinePen);
				DrawGridLines(graphics, scale * 0.1, gridlinePenHalf);
			}
		}
	}

	public class GridRulerVisual : GridVisual
	{
		private bool mShowHorizontal;
		private bool mShowVertical;
		private int mRulerSize;

		private Color mBackgroundColor;

		public GridRulerVisual([NotNull] Grid canvas) : base(canvas)
		{
			mShowHorizontal = true;
			mRulerSize = 15;
		}

		public bool ShowHorizontal
		{
			get { return mShowHorizontal; }
			set
			{
				mShowHorizontal = value;
			}
		}
		public bool ShowVertical
		{
			get { return mShowVertical; }
			set
			{
				mShowVertical = value;
			}
		}
		public int RulerSize
		{
			get { return mRulerSize; }
			set
			{
				if (value <= 1) throw new ArgumentOutOfRangeException("value");
				mRulerSize = value;
			}
		}

		public Color BackgroundColor
		{
			get { return mBackgroundColor; }
			set
			{
				mBackgroundColor = value;
				InvalidateControl();
			}
		}

		private string GetMarkerString(double unit)
		{
			//todo
			return System.Math.Round(unit, 3).ToString(CultureInfo.InvariantCulture);
		}

		private void DrawBackgroundHorizontal(Graphics g, Vector2 scale, Brush brush)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
			{
				var cell = (int)System.Math.Round(Canvas.CanvasToWorld(x) / scale.X);
				if (cell % 2 != 0)
					continue;

				var capped = System.Math.Max(ShowVertical ? RulerSize : 0, x);
				var delta = System.Math.Abs(capped - x);

				g.FillRectangle(brush, new System.Drawing.Rectangle((int)capped, 0, (int)step.X - (int)delta, RulerSize));
			}
		}
		private void DrawBackgroundVertical(Graphics g, Vector2 scale, Brush brush)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
			{
				var cell = (int)System.Math.Round(Canvas.CanvasToWorld(y) / scale.Y);
				if (cell % 2 != 0) 
					continue;

				var capped = System.Math.Max(ShowHorizontal ? RulerSize : 0, y);
				var delta = System.Math.Abs(capped - y);

				g.FillRectangle(brush, new System.Drawing.Rectangle(0, (int)capped, RulerSize, (int)step.Y - (int)delta));
			}
		}

		private void DrawGridLinesHorizontal(Graphics g, Vector2 scale, Pen pen)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
			{
				var capped = System.Math.Max(ShowVertical ? RulerSize : 0, x);

				g.DrawLine(pen, new Point((int)capped, 0), new Point((int)capped, RulerSize));
			}
		}
		private void DrawGridLinesVertical(Graphics g, Vector2 scale, Pen pen)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
			{
				var capped = System.Math.Max(ShowHorizontal ? RulerSize : 0, y);

				g.DrawLine(pen, new Point(0, (int)capped), new Point(RulerSize, (int)capped));
			}
		}

		private void DrawGridMarkersHorizontal(Graphics g, Vector2 scale, Brush background, Pen pen)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);
			var markerFont = GetFont();

			for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
			{
				var unit = System.Math.Round(Canvas.CanvasToWorld(x));
				var markerString = GetMarkerString(unit);
				var offset = g.MeasureString(markerString, markerFont);
				var position = new Point((int)(x - offset.Width / 2.0), RulerSize + 2);

				g.FillRectangle(background, new System.Drawing.Rectangle(position, new Size((int)offset.Width, (int)offset.Height)));
				g.DrawRectangle(pen, new System.Drawing.Rectangle(position, new Size((int)offset.Width, (int)offset.Height)));
				g.DrawString(markerString, markerFont, pen.Brush, position);
			}
		}
		private void DrawGridMarkersVertical(Graphics g, Vector2 scale, Brush background, Pen pen)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);
			var markerFont = GetFont();

			for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
			{
				var unit = System.Math.Round(Canvas.CanvasToWorld(y));
				var markerString = GetMarkerString(unit);
				var offset = g.MeasureString(markerString, markerFont);
				var position = new Point(RulerSize + 2, (int)(y - offset.Height / 2.0));

				g.FillRectangle(background, new System.Drawing.Rectangle(position, new Size((int)offset.Width, (int)offset.Height)));
				g.DrawRectangle(pen, new System.Drawing.Rectangle(position, new Size((int)offset.Width, (int)offset.Height)));
				g.DrawString(markerString, markerFont, pen.Brush, position);
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var glc = Color.FromArgb(0xff, GridLineColor.R, GridLineColor.G, GridLineColor.B);
			var glc05 = Color.FromArgb(0x80, GridLineColor.R, GridLineColor.G, GridLineColor.B);

			using (var backgroundBrush = new SolidBrush(BackgroundColor))
			using (var backdropBrush = new SolidBrush(BackdropColor))
			using (var gridlinePen = new Pen(glc, 1.0f))
			using (var gridlinePenHalf = new Pen(glc05, 1.0f))
			{
				var scale = new Vector2(Canvas.Scale, Canvas.Scale);

				if (ShowHorizontal && ShowVertical)
				{
					graphics.FillRectangle(backgroundBrush, new System.Drawing.Rectangle(0, 0, RulerSize, RulerSize));
				}

				if (ShowHorizontal)
				{
					DrawGridMarkersHorizontal(graphics, scale, backgroundBrush, gridlinePen);
				}

				if (ShowVertical)
				{
					DrawGridMarkersVertical(graphics, scale, backgroundBrush, gridlinePen);
				}

				if (ShowHorizontal)
				{
					graphics.FillRectangle(backgroundBrush, new System.Drawing.Rectangle(ShowVertical ? RulerSize : 0, 0, (int)Canvas.Size.X - (ShowVertical ? RulerSize : 0), RulerSize));

					DrawBackgroundHorizontal(graphics, scale, backdropBrush);
					DrawGridLinesHorizontal(graphics, scale, gridlinePen);
					DrawGridLinesHorizontal(graphics, scale * 0.1, gridlinePenHalf);

					graphics.DrawLine(gridlinePen, ShowVertical ? RulerSize : 0, RulerSize, (int)Canvas.Size.X, RulerSize);
				}

				if (ShowVertical)
				{
					graphics.FillRectangle(backgroundBrush, new System.Drawing.Rectangle(0, ShowHorizontal ? RulerSize : 0, RulerSize, (int)Canvas.Size.Y - (ShowHorizontal ? RulerSize : 0)));

					DrawBackgroundVertical(graphics, scale, backdropBrush);
					DrawGridLinesVertical(graphics, scale, gridlinePen);
					DrawGridLinesVertical(graphics, scale * 0.1, gridlinePenHalf);

					graphics.DrawLine(gridlinePen, RulerSize, ShowHorizontal ? RulerSize : 0, RulerSize, (int)Canvas.Size.Y);
				}
			}
		}
	}
}
