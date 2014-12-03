using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Input
{
	class IteratorInputHandler : InputHandler
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
		private IteratorMatrix mActiveMatrix;
		private IteratorVisual mVisual;
		private Canvas mCanvas;

		private Vector2? mDragCursor, mDragOrigin;
		private Vector2? mDragX, mDragY;

		private HitTestResult mLastHitTestResult;
		private bool mIsMouseDown;

		private IteratorInputOperation mOperation;

		private static readonly Cursor mMoveCursor;
		private static readonly Cursor mRotateCursor;
		private static readonly Cursor mScaleCursor;

		static IteratorInputHandler()
		{
			mMoveCursor = new Cursor(new MemoryStream(Resources.Cursors.Move));
			mRotateCursor = new Cursor(new MemoryStream(Resources.Cursors.Rotate));
			mScaleCursor = new Cursor(new MemoryStream(Resources.Cursors.Scale));
		}
		public IteratorInputHandler([NotNull] Control control, [NotNull] IteratorVisual visual, [NotNull] Canvas canvas, [NotNull] EditorSettings settings, IteratorMatrix activeMatrix) : base(control)
		{
			if (visual == null) throw new ArgumentNullException("visual");
			if (canvas == null) throw new ArgumentNullException("canvas");
			if (settings == null) throw new ArgumentNullException("settings");

			mVisual = visual;
			mCanvas = canvas;
			mSettings = settings;
			mActiveMatrix = activeMatrix;
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
			mActiveMatrix = default(IteratorMatrix);
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

			const float edgeProximityThreshold = 4;
			const float vertexProximityThreshold = 4;

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

			var @do = mDragOrigin.GetValueOrDefault();
			var @dx = mDragX.GetValueOrDefault();
			var @dy = mDragY.GetValueOrDefault();

			var c = mCanvas.CanvasToWorld(cursor);
			var c0 = mCanvas.CanvasToWorld(mDragCursor.GetValueOrDefault());

			switch (hitTest)
			{
				case HitTestResult.Surface:
				case HitTestResult.O:

					var o = c - c0 + @do;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = Vector2.Abs(c - c0);
						if (d.X > d.Y) o.Y = @do.Y;
						else o.X = @do.X;
					}

					mOperation = new MoveOperation(Model, @do, Origin = o);
					break;

				case HitTestResult.X:

					var x = c - c0 + (@do + @dx);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = Vector2.Abs(c - c0);
						x = d.X > d.Y ? new Vector2(x.X, @dx.Y) : new Vector2(@dx.X, x.Y);
					}

					X = x - @do;
					mOperation = new MoveOperation(Model, @dx, x);

					break;

				case HitTestResult.Y:

					var y = c - c0 + (@do + @dy);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var d = Vector2.Abs(c - c0);
						y = d.X > d.Y ? new Vector2(y.X, @dy.Y) : new Vector2(@dy.X, y.Y);
					}

					Y = y - @do;
					mOperation = new MoveOperation(Model, @dy, y);

					break;

				case HitTestResult.Ox:
				case HitTestResult.Oy:

					var primary = hitTest == HitTestResult.Ox ? @dx : @dy;
					var secondary = hitTest == HitTestResult.Ox ? @dy : @dx;

					var normalX = primary.Normal();
					var normalY = secondary.Normal();

					var deltaOrigin = c - @do;
					var angleBetweenOxAndOy = Float.Atan2(normalY.Y, normalY.X) - Float.Atan2(normalX.Y, normalX.X);
					var angleBetweenOxAndDelta = Float.Atan2(deltaOrigin.Y, deltaOrigin.X) - Float.Atan2(normalX.Y, normalX.X);

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var ang = angleBetweenOxAndDelta * 180 / Float.Pi;
						var snapped = Float.Round(ang / mSettings.AngleSnap) * mSettings.AngleSnap;

						angleBetweenOxAndDelta = snapped * Float.Pi / 180.0f;
					}

					var cos0 = Float.Cos(angleBetweenOxAndOy);
					var cos1 = Float.Cos(angleBetweenOxAndDelta);

					var sin0 = Float.Sin(angleBetweenOxAndOy);
					var sin1 = Float.Sin(angleBetweenOxAndDelta);

					if (hitTest == HitTestResult.Ox)
					{
						X = new Vector2(
							cos1 * primary.X - sin1 * primary.Y,
							sin1 * primary.X + cos1 * primary.Y);

						if (mSettings.LockAxes)
						{
							var newNormal = X.Normal();
							var length = Y.Length();

							Y = new Vector2(
								length * (cos0 * newNormal.X - sin0 * newNormal.Y),
								length * (sin0 * newNormal.X + cos0 * newNormal.Y));
						}
					}
					else if (hitTest == HitTestResult.Oy)
					{
						Y = new Vector2(
							cos1 * primary.X - sin1 * primary.Y,
							sin1 * primary.X + cos1 * primary.Y);

						if (mSettings.LockAxes)
						{
							var newNormal = Y.Normal();
							var length = X.Length();

							X = new Vector2(
								length * (cos0 * newNormal.X - sin0 * newNormal.Y),
								length * (sin0 * newNormal.X + cos0 * newNormal.Y));
						}
					}

					mOperation = new RotateOperation(Model, angleBetweenOxAndDelta, HitTestResult.Ox == hitTest ? RotationAxis.X : RotationAxis.Y);

					break;

				case HitTestResult.Xy:

					var c0dO = c0 - @do;

					var vX = (@do + @dx);
					var vY = (@do + @dy);

					var denom = c0dO.Length();
					if (Float.Abs(denom) < float.Epsilon)
						break;

					var scale = (c0dO.X * (c.X - @do.X) + c0dO.Y * (c.Y - @do.Y)) / (denom * denom);
					if (Float.Abs(scale) < float.Epsilon)
						scale = float.Epsilon;

					if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
					{
						var snap = Float.Abs(scale) < 1 ? (100 * 100 / mSettings.ScaleSnap) : mSettings.ScaleSnap;

						var ratio = scale * 100;
						var snapped = Float.Round(ratio / snap) * snap;

						scale = snapped / 100.0f;
					}

					X = scale * (vX - @do);
					Y = scale * (vY - @do);

					mOperation = new ScaleOperation(Model, scale);

					break;
			}
		}

		protected override bool OnAttachedControlKeyPress(Keys key, Keys modifiers)
		{
			if (mVisual.IsSelected)
			{
				if (key == Keys.Left)
				{
					Origin = new Vector2(Origin.X - mSettings.MoveAmount, Origin.Y);
					InvalidateControl();
					return true;
				}

				if (key == Keys.Right)
				{
					Origin = new Vector2(Origin.X + mSettings.MoveAmount, Origin.Y);
					InvalidateControl();
					InvalidateControl();
					return true;
				}

				if (key == Keys.Up)
				{
					Origin = new Vector2(Origin.X, Origin.Y + mSettings.MoveAmount);
					InvalidateControl();
					InvalidateControl();
					return true;
				}

				if (key == Keys.Down)
				{
					Origin = new Vector2(Origin.X, Origin.Y - mSettings.MoveAmount);
					InvalidateControl();
					return true;
				}

				if (key == Keys.Add)
				{
					X = X * (mSettings.ScaleSnap / 100);
					Y = Y * (mSettings.ScaleSnap / 100);
					InvalidateControl();
					return true;
				}

				if (key == Keys.Subtract)
				{
					X = X / (mSettings.ScaleSnap / 100);
					Y = Y / (mSettings.ScaleSnap / 100);
					InvalidateControl();
					return true;
				}

				if (key == Keys.Multiply || key == Keys.Divide)
				{
					var normalX = X.Normal();
					var normalY = Y.Normal();

					var angleBetweenOxAndOy = Float.Atan2(normalY.Y, normalY.X) - Float.Atan2(normalX.Y, normalX.X);
					var angle = mSettings.AngleSnap * (key == Keys.Multiply ? -1 : 1) * Float.Pi / 180.0f;

					var cos0 = Float.Cos(angleBetweenOxAndOy);
					var cos1 = Float.Cos(angle);

					var sin0 = Float.Sin(angleBetweenOxAndOy);
					var sin1 = Float.Sin(angle);

					var original = X;

					X = new Vector2(
						cos1 * original.X - sin1 * original.Y,
						sin1 * original.X + cos1 * original.Y);

					var newNormal = X.Normal();
					var length = Y.Length();

					Y = new Vector2(
						length * (cos0 * newNormal.X - sin0 * newNormal.Y),
						length * (sin0 * newNormal.X + cos0 * newNormal.Y));

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
		protected override bool OnAttachedControlMouseWheel(float delta, MouseButtons button)
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
				mDragOrigin = Origin;
				mDragX = X;
				mDragY = Y;

				switch (hitTest)
				{
					case HitTestResult.Surface:
					case HitTestResult.O:
						mOperation = new MoveOperation(mVisual.Model, mDragOrigin.Value, mDragOrigin.Value);
						break;
					case HitTestResult.X:
						mOperation = new MoveOperation(mVisual.Model, mDragX.Value, mDragX.Value);
						break;
					case HitTestResult.Y:
						mOperation = new MoveOperation(mVisual.Model, mDragY.Value, mDragY.Value);
						break;
					case HitTestResult.Ox:
					case HitTestResult.Oy:
						mOperation = new RotateOperation(mVisual.Model, 2.0f * Float.Pi, hitTest == HitTestResult.Ox ? RotationAxis.X : RotationAxis.Y);
						break;
					case HitTestResult.Xy:
						mOperation = new ScaleOperation(mVisual.Model, 1.0f);
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

		public Vector2 Origin
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PreAffine.M31, Model.PreAffine.M32);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PostAffine.M31, Model.PostAffine.M32);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						Model.PreAffine = Model.PreAffine.Alter(m31: value.X, m32: value.Y);
						break;
					case IteratorMatrix.PostAffine:
						Model.PostAffine = Model.PostAffine.Alter(m31: value.X, m32: value.Y);
						break;
				}
			}
		}
		public Vector2 X
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PreAffine.M11, Model.PreAffine.M12);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PostAffine.M11, Model.PostAffine.M12);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						Model.PreAffine = Model.PreAffine.Alter(value.X, value.Y);
						break;
					case IteratorMatrix.PostAffine:
						Model.PostAffine = Model.PostAffine.Alter(value.X, value.Y);
						break;
				}
			}
		}
		public Vector2 Y
		{
			get
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						return new Vector2(Model.PreAffine.M21, Model.PreAffine.M22);
					case IteratorMatrix.PostAffine:
						return new Vector2(Model.PostAffine.M21, Model.PostAffine.M22);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				switch (mActiveMatrix)
				{
					case IteratorMatrix.PreAffine:
						Model.PreAffine = Model.PreAffine.Alter(m21: value.X, m22: value.Y);
						break;
					case IteratorMatrix.PostAffine:
						Model.PostAffine = Model.PostAffine.Alter(m21: value.X, m22: value.Y);
						break;
				}
			}
		}
		public Iterator Model
		{
			get { return mVisual == null ? null : mVisual.Model; }
		}

		public IteratorInputOperation GetCurrentOperation()
		{
			return mOperation;
		}
		public bool PerformHitTest(Vector2 cursor)
		{
			var hitTest = HitTest(cursor);
			return hitTest != HitTestResult.None;
		}
	}
}