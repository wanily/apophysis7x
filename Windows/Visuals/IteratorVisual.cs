using System;
using System.Drawing;
using System.Numerics;
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
			const float handleSize = 0.2f;
			const int distLabel = 4;

			const string lo = "O";
			const string lx = "X";
			const string ly = "Y";

			var ox = GetEdgeOx();
			var oy = GetEdgeOy();
			var xy = GetEdgeXy();

			var fo = Origin;
			var fx = X;
			var fy = Y;

			var ao = InactiveOrigin;
			var ax = InactiveX;
			var ay = InactiveY;

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

			var color = Model.GetColor();
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

				if (!InactiveMatrix.IsIdentity || mActiveMatrix == IteratorMatrix.PostAffine)
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
			return Canvas.WorldToCanvas(Origin + X);
		}
		public Vector2 GetVertexY()
		{
			return Canvas.WorldToCanvas(Origin + Y);
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
						return new Vector2(Model.PreAffine.M31, Model.PreAffine.M32);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PostAffine.M31, Model.PostAffine.M32);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						Model.PreAffine = Model.PreAffine.Alter(m31: value.X, m32: value.Y);
						break;
					case IteratorMatrix.PostAffine:
						Model.PostAffine = Model.PostAffine.Alter(m31: value.X, m32: value.Y);
						break;
				}
			}
		}
		public Vector2 X
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PreAffine.M11, Model.PreAffine.M12);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PostAffine.M11, Model.PostAffine.M12);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						Model.PreAffine = Model.PreAffine.Alter(value.X, value.Y);
						break;
					case IteratorMatrix.PostAffine:
						Model.PostAffine = Model.PostAffine.Alter(value.X, value.Y);
						break;
				}
			}
		}
		public Vector2 Y
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PreAffine.M21, Model.PreAffine.M22);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PostAffine.M21, Model.PostAffine.M22);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						Model.PreAffine = Model.PreAffine.Alter(m21: value.X, m22: value.Y);
						break;
					case IteratorMatrix.PostAffine:
						Model.PostAffine = Model.PostAffine.Alter(m21: value.X, m22: value.Y);
						break;
				}
			}
		}
		public Iterator Model
		{
			get { return mIterator; }
		}


		public Matrix3x2 InactiveMatrix
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return Model.PostAffine;
					case IteratorMatrix.PostAffine:
						return Model.PreAffine;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		public Vector2 InactiveOrigin
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PostAffine.M31, Model.PostAffine.M32);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PreAffine.M31, Model.PreAffine.M32);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		public Vector2 InactiveX
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PostAffine.M11, Model.PostAffine.M12);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PreAffine.M11, Model.PreAffine.M12);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		public Vector2 InactiveY
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PostAffine.M21, Model.PostAffine.M22);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PreAffine.M21, Model.PreAffine.M22);
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
			var fx = Canvas.WorldToCanvas(X + Origin);
			var fy = Canvas.WorldToCanvas(Y + Origin);

			var cornerTopLeft = new Vector2(System.Math.Min(fx.X, fy.X), System.Math.Min(fx.Y, fy.Y));
			var cornerBottomRight = new Vector2(System.Math.Max(fx.X, fy.X), System.Math.Max(fx.Y, fy.Y));

			return new Math.Rectangle(cornerTopLeft, cornerBottomRight - cornerTopLeft);
		}
	}
}