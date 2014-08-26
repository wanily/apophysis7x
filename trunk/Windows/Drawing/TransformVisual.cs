using System;
using System.Drawing;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Drawing
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

		public TransformVisual([NotNull] Canvas canvas, [NotNull] Transform transform) : base(canvas)
		{
			if (transform == null) throw new ArgumentNullException("transform");
			mTransform = transform;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mTransform = null;
		}

		private Point ToPoint(Vector2 p)
		{
			const double min = -65536, max = 65535;

			var x = System.Math.Max(min, System.Math.Min(p.X, max));
			var y = System.Math.Max(min, System.Math.Min(p.Y, max));

			return new Point((int)x, (int)y);
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var ox = new Line(Canvas.WorldToCanvas(mTransform.Origin), Canvas.WorldToCanvas(mTransform.Affine.X + mTransform.Origin));
			var oy = new Line(Canvas.WorldToCanvas(mTransform.Origin), Canvas.WorldToCanvas(mTransform.Affine.Y + mTransform.Origin));
			var xy = new Line(Canvas.WorldToCanvas(mTransform.Affine.X + mTransform.Origin), Canvas.WorldToCanvas(mTransform.Affine.Y + mTransform.Origin));

			var color = mColors[mTransform.Index%mColors.Length];
			using (var linePen = new Pen(color))
			{
				graphics.DrawLine(linePen, ToPoint(ox.A), ToPoint(ox.B));
				graphics.DrawLine(linePen, ToPoint(oy.A), ToPoint(oy.B));
				graphics.DrawLine(linePen, ToPoint(xy.A), ToPoint(xy.B));
			}
		}

		public Transform Transform
		{
			get { return mTransform; }
		}
	}
}