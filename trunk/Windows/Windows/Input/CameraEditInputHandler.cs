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
			mDragOrigin = mFlame.Camera.Origin.Copy();
			mDragAngle = System.Math.Atan2(cursor.Y - AttachedControl.ClientSize.Height / 2.0, AttachedControl.ClientSize.Width / 2.0 - cursor.X);
			mDragAngleOld = mFlame.Camera.GetAxisAngle(Axis.X);

			return true;
		}
		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mFlame == null || !mIsMouseDown)
				return false;

			var c = mFlame.PixelToWorld(cursor);
			var c0 = mFlame.PixelToWorld(mDragCursor);

			switch (EditMode)
			{
				case CameraEditMode.Pan:

					var o = c - c0 + mDragOrigin;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = (c - c0).Abs();
						if (d.X > d.Y) o.Y = mDragOrigin.Y;
						else o.X = mDragOrigin.X;
					}

					mFlame.Camera.Origin.X = o.X;
					mFlame.Camera.Origin.Y = o.Y;
					mInputVisual.Operation = new PanOperation(o);

					return true;
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

					mFlame.Camera.SetAngle(mDragAngleOld + angle);
					mInputVisual.Operation = new RotateCanvasOperation(-angle);

					return true;
				case CameraEditMode.ZoomIn:
					return false;
				case CameraEditMode.ZoomOut:
					return false;
				default:
					return false;
			}
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

			return true;
		}
	}
}
