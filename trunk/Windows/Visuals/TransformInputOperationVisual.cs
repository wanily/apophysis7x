using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Visuals
{
	[PublicAPI]
	public class TransformInputOperationVisual : CanvasVisual<Canvas>
	{
		private TransformInputOperation mOperation;
		private Color mReferenceColor;

		public TransformInputOperationVisual([NotNull] Control control, [NotNull] Canvas canvas) : base(control, canvas)
		{
			mReferenceColor = Color.Gray;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mOperation = null;
		}

		public TransformInputOperation Operation
		{
			get { return mOperation; }
			set { mOperation = value; }
		}
		public Color ReferenceColor
		{
			get { return mReferenceColor; }
			set
			{
				mReferenceColor = value;
				InvalidateControl();
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			if (mOperation == null)
				return;

			using (var referenceBrush = new SolidBrush(ReferenceColor))
			using (var referencePen = new Pen(referenceBrush))
			{
				var move = mOperation as TransformMoveOperation;
				if (move != null)
				{
					var origin = Canvas.WorldToCanvas(move.Origin);
					var current = Canvas.WorldToCanvas(move.Current);

					var lh = new Line(new Vector2(0, origin.Y), new Vector2(Canvas.Size.X, origin.Y));
					var lv = new Line(new Vector2(origin.X, 0), new Vector2(origin.X, Canvas.Size.Y));

					var ch = new Line(new Vector2(0, current.Y), new Vector2(Canvas.Size.X, current.Y));
					var cv = new Line(new Vector2(current.X, 0), new Vector2(current.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, lh.A.ToPoint(), lh.B.ToPoint());
					graphics.DrawLine(referencePen, lv.A.ToPoint(), lv.B.ToPoint());

					graphics.DrawLine(referencePen, ch.A.ToPoint(), ch.B.ToPoint());
					graphics.DrawLine(referencePen, cv.A.ToPoint(), cv.B.ToPoint());
				}

				var rotate = mOperation as TransformRotateOperation;
				if (rotate != null)
				{
					var origin = Canvas.WorldToCanvas(rotate.Transform.Origin);
					var x = Canvas.WorldToCanvas(rotate.Transform.Affine.X + rotate.Transform.Origin);

					var farRadius = System.Math.Max(Canvas.Size.X, Canvas.Size.Y);
					var radius = System.Math.Max(rotate.Transform.Affine.X.Length, rotate.Transform.Affine.Y.Length) * (Canvas.Ratio.X + Canvas.Ratio.Y) * 0.5;
					var rect = new System.Drawing.Rectangle((int)(origin.X - radius), (int)(origin.Y - radius), (int)(radius * 2), (int)(radius * 2));

					var lh = new Line((x - origin).Direction * -farRadius + origin, (x - origin).Direction * farRadius + origin);
					var lv = new Line(lh.GetNormal() * -radius + origin, lh.GetNormal() * radius + origin);

					var loh = new Line(new Vector2(0, origin.Y), new Vector2(Canvas.Size.X, origin.Y));
					var lov = new Line(new Vector2(origin.X, 0), new Vector2(origin.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, loh.A.ToPoint(), loh.B.ToPoint());
					graphics.DrawLine(referencePen, lov.A.ToPoint(), lov.B.ToPoint());
					
					graphics.DrawEllipse(referencePen, rect);

					graphics.DrawLine(referencePen, lh.A.ToPoint(), lh.B.ToPoint());
					graphics.DrawLine(referencePen, lv.A.ToPoint(), lv.B.ToPoint());
				}

				var scale = mOperation as TransformScaleOperation;
				if (scale != null)
				{
					var origin = Canvas.WorldToCanvas(scale.Transform.Origin);
					var x = Canvas.WorldToCanvas(scale.Transform.Affine.X + scale.Transform.Origin);
					var y = Canvas.WorldToCanvas(scale.Transform.Affine.Y + scale.Transform.Origin);

					var farRadius = System.Math.Max(Canvas.Size.X, Canvas.Size.Y);

					var lx = new Line((x - origin).Direction * -farRadius + origin, (x - origin).Direction * farRadius + origin);
					var ly = new Line((y - origin).Direction * -farRadius + origin, (y - origin).Direction * farRadius + origin);

					var loh = new Line(new Vector2(0, origin.Y), new Vector2(Canvas.Size.X, origin.Y));
					var lov = new Line(new Vector2(origin.X, 0), new Vector2(origin.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, loh.A.ToPoint(), loh.B.ToPoint());
					graphics.DrawLine(referencePen, lov.A.ToPoint(), lov.B.ToPoint());

					graphics.DrawLine(referencePen, lx.A.ToPoint(), lx.B.ToPoint());
					graphics.DrawLine(referencePen, ly.A.ToPoint(), ly.B.ToPoint());
				}
			}
		}
	}
}