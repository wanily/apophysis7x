using System;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class IteratorVariablesController : Controller<Editor>, IIteratorVariablesController
	{
		public IteratorVariablesController([NotNull] Editor view) : base(view)
		{
		}
		protected override void DisposeOverride(bool disposing)
		{
		}

		protected override void AttachView()
		{
			View.HideUnrelatedVariablesCheckBox.Click += OnHideUnrelatedVariablesCheckBoxClick;
			View.VariablesGrid.CellValueChanged += OnCellValueChanged;
			View.VariablesGrid.CellValueReset += OnCellValueReset;
		}
		protected override void DetachView()
		{
			View.HideUnrelatedVariablesCheckBox.Click -= OnHideUnrelatedVariablesCheckBoxClick;
			View.VariablesGrid.CellValueChanged -= OnCellValueChanged;
			View.VariablesGrid.CellValueReset -= OnCellValueReset;
		}

		private void OnCellValueReset(object sender, CellValueResetEventArgs e)
		{
			var variable = View.VariablesGrid.Rows[e.Row].Cells[0].Value as string;
			if (string.IsNullOrEmpty(variable) || View.IteratorCanvas.SelectedIterator == null)
				return;

			e.Value = View.IteratorCanvas.SelectedIterator.Variations.ResetVariable(variable);
		}
		private void OnCellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			var variable = View.VariablesGrid.Rows[e.Row].Cells[0].Value as string;
			if (string.IsNullOrEmpty(variable) || View.IteratorCanvas.SelectedIterator == null)
				return;

			e.Value = View.IteratorCanvas.SelectedIterator.Variations.SetVariable(variable, e.Value);

			if (View.IteratorCanvas.Settings.ShowVariationPreview)
			{
				View.IteratorCanvas.Refresh();
			}
		}
		private void OnHideUnrelatedVariablesCheckBoxClick(object sender, EventArgs e)
		{
			UpdateList();
		}

		public void UpdateList()
		{
			View.VariablesGrid.Rows.Clear();

			if (View.IteratorCanvas.SelectedIterator == null)
				return;

			foreach (var variation in View.IteratorCanvas.SelectedIterator.Variations)
			{
				if (View.HideUnrelatedVariablesCheckBox.Checked && System.Math.Abs(variation.Weight) < float.Epsilon)
				{
					continue;
				}

				foreach (var variable in variation.EnumerateVariables())
				{
					var value = variation.GetVariable(variable);
					View.VariablesGrid.Rows.Add(variable, value.ToString(InputController.PreciseFormat, InputController.Culture));
				}
			}
		}
	}
}