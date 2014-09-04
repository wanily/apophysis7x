using System;
using System.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorSelectionController : Controller<Editor>
	{
		private EditorController mParent;

		public IteratorSelectionController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.IteratorSelectionComboBox.SelectedIndexChanged += OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged += OnIteratorSelectedFromCanvas;
		}
		protected override void DetachView()
		{
			View.IteratorSelectionComboBox.SelectedIndexChanged -= OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged -= OnIteratorSelectedFromCanvas;
		}

		private void OnIteratorSelectedFromCanvas(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy)
				return;

			SelectIterator(iterator);
		}
		private void OnIteratorSelectedFromComboBox(object sender, EventArgs e)
		{
			var iterator = View.IteratorSelectionComboBox.SelectedIndex < 0 ? null :
				mParent.Flame.Iterators[View.IteratorSelectionComboBox.SelectedIndex];

			if (mParent.Initializer.IsBusy)
				return;

			SelectIterator(iterator);
		}

		public void BuildIteratorComboBox()
		{
			View.IteratorSelectionComboBox.Items.Clear();
			View.IteratorSelectionComboBox.Items.AddRange(mParent
				.Flame
				.Iterators
				.Select(x => Extensions.GetDisplayName(x))
				.OfType<object>()
				.ToArray());
		}
		public void SelectIterator([CanBeNull] Iterator iterator)
		{
			View.IteratorCanvas.SelectedIterator = iterator;

			using (mParent.Initializer.Enter())
			{
				View.IteratorSelectionComboBox.SelectedIndex = iterator == null ? -1 : iterator.Index;
				View.IteratorNameTextBox.Text = iterator == null ? null : iterator.Name;
				View.IteratorWeightDragPanel.Value = iterator == null ? 0.5 : iterator.Weight;
				View.IteratorColorDragPanel.Value = iterator == null ? 0.0 : iterator.Color;
				View.IteratorColorSpeedDragPanel.Value = iterator == null ? 0.0 : iterator.ColorSpeed;
				View.IteratorOpacityDragPanel.Value = iterator == null ? 1.0 : iterator.Opacity;
				View.IteratorDirectColorDragPanel.Value = iterator == null ? 1.0 : iterator.DirectColor;
				View.IteratorColorScrollBar.Value = iterator == null ? 0 : (int)(iterator.Color * 1000);

				View.IteratorWeightDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorColorSpeedDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorColorDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorColorScrollBar.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorIsExclusiveCheckBox.Enabled = iterator == null || iterator.GroupIndex == 0;
			}

			mParent.UpdateCoordinates();
			mParent.UpdateToolbar();
		}
	}
}