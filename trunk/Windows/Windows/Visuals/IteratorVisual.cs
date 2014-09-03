using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class IteratorVisual : CanvasVisual<Canvas>
	{
		private Iterator mIterator;
		private IteratorMatrix mActiveMatrix;

		private static readonly Color[] mColors = 
		{
			Color.Red,
			Color.Yellow,
			Color.LightGreen,
			Color.Cyan,
			Color.Blue,
			Color.Magenta,
			Color.Orange,
			Color.LightSkyBlue,
			Color.MediumOrchid,
			Color.Salmon
		};

		public IteratorVisual([NotNull] Control control, [NotNull] Canvas canvas, [NotNull] Iterator model, IteratorMatrix activeMatrix) : base(control, canvas)
		{
			if (model == null) throw new ArgumentNullException("model");

			mIterator = model;
			mActiveMatrix = activeMatrix;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mIterator = null;
			mActiveMatrix = default(IteratorMatrix);
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

			var fo = Origin;
			var fx = Matrix.X;
			var fy = Matrix.Y;

			var ao = AlternativeOrigin;
			var ax = AlternativeMatrix.X;
			var ay = AlternativeMatrix.Y;

			var apx = Canvas.WorldToCanvas(ao + ax);
			var apy = Canvas.WorldToCanvas(ao + ay);
			var apxn = Canvas.WorldToCanvas(ao - ax);
			var apyn = Canvas.WorldToCanvas(ao - ay);

			var acorner = new[]
			{
				Canvas.WorldToCanvas(ao - ax + ay),
				Canvas.WorldToCanvas(ao + ax + ay),
				Canvas.WorldToCanvas(ao + ax - ay),
				Canvas.WorldToCanvas(ao - ax - ay)
			};

			var cornerTopLeft = fy - fx;
			var cornerTopRight = fy + fx;
			var cornerBottomLeft = -1 * fy - fx;
			var cornerBottomRight = -1 * fy + fx;

			var left = -1 * fx;
			var up = fy;
			var right = fx;
			var down = -1 * fy;

			var color = GetColor(Model);
			var translucentColorHalf = Color.FromArgb(0x80, color.R, color.G, color.B);
			var translucentColor = Color.FromArgb(0x40, color.R, color.G, color.B);
			var translucentColorLow = Color.FromArgb(0x05, color.R, color.G, color.B);

			var sizeLo = graphics.MeasureString(lo, AttachedControl.Font);
			var sizeLy = graphics.MeasureString(ly, AttachedControl.Font);

			var posLo = (ox.A + new Vector2(-distLabel - sizeLo.Width, distLabel)).ToPoint();
			var posLx = (ox.B + new Vector2(distLabel, distLabel)).ToPoint();
			var posLy = (oy.B + new Vector2(-distLabel - sizeLy.Width, -distLabel - sizeLy.Height)).ToPoint();

			var posO = (ox.A - new Vector2(vertexRadius, vertexRadius)).ToPoint();
			var posX = (ox.B - new Vector2(vertexRadius, vertexRadius)).ToPoint();
			var posY = (oy.B - new Vector2(vertexRadius, vertexRadius)).ToPoint();

			var vertexSize = new Size(2 * vertexRadius, 2 * vertexRadius);

			var rectO = new Rectangle(posO, vertexSize);
			var rectX = new Rectangle(posX, vertexSize);
			var rectY = new Rectangle(posY, vertexSize);

			using (var labelBrush = new SolidBrush(color))
			using (var hitVertexOBrush = new SolidBrush(AttachedControl.ForeColor))
			using (var hitVertexBrush = new SolidBrush(color))
			using (var vertexBrush = new SolidBrush(translucentColor))
			using (var lowFillBrush = new SolidBrush(translucentColorLow))
			using (var fillBrush = new SolidBrush(translucentColor))
			using (var widgetPen = new Pen(translucentColorHalf))
			using (var dashLinePen = new Pen(color))
			using (var dashWidgetPen = new Pen(translucentColorHalf))
			using (var hitLinePen = new Pen(color, 2.0f))
			using (var linePen = new Pen(color))
			{
				dashLinePen.DashPattern = new[] { 6.0f, 4.0f };
				dashWidgetPen.DashPattern = new[] { 6.0f, 4.0f };

				if (!AlternativeOrigin.IsZero || !AlternativeMatrix.IsLinear || mActiveMatrix == IteratorMatrix.PostAffine)
				{
					graphics.DrawLine(widgetPen, apxn.ToPoint(), apx.ToPoint());
					graphics.DrawLine(widgetPen, apyn.ToPoint(), apy.ToPoint());
					graphics.DrawLine(widgetPen, apx.ToPoint(), apy.ToPoint());

					graphics.DrawLine(dashWidgetPen, acorner[0].ToPoint(), acorner[1].ToPoint());
					graphics.DrawLine(dashWidgetPen, acorner[1].ToPoint(), acorner[2].ToPoint());
					graphics.DrawLine(dashWidgetPen, acorner[2].ToPoint(), acorner[3].ToPoint());
					graphics.DrawLine(dashWidgetPen, acorner[3].ToPoint(), acorner[0].ToPoint());
				}

				graphics.DrawLine(IsEdgeOxHit ? hitLinePen : IsSelected || IsHit ? linePen : dashLinePen, ox.A.ToPoint(), ox.B.ToPoint());
				graphics.DrawLine(IsEdgeOyHit ? hitLinePen : IsSelected || IsHit ? linePen : dashLinePen, oy.A.ToPoint(), oy.B.ToPoint());
				graphics.DrawLine(IsEdgeXyHit ? hitLinePen : dashLinePen, xy.A.ToPoint(), xy.B.ToPoint());

				graphics.FillEllipse(IsVertexOHit ? hitVertexOBrush : vertexBrush, rectO);
				graphics.FillEllipse(IsVertexXHit ? hitVertexBrush : vertexBrush, rectX);
				graphics.FillEllipse(IsVertexYHit ? hitVertexBrush : vertexBrush, rectY);

				graphics.DrawEllipse(IsVertexOHit ? hitLinePen : linePen, rectO);
				graphics.DrawEllipse(IsVertexXHit ? hitLinePen : linePen, rectX);
				graphics.DrawEllipse(IsVertexYHit ? hitLinePen : linePen, rectY);

				graphics.FillPolygon(IsHit ? fillBrush : lowFillBrush, new[] { ox.A.ToPoint(), ox.B.ToPoint(), oy.B.ToPoint() });

				if (IsSelected)
				{
					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerTopLeft + fo).ToPoint(), Canvas.WorldToCanvas(cornerTopLeft + right * handleSize + fo).ToPoint());
					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerTopLeft + fo).ToPoint(), Canvas.WorldToCanvas(cornerTopLeft + down * handleSize + fo).ToPoint());

					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerTopRight + fo).ToPoint(), Canvas.WorldToCanvas(cornerTopRight + left * handleSize + fo).ToPoint());
					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerTopRight + fo).ToPoint(), Canvas.WorldToCanvas(cornerTopRight + down * handleSize + fo).ToPoint());

					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerBottomLeft + fo).ToPoint(), Canvas.WorldToCanvas(cornerBottomLeft + right * handleSize + fo).ToPoint());
					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerBottomLeft + fo).ToPoint(), Canvas.WorldToCanvas(cornerBottomLeft + up * handleSize + fo).ToPoint());

					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerBottomRight + fo).ToPoint(), Canvas.WorldToCanvas(cornerBottomRight + left * handleSize + fo).ToPoint());
					graphics.DrawLine(IsHit ? linePen : widgetPen, Canvas.WorldToCanvas(cornerBottomRight + fo).ToPoint(), Canvas.WorldToCanvas(cornerBottomRight + up * handleSize + fo).ToPoint());
				}

				graphics.DrawString(lo, AttachedControl.Font, labelBrush, posLo.X, posLo.Y);
				graphics.DrawString(lx, AttachedControl.Font, labelBrush, posLx.X, posLx.Y);
				graphics.DrawString(ly, AttachedControl.Font, labelBrush, posLy.X, posLy.Y);
			}
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
			return Canvas.WorldToCanvas(Origin);
		}
		public Vector2 GetVertexX()
		{
			return Canvas.WorldToCanvas(Origin + Matrix.X);
		}
		public Vector2 GetVertexY()
		{
			return Canvas.WorldToCanvas(Origin + Matrix.Y);
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

		public Vector2 Origin
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return Model.PreAffine.Origin;
					case IteratorMatrix.PostAffine:
						return Model.PostAffine.Origin;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		public Matrix2X2 Matrix
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return Model.PreAffine.Matrix;
					case IteratorMatrix.PostAffine:
						return Model.PostAffine.Matrix;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		public Iterator Model
		{
			get { return mIterator; }
		}

		public Vector2 AlternativeOrigin
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return Model.PostAffine.Origin;
					case IteratorMatrix.PostAffine:
						return Model.PreAffine.Origin;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		public Matrix2X2 AlternativeMatrix
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return Model.PostAffine.Matrix;
					case IteratorMatrix.PostAffine:
						return Model.PreAffine.Matrix;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
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

		public Math.Rectangle GetBounds()
		{
			var fx = Canvas.WorldToCanvas(Matrix.X + Origin);
			var fy = Canvas.WorldToCanvas(Matrix.Y + Origin);

			var cornerTopLeft = new Vector2(System.Math.Min(fx.X, fy.X), System.Math.Min(fx.Y, fy.Y));
			var cornerBottomRight = new Vector2(System.Math.Max(fx.X, fy.X), System.Math.Max(fx.Y, fy.Y));

			return new Math.Rectangle(cornerTopLeft, cornerBottomRight - cornerTopLeft);
		}
		public static Color GetColor(Iterator iterator)
		{
			if (iterator.Index < 0)
				return Color.Transparent;

			return mColors[iterator.Index%mColors.Length];
		}
	}
}