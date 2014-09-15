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
		public Vector2 Offset { get; private set; }

		[NotNull]
		public Vector2 Origin { get; private set; }

		public PanOperation([NotNull] Flame flame, [NotNull] Vector2 offset, [NotNull] Vector2 origin) : base(flame)
		{
			if (offset == null) throw new ArgumentNullException("offset");
			if (origin == null) throw new ArgumentNullException("origin");
			Offset = offset;
			Origin = origin;
		}

		public override string ToString()
		{
			return string.Format("Center: {0} {1}",
				Offset.X.ToString("0.000", InputController.Culture).PadLeft(6),
				Offset.Y.ToString("0.000", InputController.Culture).PadLeft(6));
		}
	}
}