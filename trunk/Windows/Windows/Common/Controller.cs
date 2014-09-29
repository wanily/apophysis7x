using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	[PublicAPI]
	public abstract class Controller : IDisposable
	{
		~Controller()
		{
			Dispose(false);
		}
		protected abstract void Dispose(bool disposing);
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	[PublicAPI]
	public abstract class Controller<TView> : Controller where TView : Component, new()
	{
		private TView mView;
		private bool mDisposed;
		private bool mInitialized;

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

		protected sealed override void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			if (disposing)
			{
				if (mView != null)
				{
					DetachView();
					mView.Dispose();
					mView = null;
				}
			}

			DisposeOverride(disposing);

			mDisposed = true;
		}
		protected virtual void DisposeOverride(bool disposing)
		{
		}

		protected bool IsViewDisposed
		{
			get
			{
				if (View == null)
					return true;

				var window = View as Form;
				if (window == null)
					return false;

				return window.IsDisposed;
			}
		}

		protected abstract void AttachView();
		protected abstract void DetachView();

		public TView View
		{
			get
			{
				return mView;
			}
		}
	}
}