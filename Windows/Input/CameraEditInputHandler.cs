using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
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
		private float mDragAngle;
		private Vector2? mDragOrigin;
		private Vector2? mDragCursor;
		private bool mIsMouseDown;
		private float mDragAngleOld;
		private float mDragScale;
		private float mDragZoom;
		private bool mMoved;

		private static readonly float mLog2 = Float.LogN(2);

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
			mMoved = false;

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
		protected override bool OnAttachedControlMouseWheel(float delta, MouseButtons button)
		{
			return false;
		}

		protected override bool OnAttachedControlMouseDown(Vector2 cursor)
		{
			if (mFlame == null)
				return false;

			mIsMouseDown = true;

			mDragCursor = cursor;
			mDragOrigin = mFlame.Origin;
			mDragAngle = Float.Atan2(cursor.Y - AttachedControl.ClientSize.Height / 2.0f, AttachedControl.ClientSize.Width / 2.0f - cursor.X);
			mDragAngleOld = mFlame.Angle;
			mDragScale = mFlame.PixelsPerUnit;
			mDragZoom = mFlame.Zoom; 
			mMoved = false;

			mData = new CameraData
			{
				Origin = mDragOrigin.Value,
				Angle = mDragAngleOld,
				Scale = mFlame.PixelsPerUnit,
				Zoom = mFlame.Zoom
			};

			mInputVisual.EditData = mData;

			switch (EditMode)
			{
				case CameraEditMode.Pan:
					mInputVisual.Operation = new PanOperation(mDragOrigin.Value, mDragOrigin.Value);
					break;
				case CameraEditMode.Rotate:
					mInputVisual.Operation = new RotateCanvasOperation(mDragAngleOld, mDragAngleOld);
					break;
				case CameraEditMode.ZoomIn:
				case CameraEditMode.ZoomOut:
					var value = UseScale ? mDragScale : mDragZoom;
					mInputVisual.Operation = new ZoomOperation(value, value, 
						new Rectangle(mDragCursor.Value.ToPoint(), new Size(1,1)), 
						new Rectangle(mDragCursor.Value.ToPoint(), new Size(1,1)),
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
			mMoved = true;

			switch (EditMode)
			{
				case CameraEditMode.Pan:

					var scale = Float.Power(2, mData.Zoom) * mData.Scale;
					var point = mDragOrigin + (mDragCursor - cursor).Value / scale;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = Vector2.Abs((cursor - mDragCursor).Value);
						point = d.X > d.Y 
							? new Vector2(point.Value.X, mDragOrigin.GetValueOrDefault().Y / scale) 
							: new Vector2(mDragOrigin.GetValueOrDefault().X / scale, point.Value.Y);
					}

					mData.Origin = point.Value;
					mInputVisual.Operation = new PanOperation(point.Value, mDragOrigin.GetValueOrDefault());

					break;
				case CameraEditMode.Rotate:

					var angle = Float.Atan2(
						cursor.Y - AttachedControl.ClientSize.Height / 2.0f, 
						AttachedControl.ClientSize.Width / 2.0f - cursor.X) -
						mDragAngle;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var ang = angle * 180 / Float.Pi;
						var snapped = Float.Round(ang / 15) * 15;

						angle = snapped * Float.Pi / 180.0f;
					}

					mData.Angle = (mDragAngleOld + angle);
					mInputVisual.Operation = new RotateCanvasOperation(angle + mDragAngleOld, mDragAngleOld);

					break;
				case CameraEditMode.ZoomIn:
				case CameraEditMode.ZoomOut:
					
					Rectangle innerRectangle = new Rectangle(
						mDragCursor.GetValueOrDefault().ToPoint(), 
						new Size(
							(int)(cursor.X - mDragCursor.GetValueOrDefault().X), 
							(int)(cursor.Y - mDragCursor.GetValueOrDefault().Y))), 
							outerRectangle;

					var dx = (float)innerRectangle.Width;
					var dy = (float)innerRectangle.Height;

					var size = mInputVisual.FitFrame ? AttachedControl.ClientSize : mFlame.CanvasSize.FitToFrame(AttachedControl.ClientSize);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						if (System.Math.Abs(dy) < float.Epsilon || System.Math.Abs(dx / dy) >= (float)size.Width / size.Height)
						{
							dy = Float.Round(dx/size.Width*size.Height);
						}
						else
						{
							dx = Float.Round(dy/size.Height*size.Width);
						}

						outerRectangle = new Rectangle(innerRectangle.Left - (int)dx, innerRectangle.Top - (int)dy, (int)dx * 2, (int)dy * 2);
					}
					else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
					{
						Point p1 = mDragCursor.GetValueOrDefault().ToPoint(), p2;
						var sign = dx * dy >= 0 ? 1 : -1;

						if (System.Math.Abs(dy) < float.Epsilon || System.Math.Abs(dx / dy) >= (float)size.Width / size.Height)
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
						var dc = mDragCursor.GetValueOrDefault();

						if (System.Math.Abs(dy) < float.Epsilon || System.Math.Abs(dx / dy) >= (float)size.Width / size.Height)
						{
							var y1 = (int)((cursor.Y + dc.Y) / 2) - sign * (int)System.Math.Round(dx / 2 / size.Width * size.Height);
							var y2 = (int)((cursor.Y + dc.Y) / 2) + sign * (int)System.Math.Round(dx / 2 / size.Width * size.Height);
							outerRectangle = new Rectangle((int)dc.X, y1, innerRectangle.Width, y2 - y1);
						}
						else
						{
							var x1 = (int)((cursor.X + dc.X) / 2) - sign * (int)System.Math.Round(dy / 2 / size.Height * size.Width);
							var x2 = (int)((cursor.X + dc.X) / 2) + sign * (int)System.Math.Round(dy / 2 / size.Height * size.Width);
							outerRectangle = new Rectangle(x1, (int)dc.Y, x2 - x1, innerRectangle.Height);
						}
					}

					var oldZoom = Float.Power(2, mDragZoom);
					var cos = Float.Cos(mDragAngle);
					var sin = Float.Sin(mDragAngle);

					if (EditMode == CameraEditMode.ZoomIn)
					{
						var doc = mDragOrigin.GetValueOrDefault();
						var oldppu = mDragScale * oldZoom;

						var x = ((outerRectangle.Left + outerRectangle.Right) / 2.0f - size.Width / 2.0f) / oldppu;
						var y = ((outerRectangle.Top + outerRectangle.Bottom) / 2.0f - size.Height / 2.0f) / oldppu;

						mData.Origin = new Vector2(doc.X - cos * x + sin * y, doc.Y - sin * x - cos * y);

						if (UseScale)
						{
							mData.Scale = mDragScale * size.Width / (System.Math.Abs(outerRectangle.Right - outerRectangle.Left));
						}
						else
						{
							mData.Zoom = Float.LogN(oldZoom * (size.Width / (Float.Abs(outerRectangle.Right - outerRectangle.Left) + 1))) / mLog2;
						}
					}
					else
					{
						var doc = mDragOrigin.GetValueOrDefault();

						if (UseScale)
						{
							mData.Scale = mDragScale / size.Width * (System.Math.Abs(outerRectangle.Right - outerRectangle.Left)); 
						}
						else
						{
							mData.Zoom = Float.LogN(oldZoom / (size.Width / (Float.Abs(outerRectangle.Right - outerRectangle.Left) + 1))) / mLog2;
						}

						var newppu = mData.Scale * Float.Power(2, mData.Zoom);

						var x = ((outerRectangle.Left + outerRectangle.Right) / 2.0f - size.Width / 2.0f) / newppu;
						var y = ((outerRectangle.Top + outerRectangle.Bottom) / 2.0f - size.Height / 2.0f) / newppu;

						mData.Origin = new Vector2(doc.X + cos * x - sin * y, doc.Y + sin * x + cos * y);
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
				EndEdit(this, new CameraEndEditEventArgs(data, mMoved));

			mMoved = false;

			return true;
		}

		public event EventHandler BeginEdit;
		public event CameraEndEditEventHandler EndEdit;
		public event CameraChangedEventHandler CameraChanged;
	}
}
