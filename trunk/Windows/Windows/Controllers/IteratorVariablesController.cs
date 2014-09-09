using System;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorVariablesController : Controller<Editor>
	{
		private EditorController mParent;

		public IteratorVariablesController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.HideUnrelatedVariablesCheckBox.Click += OnHideUnrelatedVariablesCheckBoxClick;
			View.VariablesGrid.CellValueChanged += OnCellValueChanged;
		}
		protected override void DetachView()
		{
			View.HideUnrelatedVariablesCheckBox.Click -= OnHideUnrelatedVariablesCheckBoxClick;
			View.VariablesGrid.CellValueChanged -= OnCellValueChanged;
		}

		private void OnCellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			var variable = View.VariablesGrid.Rows[e.Row].Cells[0].Value as string;
			if (string.IsNullOrEmpty(variable) || View.IteratorCanvas.SelectedIterator == null)
				return;

			e.Value = View.IteratorCanvas.SelectedIterator.Variations.SetVariable(variable, e.Value);
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
				if (View.HideUnrelatedVariablesCheckBox.Checked && System.Math.Abs(variation.Weight) < double.Epsilon)
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