using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Threading;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Calculation
{
	class Renderer
	{
		private double? mLastSecondsPerIteration;
		private double mAverageIterationsPerSecond;
		private double mPureRenderingTime;

		//todo - render real fractal ... rofl
		public Bitmap CreateBitmap([NotNull] RenderParameters parameters, Action<ProgressEventArgs> progressCallback = null, ThreadStateToken threadState = null)
		{
			if (parameters == null) throw new ArgumentNullException(@"parameters");

			var timer = new NativeTimer();

			var flame = parameters.Flame;
			var size = parameters.Size;
			var density = parameters.Density;
			var withTransparency = parameters.WithTransparency;

			var memory = (size.Width*size.Height*4) / (1024.0 * 1024.0);
			parameters.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), string.Format(Messages.RenderAllocatingMessage, memory)));

			var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			var random = new Random(flame.GetHashCode() ^ (int)DateTime.Now.Ticks);

			var samples = density * size.Width * size.Height + 20;
			var col = flame.Palette[0].ToArgb();
			var icol = flame.Palette[flame.Palette.Length - 1].ToArgb();

			if (progressCallback != null)
			{
				if (threadState == null || !threadState.IsCancelling)
				{
					var estimatedDuration = mLastSecondsPerIteration == null? (TimeSpan?)null: TimeSpan.FromSeconds(samples * mLastSecondsPerIteration.Value);
					progressCallback(new ProgressEventArgs(0, estimatedDuration));
				}
			}

			if (!withTransparency)
			{
				using (var graphics = Graphics.FromImage(bitmap))
				using (var brush = new SolidBrush(flame.Background))
				{
					graphics.FillRectangle(brush, new Rectangle(new Point(), bitmap.Size));
				}
			}
			
			var data = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.WriteOnly, bitmap.PixelFormat);
			var lastExcursion = 0;

			var rcenter = new Vector2(bitmap.Size.Width, bitmap.Size.Height)/2.0;
			var scale = flame.PixelsPerUnit * bitmap.Size.Width / flame.CanvasSize.Width * System.Math.Pow(2, flame.Zoom);
			var vscale = new Vector2(1, -1) / new Vector2(scale, scale);

			const int gridPow = 0;
			double grid = System.Math.Pow(10, -gridPow);
			double gridAcc = 4 * grid/scale;

			parameters.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderInProgressMessage));

			mPureRenderingTime = 0;
			mAverageIterationsPerSecond = 0;
			timer.SetStartingTime();

			var stopwatch = new NativeTimer();
			stopwatch.SetStartingTime();

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
				var posC = flame.CanvasToWorld(new Vector2(pos.X, pos.Y), rcenter, vscale);

				var time = timer.GetElapsedTimeInSeconds();

				var isDiscrete = System.Math.Abs(posC.X - System.Math.Round(posC.X, gridPow)) < gridAcc ||
								 System.Math.Abs(posC.Y - System.Math.Round(posC.Y, gridPow)) < gridAcc;

				Marshal.WriteInt32(data.Scan0, pos.Y * data.Stride + pos.X * 4, isDiscrete ? icol : col);

				if (time > 1)
				{
					mLastSecondsPerIteration = (time/(i - lastExcursion));

					var remaining = mLastSecondsPerIteration.Value * (samples - i);
					var progress = i / samples;

					if (progressCallback != null)
					{
						progressCallback(new ProgressEventArgs(progress, TimeSpan.FromSeconds(remaining)));
					}

					var ips = (i - lastExcursion)/time;
					if (mAverageIterationsPerSecond <= 0)
						mAverageIterationsPerSecond = ips;
					else mAverageIterationsPerSecond = (ips + mAverageIterationsPerSecond)*0.5;

					timer.SetStartingTime();
					lastExcursion = i;
				}
			}

			mPureRenderingTime = stopwatch.GetElapsedTimeInSeconds();

			parameters.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), string.Format(Messages.RenderSamplingMessage, density)));

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

		public double AverageIterationsPerSecond
		{
			get { return mAverageIterationsPerSecond; }
		}
		public double PureRenderingTime
		{
			get { return mPureRenderingTime; }
		}
	}
}