using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	public abstract class CanvasVisual<T> : IDisposable where T: Canvas
	{
		private T mCanvas;
		private Control mControl;

		private Color mGridLineColor;
		private Color mBackdropColor;

		~CanvasVisual()
		{
			Dispose(false);
		}
		protected CanvasVisual([NotNull] T canvas)
		{
			if (canvas == null) throw new ArgumentNullException("canvas");
			mCanvas = canvas;
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				Detach();
			}

			mControl = null;
			mCanvas = null;
		}
		protected abstract void OnControlPaint([NotNull] Graphics graphics);

		public T Canvas
		{
			get { return mCanvas; }
		}
		public Color GridLineColor
		{
			get { return mGridLineColor; }
			set
			{
				mGridLineColor = value;
				InvalidateControl();
			}
		}
		public Color BackdropColor
		{
			get { return mBackdropColor; }
			set
			{
				mBackdropColor = value;
				InvalidateControl();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Attach([NotNull] Control control)
		{
			if (control == null) throw new ArgumentNullException("control");

			mControl = control;

			mControl.Paint += OnCanvasPaint;
			mControl.Resize += OnCanvasResize;
		}
		public void Detach()
		{
			if (mControl == null)
				return;

			mControl.Paint -= OnCanvasPaint;
			mControl.Resize -= OnCanvasResize;

			mControl = null;
		}

		protected void InvalidateControl()
		{
			if (mControl == null)
				return;

			mControl.Refresh();
		}
		protected Font GetFont()
		{
			if (mControl == null)
				return null;

			return mControl.Font;
		}

		private void OnCanvasPaint(object sender, PaintEventArgs e)
		{
			OnControlPaint(e.Graphics);
		}
		private void OnCanvasResize(object sender, EventArgs e)
		{
			mCanvas.Resize(new Vector2(mControl.Width, mControl.Height));
			InvalidateControl();
		}
	}
}