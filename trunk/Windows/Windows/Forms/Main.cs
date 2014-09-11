using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void OnBatchListResized(object sender, System.EventArgs e)
		{
			BatchListView.Columns[0].Width = BatchListView.ClientSize.Width - 3;
		}
		private void OnWindowLoaded(object sender, System.EventArgs e)
		{
			OnBatchListResized(sender, e);
		}
	}
}
