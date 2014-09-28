using System;
using System.Linq;
using System.Threading;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadedIterationManager : ProgressProvider
	{
		private readonly NativeTimer mTicker;
		private readonly Histogram mHistogram;

		private bool mCanProgress;
		private readonly int mThreadCount;

		private readonly ThreadStartedEventHandler[] mThreadStartedHandler;
		private readonly ThreadProgressEventHandler[] mThreadProgressHandler;
		private readonly ThreadFinishedEventHandler[] mThreadFinishedHandler;
		private readonly RenderState[] mRenderStates;

		private readonly object mLock = new object();

		public ThreadedIterationManager([NotNull] Histogram histogram, int threadCount)
		{
			if (histogram == null) throw new ArgumentNullException(@"histogram");
			if (threadCount <= 0) throw new ArgumentOutOfRangeException(@"threadCount");

			mHistogram = histogram;
			mThreadCount = threadCount;

			mThreadStartedHandler = new ThreadStartedEventHandler[mThreadCount];
			mThreadProgressHandler = new ThreadProgressEventHandler[mThreadCount];
			mThreadFinishedHandler = new ThreadFinishedEventHandler[mThreadCount];
			mRenderStates = new RenderState[mThreadCount];

			mTicker = new NativeTimer();

			for (int i = 0; i < mThreadCount; i++)
			{
				mThreadStartedHandler[i] = (o, e) => OnThreadStarted((ProgressManager) o, e);
				mThreadProgressHandler[i] = (o, e) => OnThreadProgress((ProgressManager)o, e);
				mThreadFinishedHandler[i] = (o, e) => OnThreadFinished((ProgressManager)o, e);

				mRenderStates[i] = new RenderState();
			}
		}

		private void UpdateState()
		{
			var timeRemainingThreads = mRenderStates.Where(x => x.RemainingTime != null).Select(x => x.RemainingTime.GetValueOrDefault()).ToArray();

			IsBusy = mRenderStates.Any(x => x.IsBusy);
			IterationsPerSecond = mRenderStates.Sum(x => x.IterationsPerSecond);
			IterationProgress = mRenderStates.Sum(x => x.Progress * x.TotalIterations) / mRenderStates.Sum(x => x.TotalIterations);
			RemainingTime = timeRemainingThreads.Any() ? TimeSpan.FromSeconds(timeRemainingThreads.Max(x => x.TotalSeconds)) : (TimeSpan?)null;
			AverageIterationsPerSecond = mRenderStates.Sum(x => x.AverageIterationsPerSecond);
			PureRenderingTime = mRenderStates.Max(x => x.PureRenderingTime);
			TotalIterations = mRenderStates.Sum(x => x.TotalIterations);
		}
		private void UpdateThreadState(ProgressManager manager, int threadIndex)
		{
			mRenderStates[threadIndex].IsBusy = manager.IsBusy;
			mRenderStates[threadIndex].IterationsPerSecond = manager.IterationsPerSecond;
			mRenderStates[threadIndex].Progress = manager.IterationProgress;
			mRenderStates[threadIndex].RemainingTime = manager.RemainingTime;
			mRenderStates[threadIndex].AverageIterationsPerSecond = manager.AverageIterationsPerSecond;
			mRenderStates[threadIndex].PureRenderingTime = manager.PureRenderingTime;
			mRenderStates[threadIndex].TotalIterations = manager.TotalIterations;
		}

		private void OnStarted()
		{
			UpdateState();

			if (mRenderStates.All(x => x.IsBusy))
			{
				RaiseStarted();
				mTicker.SetStartingTime();
				mCanProgress = true;
			}
		}
		private void OnProgress()
		{
			UpdateState();

			if (mCanProgress && mTicker.GetElapsedTimeInSeconds() > ProgressThreshold)
			{
				RaiseProgress();
				mTicker.SetStartingTime();
			}
		}
		private void OnFinished(bool cancelled)
		{
			UpdateState();

			if (!mRenderStates.Any(x => x.IsBusy))
			{
				RaiseFinished(cancelled);
			}
		}

		private void OnThreadStarted(ProgressManager manager, ThreadStartedEventArgs e)
		{
			lock (mLock)
			{
				UpdateThreadState(manager, e.ThreadIndex);
				OnStarted();
			}
		}
		private void OnThreadProgress(ProgressManager manager, ThreadProgressEventArgs e)
		{
			lock (mLock)
			{
				UpdateThreadState(manager, e.ThreadIndex);
				OnProgress();
			}
		}
		private void OnThreadFinished(ProgressManager manager, ThreadFinishedEventArgs e)
		{
			lock (mLock)
			{
				UpdateThreadState(manager, e.ThreadIndex);
				OnFinished(manager.ThreadState.IsCancelling);
			}
		}

		public void StartIterate(double density, ThreadStateToken token = null)
		{
			mCanProgress = false;

			var threadDensity = System.Math.Ceiling(density / mThreadCount);
			for (int i = 0; i < mThreadCount; i++)
			{
				StartIterate(i, threadDensity, token);
			}
		}
		private void StartIterate(int threadIndex, double threadDensity, ThreadStateToken token)
		{
			var progress = new ProgressManager(token);
			var thread = new Thread(() => mHistogram.Iterate(threadDensity, progress));

			progress.Started += (o, e) => mThreadStartedHandler[threadIndex](o, new ThreadStartedEventArgs(threadIndex));
			progress.Progress += (o, e) => mThreadProgressHandler[threadIndex](o, new ThreadProgressEventArgs(threadIndex, e.Progress, e.TimeRemaining));
			progress.Finished += (o, e) => mThreadFinishedHandler[threadIndex](o, new ThreadFinishedEventArgs(threadIndex, e.Cancelled));

			thread.Start();
		}
	}
}