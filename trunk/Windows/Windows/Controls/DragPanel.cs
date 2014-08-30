using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class DragPanel : Label
	{
		public DragPanel()
		{
			InitializeComponent();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
		}
	}
}
