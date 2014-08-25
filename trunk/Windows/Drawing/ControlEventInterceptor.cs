using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public abstract class ControlEventInterceptor : IDisposable
	{
		private Control mControl;

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

		protected void InvalidateControl()
		{
			if (mControl == null)
				return;

			mControl.Refresh();
		}

		public void Attach([NotNull] Control control)
		{
			if (control == null) throw new ArgumentNullException("control");

			mControl = control;
			RegisterEvents(control);
		}
		public void Detach()
		{
			if (mControl == null)
				return;

			UnregisterEvents(mControl);
			mControl = null;
		}
	}
}