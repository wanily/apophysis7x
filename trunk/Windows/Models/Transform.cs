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

		public Transform(Flame hostingFlame)
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
	}
}
