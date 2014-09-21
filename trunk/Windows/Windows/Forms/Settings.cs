using System.ComponentModel;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Settings : Form
	{
		public Settings()
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
