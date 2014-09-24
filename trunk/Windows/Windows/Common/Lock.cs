using System;

namespace Xyrus.Apophysis.Windows
{
	public class Lock : IDisposable
	{
		private int mState;

		public Lock Enter()
		{
			if (!IsBusy && Engaged != null)
				Engaged(this, new EventArgs());

			mState++;

			return this;
		}
		public void Dispose()
		{
			if (!IsBusy)
				return;

			mState--;

			if (!IsBusy && Released != null)
				Released(this, new EventArgs());
		}

		public bool IsBusy
		{
			get { return mState > 0; }
		}

		public event EventHandler Engaged;
		public event EventHandler Released;
	}
}