using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows
{
	[PublicAPI]
	public abstract class Controller : IController
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
	public abstract class Controller<TView> : Controller, IViewController where TView : class, IView
	{
		private TView mView;
		private bool mDisposed;
		private bool mInitialized;

		protected Controller()
		{
			mView = ApophysisApplication.Container.Resolve<TView>();
		}

		/// <summary>
		/// DO NOT USE ANYMORE!
		/// </summary>
		/// <param name="view"></param>
		[Obsolete]
		protected Controller([NotNull] TView view)
		{
			if (view == null) throw new ArgumentNullException("view");
			mView = view;
		}

		/// <summary>
		/// DO NOT USE ANYMORE!
		/// </summary>
		[Obsolete]
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

		/// <summary>
		/// DO NOT OVERRIDE ANYMORE!
		/// </summary>
		[Obsolete]
		protected virtual void AttachView() { }

		/// <summary>
		/// DO NOT OVERRIDE ANYMORE!
		/// </summary>
		[Obsolete]
		protected virtual void DetachView() { }

		public TView View
		{
			get
			{
				return mView;
			}
		}
	}
}