using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class BatchListController : Controller<Main>
	{
		private MainController mParent;

		public BatchListController([NotNull] Main view, [NotNull] MainController parent) : base(view)
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
		}
		protected override void DetachView()
		{
		}
	}
}