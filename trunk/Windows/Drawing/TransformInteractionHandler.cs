using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

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
		private HitTestResult mLastHitTestResult;
		private bool mIsMouseDown;

		public TransformInteractionHandler([NotNull] Control control, [NotNull] TransformVisual visual) : base(control)
		{
			mVisual = visual;
			if (visual == null) throw new ArgumentNullException("visual");
		}
		protected override void DisposeOverride(bool disposing)
		{
			mVisual = null;
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
			//todo	
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

			switch (hitTest)
			{
				case HitTestResult.O:
					mVisual.IsVertexOHit = true;
					break;
				case HitTestResult.X:
					mVisual.IsVertexXHit = true;
					break;
				case HitTestResult.Y:
					mVisual.IsVertexYHit = true;
					break;
				case HitTestResult.Ox:
					mVisual.IsEdgeOxHit = true;
					break;
				case HitTestResult.Oy:
					mVisual.IsEdgeOyHit = true;
					break;
				case HitTestResult.Xy:
					mVisual.IsEdgeXyHit = true;
					break;
				case HitTestResult.Surface:
					mVisual.IsSurfaceHit = true;
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
				return true;
			}

			return false;
		}
		protected override bool OnAttachedControlMouseUp()
		{
			var old = mIsMouseDown;
			mIsMouseDown = false;
			return old;
		}

		protected override bool OnAttachedControlMouseDoubleClick()
		{
			return false;
		}
	}
}