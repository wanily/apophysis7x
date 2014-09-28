using System.Threading;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class IterationManagerBase : ProgressProvider
	{
		private bool mSuspended;
		private bool mCancel;

		protected void StateReset()
		{
			mSuspended = false;
			mCancel = false;
		}

		public abstract void StartIterate([NotNull] Histogram histogram, double density);
		public abstract void Iterate([NotNull] Histogram histogram, double density);

		public void Wait()
		{
			while (IsBusy)
			{
				Thread.Sleep(10);
				Application.DoEvents();
			}
		}
		public void Suspend()
		{
			if (!IsBusy || mSuspended)
				return;

			mSuspended = true;
		}
		public void Resume()
		{
			if (!IsBusy || !mSuspended)
				return;

			mSuspended = false;
		}
		public void Cancel()
		{
			if (!IsBusy)
				return;

			if (mSuspended)
				Resume();

			mCancel = true;
			Wait();
		}

		public bool IsSuspended
		{
			get { return mSuspended; }
		}
		public bool IsCancelling
		{
			get { return mCancel; }
		}
	}
}