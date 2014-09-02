using System;
using System.ComponentModel;

namespace Xyrus.Apophysis.Windows
{
	public abstract class Controller<TView> : IDisposable where TView: Component, new()
	{
		private TView mView;
		private bool mDisposed;
		private bool mInitialized;

		~Controller()
		{
			Dispose(false);
		}
		protected Controller()
		{
			mView = new TView();
		}
		protected Controller([NotNull] TView view)
		{
			if (view == null) throw new ArgumentNullException("view");
			mView = view;
		}

		public void Initialize()
		{
			if (mInitialized)
			{
				DetachView();
			}

			AttachView();
			mInitialized = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

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

			mDisposed = true;
		}
		protected virtual void DisposeOverride(bool disposing)
		{
		}

		protected abstract void AttachView();
		protected abstract void DetachView();

		protected TView View
		{
			get
			{
				return mView;
			}
		}
	}
}