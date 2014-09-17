using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Visuals;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Input
{
	class CameraEditInputHandler : InputHandler
	{
		private PreviewInputVisual mInputVisual;
		private Flame mFlame;

		private CameraData mData;
		private double mDragAngle;
		private Vector2 mDragOrigin;
		private Vector2 mDragCursor;
		private bool mIsMouseDown;
		private double mDragAngleOld;
		private double mDragScale;
		private double mDragZoom;

		private static readonly double mLog2 = System.Math.Log(2);

		public CameraEditInputHandler([NotNull] Control control, [NotNull] PreviewInputVisual inputVisual) : base(control)
		{
			if (inputVisual == null) throw new ArgumentNullException("inputVisual");
			mInputVisual = inputVisual;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mInputVisual = null;
			mFlame = null;

			mDragCursor = null;
			mDragOrigin = null;
			mDragAngle = 0;
			mDragAngleOld = 0;
			mDragScale = 0;
			mDragZoom = 0;

			mIsMouseDown = false;
		}

		public Flame Flame
		{
			get { return mFlame; }
			set { mFlame = value; }
		}

		public bool UseScale { get; set; }
		public CameraEditMode EditMode { get; set; }

		protected override bool OnAttachedControlKeyPress(Keys key, Keys modifiers)
		{
			return false;
		}

		protected override bool OnAttachedControlMouseDoubleClick()
		{
			return false;
		}
		protected override bool OnAttachedControlMouseWheel(double delta, MouseButtons button)
		{
			return false;
		}

		protected override bool OnAttachedControlMouseDown(Vector2 cursor)
		{
			if (mFlame == null)
				return false;

			mIsMouseDown = true;

			mDragCursor = cursor;
			mDragOrigin = mFlame.Origin.Copy();
			mDragAngle = System.Math.Atan2(cursor.Y - AttachedControl.ClientSize.Height / 2.0, AttachedControl.ClientSize.Width / 2.0 - cursor.X);
			mDragAngleOld = mFlame.Angle;
			mDragScale = mFlame.PixelsPerUnit;
			mDragZoom = mFlame.Zoom;

			mData = new CameraData
			{
				Origin = mDragOrigin,
				Angle = mDragAngleOld,
				Scale = mFlame.PixelsPerUnit,
				Zoom = mFlame.Zoom
			};

			mInputVisual.EditData = mData;

			switch (EditMode)
			{
				case CameraEditMode.Pan:
					mInputVisual.Operation = new PanOperation(mDragOrigin, mDragOrigin);
					break;
				case CameraEditMode.Rotate:
					mInputVisual.Operation = new RotateCanvasOperation(mDragAngleOld, mDragAngleOld);
					break;
				case CameraEditMode.ZoomIn:
				case CameraEditMode.ZoomOut:
					var value = UseScale ? mDragScale : mDragZoom;
					mInputVisual.Operation = new ZoomOperation(value, value, 
						new Rectangle(mDragCursor.ToPoint(), new Size(1,1)), 
						new Rectangle(mDragCursor.ToPoint(), new Size(1,1)),
						UseScale);
					break;
			}

			if (BeginEdit != null)
				BeginEdit(this, new EventArgs());

			return true;
		}
		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mData == null || !mIsMouseDown)
				return false;

			mInputVisual.Operation = null;

			switch (EditMode)
			{
				case CameraEditMode.Pan:

					var scale = System.Math.Pow(2, mData.Zoom) * mData.Scale;
					var point = mDragOrigin + (cursor - mDragCursor) / scale;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = (cursor - mDragCursor).Abs();
						if (d.X > d.Y) point.Y = mDragOrigin.Y / scale;
						else point.X = mDragOrigin.X / scale;
					}

					mData.Origin = point;
					mInputVisual.Operation = new PanOperation(point, mDragOrigin);

					break;
				case CameraEditMode.Rotate:

					var angle = System.Math.Atan2(
						cursor.Y - AttachedControl.ClientSize.Height / 2.0, 
						AttachedControl.ClientSize.Width / 2.0 - cursor.X) -
						mDragAngle;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var ang = angle * 180 / System.Math.PI;
						var snapped = System.Math.Round(ang / 15) * 15;

						angle = snapped * System.Math.PI / 180.0;
					}

					mData.Angle = (mDragAngleOld + angle);
					mInputVisual.Operation = new RotateCanvasOperation(angle + mDragAngleOld, mDragAngleOld);

					break;
				case CameraEditMode.ZoomIn:
				case CameraEditMode.ZoomOut:
					
					Rectangle innerRectangle = new Rectangle(mDragCursor.ToPoint(), new Size((int)(cursor.X - mDragCursor.X), (int)(cursor.Y - mDragCursor.Y))), outerRectangle;

					var dx = (double)innerRectangle.Width;
					var dy = (double)innerRectangle.Height;

					var size = mInputVisual.FitFrame ? AttachedControl.ClientSize : mFlame.CanvasSize;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						if (System.Math.Abs(dy) < double.Epsilon || System.Math.Abs(dx/dy) >= (double) size.Width/size.Height)
						{
							dy = System.Math.Round(dx/size.Width*size.Height);
						}
						else
						{
							dx = System.Math.Round(dy/size.Height*size.Width);
						}

						outerRectangle = new Rectangle(innerRectangle.Left - (int)dx, innerRectangle.Top - (int)dy, (int)dx * 2, (int)dy * 2);
					}
					else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
					{
						Point p1 = mDragCursor.ToPoint(), p2;
						var sign = dx * dy >= 0 ? 1 : -1;

						if (System.Math.Abs(dy) < double.Epsilon || System.Math.Abs(dx/dy) >= (double) size.Width/size.Height)
						{
							p2 = new Point((int) cursor.X, p1.Y + sign*(int) System.Math.Round(dx/size.Width*size.Height));
						}
						else
						{
							p2 = new Point(p1.X + sign*(int) System.Math.Round(dy/size.Height*size.Width), (int) cursor.Y);
						}

						outerRectangle = new Rectangle(p1, new Size(p2.X - p1.X, p2.Y - p1.Y));
					}
					else
					{
						var sign = dx * dy >= 0 ? 1 : -1;

						if (System.Math.Abs(dy) < double.Epsilon || System.Math.Abs(dx / dy) >= (double)size.Width / size.Height)
						{
							var y1 = (int)((cursor.Y + mDragCursor.Y) / 2) - sign * (int)System.Math.Round(dx / 2 / size.Width * size.Height);
							var y2 = (int)((cursor.Y + mDragCursor.Y) / 2) + sign * (int)System.Math.Round(dx / 2 / size.Width * size.Height);
							outerRectangle = new Rectangle((int)mDragCursor.X, y1, innerRectangle.Width, y2 - y1);
						}
						else
						{
							var x1 = (int)((cursor.X + mDragCursor.X) / 2) - sign * (int)System.Math.Round(dy / 2 / size.Height * size.Width);
							var x2 = (int)((cursor.X + mDragCursor.X) / 2) + sign * (int)System.Math.Round(dy / 2 / size.Height * size.Width);
							outerRectangle = new Rectangle(x1, (int)mDragCursor.Y, x2 - x1, innerRectangle.Height);
						}
					}

					var oldZoom = System.Math.Pow(2, mDragZoom);
					var cos = System.Math.Cos(mDragAngle);
					var sin = System.Math.Sin(mDragAngle);

					if (EditMode == CameraEditMode.ZoomIn)
					{
						var oldppu = mDragScale * oldZoom;

						var x = ((outerRectangle.Left + outerRectangle.Right) / 2.0 - size.Width / 2.0) / oldppu;
						var y = ((outerRectangle.Top + outerRectangle.Bottom) / 2.0 - size.Height / 2.0) / oldppu;

						mData.Origin = new Vector2(mDragOrigin.X + cos * x - sin * y, mDragOrigin.Y + sin * x + cos * y);

						if (UseScale)
						{
							mData.Zoom = System.Math.Log(oldZoom * ((double)mFlame.CanvasSize.Width / (System.Math.Abs(outerRectangle.Right - outerRectangle.Left) + 1))) / mLog2;
						}
						else
						{
							mData.Scale = mDragScale * mFlame.CanvasSize.Width / (System.Math.Abs(outerRectangle.Right - outerRectangle.Left));
						}
					}
					else
					{
						if (UseScale)
						{
							mData.Zoom = System.Math.Log(oldZoom / ((double)mFlame.CanvasSize.Width / (System.Math.Abs(outerRectangle.Right - outerRectangle.Left) + 1))) / mLog2;
						}
						else
						{
							mData.Scale = mDragScale / mFlame.CanvasSize.Width * (System.Math.Abs(outerRectangle.Right - outerRectangle.Left));
						}

						var newppu = mData.Scale * System.Math.Pow(2, mData.Zoom);

						var x = ((outerRectangle.Left + outerRectangle.Right) / 2.0 - size.Width / 2.0) / newppu;
						var y = ((outerRectangle.Top + outerRectangle.Bottom) / 2.0 - size.Height / 2.0) / newppu;

						mData.Origin = new Vector2(mDragOrigin.X - cos * x + sin * y, mDragOrigin.Y - sin * x - cos * y);
					}

					mInputVisual.Operation = new ZoomOperation(UseScale ? mDragScale : mDragZoom, UseScale ? mData.Scale : mData.Zoom, innerRectangle, outerRectangle, UseScale);

					break;
			}

			if (mInputVisual.Operation != null && CameraChanged != null)
			{
				CameraChanged(this, new CameraChangedEventArgs(mInputVisual.Operation, mData));
			}

			return mInputVisual.Operation != null;
		}
		protected override bool OnAttachedControlMouseUp()
		{
			if (mData == null)
				return false;

			var data = mData;

			mInputVisual.Operation = null;
			mInputVisual.EditData = null;
			mIsMouseDown = false;

			mDragCursor = null;
			mDragOrigin = null;
			mDragAngle = 0;
			mDragAngleOld = 0;
			mDragScale = 0;
			mDragZoom = 0;

			if (EndEdit != null)
				EndEdit(this, new CameraEndEditEventArgs(data));

			return true;
		}

		public event EventHandler BeginEdit;
		public event CameraEndEditEventHandler EndEdit;
		public event CameraChangedEventHandler CameraChanged;
	}
}
