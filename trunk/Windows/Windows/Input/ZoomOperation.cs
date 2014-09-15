using System;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	class ZoomOperation : CameraInputOperation
	{
		[NotNull]
		public Vector2 X0 { get; set; }

		[NotNull]
		public Vector2 X1 { get; set; }

		public ZoomOperation([NotNull] Vector2 x0, [NotNull] Vector2 x1)
		{
			if (x0 == null) throw new ArgumentNullException("x0");
			if (x1 == null) throw new ArgumentNullException("x1");

			X0 = x0;
			X1 = x1;
		}
	}
}