using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	public class ThumbnailRenderer
	{
		//todo - render real fractal ... rofl
		public Bitmap CreateBitmap([NotNull] Flame flame, double density, Size size, ThreadStateToken threadState = null)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (density <= 0) throw new ArgumentOutOfRangeException("density");
			if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException("size");

			var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			var random = new Random((int)DateTime.Now.Ticks);

			var col = Color.FromArgb(random.Next() % 255, random.Next() % 255, random.Next() % 255).ToArgb();

			using (var graphics = Graphics.FromImage(bitmap))
			using (var brush = new SolidBrush(Color.Black))
			{
				graphics.FillRectangle(brush, new Rectangle(new Point(), bitmap.Size));
			}

			var data = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.WriteOnly, bitmap.PixelFormat);
			for (int i = 0; i < density * size.Width * size.Height + 20; i++)
			{
				if (threadState != null && threadState.IsCancelling)
					break;

				var pos = new Point(random.Next() % bitmap.Width, random.Next() % bitmap.Height);

				Marshal.WriteInt32(data.Scan0, pos.Y * data.Stride + pos.X * 4, col);
			}

			bitmap.UnlockBits(data);
			return bitmap;
		}
	}
}
