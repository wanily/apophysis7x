using System;
using System.Threading;
using Xyrus.Apophysis.Interfaces.Threading;
using ThreadState = Xyrus.Apophysis.Threading.ThreadState;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ProgressManager : ProgressProvider
	{
		private readonly NativeTimer mTicker = new NativeTimer();
		private readonly NativeTimer mStopWatch = new NativeTimer();

		private IThreadState mThreadState;

		private float? mLastSecondsPerIteration;
		private long mLastExcursion;

		public ProgressManager(IThreadState stateToken = null)
		{
			mThreadState = stateToken ?? new ThreadState();
		}

		[NotNull]
		public IThreadState ThreadState
		{
			get { return mThreadState; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mThreadState = value;
			}
		}

		public void Reset(long iterations, float density)
		{
			mLastExcursion = 0L;
			mLastSecondsPerIteration = null;

			AverageIterationsPerSecond = 0;
			ElapsedTime = TimeSpan.Zero;
			TotalIterations = iterations;
			RemainingTime = null;
			IterationProgress = 0;
			CurrentDensity = 0;
			TargetDensity = density;
			IterationCount = 0;

			mStopWatch.SetStartingTime();
			mTicker.SetStartingTime();

			IsBusy = true;

			RaiseStarted();
			RaiseProgress();
		}
		public void Continue(long iterations, float densityIncrement)
		{
			TotalIterations += iterations;

			RemainingTime = TimeSpan.FromSeconds(mLastSecondsPerIteration.GetValueOrDefault() * (TotalIterations - mLastExcursion));
			IterationProgress = TotalIterations == 0 ? 1 : (float)mLastExcursion / TotalIterations;
			TargetDensity += densityIncrement;

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
				var progress = TotalIterations == 0 ? 1 : (float)iteration / TotalIterations;

				CurrentDensity = progress*TargetDensity;
				IterationProgress = progress;

				if (remaining >= TimeSpan.MaxValue.TotalSeconds / 2)
					remaining = (float)TimeSpan.MaxValue.TotalSeconds / 2;

				RemainingTime = TimeSpan.FromSeconds(remaining);

				RaiseProgress();

				var ips = (iteration - mLastExcursion) / time;
				IterationsPerSecond = ips;
				IterationCount += (iteration - mLastExcursion);

				if (AverageIterationsPerSecond <= 0)
					AverageIterationsPerSecond = ips;
				else AverageIterationsPerSecond = (ips + AverageIterationsPerSecond) * 0.5f;

				mTicker.SetStartingTime();
				mLastExcursion = iteration;
			}
		}
		public void FinalizeProcess()
		{
			ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(mStopWatch.GetElapsedTimeInSeconds()));
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