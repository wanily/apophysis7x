using System;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class AffineTransform
	{
		private Vector2 mOrigin;
		private Matrix2X2 mMatrix;

		public AffineTransform()
		{
			mOrigin = new Vector2();
			mMatrix = new Matrix2X2();

			mMatrix.X.X = 1;
			mMatrix.Y.Y = 1;
		}

		[NotNull]
		public Vector2 Origin
		{
			get { return mOrigin; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mOrigin = value;
			}
		}

		[NotNull]
		public Matrix2X2 Matrix
		{
			get { return mMatrix; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mMatrix = value;
			}
		}

		public void Rotate(double angle)
		{
			var x = Matrix.X.Rotate(angle, Origin);
			var y = Matrix.Y.Rotate(angle, Origin);

			Matrix.X.X = x.X;
			Matrix.X.Y = x.Y;

			Matrix.Y.X = y.X;
			Matrix.Y.Y = y.Y;
		}
		public void Scale(double scale)
		{
			if (System.Math.Abs(scale) < double.Epsilon) throw new ArgumentOutOfRangeException("scale");

			Matrix.X.X *= scale;
			Matrix.X.Y *= scale;

			Matrix.Y.X *= scale;
			Matrix.Y.Y *= scale;
		}
		public void Move([NotNull] Vector2 offset)
		{
			if (offset == null) throw new ArgumentNullException("offset");

			Origin.X += offset.X;
			Origin.Y += offset.Y;
		}

		public bool IsEqual([NotNull] AffineTransform transform)
		{
			if (transform == null) throw new ArgumentNullException("transform");

			if (!Equals(mOrigin.X, transform.mOrigin.X))
				return false;

			if (!Equals(mOrigin.Y, transform.mOrigin.Y))
				return false;

			if (!Equals(mMatrix.X.X, transform.mMatrix.X.X))
				return false;

			if (!Equals(mMatrix.X.Y, transform.mMatrix.X.Y))
				return false;

			if (!Equals(mMatrix.Y.X, transform.mMatrix.Y.X))
				return false;

			if (!Equals(mMatrix.Y.Y, transform.mMatrix.Y.Y))
				return false;

			return true;
		}
	}
}
