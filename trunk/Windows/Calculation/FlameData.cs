using System;
using System.Drawing;
using System.Linq;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class FlameData : IDisposable
	{
		private Renderer mRenderer;

		private double[][] mCmap;
		private double[] mPointsPerUnit;

		private const double mWhiteLevel = 200;
		private const int mMaxFilterWidth = 25;
		private const double mFilterCutoff = 1.8;

		~FlameData()
		{
			Dispose(false);
		}
		public FlameData([NotNull] Renderer renderer)
		{
			if (renderer == null) throw new ArgumentNullException(@"renderer");
			mRenderer = renderer;
		}

		public void Initialize()
		{
			CalculateFilter();
			CalculateBuffer();
			CalculateCamera();
			CalculateColorMap();
			PrepareIterators();
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
				if (Iterators != null)
				{
					foreach (var iterator in Iterators)
						iterator.Dispose();

					Iterators = null;
				}

				if (Finals != null)
				{
					foreach (var iterator in Finals)
						iterator.Dispose();

					Finals = null;
				}
			}

			mRenderer = null;
			Cmap = null;
			PointsPerUnit = null;
			Bs = null;
			Rc = null;
			SinCos = null;
			Filter = null;
			Camera = null;
			CameraSize = null;
			Camera3D = null;
			XySize = null;
			Iterators = null;
			Finals = null;
		}

		[NotNull]
		public Renderer Renderer
		{
			get { return mRenderer; }
			set { mRenderer = value; }
		}

		public double[][] Cmap { get; private set; }
		public double[] PointsPerUnit { get; private set; }

		public double[] Bs { get; private set; }
		public double[] Rc { get; private set; }
		public double[] SinCos { get; private set; }

		public int MaxGutterWidth { get; private set; }
		public int GutterWidth { get; private set; }

		public Size BufferSize { get; private set; }
		public int BufferLength { get; private set; }

		public int FilterSize { get; private set; }
		public double[][] Filter { get; private set; }

		public double[] Camera { get; private set; }
		public double[] CameraSize { get; private set; }
		public double[][] Camera3D { get; private set; }
		public double DofCoeff { get; private set; }

		public double[] XySize { get; private set; }
		public double SampleDensity { get; private set; }

		public double WhiteLevel { get { return mWhiteLevel; } }

		public IteratorData[] Iterators { get; private set; }
		public IteratorData[] Finals { get; private set; }

		private void CalculateFilter()
		{
			var fw = (int)(2 * mFilterCutoff * mRenderer.Oversample * mRenderer.FilterRadius);
			
			FilterSize = fw + 1;
			Filter = new double[FilterSize][];

			var adjust = fw > 0 ? (mFilterCutoff*FilterSize)/fw : 1;
			var total = 0.0;

			for (int i = 0; i < FilterSize; i++)
			{
				Filter[i] = new double[FilterSize];
				for (int j = 0; j < FilterSize; j++)
				{
					var ii = ((2.0 * i + 1) / FilterSize - 1) * adjust;
					var jj = ((2.0 * j + 1) / FilterSize - 1) * adjust;

					Filter[i][j] = System.Math.Exp(-2.0*(ii*ii + jj*jj));
					total += Filter[i][j];
				}
			}

			for (int i = 0; i < FilterSize; i++)
				for (int j = 0; j < FilterSize; j++)
					Filter[i][j] /= total;

		}
		private void CalculateBuffer()
		{
			// ReSharper disable once PossibleLossOfFraction
			MaxGutterWidth = (mMaxFilterWidth - mRenderer.Oversample)/2;

			// ReSharper disable once PossibleLossOfFraction
			GutterWidth = (FilterSize - mRenderer.Oversample)/2;

			BufferSize = new Size(
				mRenderer.Oversample * mRenderer.Size.Width + 2 * MaxGutterWidth,
				mRenderer.Oversample * mRenderer.Size.Height + 2 * MaxGutterWidth);
			BufferLength = BufferSize.Width*BufferSize.Height;
		}
		private void CalculateCamera()
		{
			var scale = System.Math.Pow(2, mRenderer.Flame.Zoom);
			var ppu = mRenderer.Flame.PixelsPerUnit * mRenderer.Size.Width / mRenderer.Flame.CanvasSize.Width;

			SampleDensity = mRenderer.Density * scale * scale;

			PointsPerUnit = new[]
			{
				ppu*scale, 
				ppu*scale
			};

			var corner = new[]
			{
				mRenderer.Flame.Origin.X - mRenderer.Size.Width/PointsPerUnit[0]/2.0,
				mRenderer.Flame.Origin.Y - mRenderer.Size.Height/PointsPerUnit[1]/2.0
			};

			var t = new[]
			{
				GutterWidth / (mRenderer.Oversample * PointsPerUnit[0]),
				GutterWidth / (mRenderer.Oversample * PointsPerUnit[1]),
				(2 * MaxGutterWidth - GutterWidth) / (mRenderer.Oversample * PointsPerUnit[0]),
				(2 * MaxGutterWidth - GutterWidth) / (mRenderer.Oversample * PointsPerUnit[1])
			};

			Camera = new[]
			{
				corner[0] - t[0],
				corner[1] - t[1],
				corner[0] + mRenderer.Size.Width/PointsPerUnit[0] + t[2],
				corner[1] + mRenderer.Size.Height/PointsPerUnit[1] + t[3]
			};

			CameraSize = new[]
			{
				Camera[2] - Camera[0],
				Camera[3] - Camera[1]
			};

			XySize = new double[2];

			if (System.Math.Abs(CameraSize[0]) > 0.01)
				XySize[0] = 1.0 / CameraSize[0];
			else XySize[0] = 1.0;

			if (System.Math.Abs(CameraSize[1]) > 0.01)
				XySize[1] = 1.0 / CameraSize[1];
			else XySize[1] = 1.0;

			Bs = new[]
			{
				(BufferSize.Width - 0.5)*XySize[0],
				(BufferSize.Height - 0.5)*XySize[1]
			};

			SinCos = new[]
			{
				System.Math.Sin(mRenderer.Flame.Angle),
				System.Math.Cos(mRenderer.Flame.Angle)
			};

			Rc = new[]
			{
				mRenderer.Flame.Origin.X*(1 - SinCos[1]) - mRenderer.Flame.Origin.Y*SinCos[0] - Camera[0],
				mRenderer.Flame.Origin.Y*(1 - SinCos[1]) + mRenderer.Flame.Origin.X*SinCos[0] - Camera[1]
			};

			Camera3D = new[]
			{
				new[]
				{
					System.Math.Cos(-mRenderer.Flame.Yaw), 
					-System.Math.Sin(-mRenderer.Flame.Yaw), 
					0
				},
				new[]
				{
					System.Math.Cos(mRenderer.Flame.Pitch) * System.Math.Sin(-mRenderer.Flame.Yaw), 
					System.Math.Cos(mRenderer.Flame.Pitch) * System.Math.Cos(-mRenderer.Flame.Yaw), 
					-System.Math.Sin(mRenderer.Flame.Pitch)
				},
				new[]
				{
					System.Math.Sin(mRenderer.Flame.Pitch) * System.Math.Sin(-mRenderer.Flame.Yaw), 
					System.Math.Sin(mRenderer.Flame.Pitch) * System.Math.Cos(-mRenderer.Flame.Yaw), 
					System.Math.Cos(mRenderer.Flame.Pitch)
				}
			};

			DofCoeff = 0.1*mRenderer.Flame.DepthOfField;
		}
		private void CalculateColorMap()
		{
			var colors = mRenderer.Flame.Palette;
			Cmap = new double[colors.Length][];

			for (int i = 0; i < colors.Length; i++)
			{
				Cmap[i] = new double[3];
				Cmap[i][0] = (colors[i].R * mWhiteLevel) / 256.0;
				Cmap[i][1] = (colors[i].G * mWhiteLevel) / 256.0;
				Cmap[i][2] = (colors[i].B * mWhiteLevel) / 256.0;
			}
		}

		private void PrepareIterators()
		{
			Iterators = mRenderer.Flame.Iterators.Where(x => x.GroupIndex == 0).Select(x => new IteratorData(this, x)).ToArray();
			Finals = mRenderer.Flame.Iterators.Where(x => x.GroupIndex == 1).Select(x => new IteratorData(this, x)).ToArray();

			foreach (var iterator in Iterators.Concat(Finals))
				iterator.Initialize();
		}
	}
}