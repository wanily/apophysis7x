using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		private readonly InputController mInputHandler;

		public Editor()
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

		internal Action<Keys> KeyHandler { get; set; }
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (KeyHandler != null)
				KeyHandler(keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}


		private void OnNumericTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}
	}
}
