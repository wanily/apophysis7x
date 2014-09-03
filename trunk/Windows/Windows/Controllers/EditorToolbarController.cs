using System;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class EditorToolbarController : Controller<Editor>
	{
		private EditorController mParent;

		public EditorToolbarController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.IteratorCanvas.Settings.SettingsChanged += OnSettingsChanged;

			View.ResetAllIteratorsButton.Click += OnResetAllIteratorsClick;

			View.AddIteratorButton.Click += OnAddIteratorClick;
			View.DuplicateIteratorButton.Click += OnDuplicateIteratorClick;
			View.RemoveIteratorButton.Click += OnRemoveIteratorClick;

			View.UndoButton.Click += OnUndoClick;
			View.RedoButton.Click += OnRedoClick;

			View.IteratorResetButton.Click += OnResetIteratorClick;
			View.IteratorResetOriginButton.Click += OnResetIteratorClick;
			View.IteratorResetAngleButton.Click += OnResetIteratorClick;
			View.IteratorResetScaleButton.Click += OnResetIteratorClick;

			View.Rotate90CcwButton.Click += OnRotateClick;
			View.Rotate90CwButton.Click += OnRotateClick;
			View.FlipAllHorizontalButton.Click += OnFlipClick;
			View.FlipAllVerticalButton.Click += OnFlipClick;

			View.ToggleRulersButton.Click += OnToggleRulersClick;
			View.ToggleVariationPreviewButton.Click += OnToggleVariationPreviewClick;
			View.TogglePostMatrixButton.Click += OnTogglePostMatrixClick;
			View.ToggleUseFinalIteratorButton.Click += OnToggleUseFinalIteratorClick;
		}
		protected override void DetachView()
		{
			//View.IteratorCanvas.Settings.SettingsChanged -= OnSettingsChanged;

			View.ResetAllIteratorsButton.Click -= OnResetAllIteratorsClick;

			View.AddIteratorButton.Click -= OnAddIteratorClick;
			View.DuplicateIteratorButton.Click -= OnDuplicateIteratorClick;
			View.RemoveIteratorButton.Click -= OnRemoveIteratorClick;

			View.UndoButton.Click -= OnUndoClick;
			View.RedoButton.Click -= OnRedoClick;

			View.IteratorResetButton.Click -= OnResetIteratorClick;
			View.IteratorResetOriginButton.Click -= OnResetIteratorClick;
			View.IteratorResetAngleButton.Click -= OnResetIteratorClick;
			View.IteratorResetScaleButton.Click -= OnResetIteratorClick;

			View.Rotate90CcwButton.Click -= OnRotateClick;
			View.Rotate90CwButton.Click -= OnRotateClick;
			View.FlipAllHorizontalButton.Click -= OnFlipClick;
			View.FlipAllVerticalButton.Click -= OnFlipClick;

			View.ToggleRulersButton.Click -= OnToggleRulersClick;
			View.ToggleVariationPreviewButton.Click -= OnToggleVariationPreviewClick;
			View.TogglePostMatrixButton.Click -= OnTogglePostMatrixClick;
			View.ToggleUseFinalIteratorButton.Click -= OnToggleUseFinalIteratorClick;
		}

		private void OnResetAllIteratorsClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.ResetAll();
		}

		private void OnAddIteratorClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.NewIterator();
		}
		private void OnDuplicateIteratorClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.DuplicateSelectedIterator();
		}
		private void OnRemoveIteratorClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.RemoveSelectedIterator();
		}

		private void OnUndoClick(object sender, EventArgs e)
		{
			mParent.Flame = mParent.UndoController.Undo();
		}
		private void OnRedoClick(object sender, EventArgs e)
		{
			mParent.Flame = mParent.UndoController.Redo();
		}

		private void OnResetIteratorClick(object sender, EventArgs e)
		{
			Action action = null;

			if (ReferenceEquals(sender, View.IteratorResetButton)) action = View.IteratorCanvas.Commands.ResetSelectedIterator;
			if (ReferenceEquals(sender, View.IteratorResetOriginButton)) action = View.IteratorCanvas.Commands.ResetSelectedIteratorOrigin;
			if (ReferenceEquals(sender, View.IteratorResetAngleButton)) action = View.IteratorCanvas.Commands.ResetSelectedIteratorAngle;
			if (ReferenceEquals(sender, View.IteratorResetScaleButton)) action = View.IteratorCanvas.Commands.ResetSelectedIteratorScale;

			if (action != null)
			{
				action();
			}
		}
		private void OnRotateClick(object sender, EventArgs e)
		{
			double angle = 0;

			if (ReferenceEquals(sender, View.Rotate90CcwButton)) angle = System.Math.PI / 2.0;
			if (ReferenceEquals(sender, View.Rotate90CwButton)) angle = -System.Math.PI / 2.0;

			View.IteratorCanvas.Commands.RotateWorld(angle);
		}
		private void OnFlipClick(object sender, EventArgs e)
		{
			Action action = null;

			if (ReferenceEquals(sender, View.FlipAllHorizontalButton)) action = View.IteratorCanvas.Commands.FlipAllHorizontally;
			if (ReferenceEquals(sender, View.FlipAllVerticalButton)) action = View.IteratorCanvas.Commands.FlipAllVertically;

			if (action != null)
			{
				action();
			}
		}

		private void OnToggleRulersClick(object sender, EventArgs e)
		{
			View.ToggleRulersButton.Checked = !View.ToggleRulersButton.Checked;
			View.IteratorCanvas.ShowRuler = View.ToggleRulersButton.Checked;
		}
		private void OnToggleVariationPreviewClick(object sender, EventArgs e)
		{
			View.ToggleVariationPreviewButton.Checked = !View.ToggleVariationPreviewButton.Checked;
			View.IteratorCanvas.Settings.ShowVariationPreview = View.ToggleVariationPreviewButton.Checked;
		}
		private void OnTogglePostMatrixClick(object sender, EventArgs e)
		{
			View.TogglePostMatrixButton.Checked = !View.TogglePostMatrixButton.Checked;
			View.IteratorCanvas.ActiveMatrix = View.TogglePostMatrixButton.Checked
				? IteratorMatrix.PostAffine
				: IteratorMatrix.PreAffine;
		}
		private void OnToggleUseFinalIteratorClick(object sender, EventArgs e)
		{
			View.ToggleUseFinalIteratorButton.Checked = !View.ToggleUseFinalIteratorButton.Checked;
			View.IteratorCanvas.Commands.SetUseFinalIterator(View.ToggleUseFinalIteratorButton.Checked);
		}

		private void OnSettingsChanged(object sender, EventArgs e)
		{
			UpdateButtonStates();
		}

		public void UpdateButtonStates()
		{
			View.DuplicateIteratorButton.Enabled = View.IteratorCanvas.SelectedIterator != null;
			View.RemoveIteratorButton.Enabled = mParent.Flame.Iterators.Count > 1;

			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;

			View.ToggleRulersButton.Checked = View.IteratorCanvas.ShowRuler;
			View.ToggleVariationPreviewButton.Checked = View.IteratorCanvas.Settings.ShowVariationPreview;
			View.TogglePostMatrixButton.Checked = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine;
			View.ToggleUseFinalIteratorButton.Checked = mParent.Flame.Iterators.UseFinalIterator;
		}
	}
}