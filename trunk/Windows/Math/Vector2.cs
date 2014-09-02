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

		public Vector2 Transform([NotNull] Matrix2X2 matrix, Vector2 origin = null)
		{
			if (matrix == null) throw new ArgumentNullException("matrix");

			return Transform(matrix.X, matrix.Y, origin);
		}
		public Vector2 Transform([NotNull] Vector2 dirX, [NotNull] Vector2 dirY, Vector2 origin = null)
		{
			if (dirX == null) throw new ArgumentNullException("dirX");
			if (dirY == null) throw new ArgumentNullException("dirY");

			var o = origin ?? new Vector2(0, 0);

			return new Vector2
			{
				X = dirX.X * X + dirX.X * Y + o.X,
				Y = dirY.X * X + dirY.Y * Y + o.Y
			};
		}

		public Vector2 Rotate(double angle, [NotNull] Vector2 origin)
		{
			if (origin == null) throw new ArgumentNullException("origin");

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

		public static Vector2 Diff([NotNull] Vector2 a, [NotNull] Vector2 b, [NotNull] Vector2 c)
		{
			if (a == null) throw new ArgumentNullException("a");
			if (b == null) throw new ArgumentNullException("b");
			if (c == null) throw new ArgumentNullException("c");

			var delta1 = (b - a);
			var delta2 = (c - a);

			var dist = delta1.Abs();

			double x, y;

			if (dist.X > mEps)
			{
				x = delta2.X / delta1.X;
			}
			else
			{
				x = 0;
			}

			if (dist.Y > mEps)
			{
				y = delta2.Y / delta1.Y;
			}
			else
			{
				y = 0;
			}

			return new Vector2 { X = x, Y = y };
		}
		public static Vector2 NaN
		{
			get { return new Vector2 {X = double.NaN, Y = double.NaN}; }
		}

		public Point ToPoint()
		{
			const double min = -65536, max = 65535;

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