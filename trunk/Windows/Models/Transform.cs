using System;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Models
{
	[PublicAPI]
	public class Transform
	{
		private readonly Flame mFlame;
		private readonly Matrix2X2 mAffineMatrix;
		private readonly Vector2 mAffineOrigin;

		public Transform([NotNull] Flame hostingFlame)
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;

			mAffineMatrix = new Matrix2X2();
			mAffineMatrix.X.X = 1.0;
			mAffineMatrix.Y.Y = 1.0;

			mAffineOrigin = new Vector2();
		}

		public Matrix2X2 Affine
		{
			get { return mAffineMatrix; }
		}
		public Vector2 Origin
		{
			get { return mAffineOrigin; }
		}

		public int Index
		{
			get { return mFlame.Transforms.IndexOf(this); }
		}

		public void Rotate(double angle)
		{
			var x = mAffineMatrix.X.Rotate(angle, mAffineOrigin);
			var y = mAffineMatrix.Y.Rotate(angle, mAffineOrigin);

			mAffineMatrix.X.X = x.X;
			mAffineMatrix.X.Y = x.Y;

			mAffineMatrix.Y.X = y.X;
			mAffineMatrix.Y.Y = y.Y;
		}
		public void Scale(double scale)
		{
			if (System.Math.Abs(scale) < double.Epsilon) throw new ArgumentOutOfRangeException("scale");

			mAffineMatrix.X.X *= scale;
			mAffineMatrix.X.Y *= scale;

			mAffineMatrix.Y.X *= scale;
			mAffineMatrix.Y.Y *= scale;
		}
		public void Move([NotNull] Vector2 offset)
		{
			if (offset == null) throw new ArgumentNullException("offset");

			mAffineOrigin.X += offset.X;
			mAffineOrigin.Y += offset.Y;
		}
	}
}
