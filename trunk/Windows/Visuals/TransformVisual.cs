using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	[PublicAPI]
	public class TransformVisual : CanvasVisual<Canvas>
	{
		private Transform mTransform;
		private static readonly Color[] mColors = 
		{
			Color.Red,
			Color.Yellow,
			Color.LightGreen,
			Color.Cyan,
			Color.Blue
		};

		public TransformVisual([NotNull] Control control, [NotNull] Canvas canvas, [NotNull] Transform transform) : base(control, canvas)
		{
			if (transform == null) throw new ArgumentNullException("transform");
			mTransform = transform;
		}
		
		protected override void DisposeOverride(bool disposing)
		{
			mTransform = null;
		}
		protected override void OnControlPaint(Graphics graphics)
		{
			const int vertexRadius = 3;
			const double handleSize = 0.2;
			const int distLabel = 4;

			const string lo = "O";
			const string lx = "X";
			const string ly = "Y";

			var ox = GetEdgeOx();
			var oy = GetEdgeOy();
			var xy = GetEdgeXy();

			var fo = mTransform.Origin;
			var fx = mTransform.Affine.X;
			var fy = mTransform.Affine.Y;

			var cornerTopLeft = fy - fx;
			var cornerTopRight = fy + fx;
			var cornerBottomLeft = -1 * fy - fx;
			var cornerBottomRight = -1 * fy + fx;

			var left = -1 * fx;
			var up = fy;
			var right = fx;
			var down = -1 * fy;

			var color = mColors[mTransform.Index%mColors.Length];
			var translucentColor = Color.FromArgb(0x40, color.R, color.G, color.B);
			var translucentColorLow = Color.FromArgb(0x05, color.R, color.G, color.B);

			var sizeLo = graphics.MeasureString(lo, AttachedControl.Font);
			var sizeLx = graphics.MeasureString(lx, AttachedControl.Font);
			var sizeLy = graphics.MeasureString(ly, AttachedControl.Font);

			var posLo = ToPoint(ox.A + new Vector2(distLabel, distLabel));
			var posLx = ToPoint(ox.B + new Vector2(distLabel, distLabel));
			var posLy = ToPoint(oy.B + new Vector2(distLabel, distLabel));

			var rectLo = new Rectangle(posLo, sizeLo.ToSize());
			var rectLx = new Rectangle(posLx, sizeLx.ToSize());
			var rectLy = new Rectangle(posLy, sizeLy.ToSize());

			var posO = ToPoint(ox.A - new Vector2(vertexRadius, vertexRadius));
			var posX = ToPoint(ox.B - new Vector2(vertexRadius, vertexRadius));
			var posY = ToPoint(oy.B - new Vector2(vertexRadius, vertexRadius));

			var vertexSize = new Size(2 * vertexRadius, 2 * vertexRadius);

			var rectO = new Rectangle(posO, vertexSize);
			var rectX = new Rectangle(posX, vertexSize);
			var rectY = new Rectangle(posY, vertexSize);

			using (var backgroundBrush = new SolidBrush(AttachedControl.BackColor))
			using (var labelBrush = new SolidBrush(color))
			using (var hitVertexOBrush = new SolidBrush(AttachedControl.ForeColor))
			using (var hitVertexBrush = new SolidBrush(color))
			using (var vertexBrush = new SolidBrush(translucentColor))
			using (var lowFillBrush = new SolidBrush(translucentColorLow))
			using (var fillBrush = new SolidBrush(translucentColor))
			using (var widgetPen = new Pen(translucentColor))
			using (var dashLinePen = new Pen(color))
			using (var hitLinePen = new Pen(color, 2.0f))
			using (var linePen = new Pen(color))
			{
				dashLinePen.DashPattern = new[] {6.0f,4.0f};

				graphics.DrawLine(IsEdgeOxHit ? hitLinePen : IsSelected || IsHit ? linePen : dashLinePen, ToPoint(ox.A), ToPoint(ox.B));
				graphics.DrawLine(IsEdgeOyHit ? hitLinePen : IsSelected || IsHit ? linePen : dashLinePen, ToPoint(oy.A), ToPoint(oy.B));
				graphics.DrawLine(IsEdgeXyHit ? hitLinePen : dashLinePen, ToPoint(xy.A), ToPoint(xy.B));

				graphics.FillEllipse(IsVertexOHit ? hitVertexOBrush : vertexBrush, rectO);
				graphics.FillEllipse(IsVertexXHit ? hitVertexBrush : vertexBrush, rectX);
				graphics.FillEllipse(IsVertexYHit ? hitVertexBrush : vertexBrush, rectY);

				graphics.DrawEllipse(IsVertexOHit ? hitLinePen : linePen, rectO);
				graphics.DrawEllipse(IsVertexXHit ? hitLinePen : linePen, rectX);
				graphics.DrawEllipse(IsVertexYHit ? hitLinePen : linePen, rectY);

				graphics.FillRectangle(backgroundBrush, rectLo);
				graphics.FillRectangle(backgroundBrush, rectLx);
				graphics.FillRectangle(backgroundBrush, rectLy);

				graphics.FillPolygon(IsHit ? fillBrush : lowFillBrush, new[] { ToPoint(ox.A), ToPoint(ox.B), ToPoint(oy.B) });

				if (IsSelected)
				{
					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerTopLeft + fo)), ToPoint(Canvas.WorldToCanvas(cornerTopLeft + right * handleSize + fo)));
					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerTopLeft + fo)), ToPoint(Canvas.WorldToCanvas(cornerTopLeft + down * handleSize + fo)));

					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerTopRight + fo)), ToPoint(Canvas.WorldToCanvas(cornerTopRight + left * handleSize + fo)));
					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerTopRight + fo)), ToPoint(Canvas.WorldToCanvas(cornerTopRight + down * handleSize + fo)));

					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerBottomLeft + fo)), ToPoint(Canvas.WorldToCanvas(cornerBottomLeft + right * handleSize + fo)));
					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerBottomLeft + fo)), ToPoint(Canvas.WorldToCanvas(cornerBottomLeft + up * handleSize + fo)));

					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerBottomRight + fo)), ToPoint(Canvas.WorldToCanvas(cornerBottomRight + left * handleSize + fo)));
					graphics.DrawLine(IsHit ? linePen : widgetPen, ToPoint(Canvas.WorldToCanvas(cornerBottomRight + fo)), ToPoint(Canvas.WorldToCanvas(cornerBottomRight + up * handleSize + fo)));
				}

				graphics.DrawString(lo, AttachedControl.Font, labelBrush, posLo.X, posLo.Y);
				graphics.DrawString(lx, AttachedControl.Font, labelBrush, posLx.X, posLx.Y);
				graphics.DrawString(ly, AttachedControl.Font, labelBrush, posLy.X, posLy.Y);
			}
		}

		private Point ToPoint(Vector2 p)
		{
			const double min = -65536, max = 65535;

			var x = System.Math.Max(min, System.Math.Min(p.X, max));
			var y = System.Math.Max(min, System.Math.Min(p.Y, max));

			return new Point((int)x, (int)y);
		}

		public void Reset()
		{
			IsSurfaceHit = false;

			IsVertexOHit = false;
			IsVertexXHit = false;
			IsVertexYHit = false;

			IsEdgeOxHit = false;
			IsEdgeOyHit = false;
			IsEdgeXyHit = false;
		}

		public Polygon GetPolygon()
		{
			return new Polygon(new[] { GetVertexO(), GetVertexX(), GetVertexY() });
		}

		public Vector2 GetVertexO()
		{
			return Canvas.WorldToCanvas(mTransform.Origin);
		}
		public Vector2 GetVertexX()
		{
			return Canvas.WorldToCanvas(mTransform.Origin + mTransform.Affine.X);
		}
		public Vector2 GetVertexY()
		{
			return Canvas.WorldToCanvas(mTransform.Origin + mTransform.Affine.Y);
		}

		public Line GetEdgeOx()
		{
			return new Line(GetVertexO(), GetVertexX());
		}
		public Line GetEdgeOy()
		{
			return new Line(GetVertexO(), GetVertexY());
		}
		public Line GetEdgeXy()
		{
			return new Line(GetVertexX(), GetVertexY());
		}

		public Transform Model
		{
			get { return mTransform; }
		}

		public bool IsHit
		{
			get
			{
				return IsSurfaceHit || IsVertexOHit || IsVertexXHit || IsVertexYHit || IsEdgeOxHit || IsEdgeOyHit || IsEdgeXyHit;
			}
		}

		public bool IsSurfaceHit { get; set; }

		public bool IsVertexOHit { get; set; }
		public bool IsVertexXHit { get; set; }
		public bool IsVertexYHit { get; set; }

		public bool IsEdgeOxHit { get; set; }
		public bool IsEdgeOyHit { get; set; }
		public bool IsEdgeXyHit { get; set; }

		public bool IsActive { get; set; }
		public bool IsSelected { get; set; }
	}
}