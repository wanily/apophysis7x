using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
