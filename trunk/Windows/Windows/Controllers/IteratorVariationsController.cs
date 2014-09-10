using System;
using System.Drawing;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Windows.Controls;
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
			View.HideUnusedVariationsCheckBox.Click += OnHideUnusedVariationsCheckBoxClick;
			View.ClearVariationsButton.Click += OnClearVariationsClick;
			View.VariationsGrid.CellValueChanged += OnCellValueChanged;
			View.VariationsGrid.CellEndEdit += OnCellEndEdit;
		}
		protected override void DetachView()
		{
			View.HideUnusedVariationsCheckBox.Click -= OnHideUnusedVariationsCheckBoxClick;
			View.ClearVariationsButton.Click -= OnClearVariationsClick;
			View.VariationsGrid.CellValueChanged -= OnCellValueChanged;
			View.VariationsGrid.CellEndEdit -= OnCellEndEdit;
		}

		private void OnCellEndEdit(object sender, EventArgs e)
		{
			mParent.UpdateVariables();
		}
		private void OnCellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			var variation = View.VariationsGrid.Rows[e.Row].Cells[0].Value as Variation;
			if (variation == null || View.IteratorCanvas.SelectedIterator == null)
				return;

			e.Value = View.IteratorCanvas.SelectedIterator.Variations.SetWeight(variation.Name, e.Value);

			if (View.IteratorCanvas.Settings.ShowVariationPreview)
			{
				View.IteratorCanvas.Refresh();
			}
		}
		private void OnHideUnusedVariationsCheckBoxClick(object sender, EventArgs e)
		{
			UpdateList();
		}
		private void OnClearVariationsClick(object sender, EventArgs e)
		{
			foreach (var variation in View.IteratorCanvas.SelectedIterator.Variations)
			{
				variation.Weight = 0;
			}

			UpdateList();
		}

		public void UpdateList()
		{
			View.VariationsGrid.Rows.Clear();

			if (View.IteratorCanvas.SelectedIterator == null)
				return;

			foreach (var variation in View.IteratorCanvas.SelectedIterator.Variations)
			{
				if (View.HideUnusedVariationsCheckBox.Checked && System.Math.Abs(variation.Weight) < double.Epsilon)
				{
					continue;
				}

				var color = SystemColors.Window;

				if (variation is ExtendedVariation)
				{
					color = Color.AliceBlue;
				}
				else if (variation is ExternalVariation)
				{
					color = Color.FloralWhite;
				}

				View.VariationsGrid.Rows.Add(variation, variation.Weight.ToString(InputController.PreciseFormat, InputController.Culture));
				View.VariationsGrid.Rows[View.VariationsGrid.RowCount - 1].Cells[0].Style.BackColor = color;
				View.VariationsGrid.Rows[View.VariationsGrid.RowCount - 1].Cells[1].Style.BackColor = color;

			}
		}
	}
}