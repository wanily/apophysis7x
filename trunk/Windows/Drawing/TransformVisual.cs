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

		protected override void OnControlPaint(Graphics graphics)
		{
			var ox = new Line(Canvas.WorldToCanvas(mTransform.Origin), Canvas.WorldToCanvas(mTransform.Affine.X));
			var oy = new Line(Canvas.WorldToCanvas(mTransform.Origin), Canvas.WorldToCanvas(mTransform.Affine.Y));
			var xy = new Line(Canvas.WorldToCanvas(mTransform.Affine.X), Canvas.WorldToCanvas(mTransform.Affine.Y));

			var color = mColors[mTransform.Index%mColors.Length];
			using (var linePen = new Pen(color))
			{
				graphics.DrawLine(linePen, new Point((int)ox.A.X, (int)ox.A.Y), new Point((int)ox.B.X, (int)ox.B.Y));
				graphics.DrawLine(linePen, new Point((int)oy.A.X, (int)oy.A.Y), new Point((int)oy.B.X, (int)oy.B.Y));
				graphics.DrawLine(linePen, new Point((int)xy.A.X, (int)xy.A.Y), new Point((int)xy.B.X, (int)xy.B.Y));
			}
		}

		public Transform Transform
		{
			get { return mTransform; }
		}
	}
}