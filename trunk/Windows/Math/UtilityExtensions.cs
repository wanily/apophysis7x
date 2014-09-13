using System.Drawing;

namespace Xyrus.Apophysis.Math
{
	[PublicAPI]
	public static class UtilityExtensions
	{
		public static Size FitToFrame(this Size size, Size canvasSize)
		{
			if (size.Width <= 0 || size.Height <= 0)
				return size;

			if (canvasSize.Width <= 0 || canvasSize.Height <= 0)
				return canvasSize;

			Size outputSize;

			if ((double)size.Width / size.Height > (double)canvasSize.Width / canvasSize.Height)
			{
				outputSize = new Size(canvasSize.Width, (int)(canvasSize.Width * (double)size.Height / size.Width));
			}
			else
			{
				outputSize = new Size((int)(canvasSize.Height * (double)size.Width / size.Height), canvasSize.Height);
			}

			return outputSize;
		}
	}
}