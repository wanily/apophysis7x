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

			var ox = GetEdgeOx();
			var oy = GetEdgeOy();
			var xy = GetEdgeXy();

			var color = mColors[mTransform.Index%mColors.Length];
			var translucentColor = Color.FromArgb(0x80, color.R, color.G, color.B);
			var translucentColorLow = Color.FromArgb(0x33, color.R, color.G, color.B);

			using (var hitVertexBrush = new SolidBrush(color))
			using (var vertexBrush = new SolidBrush(translucentColor))
			using (var lowFillBrush = new SolidBrush(translucentColorLow))
			using (var fillBrush = new SolidBrush(translucentColor))
			using (var hitLinePen = new Pen(color, 2.0f))
			using (var linePen = new Pen(color))
			{
				graphics.FillPolygon(IsSurfaceHit ? fillBrush : lowFillBrush, new[] { ToPoint(ox.A), ToPoint(ox.B), ToPoint(oy.B) });

				graphics.DrawLine(IsEdgeOxHit ? hitLinePen : linePen, ToPoint(ox.A), ToPoint(ox.B));
				graphics.DrawLine(IsEdgeOyHit ? hitLinePen : linePen, ToPoint(oy.A), ToPoint(oy.B));
				graphics.DrawLine(IsEdgeXyHit ? hitLinePen : linePen, ToPoint(xy.A), ToPoint(xy.B));

				var o = ToPoint(ox.A - new Vector2(vertexRadius, vertexRadius));
				var x = ToPoint(ox.B - new Vector2(vertexRadius, vertexRadius));
				var y = ToPoint(oy.B - new Vector2(vertexRadius, vertexRadius));

				var size = new Size(2 * vertexRadius, 2 * vertexRadius);

				var ro = new Rectangle(o, size);
				var rx = new Rectangle(x, size);
				var ry = new Rectangle(y, size);

				graphics.FillEllipse(IsVertexOHit ? hitVertexBrush : vertexBrush, ro);
				graphics.FillEllipse(IsVertexXHit ? hitVertexBrush : vertexBrush, rx);
				graphics.FillEllipse(IsVertexYHit ? hitVertexBrush : vertexBrush, ry);

				graphics.DrawEllipse(IsVertexOHit ? hitLinePen : linePen, ro);
				graphics.DrawEllipse(IsVertexXHit ? hitLinePen : linePen, rx);
				graphics.DrawEllipse(IsVertexYHit ? hitLinePen : linePen, ry);
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

		public bool IsSurfaceHit { get; set; }

		public bool IsVertexOHit { get; set; }
		public bool IsVertexXHit { get; set; }
		public bool IsVertexYHit { get; set; }

		public bool IsEdgeOxHit { get; set; }
		public bool IsEdgeOyHit { get; set; }
		public bool IsEdgeXyHit { get; set; }

		public bool IsActive { get; set; }
	}
}