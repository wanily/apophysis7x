using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Windows;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class Histogram : IDisposable
	{
		[DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
		static extern void ZeroMemory(IntPtr dest, IntPtr size);

		private readonly int mStride;

		private const int mQuadDouble = sizeof (double)*4;
		private const int mOffsR = 0;
		private const int mOffsG = sizeof(double);
		private const int mOffsB = sizeof(double) * 2;
		private const int mOffsAcc = sizeof(double) * 3;

		private const double mWhite = 1024;
		private double mDensity;

		private IntPtr mData;
		private Renderer mRenderer;
		private Size mSize;

		private readonly object mLock = new object();

		~Histogram()
		{
			Dispose(false);
		}
		public Histogram([NotNull] Renderer renderer, Size size)
		{
			if (renderer == null) throw new ArgumentNullException("renderer");
			if (size.Width <= 0 || size.Height <= 0)
				throw new ArgumentOutOfRangeException(@"size");

			mSize = size;
			mRenderer = renderer;
			mStride = (int)GetMemorySize(new Size(size.Width, 1));

			Brightness = 4;
			Gamma = 4;
			GammaThreshold = 0;
			Vibrancy = 1;
			Contrast = 1;
			Background = Color.Black;
			Transparent = false;

			var memoryLength = GetMemorySize(size);
			var ptr = Marshal.AllocHGlobal(new IntPtr(memoryLength));

			ZeroMemory(ptr, new IntPtr(memoryLength));

			mData = ptr;
		}

		public static long GetMemorySize(Size size)
		{
			return size.Width*size.Height*4*sizeof (double);
		}

		public void Add(int x, int y, [NotNull] double[] color)
		{
			var addr = y * mStride + x * mQuadDouble;

			IncAddr(addr, mOffsR, color[0]);
			IncAddr(addr, mOffsG, color[1]);
			IncAddr(addr, mOffsB, color[2]);
			IncAddr(addr, mOffsAcc, 1);
		}

		private double MapScalar(double x, double density, ref double? c1, ref double? c2)
		{
			const double adjust = 2.3;
			

			if (c1 == null || c2 == null)
			{
				var logArea = mRenderer.Size.Width * mRenderer.Size.Height / (mRenderer.Data.PointsPerUnit.X * mRenderer.Data.PointsPerUnit.Y);

				c1 = (Contrast * adjust * Brightness * mWhite * 268) / 256.0;
				c2 = (mRenderer.Oversample * mRenderer.Oversample) / (Contrast * logArea * mRenderer.Data.WhiteLevel * density);
			}

			return c1.Value * System.Math.Log10(1 + mRenderer.Data.WhiteLevel * x * c2.Value) / (mRenderer.Data.WhiteLevel * x);
		}
		private void SetBackground(ref Bitmap bitmap, Color background)
		{
			var newBitmap = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, PixelFormat.Format24bppRgb);

			using (var graphics = Graphics.FromImage(newBitmap))
			using (var brush = new SolidBrush(background))
			{
				graphics.FillRectangle(brush, new Rectangle(new Point(), newBitmap.Size));
				graphics.DrawImageUnscaled(bitmap, new Point());

				bitmap.Dispose();
				bitmap = newBitmap;
			}
		}
		private long Iterate(bool isContinuing, long batchSize, ProgressManager progressManager = null)
		{
			const int fuse = 20;

			progressManager = progressManager ?? new ProgressManager();

			var random = new Random(mRenderer.Data.Flame.GetHashCode() ^ (int)DateTime.Now.Ticks);
			var iterator = mRenderer.Data.Iterators[0];
			var vector = new Vector3(2 * random.NextDouble() - 1, 2 * random.NextDouble() - 1, 0);
			var color = random.NextDouble();

			if (!isContinuing)
			{
				progressManager.Reset(batchSize);

				for (long i = 0; i < fuse; i++)
				{
					iterator = iterator.Next(random);
					iterator.Process(random, vector, ref color);
				}
			}
			else
			{
				progressManager.Continue(batchSize);
			}

			for (long i = 0; i < batchSize; i++)
			{
				if (progressManager.ThreadState.IsCancelling)
					break;

				if (progressManager.ThreadState.IsSuspended)
				{
					progressManager.Wait(ref i);
					continue;
				}

				progressManager.CheckSendProgressEvent(i);

				iterator = iterator.Next(random);
				if (!iterator.Process(random, vector, ref color))
					continue;

				foreach (var final in mRenderer.Data.Finals)
				{
					final.Process(random, vector, ref color);
				}

				var projection = vector.Copy();
				if (!mRenderer.Data.ProjectPoint(ref projection, random))
					continue;

				if (color < 0) color = 0;
				if (color > 1) color = 1;

				var bufferLocation = new Point(
					(int)(projection.X * mRenderer.Data.UnitToPixelFactor.X),
					(int)(projection.Y * mRenderer.Data.UnitToPixelFactor.Y));
				var mapColor = mRenderer.Data.ColorMap[(int)(color * mRenderer.Data.ColorMap.Length)];

				Add(bufferLocation.X, bufferLocation.Y, mapColor);
			}

			progressManager.FinalizeProcess();

			return batchSize;
		}

		public void Iterate(double untilDensity, ProgressManager progressManager = null)
		{
			if (untilDensity <= mDensity)
				return;

			bool firstBatch;
			lock (mLock)
			{
				firstBatch = (mDensity <= 0);
			}

			if (firstBatch)
			{
				var length = (mRenderer.Data.BufferLength*untilDensity/(mRenderer.Oversample*mRenderer.Oversample));
				var newDensity = Iterate(false, (long)length, progressManager) / length;

				lock (mLock)
				{
					mDensity = newDensity;
				}

				return;
			}

			var increment = (mRenderer.Data.BufferLength * (untilDensity - mDensity) / (mRenderer.Oversample * mRenderer.Oversample));
			var densityIncrement = Iterate(true, (long)increment , progressManager) / increment;

			lock (mLock)
			{
				mDensity += densityIncrement;
			}
		}
		public Bitmap CreateBitmap()
		{
			var bitmap = new Bitmap(mRenderer.Size.Width, mRenderer.Size.Height, PixelFormat.Format32bppArgb);
			var bitmapData = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.WriteOnly, bitmap.PixelFormat);

			var gammaDenom = System.Math.Abs(Gamma) < double.Epsilon ? 0 : 1.0 / Gamma;

			var scaledVibrancy = (int)(Vibrancy * 256);
			var inverseScaledVibrancy = 256 - scaledVibrancy;

			var gutter = mSize.Width - mRenderer.Oversample*mRenderer.Size.Width;
			var getPointFunc = (mRenderer.Data.FilterSize > gutter/2) ? SafeGet : new Func<int, int, double[]>(Get);

			var sampleDensity = System.Math.Max(0.001, mDensity*mRenderer.Data.TwoPowerZoom*mRenderer.Data.TwoPowerZoom);

			var precalcLogMap = new double[1024];

			double? logc1 = null, logc2 = null;
			for (int i = 1; i < precalcLogMap.Length; i++)
			{
				precalcLogMap[i] = MapScalar(i, sampleDensity, ref logc1, ref logc2);
			}
			var bufferY = 0;

			for (int bitmapY = 0; bitmapY < mRenderer.Size.Height; bitmapY++)
			{
				var bufferX = 0;

				for (int bitmapX = 0; bitmapX < mRenderer.Size.Width; bitmapX++)
				{
					var histogramPoint = new double[4];
					var bitmapAddress = bitmapY*bitmapData.Stride + bitmapX*4;

					if (mRenderer.Data.FilterSize > 1)
					{
						for (int ii = 0; ii < mRenderer.Data.FilterSize; ii++)
							for (int jj = 0; jj < mRenderer.Data.FilterSize; jj++)
							{
								var value = mRenderer.Data.FilterKernel[ii][jj];
								var point = getPointFunc(bufferX + jj, bufferY + ii);

								var mapped = point[3] < 1024 ? precalcLogMap[(int) point[3]] : MapScalar(point[3], sampleDensity, ref logc1, ref logc2);

								histogramPoint[0] += value*mapped*point[0];
								histogramPoint[1] += value*mapped*point[1];
								histogramPoint[2] += value*mapped*point[2];
								histogramPoint[3] += value*mapped*point[3];
							}

						histogramPoint[0] /= mWhite;
						histogramPoint[1] /= mWhite;
						histogramPoint[2] /= mWhite;
						histogramPoint[3] = mRenderer.Data.WhiteLevel*histogramPoint[3]/mWhite;
					}
					else
					{
						var point = getPointFunc(bufferX, bufferY);
						var mapped = point[3] < 1024 ? precalcLogMap[(int) point[3]] : MapScalar(point[3], sampleDensity, ref logc1, ref logc2);

						mapped /= mWhite;

						histogramPoint[0] = mapped * point[0];
						histogramPoint[1] = mapped * point[1];
						histogramPoint[2] = mapped * point[2];
						histogramPoint[3] = mapped * point[3] * mRenderer.Data.WhiteLevel;
					}

					bufferX += mRenderer.Oversample;

					if (histogramPoint[3] > 0)
					{
						var power = System.Math.Abs(GammaThreshold) < double.Epsilon ? 0 : System.Math.Pow(GammaThreshold, gammaDenom - 1);
						var alpha = (histogramPoint[3] < GammaThreshold)
							? (1 - (histogramPoint[3] / GammaThreshold)) * histogramPoint[3] * power + (histogramPoint[3] / GammaThreshold) * System.Math.Pow(histogramPoint[3], gammaDenom)
							: System.Math.Pow(histogramPoint[3], gammaDenom);
						var multiplier = scaledVibrancy * alpha / histogramPoint[3];

						var ri = (int)System.Math.Max(0, System.Math.Min(((inverseScaledVibrancy > 0) ? (multiplier * histogramPoint[0] + inverseScaledVibrancy * System.Math.Pow(histogramPoint[0], gammaDenom)) : (multiplier * histogramPoint[0])), 255));
						var gi = (int)System.Math.Max(0, System.Math.Min(((inverseScaledVibrancy > 0) ? (multiplier * histogramPoint[1] + inverseScaledVibrancy * System.Math.Pow(histogramPoint[1], gammaDenom)) : (multiplier * histogramPoint[1])), 255));
						var bi = (int)System.Math.Max(0, System.Math.Min(((inverseScaledVibrancy > 0) ? (multiplier * histogramPoint[2] + inverseScaledVibrancy * System.Math.Pow(histogramPoint[2], gammaDenom)) : (multiplier * histogramPoint[2])), 255));
						var ai = (int)System.Math.Max(0, System.Math.Min(alpha * 255, 255));

						Marshal.WriteInt32(bitmapData.Scan0, bitmapAddress, Color.FromArgb(ai, ri, gi, bi).ToArgb());
					}
					else
					{
						Marshal.WriteInt32(bitmapData.Scan0, bitmapAddress, 0);
					}
				}

				bufferY += mRenderer.Oversample;
			}

			bitmap.UnlockBits(bitmapData);

			if (!mRenderer.WithTransparency)
			{
				SetBackground(ref bitmap, Background);
			}

			return bitmap;
		}

		public double Brightness { get; set; }
		public double Gamma { get; set; }
		public double GammaThreshold { get; set; }
		public double Vibrancy { get; set; }
		public double Contrast { get; set; }

		public bool Transparent { get; set; }
		public Color Background { get; set; }

		public double CurrentDensity
		{
			get { return mDensity; }
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(mData);
					mData = IntPtr.Zero;
				}
			}

			mRenderer = null;
		}

		private double[] Get(int x, int y)
		{
			var addr = y * mStride + x * mQuadDouble;

			return new[]
			{
				BitConverter.Int64BitsToDouble(Marshal.ReadInt64(mData, addr + mOffsR)),
				BitConverter.Int64BitsToDouble(Marshal.ReadInt64(mData, addr + mOffsG)),
				BitConverter.Int64BitsToDouble(Marshal.ReadInt64(mData, addr + mOffsB)),
				BitConverter.Int64BitsToDouble(Marshal.ReadInt64(mData, addr + mOffsAcc))
			};
		}
		private double[] SafeGet(int x, int y)
		{
			if (x < 0 || y < 0 || x >= mSize.Width || y >= mSize.Height)
				return new double[4];

			return Get(x, y);
		}

		private void IncAddr(int address, int offset, double value)
		{
			var mem = Marshal.ReadInt64(mData, address + offset);
			var newValue = BitConverter.Int64BitsToDouble(mem) + value;

			mem = BitConverter.DoubleToInt64Bits(newValue);
			Marshal.WriteInt64(mData, address + offset, mem);
		}
	}
}