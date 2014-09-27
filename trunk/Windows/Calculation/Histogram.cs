using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

		private IntPtr mData;
		private Renderer mRenderer;
		private Size mSize;

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
		public Bitmap CreateBitmap(double density)
		{
			const double contrast = 1.0;
			const double adjust = 2.3;
			const double prefilterWhite = 1024;

			var bitmap = new Bitmap(mRenderer.Size.Width, mRenderer.Size.Height, PixelFormat.Format32bppArgb);

			var gamma = System.Math.Abs(mRenderer.Flame.Gamma) < double.Epsilon ? 0 : 1.0 / mRenderer.Flame.Gamma;
			var vibrancy = (int)(mRenderer.Flame.Vibrancy * 256);
			var invVibrancy = 256 - vibrancy;
			var power = System.Math.Abs(mRenderer.Flame.GammaThreshold) < double.Epsilon ? 0 : System.Math.Pow(mRenderer.Flame.GammaThreshold, gamma - 1);

			var gutter = mSize.Width - mRenderer.Oversample*mRenderer.Size.Width;
			var get = new Func<int, int, double[]>(Get);

			if (mRenderer.Data.FilterSize > gutter/2)
				get = SafeGet;

			var scale = System.Math.Pow(2, mRenderer.Flame.Zoom);
			var sampleDensity = System.Math.Max(0.001, density*scale*scale);

			var logMapC1 = (contrast*adjust*mRenderer.Flame.Brightness*prefilterWhite*268)/256.0;
			var logArea = mRenderer.Size.Width * mRenderer.Size.Height / (mRenderer.Data.PointsPerUnit[0] * mRenderer.Data.PointsPerUnit[1]);
			var logMapC2 = (mRenderer.Oversample*mRenderer.Oversample)/(contrast*logArea*mRenderer.Data.WhiteLevel*sampleDensity);

			var precalcLogMap = new double[1024];
			for (int i = 1; i < precalcLogMap.Length; i++)
			{
				precalcLogMap[i] = logMapC1 * System.Math.Log10(1 + mRenderer.Data.WhiteLevel * i * logMapC2) / (mRenderer.Data.WhiteLevel * i);
			}

			var data = bitmap.LockBits(new Rectangle(new Point(), bitmap.Size), ImageLockMode.WriteOnly, bitmap.PixelFormat);

			int x, y = 0;
			for (int i = 0; i < mRenderer.Size.Height; i++)
			{
				x = 0;

				for (int j = 0; j < mRenderer.Size.Width; j++)
				{
					var fp = new double[4];
					var pos = i*data.Stride + j*4;

					if (mRenderer.Data.FilterSize > 1)
					{
						for (int ii = 0; ii < mRenderer.Data.FilterSize; ii++)
							for (int jj = 0; jj < mRenderer.Data.FilterSize; jj++)
							{
								var value = mRenderer.Data.Filter[ii][jj];
								var point = get(x + jj, y + ii);

								double mapped;

								if (point[3] < 1024)
								{
									mapped = precalcLogMap[(int) point[3]];
								}
								else
								{
									mapped = (logMapC1*System.Math.Log10(1 + mRenderer.Data.WhiteLevel*point[3]*logMapC2))/(mRenderer.Data.WhiteLevel*point[3]);
								}

								fp[0] += value*mapped*point[0];
								fp[1] += value*mapped*point[1];
								fp[2] += value*mapped*point[2];
								fp[3] += value*mapped*point[3];
							}

						fp[0] /= prefilterWhite;
						fp[1] /= prefilterWhite;
						fp[2] /= prefilterWhite;
						fp[3] = mRenderer.Data.WhiteLevel*fp[3]/prefilterWhite;
					}
					else
					{
						var point = get(x, y);
						double mapped;

						if (point[3] < 1024)
						{
							mapped = precalcLogMap[(int)point[3]];
						}
						else
						{
							mapped = (logMapC1 * System.Math.Log10(1 + mRenderer.Data.WhiteLevel * point[3] * logMapC2)) / (mRenderer.Data.WhiteLevel * point[3]);
						}

						mapped /= prefilterWhite;

						fp[0] = mapped * point[0];
						fp[1] = mapped * point[1];
						fp[2] = mapped * point[2];
						fp[3] = mapped * point[3] * mRenderer.Data.WhiteLevel;
					}

					x += mRenderer.Oversample;

					if (fp[3] > 0)
					{
						var alpha = (fp[3] < mRenderer.Flame.GammaThreshold)
							? (1 - (fp[3] / mRenderer.Flame.GammaThreshold)) * fp[3] * power + (fp[3] / mRenderer.Flame.GammaThreshold) * System.Math.Pow(fp[3], gamma)
							: System.Math.Pow(fp[3], gamma);
						var multiplier = vibrancy * alpha / fp[3];

						var ri = (int)System.Math.Max(0, System.Math.Min(((invVibrancy > 0) ? (multiplier * fp[0] + invVibrancy * System.Math.Pow(fp[0], gamma)) : (multiplier * fp[0])), 255));
						var gi = (int)System.Math.Max(0, System.Math.Min(((invVibrancy > 0) ? (multiplier * fp[1] + invVibrancy * System.Math.Pow(fp[1], gamma)) : (multiplier * fp[1])), 255));
						var bi = (int)System.Math.Max(0, System.Math.Min(((invVibrancy > 0) ? (multiplier * fp[2] + invVibrancy * System.Math.Pow(fp[2], gamma)) : (multiplier * fp[2])), 255));
						var ai = (int)System.Math.Max(0, System.Math.Min(alpha * 255, 255));

						Marshal.WriteInt32(data.Scan0, pos, Color.FromArgb(ai, ri, gi, bi).ToArgb());
					}
					else
					{
						Marshal.WriteInt32(data.Scan0, pos, 0);
					}
				}

				y += mRenderer.Oversample;
			}

			bitmap.UnlockBits(data);

			if (!mRenderer.WithTransparency)
			{
				var newBitmap = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, PixelFormat.Format24bppRgb);

				using (var graphics = Graphics.FromImage(newBitmap))
				using (var brush = new SolidBrush(mRenderer.Flame.Background))
				{
					graphics.FillRectangle(brush, new Rectangle(new Point(), newBitmap.Size));
					graphics.DrawImageUnscaled(bitmap, new Point());

					bitmap.Dispose();
					bitmap = newBitmap;
				}
			}

			return bitmap;
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