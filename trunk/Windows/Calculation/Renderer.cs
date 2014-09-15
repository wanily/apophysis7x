using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	class Renderer
	{
		private double? mLastSecondsPerIteration;

		//todo - render real fractal ... rofl
		public Bitmap CreateBitmap([NotNull] Flame flame, double density, Size size, bool withTransparency, Action<ProgressEventArgs> progressCallback = null, ThreadStateToken threadState = null)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (density <= 0) throw new ArgumentOutOfRangeException("density");
			if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException("size");

			var timer = new NativeTimer();

			var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			var random = new Random(flame.GetHashCode());

			var samples = density * size.Width * size.Height + 20;
			var col = Color.FromArgb(random.Next() % 255, random.Next() % 255, random.Next() % 255).ToArgb();

			random = new Random(flame.GetHashCode() ^ (int)DateTime.Now.Ticks);

			if (progressCallback != null)
			{
				if (threadState == null || !threadState.IsCancelling)
				{
					var estimatedDuration = mLastSecondsPerIteration == null? (TimeSpan?)null: TimeSpan.FromSeconds(samples * mLastSecondsPerIteration.Value);
					progressCallback(new ProgressEventArgs(0, estimatedDuration));
				}
			}

			if (withTransparency)
			{
				using (var graphics = Graphics.FromImage(bitmap))
				using (var brush = new SolidBrush(flame.Background))
				{
					graphics.FillRectangle(brush, new Rectangle(new Point(), bitmap.Size));
				}
			}
			

			timer.SetStartingTime();

			var data = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.WriteOnly, bitmap.PixelFormat);
			var lastExcursion = 0;

			for (int i = 0; i < samples; i++)
			{
				if (threadState != null && threadState.IsCancelling)
					break;

				if (threadState != null && threadState.IsSuspended)
				{
					Thread.Sleep(10);
					i--;
					continue;
				}

				var pos = new Point(random.Next() % bitmap.Width, random.Next() % bitmap.Height);
				var time = timer.GetElapsedTimeInSeconds();

				Marshal.WriteInt32(data.Scan0, pos.Y * data.Stride + pos.X * 4, col);

				if (time > 1)
				{
					mLastSecondsPerIteration = (time/(i - lastExcursion));

					var remaining = mLastSecondsPerIteration.Value * (samples - i);
					var progress = i / samples;

					if (progressCallback != null)
					{
						progressCallback(new ProgressEventArgs(progress, TimeSpan.FromSeconds(remaining)));
					}

					timer.SetStartingTime();
					lastExcursion = i;
				}
			}

			bitmap.UnlockBits(data);

			if (progressCallback != null)
			{
				if (threadState == null || !threadState.IsCancelling)
				{
					progressCallback(new ProgressEventArgs(1, TimeSpan.FromSeconds(0)));
				}
			}

			if (threadState != null && threadState.IsCancelling)
			{
				bitmap.Dispose();
				return null;
			}

			return bitmap;
		}
	}
}