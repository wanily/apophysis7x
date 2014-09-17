using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class ControlVisualChain<T> : ControlChain<T, ControlVisual<T>> where T: Control
	{
		public ControlVisualChain([NotNull] Control control) : base(control)
		{
		}

		protected sealed override void RegisterEvents(Control control)
		{
			control.Paint += OnControlPaint;
		}
		protected sealed override void UnregisterEvents(Control control)
		{
			control.Paint -= OnControlPaint;
		}

		private void OnControlPaint(object sender, PaintEventArgs e)
		{
			OnControlPaint(e.Graphics);
		}
		private void OnControlPaint(Graphics graphics)
		{
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			foreach (var item in GetChainItems())
			{
				item.Paint(graphics);
			}
		}
	}

	class ControlVisualChain : ControlVisualChain<Control>
	{
		public ControlVisualChain([NotNull] Control control) : base(control)
		{
		}
	}
}