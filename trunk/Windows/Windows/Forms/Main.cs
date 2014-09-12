using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		private readonly InputController mInputController;

		public Main()
		{
			InitializeComponent();
			mInputController = new InputController();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void OnBatchListResized(object sender, System.EventArgs e)
		{
			UpdateBatchListColumnSize();
		}
		private void OnWindowLoaded(object sender, System.EventArgs e)
		{
			UpdateBatchListColumnSize();
		}

		internal void UpdateBatchListColumnSize()
		{
			BatchListView.Columns[0].Width = BatchListView.ClientSize.Width - 3;
		}

		private void OnDensityKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputController.HandleKeyPressForIntegerTextBox(e);
		}
	}
}
