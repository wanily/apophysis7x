using System.ComponentModel;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Render : Form
	{
		private InputController mInputHandler;

		public Render()
		{
			InitializeComponent();

			mInputHandler = new InputController();

			// hack http://stackoverflow.com/questions/2646606/c-sharp-winforms-statusstrip-how-do-i-reclaim-the-space-from-the-grip
			mStatusBar.Padding = new Padding(mStatusBar.Padding.Left, mStatusBar.Padding.Top, mStatusBar.Padding.Left, mStatusBar.Padding.Bottom);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			e.Cancel = true;
			Hide();
		}

		private void OnNumericTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}
	}
}
