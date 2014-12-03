using System;
using System.Numerics;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class PanOperation : CameraInputOperation
	{
		public Vector2 NewOffset { get; private set; }
		public Vector2 OldOffset { get; private set; }

		public PanOperation(Vector2 newOffset, Vector2 oldOffset)
		{
			if (newOffset == null) throw new ArgumentNullException("newOffset");
			if (oldOffset == null) throw new ArgumentNullException("oldOffset");
			NewOffset = newOffset;
			OldOffset = oldOffset;
		}

		public override string ToString()
		{
			return string.Format("Center: {0} {1}",
				NewOffset.X.ToString("0.000", InputController.Culture).PadLeft(6),
				NewOffset.Y.ToString("0.000", InputController.Culture).PadLeft(6));
		}
	}
}