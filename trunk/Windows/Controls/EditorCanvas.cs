using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Drawing;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;
using Rectangle = Xyrus.Apophysis.Windows.Math.Rectangle;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class EditorCanvas : UserControl
	{
		private TransformCollection mTransforms;

		private Color mGridLineColor; 
		private Color mBackdropColor;

		private Vector2 mDragStart;
		private Vector2 mOffsetStart;
		private bool mIsDragging;

		private Grid mGrid;

		public EditorCanvas()
		{
			InitializeComponent();

			GridLineColor = Color.DarkGray;
			BackdropColor = Color.FromArgb(0x10, 0xff, 0xff, 0xff);

			MouseWheel += OnCanvasMouseWheel;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				if (mTransforms != null)
				{
					mTransforms.ContentChanged -= OnTransformCollectionChanged;
					mTransforms = null;
				}
			}

			base.Dispose(disposing);
		}

		public TransformCollection Transforms
		{
			get { return mTransforms; }
			set
			{
				if (mTransforms != null)
					mTransforms.ContentChanged -= OnTransformCollectionChanged;

				mTransforms = value;

				if (value != null)
					value.ContentChanged += OnTransformCollectionChanged;
			}
		}
		public Color GridLineColor
		{
			get { return mGridLineColor; }
			set
			{
				mGridLineColor = value;
				Refresh();
			}
		}
		public Color BackdropColor
		{
			get { return mBackdropColor; }
			set
			{
				mBackdropColor = value;
				Refresh();
			}
		}

		private Rectangle GetWorldBounds(Vector2 snapScale)
		{
			var u = mGrid.CanvasToWorld(new Vector2(0, 0));
			var v = mGrid.CanvasToWorld(new Vector2(Width, Height));

			var sx0 = mGrid.Snap(u.X, snapScale.X, CanvasSnapBehavior.Floor);
			var sx1 = mGrid.Snap(v.X, snapScale.X, CanvasSnapBehavior.Ceil);

			var sy0 = mGrid.Snap(u.Y, snapScale.Y, CanvasSnapBehavior.Ceil);
			var sy1 = mGrid.Snap(v.Y, snapScale.Y, CanvasSnapBehavior.Floor);

			var c0 = new Vector2(sx0, sy0);
			var c1 = new Vector2(sx1, sy1);

			return new Rectangle(c0, c1 - c0);
		}
		private Rectangle GetCanvasBounds(Vector2 snapScale)
		{
			var wb = GetWorldBounds(snapScale);

			var c0 = mGrid.WorldToCanvas(wb.TopLeft);
			var c1 = mGrid.WorldToCanvas(wb.BottomRight);

			return new Rectangle(c0, c1 - c0);
		}

		private void DrawBackground(Graphics g, Vector2 scale, Brush brush, Orientation? orientation)
		{
			if (mGrid == null)
				return;

			var step = (scale * mGrid.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);
			var invScale = 1.0/scale;

			if (!orientation.HasValue)
			{
				for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
				{
					for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
					{
						var xy = new Vector2(x, y);
						var uv = mGrid.CanvasToWorld(xy * invScale);

						var uI = (int)System.Math.Round(uv.X);
						var vI = (int)System.Math.Round(uv.Y);

						if ((uI % 2 == 0 && vI % 2 == 0) || (uI % 2 != 0 && vI % 2 != 0))
						{
							g.FillRectangle(brush, new System.Drawing.Rectangle((int)xy.X, (int)xy.Y, (int)step.X, (int)step.Y));
						}
					}
				}
			}
			else if (orientation.Value == Orientation.Horizontal)
			{
				for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
				{
					var vI = (int)System.Math.Round(mGrid.CanvasToWorld(y * invScale.Y));
					if (vI % 2 == 0)
					{
						g.FillRectangle(brush, new System.Drawing.Rectangle((int)bounds.Corner.X, (int)y, (int)bounds.Size.X, (int)y));
					}
				}
			}
			else if (orientation.Value == Orientation.Horizontal)
			{
				for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
				{
					var uI = (int)System.Math.Round(mGrid.CanvasToWorld(x * invScale.X));
					if (uI % 2 == 0)
					{
						g.FillRectangle(brush, new System.Drawing.Rectangle((int)x, (int)bounds.Corner.Y, (int)x, (int)bounds.Size.Y));
					}
				}
			}
		}
		private void DrawGrid(Graphics g, Vector2 scale, Pen pen, Orientation? orientation)
		{
			if (mGrid == null)
				return;

			var step = (scale * mGrid.Ratio).Abs();
			var bounds = GetCanvasBounds(scale);

			if (!orientation.HasValue || orientation.Value == Orientation.Horizontal)
			{
				for (double y = bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y += step.Y)
				{
					g.DrawLine(pen, new Point((int)bounds.TopLeft.X, (int)y), new Point((int)bounds.BottomRight.X, (int)y));
				}
			}

			if (!orientation.HasValue || orientation.Value == Orientation.Vertical)
			{
				for (double x = bounds.TopLeft.X; x <= bounds.BottomRight.X; x += step.X)
				{
					g.DrawLine(pen, new Point((int)x, (int)bounds.TopLeft.Y), new Point((int)x, (int)bounds.BottomRight.Y));
				}
			}

		}

		private void BeginDrag()
		{

		}
		private void DragExecute(Vector2 cursor)
		{
			mGrid.Pan((mOffsetStart - cursor + mDragStart) / mGrid.Ratio);
		}
		private void WheelExecute(int delta)
		{
			mGrid.Zoom(delta);
		}
		private void ResetExecute()
		{
			mGrid.Reset();
		}
		private void EndDrag()
		{

		}

		private void OnTransformCollectionChanged(object sender, EventArgs e)
		{
			Refresh();
		}
		private void OnCanvasPaint(object sender, PaintEventArgs e)
		{
			if (mGrid == null)
				return;

			var g = e.Graphics;

			var glc = Color.FromArgb(0xff, GridLineColor.R, GridLineColor.G, GridLineColor.B);
			var glc05 = Color.FromArgb(0x80, GridLineColor.R, GridLineColor.G, GridLineColor.B);

			using (var backdropBrush = new SolidBrush(BackdropColor))
			using (var gridlinePen = new Pen(glc, 1.0f))
			using (var gridlinePenHalf = new Pen(glc05, 1.0f))
			{
				var scale = new Vector2(mGrid.Scale, mGrid.Scale);

				DrawBackground(g, scale, backdropBrush, null);
				DrawGrid(g, scale, gridlinePen, null);
				DrawGrid(g, scale * 0.1, gridlinePenHalf, null);
			}
		}
		private void OnCanvasResized(object sender, EventArgs e)
		{
			if (mGrid == null)
			{
				mGrid = new Grid(new Vector2(Width, Height));
				Refresh();
				return;
			}

			mGrid.Resize(new Vector2(Width, Height));
			Refresh();
		}

		private void OnCanvasMouseDown(object sender, MouseEventArgs e)
		{
			var cursor = new Vector2(e.X, e.Y);
			var offset = mGrid.Offset*mGrid.Ratio;

			mDragStart = cursor;
			mOffsetStart = offset;
			mIsDragging = true;

			if (!DesignMode)
			{
				BeginDrag();
				Refresh();
			}
		}
		private void OnCanvasMouseUp(object sender, MouseEventArgs e)
		{
			if (!DesignMode)
			{
				EndDrag();
			}

			mDragStart = null;
			mIsDragging = false;

			if (!DesignMode)
			{
				Refresh();
			}
		}
		private void OnCanvasMouseMove(object sender, MouseEventArgs e)
		{
			if (DesignMode)
			{
				return;
			}

			if (mIsDragging)
			{
				var cursor = new Vector2(e.X, e.Y);
				DragExecute(cursor);
			}

			Refresh();
		}
		private void OnCanvasMouseWheel(object sender, MouseEventArgs e)
		{
			WheelExecute(e.Delta);
			Refresh();
		}
		private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
		{
			ResetExecute();
		}
	}
}
