using System;
using System.Numerics;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	abstract class CanvasInputStrategy<T> : InputHandler where T : Canvas
	{
		private T mCanvas;
		private Vector2? mNavigationOrigin;
		private Vector2? mNavigationOffset;
		private bool mIsNavigating;

		protected CanvasInputStrategy([NotNull] Control control, [NotNull] T canvas) : base(control)
		{
			if (canvas == null) throw new ArgumentNullException("canvas");
			mCanvas = canvas;
		}
		protected override void DisposeOverride(bool disposing)
		{
			EndNavigate();
			mCanvas = null;
		}

		protected abstract Vector2 GetCurrentOffset();
		protected Vector2 GetNextOffset(Vector2 cursor)
		{
			return (mNavigationOffset.GetValueOrDefault() - cursor + mNavigationOrigin.GetValueOrDefault())/Canvas.Ratio;
		}

		public T Canvas
		{
			get { return mCanvas; }
		}
		public bool IsNavigating
		{
			get { return mIsNavigating; }
		}

		public void BeginNavigate(Vector2 cursor)
		{
			var offset = GetCurrentOffset() * Canvas.Ratio;

			mNavigationOrigin = cursor;
			mNavigationOffset = offset;
			mIsNavigating = true;
		}
		public void EndNavigate()
		{
			mNavigationOrigin = null;
			mNavigationOffset = null;
			mIsNavigating = false;
		}

		public abstract void NavigateOffset(Vector2 cursor);
		public abstract void NavigateZoom(float delta);
		public abstract void NavigateReset();

		protected override bool OnAttachedControlKeyPress(Keys key, Keys modifiers)
		{
			if (key == Keys.Left)
			{
				BeginNavigate(new Vector2(0, 0));
				NavigateOffset(new Vector2(-Canvas.Scale, 0) * Canvas.Ratio);
				EndNavigate();
				InvalidateControl();
				return true;
			}

			if (key == Keys.Right)
			{
				BeginNavigate(new Vector2(0, 0));
				NavigateOffset(new Vector2(Canvas.Scale, 0) * Canvas.Ratio);
				EndNavigate();
				InvalidateControl();
				return true;
			}

			if (key == Keys.Up)
			{
				BeginNavigate(new Vector2(0, 0));
				NavigateOffset(new Vector2(0, -Canvas.Scale) * Canvas.Ratio);
				EndNavigate();
				InvalidateControl();
				return true;
			}

			if (key == Keys.Down)
			{
				BeginNavigate(new Vector2(0, 0));
				NavigateOffset(new Vector2(0, Canvas.Scale) * Canvas.Ratio);
				EndNavigate();
				InvalidateControl();
				return true;
			}

			if (key == Keys.Multiply)
			{
				NavigateZoom(120);
				InvalidateControl();
				return true;
			}

			if (key == Keys.Divide)
			{
				NavigateZoom(-120);
				InvalidateControl();
				return true;
			}

			return false;
		}

		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (button == MouseButtons.Left)
			{
				NavigateOffset(cursor);
				return true;
			}

			return false;
		}
		protected override bool OnAttachedControlMouseWheel(float delta, MouseButtons button)
		{
			if (button == MouseButtons.None)
			{
				NavigateZoom(delta);
				return true;
			}

			return false;
		}

		protected sealed override bool OnAttachedControlMouseDown(Vector2 cursor)
		{
			BeginNavigate(cursor);
			return true;
		}
		protected sealed override bool OnAttachedControlMouseUp()
		{
			EndNavigate();
			return true;
		}

		protected sealed override bool OnAttachedControlMouseDoubleClick()
		{
			NavigateReset();
			return true;
		}
	}
}
