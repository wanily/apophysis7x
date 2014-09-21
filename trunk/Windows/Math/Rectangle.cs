using System;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public class Rectangle
	{
		private readonly Vector2 mCorner;
		private readonly Vector2 mSize;

		public Rectangle([NotNull] Vector2 corner, [NotNull] Vector2 size)
		{
			if (corner == null) throw new ArgumentNullException(@"corner");
			if (size == null) throw new ArgumentNullException(@"size");

			mCorner = corner;
			mSize = size;
		}

		public Vector2 Corner
		{
			get { return mCorner; }
		}
		public Vector2 Size
		{
			get { return mSize; }
		}

		public Vector2 TopLeft
		{
			get { return mCorner; }
		}
		public Vector2 BottomRight
		{
			get { return mCorner + new Vector2(mSize.X, mSize.Y); }
		}

		public bool IsOnSurface(Vector2 point)
		{
			return point.X >= TopLeft.X && point.X < BottomRight.X && point.Y >= TopLeft.Y && point.Y < BottomRight.Y;
		}
	}
}