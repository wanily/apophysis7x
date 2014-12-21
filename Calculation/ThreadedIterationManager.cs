using System;
using System.Linq;
using System.Threading;
using Xyrus.Apophysis.Strings;
using ThreadState = Xyrus.Apophysis.Threading.ThreadState;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadedIterationManager : IterationManagerBase, IThreaded
	{
		private readonly NativeTimer mTicker;

		private bool mCanProgress;
		private int mThreadCount;

		private ThreadStartedEventHandler[] mThreadStartedHandler;
		private ThreadProgressEventHandler[] mThreadProgressHandler;
		private ThreadFinishedEventHandler[] mThreadFinishedHandler;
		private RenderState[] mRenderStates;

		private readonly object mLock = new object();

		public ThreadedIterationManager()
		{
			mTicker = new NativeTimer();
			SetThreadCount(null);
		}

		private void UpdateState()
		{
			var timeRemainingThreads = mRenderStates.Where(x => x.RemainingTime != null).Select(x => x.RemainingTime.GetValueOrDefault()).ToArray();

			IsBusy = mRenderStates.Any(x => x.IsBusy);
			IterationsPerSecond = mRenderStates.Sum(x => x.IterationsPerSecond);
			IterationProgress = mRenderStates.Sum(x => x.Progress * x.TotalIterations) / mRenderStates.Sum(x => x.TotalIterations);
			RemainingTime = timeRemainingThreads.Any() ? TimeSpan.FromSeconds(timeRemainingThreads.Max(x => x.TotalSeconds)) : (TimeSpan?)null;
			AverageIterationsPerSecond = mRenderStates.Sum(x => x.AverageIterationsPerSecond);
			ElapsedTime = mRenderStates.Max(x => x.ElapsedTime);
			TotalIterations = mRenderStates.Sum(x => x.TotalIterations);
			TargetDensity = mRenderStates.Sum(x => x.TargetDensity);
			CurrentDensity = mRenderStates.Sum(x => x.CurrentDensity);
			IterationCount = mRenderStates.Sum(x => x.IterationCount);

			UpdateState(this);
		}
		private void UpdateThreadState(ProgressManager manager, int threadIndex)
		{
			mRenderStates[threadIndex].IsBusy = manager.IsBusy;
			mRenderStates[threadIndex].IterationsPerSecond = manager.IterationsPerSecond;
			mRenderStates[threadIndex].Progress = manager.IterationProgress;
			mRenderStates[threadIndex].RemainingTime = manager.RemainingTime;
			mRenderStates[threadIndex].AverageIterationsPerSecond = manager.AverageIterationsPerSecond;
			mRenderStates[threadIndex].ElapsedTime = manager.ElapsedTime;
			mRenderStates[threadIndex].TotalIterations = manager.TotalIterations;
			mRenderStates[threadIndex].TargetDensity = manager.TargetDensity;
			mRenderStates[threadIndex].CurrentDensity = manager.CurrentDensity;
			mRenderStates[threadIndex].IterationCount = manager.IterationCount;
		}

		protected override void UpdateState(ProgressProvider manager)
		{
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

		public void SetThreadCount(int? threadCount)
		{
			if (threadCount.HasValue && threadCount.Value <= 0) throw new ArgumentOutOfRangeException(@"threadCount");

			if (IsBusy)
			{
				throw new InvalidOperationException(Messages.AttemptedThreadCountChangeWhileRendererBusyErrorMessage);
			}

			var defaultCount = System.Math.Max(1, Environment.ProcessorCount - 1);

			mThreadCount = threadCount.GetValueOrDefault(defaultCount);
			mThreadStartedHandler = new ThreadStartedEventHandler[mThreadCount];
			mThreadProgressHandler = new ThreadProgressEventHandler[mThreadCount];
			mThreadFinishedHandler = new ThreadFinishedEventHandler[mThreadCount];
			mRenderStates = new RenderState[mThreadCount];

			for (int i = 0; i < mThreadCount; i++)
			{
				mThreadStartedHandler[i] = (o, e) => OnThreadStarted((ProgressManager)o, e);
				mThreadProgressHandler[i] = (o, e) => OnThreadProgress((ProgressManager)o, e);
				mThreadFinishedHandler[i] = (o, e) => OnThreadFinished((ProgressManager)o, e);

				mRenderStates[i] = new RenderState();
			}
		}

		public override void StartIterate(Histogram histogram, float density)
		{
			if (histogram == null) throw new ArgumentNullException("histogram");

			mCanProgress = false;
			ResetState();

			var threadDensity = Float.Ceiling(density / mThreadCount);
			for (int i = 0; i < mThreadCount; i++)
			{
				ThreadStartIterate(histogram, i, threadDensity, new IterationThreadStateToken(this));
			}
		}
		public override void Iterate(Histogram histogram, float density)
		{
			if (histogram == null) throw new ArgumentNullException("histogram");

			StartIterate(histogram, density);

			while (!IsBusy)
			{
				Thread.Sleep(10);
			}

			Wait();
		}

		private void ThreadStartIterate([NotNull] Histogram histogram, int threadIndex, float threadDensity, ThreadState token)
		{
			var progress = new ProgressManager(token);
			var thread = new Thread(() => histogram.Iterate(threadDensity, progress));

			progress.Started += (o, e) => mThreadStartedHandler[threadIndex](o, new ThreadStartedEventArgs(threadIndex));
			progress.Progress += (o, e) => mThreadProgressHandler[threadIndex](o, new ThreadProgressEventArgs(threadIndex));
			progress.Finished += (o, e) => mThreadFinishedHandler[threadIndex](o, new ThreadFinishedEventArgs(threadIndex, e.Cancelled));

			thread.Start();
		}
	}
}