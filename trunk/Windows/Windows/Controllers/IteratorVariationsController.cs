using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorVariationsController : Controller<Editor>
	{
		private EditorController mParent;

		public IteratorVariationsController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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

		public void UpdateList()
		{
			//View.VariationGrid.Rows.Clear();

			if (View.IteratorCanvas.SelectedIterator == null)
				return;

			/*foreach (var variation in View.IteratorCanvas.SelectedIterator.Variations)
			{
				View.VariationGrid.Rows.Add(variation, variation.Weight.ToString(InputController.PreciseFormat, InputController.Culture));
			}*/
		}
	}
}