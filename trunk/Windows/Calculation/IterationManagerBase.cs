using System;
using System.Threading;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class IterationManagerBase : ProgressProvider, IDisposable
	{
		private bool mSuspended;
		private bool mCancel;

		protected void ResetState()
		{
			mSuspended = false;
			mCancel = false;
		}
		protected virtual void UpdateState(ProgressProvider manager)
		{
			IsBusy = manager.IsBusy;
			IterationsPerSecond = manager.IterationsPerSecond;
			IterationProgress = manager.IterationProgress;
			RemainingTime = manager.RemainingTime;
			AverageIterationsPerSecond = manager.AverageIterationsPerSecond;
			ElapsedTime = manager.ElapsedTime;
			TotalIterations = manager.TotalIterations;
			TargetDensity = manager.TargetDensity;
			CurrentDensity = manager.CurrentDensity;
			IterationCount = manager.IterationCount;
		}

		public abstract void StartIterate([NotNull] Histogram histogram, double density);
		public abstract void Iterate([NotNull] Histogram histogram, double density);

		public virtual void Wait()
		{
			while (IsBusy)
			{
				Thread.Sleep(10);
				Application.DoEvents();
			}
		}
		public virtual void Suspend()
		{
			if (!IsBusy || mSuspended)
				return;

			mSuspended = true;
		}
		public virtual void Resume()
		{
			if (!IsBusy || !mSuspended)
				return;

			mSuspended = false;
		}
		public virtual void Cancel()
		{
			if (!IsBusy)
				return;

			if (mSuspended)
				Resume();

			mCancel = true;
			Wait();
		}

		public virtual bool IsSuspended
		{
			get { return mSuspended; }
		}
		public virtual bool IsCancelling
		{
			get { return mCancel; }
		}

		public void Dispose()
		{
			Cancel();
		}
	}
}