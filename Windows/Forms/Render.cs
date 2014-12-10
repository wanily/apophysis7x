using System.ComponentModel;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Render : Form, IRenderView
	{
		public Render()
		{
			InitializeComponent();

			// hack http://stackoverflow.com/questions/2646606/c-sharp-winforms-statusstrip-how-do-i-reclaim-the-space-from-the-grip
			mStatusBar.Padding = new Padding(mStatusBar.Padding.Left, mStatusBar.Padding.Top, mStatusBar.Padding.Left, mStatusBar.Padding.Bottom);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			e.Cancel = true;
			Hide();
		}
	}
}
