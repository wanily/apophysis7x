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
			mMatrix = new Matrix2X2
			{
				X = {X = 1}, 
				Y = {Y = 1}
			};
		}

		[NotNull]
		public Vector2 Origin
		{
			get { return mOrigin; }
			set
			{
				if (value == null) throw new ArgumentNullException(@"value");
				mOrigin = value;
			}
		}

		[NotNull]
		public Matrix2X2 Matrix
		{
			get { return mMatrix; }
			set
			{
				if (value == null) throw new ArgumentNullException(@"value");
				mMatrix = value;
			}
		}

		public bool IsIdentity
		{
			get
			{
				return 
					   System.Math.Abs(Matrix.X.X - 1) < double.Epsilon
					&& System.Math.Abs(Matrix.X.Y) < double.Epsilon
					&& System.Math.Abs(Matrix.Y.X) < double.Epsilon
					&& System.Math.Abs(Matrix.Y.Y - 1) < double.Epsilon
					&& System.Math.Abs(Origin.X) < double.Epsilon
					&& System.Math.Abs(Origin.Y) < double.Epsilon;
			}
		}

		[NotNull]
		public Vector2 TransformPoint([NotNull] Vector2 point)
		{
			if (point == null) throw new ArgumentNullException(@"point");
			return new Vector2
			{
				X = Matrix.X.X * point.X + Matrix.Y.X * point.Y + Origin.X,
				Y = Matrix.X.Y * point.X + Matrix.Y.Y * point.Y + Origin.Y,
			};
		}

		public void MatrixTransform(Matrix2X2 operand)
		{
			var m1 = new[,]
			{
				{ Matrix.X.X, Matrix.X.Y, 0 },
				{ Matrix.Y.X, Matrix.Y.Y, 0 },
				{ Origin.X, Origin.Y, 0 }
			};

			var m2 = new[,]
			{
				{ operand.X.X, operand.X.Y, 0 },
				{ operand.Y.X, operand.Y.Y, 0 },
				{ 0, 0, 0 }
			};

			var result = new double[3, 3];

			result[0, 0] = m1[0, 0] * m2[0, 0] + m1[0, 1] * m2[1, 0] + m1[0, 2] * m2[2, 0];
			result[0, 1] = m1[0, 0] * m2[0, 1] + m1[0, 1] * m2[1, 1] + m1[0, 2] * m2[2, 1];
			result[0, 2] = m1[0, 0] * m2[0, 2] + m1[0, 1] * m2[1, 2] + m1[0, 2] * m2[2, 2];
			result[1, 0] = m1[1, 0] * m2[0, 0] + m1[1, 1] * m2[1, 0] + m1[1, 2] * m2[2, 0];
			result[1, 1] = m1[1, 0] * m2[0, 1] + m1[1, 1] * m2[1, 1] + m1[1, 2] * m2[2, 1];
			result[1, 2] = m1[1, 0] * m2[0, 2] + m1[1, 1] * m2[1, 2] + m1[1, 2] * m2[2, 2];
			result[2, 0] = m1[2, 0] * m2[0, 0] + m1[2, 1] * m2[1, 0] + m1[2, 2] * m2[2, 0];
			result[2, 0] = m1[2, 0] * m2[0, 1] + m1[2, 1] * m2[1, 1] + m1[2, 2] * m2[2, 1];
			result[2, 0] = m1[2, 0] * m2[0, 2] + m1[2, 1] * m2[1, 2] + m1[2, 2] * m2[2, 2];

			Matrix.X.X = result[0, 0];
			Matrix.X.Y = result[0, 1];
			Matrix.Y.X = result[1, 0];
			Matrix.Y.Y = result[1, 1];

			Origin.X = result[2, 0];
			Origin.Y = result[2, 1];
		}

		public void Rotate(double angle)
		{
			var x = Matrix.X.Rotate(angle, new Vector2());
			var y = Matrix.Y.Rotate(angle, new Vector2());

			Matrix.X.X = x.X;
			Matrix.X.Y = x.Y;

			Matrix.Y.X = y.X;
			Matrix.Y.Y = y.Y;
		}
		public void Scale(double scale)
		{
			if (System.Math.Abs(scale) < double.Epsilon) throw new ArgumentOutOfRangeException(@"scale");

			Matrix.X.X *= scale;
			Matrix.X.Y *= scale;

			Matrix.Y.X *= scale;
			Matrix.Y.Y *= scale;
		}
		public void Move([NotNull] Vector2 offset)
		{
			if (offset == null) throw new ArgumentNullException(@"offset");

			Origin.X += offset.X;
			Origin.Y += offset.Y;
		}

		public bool IsEqual([NotNull] AffineTransform transform)
		{
			if (transform == null) throw new ArgumentNullException(@"transform");

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
