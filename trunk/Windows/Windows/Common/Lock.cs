using System;

namespace Xyrus.Apophysis.Windows
{
	public class Lock : IDisposable
	{
		private bool mState;

		public Lock Enter()
		{
			mState = true;

			if (Engaged != null)
				Engaged(this, new EventArgs());

			return this;
		}
		public void Dispose()
		{
			if (Released != null)
				Released(this, new EventArgs());

			mState = false;
		}

		public bool IsBusy
		{
			get { return mState; }
		}

		public event EventHandler Engaged;
		public event EventHandler Released;
	}
}