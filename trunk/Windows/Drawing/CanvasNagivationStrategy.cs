using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public abstract class CanvasNagivationStrategy<T> : IDisposable where T: Canvas
	{
		private T mCanvas;
		private EventHandler mCanvasUpdated;

		private Vector2 mNavigationOrigin;
		private Vector2 mNavigationOffset;
		private bool mIsNavigating;

		private Control mControl;

		~CanvasNagivationStrategy()
		{
			Dispose(false);
		}
		protected CanvasNagivationStrategy([NotNull] T canvas)
		{
			if (canvas == null) throw new ArgumentNullException("canvas");
			mCanvas = canvas;
		}

		protected void RaiseCanvasUpdated()
		{
			if (mCanvasUpdated == null)
				return;

			mCanvasUpdated(this, new EventArgs());
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				Detach();
			}

			EndNavigate();
			mControl = null;
			mCanvas = null;
		}
		
		[NotNull] protected abstract Vector2 GetCurrentOffset();
		[NotNull] protected virtual Vector2 GetNextOffset(Vector2 cursor)
		{
			return (mNavigationOffset - cursor + mNavigationOrigin)/Canvas.Ratio;
		}

		public event EventHandler CanvasUpdated
		{
			add { mCanvasUpdated += value; }
			remove { mCanvasUpdated -= value; }
		}

		public T Canvas
		{
			get { return mCanvas; }
		}
		public bool IsNavigating
		{
			get { return mIsNavigating; }
		}

		public void BeginNavigate(Vector2 cursor)
		{
			var offset = GetCurrentOffset() * Canvas.Ratio;

			mNavigationOrigin = cursor;
			mNavigationOffset = offset;
			mIsNavigating = true;
		}
		public void EndNavigate()
		{
			mNavigationOrigin = null;
			mNavigationOffset = null;
			mIsNavigating = false;
		}

		public abstract void NavigateOffset(Vector2 cursor);
		public abstract void NavigateRotate(Vector2 cursor);
		public abstract void NavigateZoom(double delta);
		public abstract void NavigateReset();

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Attach([NotNull] Control control)
		{
			if (control == null) throw new ArgumentNullException("control");

			mControl = control;

			mControl.MouseDown += OnCanvasMouseDown;
			mControl.MouseUp += OnCanvasMouseUp;
			mControl.MouseMove += OnCanvasMouseMove;
			mControl.MouseWheel += OnCanvasMouseWheel;
			mControl.MouseDoubleClick += OnCanvasMouseDoubleClick;
		}
		public void Detach()
		{
			if (mControl == null)
				return;

			mControl.MouseDown -= OnCanvasMouseDown;
			mControl.MouseUp -= OnCanvasMouseUp;
			mControl.MouseMove -= OnCanvasMouseMove;
			mControl.MouseWheel -= OnCanvasMouseWheel;
			mControl.MouseDoubleClick -= OnCanvasMouseDoubleClick;

			mControl = null;
		}

		protected virtual void OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (button == MouseButtons.Left)
			{
				NavigateOffset(cursor);
			}
		}
		protected virtual void OnAttachedControlMouseWheel(double delta, MouseButtons button)
		{
			if (button == MouseButtons.None)
			{
				NavigateZoom(delta);
			}
		}

		private void InvalidateControl()
		{
			if (mControl == null)
				return;

			mControl.Refresh();
		}

		private void OnCanvasMouseDown(object sender, MouseEventArgs e)
		{
			var cursor = new Vector2(e.X, e.Y);
			BeginNavigate(cursor);
		}
		private void OnCanvasMouseUp(object sender, MouseEventArgs e)
		{
			EndNavigate();
		}
		private void OnCanvasMouseMove(object sender, MouseEventArgs e)
		{
			OnAttachedControlMouseMove(new Vector2(e.X, e.Y), e.Button);
			InvalidateControl();
		}
		private void OnCanvasMouseWheel(object sender, MouseEventArgs e)
		{
			OnAttachedControlMouseWheel(e.Delta, e.Button);
			InvalidateControl();
		}
		private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
		{
			NavigateReset();
			InvalidateControl();
		}
	}
}
