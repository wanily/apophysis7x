using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public abstract class CanvasNagivationStrategy<T> : ControlEventInterceptor where T: Canvas
	{
		private T mCanvas;
		private Vector2 mNavigationOrigin;
		private Vector2 mNavigationOffset;
		private bool mIsNavigating;
		private bool mIsSuspended;

		protected CanvasNagivationStrategy([NotNull] Control control, [NotNull] T canvas) : base(control)
		{
			if (canvas == null) throw new ArgumentNullException("canvas");
			mCanvas = canvas;
		}
		protected override void DisposeOverride(bool disposing)
		{
			EndNavigate();
			mCanvas = null;
		}

		[NotNull] protected abstract Vector2 GetCurrentOffset();
		[NotNull] protected virtual Vector2 GetNextOffset([NotNull] Vector2 cursor)
		{
			return (mNavigationOffset - cursor + mNavigationOrigin)/Canvas.Ratio;
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

		public abstract void NavigateOffset([NotNull] Vector2 cursor);
		public abstract void NavigateRotate([NotNull] Vector2 cursor);
		public abstract void NavigateZoom(double delta);
		public abstract void NavigateReset();

		public void Suspend()
		{
			mIsSuspended = true;
			EndNavigate();
		}
		public void Resume()
		{
			mIsSuspended = false;
		}

		protected virtual void OnAttachedControlMouseMove([NotNull] Vector2 cursor, MouseButtons button)
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

		protected override void RegisterEvents(Control control)
		{
			control.MouseDown += OnCanvasMouseDown;
			control.MouseUp += OnCanvasMouseUp;
			control.MouseMove += OnCanvasMouseMove;
			control.MouseWheel += OnCanvasMouseWheel;
			control.MouseDoubleClick += OnCanvasMouseDoubleClick;
		}
		protected override void UnregisterEvents(Control control)
		{
			control.MouseDown -= OnCanvasMouseDown;
			control.MouseUp -= OnCanvasMouseUp;
			control.MouseMove -= OnCanvasMouseMove;
			control.MouseWheel -= OnCanvasMouseWheel;
			control.MouseDoubleClick -= OnCanvasMouseDoubleClick;
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
			if (mIsSuspended)
				return;

			OnAttachedControlMouseMove(new Vector2(e.X, e.Y), e.Button);
			InvalidateControl();
		}
		private void OnCanvasMouseWheel(object sender, MouseEventArgs e)
		{
			if (mIsSuspended)
				return;

			OnAttachedControlMouseWheel(e.Delta, e.Button);
			InvalidateControl();
		}
		private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (mIsSuspended)
				return;

			NavigateReset();
			InvalidateControl();
		}
	}
}
