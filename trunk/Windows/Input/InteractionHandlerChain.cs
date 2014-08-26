using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class InteractionHandlerChain : ControlChain<InteractionHandler>
	{
		public InteractionHandlerChain([NotNull] Control control) : base(control)
		{
		}

		private void OnAttachedControlMouseMove([NotNull] Vector2 cursor, MouseButtons button)
		{
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
			control.MouseDown += OnCanvasMouseDown;
			control.MouseUp += OnCanvasMouseUp;
			control.MouseMove += OnCanvasMouseMove;
			control.MouseWheel += OnCanvasMouseWheel;
			control.MouseDoubleClick += OnCanvasMouseDoubleClick;
		}
		protected override void UnregisterEvents(Control control)
		{
			control.MouseDown -= OnCanvasMouseDown;
			control.MouseUp -= OnCanvasMouseUp;
			control.MouseMove -= OnCanvasMouseMove;
			control.MouseWheel -= OnCanvasMouseWheel;
			control.MouseDoubleClick -= OnCanvasMouseDoubleClick;
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
	}
}