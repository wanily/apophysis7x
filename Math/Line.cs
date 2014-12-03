using System;
using System.Numerics;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Line
	{
		private readonly Vector2 mA;
		private readonly Vector2 mB;

		public Line(Vector2 a, Vector2 b)
		{
			if (a == null) throw new ArgumentNullException(@"a");
			if (b == null) throw new ArgumentNullException(@"b");

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

		public Vector2 NormalVector
		{
			get
			{
				var d = B - A;
				return new Vector2(-d.Y, d.X).Normal();
			}
		}

		public bool IsInProximity(Vector2 point, float epsilon = 1)
		{
			if (point == null) throw new ArgumentNullException(@"point");

			var delta = B - A;
			var deltaSq = delta*delta;
			var denom = deltaSq.X + deltaSq.Y;

			var p0 = point - A;
			var p1 = point - B;

			float dist;

			if (denom > float.Epsilon)
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
	}
}