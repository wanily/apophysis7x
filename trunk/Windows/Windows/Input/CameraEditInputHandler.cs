using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Input
{
	class CameraEditInputHandler : InputHandler
	{
		private PreviewInputVisual mInputVisual;
		private Flame mFlame;

		private double mDragAngle;
		private Vector2 mDragOrigin;
		private Vector2 mDragCursor;
		private bool mIsMouseDown;
		private double mDragAngleOld;

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

			if (BeginEdit != null)
				BeginEdit(this, new EventArgs());

			return true;
		}
		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mFlame == null || !mIsMouseDown)
				return false;

			mInputVisual.Operation = null;

			switch (EditMode)
			{
				case CameraEditMode.Pan:

					var scale = System.Math.Pow(2, mFlame.Zoom) * mFlame.PixelsPerUnit;
					var point = mDragOrigin + (cursor - mDragCursor) / scale;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = (cursor - mDragCursor).Abs();
						if (d.X > d.Y) point.Y = mDragOrigin.Y / scale;
						else point.X = mDragOrigin.X / scale;
					}

					mFlame.Origin = point;
					mInputVisual.Operation = new PanOperation(mFlame, point, mDragOrigin);

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

					mFlame.Angle = (mDragAngleOld + angle);
					mInputVisual.Operation = new RotateCanvasOperation(mFlame, angle + mDragAngleOld, mDragAngleOld);

					break;
				case CameraEditMode.ZoomIn:
					break;
				case CameraEditMode.ZoomOut:
					break;
			}

			if (mInputVisual.Operation != null && CameraChanged != null)
			{
				CameraChanged(this, new CameraChangedEventArgs(mInputVisual.Operation));
			}

			return mInputVisual.Operation != null;
		}
		protected override bool OnAttachedControlMouseUp()
		{
			if (mFlame == null)
				return false;

			mInputVisual.Operation = null;
			mIsMouseDown = false;

			mDragCursor = null;
			mDragOrigin = null;
			mDragAngle = 0;
			mDragAngleOld = 0;

			if (EndEdit != null)
				EndEdit(this, new EventArgs());

			return true;
		}

		public event EventHandler BeginEdit;
		public event EventHandler EndEdit;
		public event CameraChangedEventHandler CameraChanged;
	}
}
