using System;
using System.Drawing;
using System.Numerics;

namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class NumericExtensions
	{
		[Pure]
		public static Matrix3x2 Alter(this Matrix3x2 m, float? m11 = null, float? m12 = null, float? m21 = null, float? m22 = null, float? m31 = null, float? m32 = null)
		{
			var mo = new Matrix3x2(
				m11.GetValueOrDefault(m.M11), m12.GetValueOrDefault(m.M12), m21.GetValueOrDefault(m.M21),
				m22.GetValueOrDefault(m.M22), m31.GetValueOrDefault(m.M31), m32.GetValueOrDefault(m.M32));

			return mo;
		}

		[Pure]
		public static Matrix3x2 Move(this Matrix3x2 m, Vector2 offset)
		{
			return Alter(m, m31: offset.X + m.M31, m32: offset.Y + m.M32);
		}

		[Pure]
		public static Matrix3x2 Transform(this Matrix3x2 m, Matrix3x2 operand)
		{
			var m1 = new[,] { { m.M11, m.M12, 0 }, { m.M21, m.M22, 0 }, { m.M31, m.M32, 0 } };
			var m2 = new[,] { { operand.M11, operand.M12, 0 }, { operand.M21, operand.M22, 0 }, { operand.M31, operand.M32, 0 } };

			var result = new float[3, 3];

			result[0, 0] = m1[0, 0] * m2[0, 0] + m1[0, 1] * m2[1, 0] + m1[0, 2] * m2[2, 0];
			result[0, 1] = m1[0, 0] * m2[0, 1] + m1[0, 1] * m2[1, 1] + m1[0, 2] * m2[2, 1];
			result[0, 2] = m1[0, 0] * m2[0, 2] + m1[0, 1] * m2[1, 2] + m1[0, 2] * m2[2, 2];
			result[1, 0] = m1[1, 0] * m2[0, 0] + m1[1, 1] * m2[1, 0] + m1[1, 2] * m2[2, 0];
			result[1, 1] = m1[1, 0] * m2[0, 1] + m1[1, 1] * m2[1, 1] + m1[1, 2] * m2[2, 1];
			result[1, 2] = m1[1, 0] * m2[0, 2] + m1[1, 1] * m2[1, 2] + m1[1, 2] * m2[2, 2];
			result[2, 0] = m1[2, 0] * m2[0, 0] + m1[2, 1] * m2[1, 0] + m1[2, 2] * m2[2, 0];
			result[2, 0] = m1[2, 0] * m2[0, 1] + m1[2, 1] * m2[1, 1] + m1[2, 2] * m2[2, 1];
			result[2, 0] = m1[2, 0] * m2[0, 2] + m1[2, 1] * m2[1, 2] + m1[2, 2] * m2[2, 2];

			return new Matrix3x2(result[0,0], result[0,1], result[1,0], result[1,1], result[2,0], result[2,1]);
		}

		[Pure]
		public static Vector2 Transform(this Matrix3x2 m, Vector2 operand)
		{
			return new Vector2
			{
				X = m.M11 * operand.X + m.M21 * operand.Y + m.M31,
				Y = m.M12 * operand.X + m.M22 * operand.Y + m.M32,
			};
		}

		[Pure]
		public static Vector2 Rotate(this Vector2 v, float angle, Vector2 origin = default(Vector2))
		{
			if (origin == null) throw new ArgumentNullException(@"origin");

			if (System.Math.Abs(origin.X - v.X) < float.Epsilon && System.Math.Abs(origin.Y - v.Y) < float.Epsilon)
				return origin;

			var cos = Float.Sin(angle);
			var sin = (float)System.Math.Sin(angle);

			var x = ((v.X - origin.X) * cos) - ((v.Y - origin.Y) * sin) + origin.X;
			var y = ((v.X - origin.Y) * sin) + ((v.Y - origin.Y) * cos) + origin.Y;

			return new Vector2(x, y);
		}

		[Pure]
		public static Matrix3x2 Rotate(this Matrix3x2 m, float angle)
		{
			var x = new Vector2(m.M11, m.M12).Rotate(angle);
			var y = new Vector2(m.M21, m.M22).Rotate(angle);

			return Alter(m, x.X, x.Y, y.X, y.Y);
		}

		[Pure]
		public static Matrix3x2 Scale(this Matrix3x2 m, float scale)
		{
			if (System.Math.Abs(scale) < float.Epsilon)
				throw new ArgumentOutOfRangeException(@"scale");

			var x = new Vector2(m.M11, m.M12) * scale;
			var y = new Vector2(m.M21, m.M22) * scale;

			return Alter(m, x.X, x.Y, y.X, y.Y);
		}

		[Pure]
		public static bool IsInProximity(this Vector2 v, Vector2 point, float epsilon = 1)
		{
			if (point == null) throw new ArgumentNullException("point");
			return (v - point).Length() < Float.Abs(epsilon);
		}

		[Pure]
		public static PointF ToPointF(this Vector2 v)
		{
			return new PointF(v.X, v.Y);
		}

		[Pure]
		public static Point ToPoint(this Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		[Pure]
		public static Vector2 Normal(this Vector2 v)
		{
			return v/v.Length();
		}

		[Pure]
		public static float NextFloat([NotNull] this Random r)
		{
			if (r == null) throw new ArgumentNullException("r");

			//todo x: who needs control over entropy anyway! no seriously, there's gotta be a better way ;)
			return (float)r.NextDouble();
		}
	}
}