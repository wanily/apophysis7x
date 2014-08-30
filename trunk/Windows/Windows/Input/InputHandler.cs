using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public abstract class InputHandler : ChainItem
	{
		protected InputHandler([NotNull] Control control) : base(control)
		{
		}

		protected abstract bool OnAttachedControlKeyPress(Keys key, Keys modifiers);

		protected abstract bool OnAttachedControlMouseMove([NotNull] Vector2 cursor, MouseButtons button);
		protected abstract bool OnAttachedControlMouseWheel(double delta, MouseButtons button);

		protected abstract bool OnAttachedControlMouseDown([NotNull] Vector2 cursor);
		protected abstract bool OnAttachedControlMouseUp();

		protected abstract bool OnAttachedControlMouseDoubleClick();

		protected sealed override void RegisterEvents(Control control)
		{
		}
		protected sealed override void UnregisterEvents(Control control)
		{
		}

		public bool HandleKeyPress(Keys key, Keys modifiers)
		{
			return OnAttachedControlKeyPress(key, modifiers);
		}

		public bool HandleMouseDown([NotNull] Vector2 cursor)
		{
			if (cursor == null) throw new ArgumentNullException("cursor");
			return OnAttachedControlMouseDown(cursor);
		}
		public bool HandleMouseUp()
		{
			return OnAttachedControlMouseUp();
		}

		public bool HandleMouseMove([NotNull] Vector2 cursor, MouseButtons button)
		{
			if (cursor == null) throw new ArgumentNullException("cursor");
			return OnAttachedControlMouseMove(cursor, button);
		}
		public bool HandleMouseWheel(double delta, MouseButtons button)
		{
			return OnAttachedControlMouseWheel(delta, button);
		}

		public bool HandleMouseDoubleClick()
		{
			return OnAttachedControlMouseDoubleClick();
		}
	}
}