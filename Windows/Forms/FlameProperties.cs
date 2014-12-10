using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class FlameProperties : Form, IFlamePropertiesView
	{
		private readonly InputController mInputHandler;

		public FlameProperties()
		{
			InitializeComponent();
			mInputHandler = new InputController();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
					components = null;
				}
			}
			base.Dispose(disposing);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
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
		private void OnSelectPaletteEditHandlerClick(object sender, System.EventArgs e)
		{
			mHandlerSelectMenu.Show(PaletteEditModeButton, new Point(0, PaletteEditModeButton.Height));
		}
	}
}
