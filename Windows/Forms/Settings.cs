using System.ComponentModel;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Settings : Form
	{
		private InputController mInputHandler;

		public Settings()
		{
			InitializeComponent();
			mInputHandler = new InputController();
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
