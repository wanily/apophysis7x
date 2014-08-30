using System;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Line
	{
		private readonly Vector2 mA;
		private readonly Vector2 mB;
		private const double mEps = 1e-300;

		public Line([NotNull] Vector2 a, [NotNull] Vector2 b)
		{
			if (a == null) throw new ArgumentNullException("a");
			if (b == null) throw new ArgumentNullException("b");

			mA = a;
			mB = b;
		}

		public Vector2 A
		{
			get { return mA; }
		}
		public Vector2 B
		{
			get { return mB; }
		}

		public bool IsInProximity([NotNull] Vector2 point, double epsilon = 1)
		{
			if (point == null) throw new ArgumentNullException("point");

			var delta = B - A;
			var deltaSq = delta*delta;
			var denom = deltaSq.X + deltaSq.Y;

			var p0 = point - A;
			var p1 = point - B;

			double dist;

			if (denom > mEps)
			{
				var t = (p0.X*delta.X + p0.Y*delta.Y)/denom;

				if (t < 0)
				{
					dist = p0.X*p0.X + p0.Y*p0.Y;
				}
				else if (t > 1)
				{
					dist = p1.X*p1.X + p1.Y*p1.Y;
				}
				else
				{
					var pp = A + t*delta;
					dist = (point.X - pp.X)*(point.X - pp.X) + (point.Y - pp.Y)*(point.Y - pp.Y);
				}
			}
			else
			{
				dist = p0.X*p0.X + p0.Y*p0.Y;
			}

			return System.Math.Sqrt(dist) < epsilon;
		}
		public bool IsIntersecting([NotNull] Line other)
		{
			if (other == null) throw new ArgumentNullException("other");

			return GetIntersectionPoint(other).IsNaN;
		}

		public Vector2 GetIntersectionPoint([NotNull] Line other)
		{
			if (other == null) throw new ArgumentNullException("other");

			var direction = new Line(other.A - A, other.B - B);

			var delta = direction.A.Y*-direction.B.X - direction.B.Y*-direction.A.X;
			if (System.Math.Abs(delta) < mEps)
				return Vector2.NaN;

			var uu = direction.A.Y*A.X - direction.A.X*A.Y;
			var vv = direction.B.Y*B.X - direction.B.X*B.Y;

			return new Vector2
			{
				X = (-direction.B.X*uu + direction.A.X*vv)/delta,
				Y = (direction.A.Y*vv - direction.B.Y*uu)/delta
			};
		}
		public Vector2 GetNormal()
		{
			var d = B - A;
			return new Vector2(-d.Y, d.X).Direction;
		}

		public Line Rotate(double angle, [NotNull] Vector2 origin)
		{
			if (origin == null) throw new ArgumentNullException("origin");
			return new Line(A.Rotate(angle, origin), B.Rotate(angle, origin));
		}
	}
}