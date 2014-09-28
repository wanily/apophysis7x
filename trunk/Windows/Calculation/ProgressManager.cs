using System;
using System.Threading;
using Xyrus.Apophysis.Threading;
using ThreadState = Xyrus.Apophysis.Threading.ThreadState;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ProgressManager : ProgressProvider
	{
		private readonly NativeTimer mTicker = new NativeTimer();
		private readonly NativeTimer mStopWatch = new NativeTimer();

		private ThreadState mThreadState;
		
		private double? mLastSecondsPerIteration;
		private long mLastExcursion;

		public ProgressManager(ThreadState stateToken = null)
		{
			mThreadState = stateToken ?? new ThreadState();
		}

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

		public void Reset(long iterations)
		{
			mLastExcursion = 0L;
			mLastSecondsPerIteration = null;

			AverageIterationsPerSecond = 0;
			PureRenderingTime = 0;
			TotalIterations = iterations;
			RemainingTime = null;
			IterationProgress = 0;

			mStopWatch.SetStartingTime();
			mTicker.SetStartingTime();

			IsBusy = true;

			RaiseStarted();
			RaiseProgress();
		}
		public void Continue(long iterations)
		{
			TotalIterations += iterations;

			RemainingTime = TimeSpan.FromSeconds(mLastSecondsPerIteration.GetValueOrDefault() * (TotalIterations - mLastExcursion));
			IterationProgress = TotalIterations == 0 ? 1 : (double)mLastExcursion / TotalIterations;

			mStopWatch.SetStartingTime();
			mTicker.SetStartingTime();

			IsBusy = true;

			RaiseStarted();
			RaiseProgress();
		}
		public void Wait(ref long iteration)
		{
			Thread.Sleep(10);
			iteration--;
		}
		public void CheckSendProgressEvent(long iteration)
		{
			var time = mTicker.GetElapsedTimeInSeconds();
			if (time > ProgressThreshold)
			{
				mLastSecondsPerIteration = (time / (iteration - mLastExcursion));

				var remaining = mLastSecondsPerIteration.Value * (TotalIterations - iteration);
				var progress = TotalIterations == 0 ? 1 : (double)iteration / TotalIterations;

				IterationProgress = progress;
				RemainingTime = TimeSpan.FromSeconds(remaining);

				RaiseProgress();

				var ips = (iteration - mLastExcursion) / time;
				IterationsPerSecond = ips;

				if (AverageIterationsPerSecond <= 0)
					AverageIterationsPerSecond = ips;
				else AverageIterationsPerSecond = (ips + AverageIterationsPerSecond) * 0.5;

				mTicker.SetStartingTime();
				mLastExcursion = iteration;
			}
		}
		public void FinalizeProcess()
		{
			PureRenderingTime += mStopWatch.GetElapsedTimeInSeconds();
			RemainingTime = TimeSpan.FromSeconds(0);
			IterationProgress = 1;

			if (!ThreadState.IsCancelling)
			{
				RaiseProgress();
			}

			IsBusy = false;

			RaiseFinished(ThreadState.IsCancelling);
		}
	}
}