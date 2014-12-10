using System.ComponentModel;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Settings : Form, ISettingsView
	{
		private readonly InputController mInputHandler;

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
