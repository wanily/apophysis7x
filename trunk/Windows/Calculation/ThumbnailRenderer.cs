using System;
using System.Drawing;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	public class ThumbnailRenderer
	{
		public Bitmap CreateBitmap([NotNull] Flame flame, double density, Size size, ThreadStateToken threadState = null)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (density <= 0) throw new ArgumentOutOfRangeException("density");
			if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException("size");

			var renderer = new Renderer();

			return renderer.CreateBitmap(flame, density, size, null, threadState);
		}
	}
}
