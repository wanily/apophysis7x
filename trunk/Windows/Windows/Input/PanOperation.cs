using System;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	class PanOperation : CameraInputOperation
	{
		[NotNull]
		public Vector2 Offset { get; private set; }

		public PanOperation([NotNull] Vector2 offset)
		{
			if (offset == null) throw new ArgumentNullException("offset");
			Offset = offset;
		}
	}
}