using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	abstract class ControlEventInterceptor<T> : IDisposable where T: Control
	{
		private T mControl;

		~ControlEventInterceptor()
		{
			Dispose(false);
		}

		protected ControlEventInterceptor([NotNull] T control)
		{
			if (control == null) throw new ArgumentNullException("control");
			Attach(control);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				Detach();
			}

			DisposeOverride(disposing);
			mControl = null;
		}
		protected virtual void DisposeOverride(bool disposing)
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected T AttachedControl
		{
			get { return mControl; }
		}

		protected abstract void RegisterEvents(T control);
		protected abstract void UnregisterEvents(T control);

		public void InvalidateControl()
		{
			if (mControl == null)
				return;

			mControl.Refresh();
		}

		private void Attach([NotNull] T control)
		{
			if (control == null) throw new ArgumentNullException("control");

			mControl = control;
			RegisterEvents(control);
		}
		private void Detach()
		{
			if (mControl == null)
				return;

			UnregisterEvents(mControl);
			mControl = null;
		}
	}

	abstract class ControlEventInterceptor : ControlEventInterceptor<Control>
	{
		protected ControlEventInterceptor([NotNull] Control control) : base(control)
		{
		}
	}
}