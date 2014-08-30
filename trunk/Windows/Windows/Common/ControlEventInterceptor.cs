using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	abstract class ControlEventInterceptor : IDisposable
	{
		private Control mControl;

		~ControlEventInterceptor()
		{
			Dispose(false);
		}

		protected ControlEventInterceptor([NotNull] Control control)
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

		protected Control AttachedControl
		{
			get { return mControl; }
		}

		protected abstract void RegisterEvents(Control control);
		protected abstract void UnregisterEvents(Control control);

		public void InvalidateControl()
		{
			if (mControl == null)
				return;

			mControl.Refresh();
		}

		private void Attach([NotNull] Control control)
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
}