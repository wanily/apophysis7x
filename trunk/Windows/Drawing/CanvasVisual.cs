using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public abstract class CanvasVisual<T> : ControlEventInterceptor where T: Canvas
	{
		private T mCanvas;

		private Color mGridZeroLineColor;
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
		protected override void DisposeOverride(bool disposing)
		{
			mCanvas = null;
		}

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
		public Color GridZeroLineColor
		{
			get { return mGridZeroLineColor; }
			set
			{
				mGridZeroLineColor = value;
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

		protected override void RegisterEvents(Control control)
		{
			control.Paint += OnCanvasPaint;
			control.Resize += OnCanvasResize;
		}
		protected override void UnregisterEvents(Control control)
		{
			control.Paint -= OnCanvasPaint;
			control.Resize -= OnCanvasResize;
		}

		protected abstract void OnControlPaint([NotNull] Graphics graphics);

		private void OnCanvasPaint(object sender, PaintEventArgs e)
		{
			OnControlPaint(e.Graphics);
		}
		private void OnCanvasResize(object sender, EventArgs e)
		{
			var control = sender as Control;
			if (control == null)
				return;

			mCanvas.Resize(new Vector2(control.Width, control.Height));
			InvalidateControl();
		}
	}
}