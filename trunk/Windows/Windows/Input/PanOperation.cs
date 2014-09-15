using System;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class PanOperation : CameraInputOperation
	{
		[NotNull]
		public Vector2 NewOffset { get; private set; }

		[NotNull]
		public Vector2 OldOffset { get; private set; }

		public PanOperation([NotNull] Flame flame, [NotNull] Vector2 newOffset, [NotNull] Vector2 oldOffset) : base(flame)
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