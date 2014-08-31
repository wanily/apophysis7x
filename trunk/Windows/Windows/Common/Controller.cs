using System;
using System.ComponentModel;

namespace Xyrus.Apophysis.Windows
{
	public abstract class Controller<TView> : IDisposable where TView: Component, new()
	{
		private TView mView;

		~Controller()
		{
			Dispose(false);
		}
		protected Controller()
		{
			mView = new TView();
			
			// ReSharper disable once DoNotCallOverridableMethodsInConstructor
			AttachView();
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			DisposeOverride(disposing);

			if (disposing)
			{
				if (mView != null)
				{
					DetachView();
					mView.Dispose();
					mView = null;
				}
			}
		}
		protected virtual void DisposeOverride(bool disposing)
		{
		}

		protected abstract void AttachView();
		protected abstract void DetachView();

		protected TView View
		{
			get { return mView; }
		}
	}
}