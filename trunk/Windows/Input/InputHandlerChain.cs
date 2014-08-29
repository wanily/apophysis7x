using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class InputHandlerChain : ControlChain<InputHandler>
	{
		private Vector2 mCursor;

		public InputHandlerChain([NotNull] Control control) : base(control)
		{
			mCursor = new Vector2();
		}

		private void OnAttachedControlKeyPress(Keys keyCode, Keys modifiers)
		{
			foreach (var item in GetChainItems())
			{
				if (item.HandleKeyPress(keyCode, modifiers))
					break;
			}
		}

		private void OnAttachedControlMouseMove([NotNull] Vector2 cursor, MouseButtons button)
		{
			mCursor = cursor;

			foreach (var item in GetChainItems())
			{
				if (item.HandleMouseMove(cursor, button))
					break;
			}
		}
		private void OnAttachedControlMouseWheel(double delta, MouseButtons button)
		{
			foreach (var item in GetChainItems())
			{
				if (item.HandleMouseWheel(delta, button))
					break;
			}
		}

		private void OnAttachedControlMouseDown([NotNull] Vector2 cursor)
		{
			foreach (var item in GetChainItems())
			{
				if (item.HandleMouseDown(cursor))
					break;
			}
		}
		private void OnAttachedControlMouseUp()
		{
			foreach (var item in GetChainItems())
			{
				if (item.HandleMouseUp())
					break;
			}
		}

		private void OnAttachedControlMouseDoubleClick()
		{
			foreach (var item in GetChainItems())
			{
				if (item.HandleMouseDoubleClick())
					break;
			}
		}

		protected override void RegisterEvents(Control control)
		{
			control.KeyDown += OnCanvasKeyPress;
			control.MouseDown += OnCanvasMouseDown;
			control.MouseUp += OnCanvasMouseUp;
			control.MouseMove += OnCanvasMouseMove;
			control.MouseWheel += OnCanvasMouseWheel;
			control.MouseDoubleClick += OnCanvasMouseDoubleClick;
		}
		protected override void UnregisterEvents(Control control)
		{
			control.KeyDown -= OnCanvasKeyPress;
			control.MouseDown -= OnCanvasMouseDown;
			control.MouseUp -= OnCanvasMouseUp;
			control.MouseMove -= OnCanvasMouseMove;
			control.MouseWheel -= OnCanvasMouseWheel;
			control.MouseDoubleClick -= OnCanvasMouseDoubleClick;
		}

		private void OnCanvasKeyPress(object sender, KeyEventArgs e)
		{
			OnAttachedControlKeyPress(e.KeyCode, e.Modifiers);
		}
		private void OnCanvasMouseDown(object sender, MouseEventArgs e)
		{
			var cursor = new Vector2(e.X, e.Y);
			OnAttachedControlMouseDown(cursor);
		}
		private void OnCanvasMouseUp(object sender, MouseEventArgs e)
		{
			OnAttachedControlMouseUp();
		}
		private void OnCanvasMouseMove(object sender, MouseEventArgs e)
		{
			OnAttachedControlMouseMove(new Vector2(e.X, e.Y), e.Button);
			InvalidateControl();
		}
		private void OnCanvasMouseWheel(object sender, MouseEventArgs e)
		{
			OnAttachedControlMouseWheel(e.Delta, e.Button);
			InvalidateControl();
		}
		private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
		{
			OnAttachedControlMouseDoubleClick();
			InvalidateControl();
		}

		public void TriggerKeyPress(Keys key, Keys modifiers = Keys.None)
		{
			OnAttachedControlKeyPress(key, modifiers);
		}

		public Vector2 CursorPosition
		{
			get { return mCursor; }
		}
	}
}