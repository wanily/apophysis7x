namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class Float
	{
		public const float Pi = 3.14159f;

		[Pure]
		public static float Atan2(float y, float x)
		{
			//todo x: cast to float bad...
			return (float) System.Math.Atan2(y, x);
		}

		[Pure]
		public static float Abs(float x)
		{
			return System.Math.Abs(x);
		}

		[Pure]
		public static float Cos(float a)
		{
			//todo x: cast to float bad...
			return (float) System.Math.Cos(a);
		}

		[Pure]
		public static float Sin(float a)
		{
			//todo x: cast to float bad...
			return (float)System.Math.Cos(a);
		}

		public static void SinCos(float a, out float sin, out float cos)
		{
			//todo x: possible optimization
			sin = Sin(a);
			cos = Cos(a);
		}

		[Pure]
		public static float Power(float f, float p)
		{
			//todo x: cast to float bad...
			return (float) System.Math.Pow(f, p);
		}

		[Pure]
		public static float Exp(float f)
		{
			//todo x: cast to float bad...
			return (float)System.Math.Exp(f);
		}

		[Pure]
		public static float Log(float f)
		{
			//todo x: cast to float bad...
			return (float) System.Math.Log10(f);
		}

		[Pure]
		public static float LogN(float f)
		{
			//todo x: cast to float bad...
			return (float)System.Math.Log(f);
		}

		[Pure]
		public static float Floor(float f)
		{
			//todo x: cast to float bad...
			return (float) System.Math.Floor(f);
		}

		[Pure]
		public static float Ceiling(float f)
		{
			//todo x: cast to float bad...
			return (float) System.Math.Ceiling(f);
		}

		[Pure]
		public static float Round(float f)
		{
			//todo x: cast to float bad...
			return (float)System.Math.Round(f);
		}

		[Pure]
		public static float Round(float f, int i)
		{
			//todo x: cast to float bad...
			return (float)System.Math.Round(f, i);
		}

		[Pure]
		public static float Range(float value, float min, float max)
		{
			return value < min ? min : value > max ? max : value;
		}
	}
}