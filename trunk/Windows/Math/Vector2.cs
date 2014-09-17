using System;
using System.Drawing;
using System.Globalization;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Vector2
	{
		private double mX, mY;
		private const double mEps = 1e-300;

		public Vector2()
		{
		}
		public Vector2(double x, double y) : this()
		{
			mX = x;
			mY = y;
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

		public Vector2 Add([CanBeNull] Vector2 other)
		{
			if (other == null)
				return Copy();

			return new Vector2
			{
				X = mX + other.X,
				Y = mY + other.Y
			};
		}
		public Vector2 Add(double value)
		{
			return new Vector2
			{
				X = mX + value,
				Y = mY + value
			};
		}

		public Vector2 Subtract([CanBeNull] Vector2 other)
		{
			if (other == null)
				return Copy();

			return new Vector2
			{
				X = mX - other.X,
				Y = mY - other.Y
			};
		}
		public Vector2 Subtract(double value)
		{
			return new Vector2
			{
				X = mX - value,
				Y = mY - value
			};
		}

		public Vector2 Multiply([CanBeNull] Vector2 other)
		{
			if (other == null)
				return Copy();

			return new Vector2
			{
				X = mX * other.X,
				Y = mY * other.Y
			};
		}
		public Vector2 Multiply(double value)
		{
			return new Vector2
			{
				X = mX * value,
				Y = mY * value
			};
		}

		public Vector2 Divide([CanBeNull] Vector2 other)
		{
			if (other == null)
				return Copy();

			return new Vector2
			{
				X = mX / other.X,
				Y = mY / other.Y
			};
		}
		public Vector2 Divide(double value)
		{
			return new Vector2
			{
				X = mX / value,
				Y = mY / value
			};
		}

		public Vector2 Abs()
		{
			return new Vector2
			{
				X = System.Math.Abs(mX),
				Y = System.Math.Abs(mY)
			};
		}
		public Vector2 Copy()
		{
			return new Vector2
			{
				X = mX,
				Y = mY
			};
		}

		public ReadOnlyVector2 Freeze()
		{
			return new ReadOnlyVector2(this);
		}

		public Vector2 Rotate(double angle, [NotNull] Vector2 origin)
		{
			if (origin == null) throw new ArgumentNullException("origin");

			if (System.Math.Abs(origin.X - X) < double.Epsilon && System.Math.Abs(origin.Y - Y) < double.Epsilon)
				return origin.Copy();

			var cos = System.Math.Cos(angle);
			var sin = System.Math.Sin(angle);

			var x = ((X - origin.X) * cos) - ((Y - origin.Y) * sin) + origin.X;
			var y = ((X - origin.Y) * sin) + ((Y - origin.Y) * cos) + origin.Y;

			return new Vector2(x, y);
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			if (a == null)
				return null;

			return a.Add(b);
		}
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			if (a == null)
				return null;

			return a.Subtract(b);
		}
		public static Vector2 operator *(Vector2 a, Vector2 b)
		{
			if (a == null)
				return null;

			return a.Multiply(b);
		}
		public static Vector2 operator /(Vector2 a, Vector2 b)
		{
			if (a == null)
				return null;

			return a.Divide(b);
		}

		public static Vector2 operator +(Vector2 a, double b)
		{
			if (a == null)
				return null;

			return a.Add(b);
		}
		public static Vector2 operator -(Vector2 a, double b)
		{
			if (a == null)
				return null;

			return a.Subtract(b);
		}
		public static Vector2 operator *(Vector2 a, double b)
		{
			if (a == null)
				return null;

			return a.Multiply(b);
		}
		public static Vector2 operator /(Vector2 a, double b)
		{
			if (a == null)
				return null;

			return a.Divide(b);
		}

		public static Vector2 operator +(double a, Vector2 b)
		{
			return b.Add(a);
		}
		public static Vector2 operator -(double a, Vector2 b)
		{
			return new Vector2{X = a, Y = a}.Subtract(b);
		}
		public static Vector2 operator *(double a, Vector2 b)
		{
			return b.Multiply(a);
		}
		public static Vector2 operator /(double a, Vector2 b)
		{
			return new Vector2 { X = a, Y = a }.Divide(b);
		}

		public double Length
		{
			get { return System.Math.Sqrt(X*X + Y*Y); }
		}
		public Vector2 Direction
		{
			get
			{
				var length = Length;
				if (length < mEps)
				{
					return new Vector2();
				}

				return new Vector2
				{
					X = mX/length,
					Y = mY/length
				};
			}
		}

		public bool IsNaN
		{
			get { return double.IsNaN(mX) || double.IsNaN(mY); }
		}
		public bool IsInProximity([NotNull] Vector2 point, double epsilon = 1)
		{
			if (point == null) throw new ArgumentNullException("point");
			return (this - point).Length < System.Math.Abs(epsilon);
		}
		public bool IsZero
		{
			get { return System.Math.Abs(mX) < double.Epsilon && System.Math.Abs(mY) < double.Epsilon; }
		}

		public Point ToPoint()
		{
			const double min = -65536, max = 65535;

			if (IsNaN)
			{
				return new Point((int)min, (int)min);
			}

			var x = System.Math.Max(min, System.Math.Min(X, max));
			var y = System.Math.Max(min, System.Math.Min(Y, max));

			return new Point((int)x, (int)y);
		}
		public override string ToString()
		{
			return string.Format("{0}, {1}", X.ToString(CultureInfo.InvariantCulture), Y.ToString(CultureInfo.InvariantCulture));
		}
	}
}