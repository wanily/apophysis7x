using System;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public abstract class CameraInputOperation
	{
		[NotNull]
		public Flame Flame { get; private set; }

		protected CameraInputOperation([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			Flame = flame;
		}
	}
}