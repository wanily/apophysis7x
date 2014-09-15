using System.Drawing;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class PreviewInputVisual : ControlVisual<PictureBox>
	{
		public PreviewInputVisual([NotNull] PictureBox control) : base(control)
		{
		}

		protected override void DisposeOverride(bool disposing)
		{
		}

		protected override void RegisterEvents(PictureBox control)
		{
		}
		protected override void UnregisterEvents(PictureBox control)
		{
		}

		protected override void OnControlPaint(Graphics graphics)
		{
		}
	}
}