using System;
using System.IO;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Properties;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class TransformInteractionHandler : InteractionHandler
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

		private TransformVisual mVisual;
		private Canvas mCanvas;

		private Vector2 mDragCursor, mDragOrigin;
		private Vector2 mDragX, mDragY;

		private HitTestResult mLastHitTestResult;
		private bool mIsMouseDown;

		private static readonly Cursor mMoveCursor;
		private static readonly Cursor mRotateCursor;
		private static readonly Cursor mScaleCursor;

		static TransformInteractionHandler()
		{
			mMoveCursor = new Cursor(new MemoryStream(Resources.Move));
			mRotateCursor = new Cursor(new MemoryStream(Resources.Rotate));
			mScaleCursor = new Cursor(new MemoryStream(Resources.Scale));
		}
		public TransformInteractionHandler([NotNull] Control control, [NotNull] TransformVisual visual, [NotNull] Canvas canvas) : base(control)
		{
			if (visual == null) throw new ArgumentNullException("visual");
			if (canvas == null) throw new ArgumentNullException("canvas");

			mVisual = visual;
			mCanvas = canvas;
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

			mDragCursor = null;
			mDragOrigin = null;
			mDragX = null;
			mDragY = null;

			mIsMouseDown = false;
			mLastHitTestResult = HitTestResult.None;
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

			switch (hitTest)
			{
				case HitTestResult.Surface:
				case HitTestResult.O:

					var o = mCanvas.CanvasToWorld(cursor) - mCanvas.CanvasToWorld(mDragCursor) + mDragOrigin;

					mVisual.Model.Origin.X = o.X;
					mVisual.Model.Origin.Y = o.Y;

					break;

				case HitTestResult.X:

					var x = mCanvas.CanvasToWorld(cursor) - mCanvas.CanvasToWorld(mDragCursor) + (mDragOrigin + mDragX);

					mVisual.Model.Affine.X.X = x.X - mDragOrigin.X;
					mVisual.Model.Affine.X.Y = x.Y - mDragOrigin.Y;

					break;

				case HitTestResult.Y:
					
					var y = mCanvas.CanvasToWorld(cursor) - mCanvas.CanvasToWorld(mDragCursor) + (mDragOrigin + mDragY);

					mVisual.Model.Affine.Y.X = y.X - mDragOrigin.X;
					mVisual.Model.Affine.Y.Y = y.Y - mDragOrigin.Y;

					break;

				case HitTestResult.Ox:
				case HitTestResult.Oy:

					var primary = hitTest == HitTestResult.Ox ? mDragX : mDragY;
					var secondary = hitTest == HitTestResult.Ox ? mDragY : mDragX;

					var normalX = primary.Direction;
					var normalY = secondary.Direction;

					var deltaOrigin = mCanvas.CanvasToWorld(cursor) - mDragOrigin;
					var angleBetweenOxAndOy = System.Math.Atan2(normalY.Y, normalY.X) - System.Math.Atan2(normalX.Y, normalX.X);
					var angleBetweenOxAndDelta = System.Math.Atan2(deltaOrigin.Y, deltaOrigin.X) - System.Math.Atan2(normalX.Y, normalX.X);

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

					break;

				case HitTestResult.Xy:

					break;
			}
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

				mVisual.IsActive = true;

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
	}
}