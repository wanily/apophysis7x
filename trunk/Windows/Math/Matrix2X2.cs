using System;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Matrix2X2
	{
		private Vector2 mRow0, mRow1;

		public Matrix2X2()
		{
			mRow0 = new Vector2();
			mRow1 = new Vector2();
		}

		public Vector2 X
		{
			get { return mRow0; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mRow0 = value;
			}
		}
		public Vector2 Y
		{
			get { return mRow1; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mRow1 = value;
			}
		}

		public bool IsLinear
		{
			get 
			{ 
				return 
					System.Math.Abs(mRow0.X - 1) < double.Epsilon && 
					System.Math.Abs(mRow0.Y) < double.Epsilon && 
					System.Math.Abs(mRow1.X) < double.Epsilon && 
					System.Math.Abs(mRow1.Y - 1) < double.Epsilon; 
			}
		}
	}
}