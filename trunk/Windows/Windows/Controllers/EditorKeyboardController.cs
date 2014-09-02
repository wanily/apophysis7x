using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class EditorKeyboardController : Controller<Editor>
	{
		private EditorController mParent;

		public EditorKeyboardController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.KeyHandler = OnKeyDown;
		}
		protected override void DetachView()
		{
			View.KeyHandler = null;
		}

		private void OnKeyDown(Keys keys)
		{
			if ((keys & Keys.Control) == Keys.Control && (keys & Keys.V) == Keys.V)
			{
				mParent.ReadFlameFromClipboard();
			}
		}
	}
}