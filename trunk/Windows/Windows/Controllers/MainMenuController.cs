using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainMenuController : Controller<Main>
	{
		private MainController mParent;

		public MainMenuController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mParent = null;
		}

		protected override void AttachView()
		{
			View.ExitToolStripMenuItem.Click += OnExitClick;
		}
		protected override void DetachView()
		{
			View.ExitToolStripMenuItem.Click -= OnExitClick;
		}

		private void OnExitClick(object sender, EventArgs e)
		{
			View.Close();
		}
	}
}