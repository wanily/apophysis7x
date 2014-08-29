using System;
using System.IO;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Models;
using Xyrus.Apophysis.Windows.Visuals;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Properties;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformInputHandler : InputHandler
	{
		enum HitTestResult
		{
			None = 0,
			O,
			X,
			Y,
			Ox,
			Oy,
			Xy,
			Surface
		}

		private EditorSettings mSettings;
		private TransformVisual mVisual;
		private Canvas mCanvas;

		private Vector2 mDragCursor, mDragOrigin;
		private Vector2 mDragX, mDragY;

		private HitTestResult mLastHitTestResult;
		private bool mIsMouseDown;

		private TransformInputOperation mOperation;

		private static readonly Cursor mMoveCursor;
		private static readonly Cursor mRotateCursor;
		private static readonly Cursor mScaleCursor;

		static TransformInputHandler()
		{
			mMoveCursor = new Cursor(new MemoryStream(Resources.Move));
			mRotateCursor = new Cursor(new MemoryStream(Resources.Rotate));
			mScaleCursor = new Cursor(new MemoryStream(Resources.Scale));
		}
		public TransformInputHandler([NotNull] Control control, [NotNull] TransformVisual visual, [NotNull] Canvas canvas,
			[NotNull] EditorSettings settings) : base(control)
		{
			if (visual == null) throw new ArgumentNullException("visual");
			if (canvas == null) throw new ArgumentNullException("canvas");
			if (settings == null) throw new ArgumentNullException("settings");

			mVisual = visual;
			mCanvas = canvas;
			mSettings = settings;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mVisual != null)
			{
				mVisual.Reset();
				mVisual.IsActive = false;
			}

			mVisual = null;
			mCanvas = null;
			mSettings = null;

			mDragCursor = null;
			mDragOrigin = null;
			mDragX = null;
			mDragY = null;

			mIsMouseDown = false;
			mLastHitTestResult = HitTestResult.None;
			mOperation = null;
		}

		private HitTestResult HitTest(Vector2 cursor)
		{
			var shape = mVisual.GetPolygon();

			var ox = mVisual.GetEdgeOx();
			var oy = mVisual.GetEdgeOy();
			var xy = mVisual.GetEdgeXy();

			var o = ox.A;
			var x = ox.B;
			var y = oy.B;

			const double edgeProximityThreshold = 4;
			const double vertexProximityThreshold = 4;

			if (y.IsInProximity(cursor, vertexProximityThreshold)) return HitTestResult.Y;
			if (x.IsInProximity(cursor, vertexProximityThreshold)) return HitTestResult.X;
			if (o.IsInProximity(cursor, vertexProximityThreshold)) return HitTestResult.O;

			if (xy.IsInProximity(cursor, edgeProximityThreshold)) return HitTestResult.Xy;
			if (ox.IsInProximity(cursor, edgeProximityThreshold)) return HitTestResult.Ox;
			if (oy.IsInProximity(cursor, edgeProximityThreshold)) return HitTestResult.Oy;

			if (shape.IsOnSurface(cursor)) 
				return HitTestResult.Surface;

			return HitTestResult.None;
		}
		private void DragNode(Vector2 cursor, MouseButtons button, HitTestResult hitTest)
		{
			if (button != MouseButtons.Left)
				return;

			var c = mCanvas.CanvasToWorld(cursor);
			var c0 = mCanvas.CanvasToWorld(mDragCursor);

			switch (hitTest)
			{
				case HitTestResult.Surface:
				case HitTestResult.O:

					var o = c - c0 + mDragOrigin;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = (c - c0).Abs();
						if (d.X > d.Y) o.Y = mDragOrigin.Y;
						else o.X = mDragOrigin.X;
					}

					mVisual.Model.Origin.X = o.X;
					mVisual.Model.Origin.Y = o.Y;

					mOperation = new TransformMoveOperation(Transform, mDragOrigin, o);

					break;

				case HitTestResult.X:

					var x = c - c0 + (mDragOrigin + mDragX);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = (c - c0).Abs();
						if (d.X > d.Y) x.Y = mDragX.Y;
						else x.X = mDragX.X;
					}

					mVisual.Model.Affine.X.X = x.X - mDragOrigin.X;
					mVisual.Model.Affine.X.Y = x.Y - mDragOrigin.Y;

					mOperation = new TransformMoveOperation(Transform, mDragX, x);

					break;

				case HitTestResult.Y:
					
					var y = c - c0 + (mDragOrigin + mDragY);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = (c - c0).Abs();
						if (d.X > d.Y) y.Y = mDragY.Y;
						else y.X = mDragY.X;
					}

					mVisual.Model.Affine.Y.X = y.X - mDragOrigin.X;
					mVisual.Model.Affine.Y.Y = y.Y - mDragOrigin.Y;

					mOperation = new TransformMoveOperation(Transform, mDragY, y);

					break;

				case HitTestResult.Ox:
				case HitTestResult.Oy:

					var primary = hitTest == HitTestResult.Ox ? mDragX : mDragY;
					var secondary = hitTest == HitTestResult.Ox ? mDragY : mDragX;

					var normalX = primary.Direction;
					var normalY = secondary.Direction;

					var deltaOrigin = c - mDragOrigin;
					var angleBetweenOxAndOy = System.Math.Atan2(normalY.Y, normalY.X) - System.Math.Atan2(normalX.Y, normalX.X);
					var angleBetweenOxAndDelta = System.Math.Atan2(deltaOrigin.Y, deltaOrigin.X) - System.Math.Atan2(normalX.Y, normalX.X);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var ang = angleBetweenOxAndDelta * 180 / System.Math.PI;
						var snapped = System.Math.Round(ang / mSettings.AngleSnap) * mSettings.AngleSnap;

						angleBetweenOxAndDelta = snapped * System.Math.PI / 180.0;
					}

					var cos0 = System.Math.Cos(angleBetweenOxAndOy);
					var cos1 = System.Math.Cos(angleBetweenOxAndDelta);

					var sin0 = System.Math.Sin(angleBetweenOxAndOy);
					var sin1 = System.Math.Sin(angleBetweenOxAndDelta);

					if (hitTest == HitTestResult.Ox)
					{
						mVisual.Model.Affine.X.X = cos1*primary.X - sin1*primary.Y;
						mVisual.Model.Affine.X.Y = sin1*primary.X + cos1*primary.Y;

						var newNormal = mVisual.Model.Affine.X.Direction;
						var length = mVisual.Model.Affine.Y.Length;

						mVisual.Model.Affine.Y.X = length*(cos0*newNormal.X - sin0*newNormal.Y);
						mVisual.Model.Affine.Y.Y = length*(sin0*newNormal.X + cos0*newNormal.Y);
					}
					else if (hitTest == HitTestResult.Oy)
					{
						mVisual.Model.Affine.Y.X = cos1 * primary.X - sin1 * primary.Y;
						mVisual.Model.Affine.Y.Y = sin1 * primary.X + cos1 * primary.Y;

						var newNormal = mVisual.Model.Affine.Y.Direction;
						var length = mVisual.Model.Affine.X.Length;

						mVisual.Model.Affine.X.X = length * (cos0 * newNormal.X - sin0 * newNormal.Y);
						mVisual.Model.Affine.X.Y = length * (sin0 * newNormal.X + cos0 * newNormal.Y);
					}

					mOperation = new TransformRotateOperation(Transform, angleBetweenOxAndDelta);

					break;

				case HitTestResult.Xy:

					var c0dO = c0 - mDragOrigin;

					var vX = (mDragOrigin + mDragX);
					var vY = (mDragOrigin + mDragY);

					var denom = c0dO.Length;
					if (System.Math.Abs(denom) < double.Epsilon)
						break;

					var scale = (c0dO.X * (c.X - mDragOrigin.X) + c0dO.Y * (c.Y - mDragOrigin.Y)) / (denom * denom);
					if (System.Math.Abs(scale) < double.Epsilon)
						scale = double.Epsilon;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var snap = System.Math.Abs(scale) < 1 ? (100 * 100 / mSettings.ScaleSnap) : mSettings.ScaleSnap;

						var ratio = scale * 100;
						var snapped = System.Math.Round(ratio / snap) * snap;

						scale = snapped / 100.0;
					}

					var vXOut = scale * (vX - mDragOrigin);
					var vYOut = scale * (vY - mDragOrigin);

					mVisual.Model.Affine.X.X = vXOut.X;
					mVisual.Model.Affine.X.Y = vXOut.Y;

					mVisual.Model.Affine.Y.X = vYOut.X;
					mVisual.Model.Affine.Y.Y = vYOut.Y;

					mOperation = new TransformScaleOperation(Transform, scale);

					break;
			}
		}

		protected override bool OnAttachedControlKeyPress(Keys key, Keys modifiers)
		{
			if (mVisual.IsSelected)
			{
				if (key == Keys.Left)
				{
					Transform.Origin.X -= mSettings.MoveAmount;
					InvalidateControl();
					return true;
				}

				if (key == Keys.Right)
				{
					Transform.Origin.X += mSettings.MoveAmount;
					InvalidateControl();
					InvalidateControl();
					return true;
				}

				if (key == Keys.Up)
				{
					Transform.Origin.Y += mSettings.MoveAmount;
					InvalidateControl();
					InvalidateControl();
					return true;
				}

				if (key == Keys.Down)
				{
					Transform.Origin.Y -= mSettings.MoveAmount;
					InvalidateControl();
					return true;
				}

				if (key == Keys.Add)
				{
					Transform.Affine.X.X *= mSettings.ScaleSnap / 100.0;
					Transform.Affine.X.Y *= mSettings.ScaleSnap / 100.0;
					Transform.Affine.Y.X *= mSettings.ScaleSnap / 100.0;
					Transform.Affine.Y.Y *= mSettings.ScaleSnap / 100.0;
					InvalidateControl();
					return true;
				}

				if (key == Keys.Subtract)
				{
					Transform.Affine.X.X /= mSettings.ScaleSnap / 100.0;
					Transform.Affine.X.Y /= mSettings.ScaleSnap / 100.0;
					Transform.Affine.Y.X /= mSettings.ScaleSnap / 100.0;
					Transform.Affine.Y.Y /= mSettings.ScaleSnap / 100.0;
					InvalidateControl();
					return true;
				}

				if (key == Keys.Multiply || key == Keys.Divide)
				{
					var normalX = Transform.Affine.X.Direction;
					var normalY = Transform.Affine.Y.Direction;

					var angleBetweenOxAndOy = System.Math.Atan2(normalY.Y, normalY.X) - System.Math.Atan2(normalX.Y, normalX.X);
					var angle = mSettings.AngleSnap * (key == Keys.Multiply ? -1 : 1) * System.Math.PI / 360.0;

					var cos0 = System.Math.Cos(angleBetweenOxAndOy);
					var cos1 = System.Math.Cos(angle);

					var sin0 = System.Math.Sin(angleBetweenOxAndOy);
					var sin1 = System.Math.Sin(angle);

					var original = Transform.Affine.X.Copy();

					Transform.Affine.X.X = cos1 * original.X - sin1 * original.Y;
					Transform.Affine.X.Y = sin1 * original.X + cos1 * original.Y;

					var newNormal = mVisual.Model.Affine.X.Direction;
					var length = mVisual.Model.Affine.Y.Length;

					Transform.Affine.Y.X = length * (cos0 * newNormal.X - sin0 * newNormal.Y);
					Transform.Affine.Y.Y = length * (sin0 * newNormal.X + cos0 * newNormal.Y);

					InvalidateControl();
					return true;
				}
			}
			return false;
		}

		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mIsMouseDown)
			{
				DragNode(cursor, button, mLastHitTestResult);
				return true;
			}

			var hitTest = HitTest(cursor);
			
			mLastHitTestResult = hitTest;
			mVisual.Reset();

			AttachedControl.Cursor = null;

			switch (hitTest)
			{
				case HitTestResult.O:
					mVisual.IsVertexOHit = true;
					AttachedControl.Cursor = mMoveCursor;
					break;
				case HitTestResult.X:
					mVisual.IsVertexXHit = true;
					AttachedControl.Cursor = mMoveCursor;
					break;
				case HitTestResult.Y:
					mVisual.IsVertexYHit = true;
					AttachedControl.Cursor = mMoveCursor;
					break;
				case HitTestResult.Ox:
					mVisual.IsEdgeOxHit = true;
					AttachedControl.Cursor = mRotateCursor;
					break;
				case HitTestResult.Oy:
					mVisual.IsEdgeOyHit = true;
					AttachedControl.Cursor = mRotateCursor;
					break;
				case HitTestResult.Xy:
					mVisual.IsEdgeXyHit = true;
					AttachedControl.Cursor = mScaleCursor;
					break;
				case HitTestResult.Surface:
					mVisual.IsSurfaceHit = true;
					AttachedControl.Cursor = mMoveCursor;
					break;
			}

			return hitTest != HitTestResult.None;
		}
		protected override bool OnAttachedControlMouseWheel(double delta, MouseButtons button)
		{
			return false;
		}

		protected override bool OnAttachedControlMouseDown(Vector2 cursor)
		{
			var hitTest = HitTest(cursor);

			if (hitTest != HitTestResult.None)
			{
				mIsMouseDown = true;

				mDragCursor = cursor;
				mDragOrigin = mVisual.Model.Origin.Copy();
				mDragX = mVisual.Model.Affine.X.Copy();
				mDragY = mVisual.Model.Affine.Y.Copy();

				switch (hitTest)
				{
					case HitTestResult.Surface:
					case HitTestResult.O:
						mOperation = new TransformMoveOperation(mVisual.Model, mDragOrigin, mDragOrigin);
						break;
					case HitTestResult.X:
						mOperation = new TransformMoveOperation(mVisual.Model, mDragX, mDragX);
						break;
					case HitTestResult.Y:
						mOperation = new TransformMoveOperation(mVisual.Model, mDragY, mDragY);
						break;
					case HitTestResult.Ox:
					case HitTestResult.Oy:
						mOperation = new TransformRotateOperation(mVisual.Model, 2.0 * System.Math.PI);
						break;
					case HitTestResult.Xy:
						mOperation = new TransformScaleOperation(mVisual.Model, 1.0);
						break;
					default:
						mOperation = null;
						break;
				}

				mVisual.IsActive = true;
				mVisual.IsSelected = true;

				return true;
			}

			return false;
		}
		protected override bool OnAttachedControlMouseUp()
		{
			var old = mIsMouseDown;
			
			mIsMouseDown = false;

			mDragCursor = null;
			mDragOrigin = null;
			mDragX = null;
			mDragY = null;

			mVisual.IsActive = false;
			mOperation = null;

			return old;
		}

		protected override bool OnAttachedControlMouseDoubleClick()
		{
			return false;
		}

		public bool IsDragging
		{
			get { return mIsMouseDown; }
		}
		public void InvalidateHitTest()
		{
			mVisual.Reset();
		}

		public Transform Transform
		{
			get { return mVisual == null ? null : mVisual.Model; }
		}
		public TransformInputOperation GetCurrentOperation()
		{
			return mOperation;
		}
	}
}