using System.ComponentModel;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Messages : Form, IMessagesView
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
