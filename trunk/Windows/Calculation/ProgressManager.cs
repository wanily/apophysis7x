using System;
using System.Threading;
using Xyrus.Apophysis.Threading;
using ThreadState = Xyrus.Apophysis.Threading.ThreadState;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ProgressManager
	{
		private readonly NativeTimer mTicker = new NativeTimer();
		private readonly NativeTimer mStopWatch = new NativeTimer();

		private ThreadState mThreadState;

		private double? mLastSecondsPerIteration;
		private long mLastExcursion;

		private double mAverageIterationsPerSecond;
		private double mPureRenderingTime;
		private long mTotalIterations;

		public ProgressManager(ThreadStateToken stateToken = null)
		{
			mThreadState = stateToken ?? new ThreadState();
		}

		public event ProgressEventHandler Progress;

		[NotNull]
		public ThreadState ThreadState
		{
			get { return mThreadState; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mThreadState = value;
			}
		}

		public long TotalIterations
		{
			get { return mTotalIterations; }
		}
		public double AverageIterationsPerSecond
		{
			get { return mAverageIterationsPerSecond; }
		}
		public double PureRenderingTime
		{
			get { return mPureRenderingTime; }
		}

		public void Reset(long iterations)
		{
			mLastExcursion = 0L;
			mLastSecondsPerIteration = null;
			mAverageIterationsPerSecond = 0;
			mPureRenderingTime = 0;
			mTotalIterations = iterations;

			mStopWatch.SetStartingTime();
			mTicker.SetStartingTime();

			if (Progress != null && !ThreadState.IsCancelling)
			{
				Progress(this, new ProgressEventArgs(0));
			}
		}
		public void Continue(long iterations)
		{
			mTotalIterations += iterations;

			mStopWatch.SetStartingTime();
			mTicker.SetStartingTime();

			if (Progress != null && !ThreadState.IsCancelling)
			{
				var remaining = mLastSecondsPerIteration.GetValueOrDefault() * (TotalIterations - mLastExcursion);
				var progress = TotalIterations == 0 ? 1 : (double)mLastExcursion / TotalIterations;

				Progress(this, new ProgressEventArgs(progress, TimeSpan.FromSeconds(remaining)));
			}
		}
		public void Wait(ref long iteration)
		{
			Thread.Sleep(10);
			iteration--;
		}
		public void CheckSendProgressEvent(long iteration)
		{
			var time = mTicker.GetElapsedTimeInSeconds();
			if (time > 1)
			{
				mLastSecondsPerIteration = (time / (iteration - mLastExcursion));

				var remaining = mLastSecondsPerIteration.Value * (TotalIterations - iteration);
				var progress = TotalIterations == 0 ? 1 : (double)iteration / TotalIterations;

				if (Progress != null)
				{
					Progress(this, new ProgressEventArgs(progress, TimeSpan.FromSeconds(remaining)));
				}

				var ips = (iteration - mLastExcursion) / time;
				if (AverageIterationsPerSecond <= 0)
					mAverageIterationsPerSecond = ips;
				else mAverageIterationsPerSecond = (ips + AverageIterationsPerSecond) * 0.5;

				mTicker.SetStartingTime();
				mLastExcursion = iteration;
			}
		}
		public void FinalizeProcess()
		{
			mPureRenderingTime += mStopWatch.GetElapsedTimeInSeconds();

			if (Progress != null && !ThreadState.IsCancelling)
			{
				Progress(this, new ProgressEventArgs(1, TimeSpan.FromSeconds(0)));
			}
		}
	}
}