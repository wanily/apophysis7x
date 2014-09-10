using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorPropertiesController : Controller<Editor>
	{
		private EditorController mParent;

		public IteratorPropertiesController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.IteratorWeightDragPanel.ValueChanged += OnWeightChanged;
			View.IteratorNameTextBox.TextChanged += OnNameChanged;

			View.PreviewDensityTrackBar.ValueChanged += OnDensityChanged;
			View.PreviewRangeTrackBar.ValueChanged += OnRangeChanged;

			using (mParent.Initializer.Enter())
			{
				View.PreviewDensityTrackBar.Value = (int)View.IteratorCanvas.PreviewDensity;
				View.PreviewRangeTrackBar.Value = (int) View.IteratorCanvas.PreviewRange;
			}
		}
		protected override void DetachView()
		{
			View.IteratorWeightDragPanel.ValueChanged -= OnWeightChanged;
			View.IteratorNameTextBox.TextChanged -= OnNameChanged;

			View.PreviewDensityTrackBar.ValueChanged -= OnDensityChanged;
			View.PreviewRangeTrackBar.ValueChanged -= OnRangeChanged;
		}

		private void OnNameChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy || iterator == null)
				return;

			iterator.Name = View.IteratorNameTextBox.Text;

			using (mParent.Initializer.Enter())
			{
				View.IteratorSelectionComboBox.Items.RemoveAt(iterator.Index);
				View.IteratorSelectionComboBox.Items.Insert(iterator.Index, iterator.GetDisplayName());
				View.IteratorSelectionComboBox.SelectedIndex = iterator.Index;
			}
		}
		private void OnWeightChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mParent.Initializer.IsBusy || iterator == null)
				return;

			iterator.Weight = View.IteratorWeightDragPanel.Value;
		}
		private void OnDensityChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;
			View.IteratorCanvas.PreviewDensity = View.PreviewDensityTrackBar.Value;
		}
		private void OnRangeChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;
			View.IteratorCanvas.PreviewRange = View.PreviewRangeTrackBar.Value;
		}
	}
}