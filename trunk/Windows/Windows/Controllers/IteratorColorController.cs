using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorColorController : Controller<Editor>
	{
		private EditorController mParent;

		public IteratorColorController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.IteratorColorDragPanel.ValueChanged += OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged += OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged += OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged += OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged += OnColorChanged;
		}
		protected override void DetachView()
		{
			View.IteratorColorDragPanel.ValueChanged -= OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged -= OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged -= OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged -= OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged -= OnColorChanged;
		}

		private void OnColorChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy || iterator == null)
				return;

			if (ReferenceEquals(sender, View.IteratorColorDragPanel))
			{
				iterator.Color = View.IteratorColorDragPanel.Value;
				using (mParent.Initializer.Enter())
				{
					View.IteratorColorScrollBar.Value = (int)(iterator.Color * 1000);
				}
			}
			else if (ReferenceEquals(sender, View.IteratorColorScrollBar))
			{
				iterator.Color = View.IteratorColorScrollBar.Value / 1000.0;
				using (mParent.Initializer.Enter())
				{
					View.IteratorColorDragPanel.Value = iterator.Color;
				}
			}
		}
		private void OnColorSpeedChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy || iterator == null)
				return;

			iterator.ColorSpeed = View.IteratorColorSpeedDragPanel.Value;
		}
		private void OnOpacityChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy || iterator == null)
				return;

			iterator.Opacity = View.IteratorOpacityDragPanel.Value;
		}
		private void OnDirectColorChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy || iterator == null)
				return;

			iterator.DirectColor = View.IteratorDirectColorDragPanel.Value;
		}
	}
}