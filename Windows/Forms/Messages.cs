using System.ComponentModel;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Messages : Form
	{
		public Messages()
		{
			InitializeComponent();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			e.Cancel = true;
			Hide();
		}
	}
}
