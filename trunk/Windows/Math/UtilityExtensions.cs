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

		public static Color Invert(this Color color)
		{
			return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
		}

		public static System.Drawing.Rectangle Scale(this System.Drawing.Rectangle rect, double factor)
		{
			return new System.Drawing.Rectangle((int)(rect.Left * factor), (int)(rect.Top * factor), (int)(rect.Right * factor - rect.Left * factor), (int)(rect.Bottom * factor - rect.Top * factor));
		}
	}
}