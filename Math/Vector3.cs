using System;
using System.Globalization;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Vector3
	{
		private double mX, mY, mZ;
		private const double mEps = 1e-300;

		public Vector3()
		{
		}
		public Vector3(double x, double y, double z)
			: this()
		{
			mX = x;
			mY = y;
			mZ = z;
		}

		public double X
		{
			get { return mX; }
			set { mX = value; }
		}
		public double Y
		{
			get { return mY; }
			set { mY = value; }
		}
		public double Z
		{
			get { return mZ; }
			set { mZ = value; }
		}

		public Vector3 Add([CanBeNull] Vector3 other)
		{
			if (other == null)
				return Copy();

			return new Vector3
			{
				X = mX + other.X,
				Y = mY + other.Y,
				Z = mZ + other.Z
			};
		}
		public Vector3 Add(double value)
		{
			return new Vector3
			{
				X = mX + value,
				Y = mY + value,
				Z = mZ + value
			};
		}

		public Vector3 Subtract([CanBeNull] Vector3 other)
		{
			if (other == null)
				return Copy();

			return new Vector3
			{
				X = mX - other.X,
				Y = mY - other.Y,
				Z = mZ - other.Z
			};
		}
		public Vector3 Subtract(double value)
		{
			return new Vector3
			{
				X = mX - value,
				Y = mY - value,
				Z = mZ - value
			};
		}

		public Vector3 Multiply([CanBeNull] Vector3 other)
		{
			if (other == null)
				return Copy();

			return new Vector3
			{
				X = mX * other.X,
				Y = mY * other.Y,
				Z = mZ * other.Z
			};
		}
		public Vector3 Multiply(double value)
		{
			return new Vector3
			{
				X = mX * value,
				Y = mY * value,
				Z = mZ * value
			};
		}

		public Vector3 Divide([CanBeNull] Vector3 other)
		{
			if (other == null)
				return Copy();

			return new Vector3
			{
				X = mX / other.X,
				Y = mY / other.Y,
				Z = mZ / other.Z
			};
		}
		public Vector3 Divide(double value)
		{
			return new Vector3
			{
				X = mX / value,
				Y = mY / value,
				Z = mZ / value
			};
		}

		public Vector3 Abs()
		{
			return new Vector3
			{
				X = System.Math.Abs(mX),
				Y = System.Math.Abs(mY),
				Z = System.Math.Abs(mZ)
			};
		}
		public Vector3 Copy()
		{
			return new Vector3
			{
				X = mX,
				Y = mY,
				Z = mZ
			};
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			if (a == null)
				return null;

			return a.Add(b);
		}
		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			if (a == null)
				return null;

			return a.Subtract(b);
		}
		public static Vector3 operator *(Vector3 a, Vector3 b)
		{
			if (a == null)
				return null;

			return a.Multiply(b);
		}
		public static Vector3 operator /(Vector3 a, Vector3 b)
		{
			if (a == null)
				return null;

			return a.Divide(b);
		}

		public static Vector3 operator +(Vector3 a, double b)
		{
			if (a == null)
				return null;

			return a.Add(b);
		}
		public static Vector3 operator -(Vector3 a, double b)
		{
			if (a == null)
				return null;

			return a.Subtract(b);
		}
		public static Vector3 operator *(Vector3 a, double b)
		{
			if (a == null)
				return null;

			return a.Multiply(b);
		}
		public static Vector3 operator /(Vector3 a, double b)
		{
			if (a == null)
				return null;

			return a.Divide(b);
		}

		public static Vector3 operator +(double a, Vector3 b)
		{
			return b.Add(a);
		}
		public static Vector3 operator -(double a, Vector3 b)
		{
			return new Vector3 { X = a, Y = a, Z = a }.Subtract(b);
		}
		public static Vector3 operator *(double a, Vector3 b)
		{
			return b.Multiply(a);
		}
		public static Vector3 operator /(double a, Vector3 b)
		{
			return new Vector3 { X = a, Y = a, Z = a }.Divide(b);
		}

		public double Length
		{
			get { return System.Math.Sqrt(X * X + Y * Y + Z * Z); }
		}
		public Vector3 Direction
		{
			get
			{
				var length = Length;
				if (length < mEps)
				{
					return new Vector3();
				}

				return new Vector3
				{
					X = mX / length,
					Y = mY / length,
					Z = mZ / length
				};
			}
		}

		public bool IsNaN
		{
			get { return double.IsNaN(mX) || double.IsNaN(mY) || double.IsNaN(mZ); }
		}
		public bool IsInProximity([NotNull] Vector3 point, double epsilon = 1)
		{
			if (point == null) throw new ArgumentNullException(@"point");
			return (this - point).Length < System.Math.Abs(epsilon);
		}
		public bool IsZero
		{
			get { return System.Math.Abs(mX) < double.Epsilon && System.Math.Abs(mY) < double.Epsilon && System.Math.Abs(mZ) < double.Epsilon; }
		}

		public override string ToString()
		{
			return string.Format(@"{0}, {1}, {2}", X.ToString(CultureInfo.InvariantCulture), Y.ToString(CultureInfo.InvariantCulture), Z.ToString(CultureInfo.InvariantCulture));
		}
	}
}