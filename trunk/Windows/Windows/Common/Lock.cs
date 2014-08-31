using System;

namespace Xyrus.Apophysis.Windows
{
	class Lock : IDisposable
	{
		private bool mState;

		public Lock Enter()
		{
			mState = true;
			return this;
		}
		public void Dispose()
		{
			mState = false;
		}

		public bool IsBusy
		{
			get { return mState; }
		}
	}
}