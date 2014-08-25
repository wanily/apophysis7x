using System;

namespace Xyrus.Apophysis.Windows.Math
{
	[PublicAPI]
	public class ImmutableVector2 : Vector2
	{
		public ImmutableVector2([NotNull] Vector2 vector)
		{
			if (vector == null) throw new ArgumentNullException("vector");

			X = vector.X;
			Y = vector.Y;
		}

		public new double X
		{
			get { return base.X; }
			private set { base.X = value; }
		}
		public new double Y
		{
			get { return base.Y; }
			private set { base.Y = value; }
		}
	}
}