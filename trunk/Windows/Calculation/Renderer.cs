using System;
using System.Drawing;
using System.Threading;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class Renderer : IDisposable
	{
		private const int mFuse = 20;
		private const double mNotifyThreshold = 1.0;

		private readonly RenderMessengerBase mNullMessenger = new RenderMessengerBase();
		private RenderMessengerBase mMessenger;

		private NativeTimer mProgressTicker;

		private FlameData mData;
		private Flame mFlame;

		private long mSamplingLevel;
		private double? mLastSecondsPerIteration;
		private double mAverageIterationsPerSecond;
		private double mPureRenderingTime;

		~Renderer()
		{
			Dispose(false);
		}
		public Renderer([NotNull] Flame flame, Size size, double density, int oversample = 1, double filterRadius = 0, bool withTransparency = true)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (density <= 0) throw new ArgumentOutOfRangeException(@"density");
			if (oversample <= 0) throw new ArgumentOutOfRangeException(@"oversample");
			if (filterRadius < 0) throw new ArgumentOutOfRangeException(@"filterRadius");
			if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException(@"size");

			Oversample = oversample;
			FilterRadius = filterRadius;
			WithTransparency = withTransparency;
			Flame = flame.Copy();
			Density = density;
			Size = size;

			mMessenger = mNullMessenger;
			mProgressTicker = new NativeTimer();

			Data = new FlameData(this);
		}

		public void Initialize()
		{
			Data.Initialize();

			var size = Data.BufferSize;
			var memSize = Histogram.GetMemorySize(size);

			Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), string.Format(Messages.RenderAllocatingMessage, memSize)));
			Histogram = new Histogram(this, size);
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
				if (mData != null)
				{
					mData.Dispose();
					mData = null;
				}

				if (Histogram != null)
				{
					Histogram.Dispose();
					Histogram = null;
				}
			}

			mProgressTicker = null;
			mMessenger = null;
			mFlame = null;
		}

		public Size Size { get; private set; }
		public double Density { get; private set; }
		public double FilterRadius { get; private set; }
		public int Oversample { get; private set; }
		public bool WithTransparency { get; private set; }

		public double AverageIterationsPerSecond
		{
			get { return mAverageIterationsPerSecond; }
		}
		public double PureRenderingTime
		{
			get { return mPureRenderingTime; }
		}

		[NotNull]
		public Flame Flame
		{
			get { return mFlame; }
			private set
			{
				if (value == null) throw new ArgumentNullException("value");
				mFlame = value;
			}
		}

		[NotNull]
		public RenderMessengerBase Messenger
		{
			get { return mMessenger; }
			set
			{
				// ReSharper disable once ConstantNullCoalescingCondition
				mMessenger = value ?? mMessenger;
			}
		}

		[NotNull]
		public FlameData Data
		{
			get { return mData; }
			private set
			{
				if (value == null) throw new ArgumentNullException("value");
				mData = value;
			}
		}
		public Histogram Histogram
		{
			get; 
			private set;
		}

		public long CalculateHistogram(long batchSize, Action<ProgressEventArgs> progressUpdate = null, ThreadStateToken threadState = null)
		{
			var random = new Random(mFlame.GetHashCode() ^ (int)DateTime.Now.Ticks);
			var samples = (long) (Data.BufferLength*Data.SampleDensity/(Oversample*Oversample));

			batchSize = batchSize <= 0 ? samples : batchSize;
			mLastSecondsPerIteration = null;
			mAverageIterationsPerSecond = 0;
			mProgressTicker.SetStartingTime();

			var iterator = Data.Iterators[0];
			var vector = new Vector3(2 * random.NextDouble() - 1, 2 * random.NextDouble() - 1, 0);
			var color = random.NextDouble();

			for (long i = 0; i < mFuse; i++)
			{
				iterator = iterator.Next(random);
				iterator.Process(random, vector, ref color);
			}

			if (progressUpdate != null)
			{
				if (threadState == null || !threadState.IsCancelling)
				{
					var estimatedDuration = mLastSecondsPerIteration == null? (TimeSpan?)null: TimeSpan.FromSeconds(samples * mLastSecondsPerIteration.Value);
					progressUpdate(new ProgressEventArgs(0, estimatedDuration));
				}
			}

			var lastExcursion = 0L;
			var stopwatch = new NativeTimer();

			stopwatch.SetStartingTime();

			for (long i = 0; i < batchSize; i++)
			{
				if (threadState != null && threadState.IsCancelling)
					break;

				if (threadState != null && threadState.IsSuspended)
				{
					Thread.Sleep(10);
					i--;
					continue;
				}

				var time = mProgressTicker.GetElapsedTimeInSeconds();
				if (time > 1)
				{
					mLastSecondsPerIteration = (time / (i - lastExcursion));

					var remaining = mLastSecondsPerIteration.Value * (samples - i);
					var progress = (double)i / samples;

					if (progressUpdate != null)
					{
						progressUpdate(new ProgressEventArgs(progress, TimeSpan.FromSeconds(remaining)));
					}

					var ips = (i - lastExcursion) / time;
					if (mAverageIterationsPerSecond <= 0)
						mAverageIterationsPerSecond = ips;
					else mAverageIterationsPerSecond = (ips + mAverageIterationsPerSecond) * 0.5;

					mProgressTicker.SetStartingTime();
					lastExcursion = i;
				}

				iterator = iterator.Next(random);
				if (!iterator.Process(random, vector, ref color))
					continue;

				for (int j = 0; j < Data.Finals.Length; j++)
				{
					Data.Finals[j].Process(random, vector, ref color);
				}

				var camera3DProjection = vector; //todo

				var canvasProjection = new Vector2(
					(camera3DProjection.X * Data.SinCos[1] + camera3DProjection.Y * Data.SinCos[0] + Data.Rc[0]),
					(camera3DProjection.Y * Data.SinCos[1] - camera3DProjection.X * Data.SinCos[0] + Data.Rc[1]));

				if (canvasProjection.X < 0 || canvasProjection.Y < 0 || canvasProjection.X > Data.CameraSize[0] || canvasProjection.Y > Data.CameraSize[1])
					continue;

				if (color < 0) color = 0;
				if (color > 1) color = 1;

				var bufferLocation = new Point(
					(int) (canvasProjection.X*Data.Bs[0]),
					(int) (canvasProjection.Y*Data.Bs[1]));
				var mapColor = Data.Cmap[(int) (color*Data.Cmap.Length)];

				Histogram.Add(bufferLocation.X, bufferLocation.Y, mapColor);
			}

			mPureRenderingTime = stopwatch.GetElapsedTimeInSeconds();

			if (progressUpdate != null)
			{
				if (threadState == null || !threadState.IsCancelling)
				{
					progressUpdate(new ProgressEventArgs(1, TimeSpan.FromSeconds(0)));
				}
			}

			return batchSize;
		}
	}
}