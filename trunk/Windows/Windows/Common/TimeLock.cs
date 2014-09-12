using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	class TimeLock : IDisposable
	{
		private int mDelay;
		private Timer mTimer;
		private Action mCallback;
		private bool mIsBusy;

		~TimeLock()
		{
			Dispose(false);
		}
		public TimeLock([NotNull] Action callback)
		{
			mTimer = new Timer();
			mTimer.Tick += OnTick;

			if (callback == null) throw new ArgumentNullException("callback");
			mCallback = callback;

			Delay = 500;
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mTimer != null)
				{
					mTimer.Tick -= OnTick;
					mTimer.Dispose();
					mTimer = null;
				}
			}

			mCallback = null;
			mIsBusy = false;
		}
		private void OnTick(object sender, EventArgs e)
		{
			if (!mIsBusy)
				return;

			mTimer.Enabled = false;

			mCallback();
			mIsBusy = false;
		}

		public int Delay
		{
			get { return mDelay; }
			set
			{
				if (value < 1) throw new ArgumentOutOfRangeException("value");

				if (Equals(mDelay, value))
					return;

				mTimer.Enabled = false;
				mDelay = value;
				mTimer.Interval = value;
			}
		}
		public bool IsBusy
		{
			get { return mIsBusy; }
		}

		public void Enter()
		{
			mTimer.Enabled = false;
			mTimer.Enabled = true;
			mIsBusy = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}