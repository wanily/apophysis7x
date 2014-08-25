using System;
using System.Drawing;
using System.Globalization;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class GridRulerVisual : GridVisual
	{
		private bool mShowHorizontal;
		private bool mShowVertical;
		private int mRulerSize;
		private Color mBackgroundColor;
		private bool mShowLabels;

		public GridRulerVisual([NotNull] Grid canvas) : base(canvas)
		{
			mShowHorizontal = true;
			mRulerSize = 15;
			mShowLabels = true;
		}

		public bool ShowHorizontal
		{
			get { return mShowHorizontal; }
			set
			{
				mShowHorizontal = value;
				InvalidateControl();
			}
		}
		public bool ShowVertical
		{
			get { return mShowVertical; }
			set
			{
				mShowVertical = value;
				InvalidateControl();
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
		public bool ShowLabels
		{
			get { return mShowLabels; }
			set
			{
				mShowLabels = value;
				InvalidateControl();
			}
		}

		private string GetMarkerString(double unit)
		{
			var scale = Canvas.Scale;
			var digits = scale > 1 ? 0 : (int)-System.Math.Ceiling(System.Math.Log10(scale));

			if (System.Math.Abs(unit) > 9999)
			{
				var log = System.Math.Floor(System.Math.Log10(System.Math.Abs(unit)));
				var pow = System.Math.Pow(10, log);

				var b = (int)System.Math.Round(System.Math.Abs(unit)/pow,1) * (unit < 0 ? -1 : 1);
				return string.Format("{0}E+{1}", b.ToString(CultureInfo.InvariantCulture), log.ToString(CultureInfo.InvariantCulture));
			}

			return (System.Math.Round(unit, digits)).ToString("G", CultureInfo.InvariantCulture);
		}

		private void DrawBackgroundHorizontal(Graphics g, Vector2 scale, Brush brush)
		{
			var step = (scale * Canvas.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
			{
				var cell = (int)System.Math.Round(Canvas.CanvasToWorld(new Vector2(x, 0)).X / scale.X);
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
				var cell = (int)System.Math.Round(Canvas.CanvasToWorld(new Vector2(0, y)).Y / scale.Y);
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
				var unit = Canvas.CanvasToWorld(new Vector2(x, 0)).X;
				var markerString = GetMarkerString(unit);
				var offset = g.MeasureString(markerString, markerFont);
				var position = new Point((int)(x - offset.Width / 2.0), RulerSize + 8);
				var frame = new System.Drawing.Rectangle(new Point(position.X - 2, position.Y - 2), new Size((int) offset.Width + 4, (int) offset.Height + 4));

				if (position.X < RulerSize + 16 + offset.Height || (position.X + offset.Width + 8) > Canvas.Size.X)
					continue;

				g.FillRectangle(background, frame);
				g.DrawRectangle(pen, frame);
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
				var unit = Canvas.CanvasToWorld(new Vector2(0, y)).Y;
				var markerString = GetMarkerString(unit);
				var offset = g.MeasureString(markerString, markerFont);
				var position = new Point(RulerSize + 8, (int)(y - offset.Height / 2.0));
				var frame = new System.Drawing.Rectangle(new Point(position.X - 2, position.Y - 2), new Size((int)offset.Width + 4, (int)offset.Height + 4));

				if (position.Y < RulerSize + 16 + offset.Height || (position.Y + offset.Height + 8) > Canvas.Size.Y)
					continue;

				g.FillRectangle(background, frame);
				g.DrawRectangle(pen, frame);
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

				if (ShowHorizontal && ShowLabels)
				{
					DrawGridMarkersHorizontal(graphics, scale, backgroundBrush, gridlinePen);
				}

				if (ShowVertical && ShowLabels)
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