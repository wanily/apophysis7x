namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Matrix2X2
	{
		private readonly Vector2 mRow0, mRow1;

		public Matrix2X2()
		{
			mRow0 = new Vector2();
			mRow1 = new Vector2();
		}

		public Vector2 X
		{
			get { return mRow0; }
		}
		public Vector2 Y
		{
			get { return mRow1; }
		}
	}
}