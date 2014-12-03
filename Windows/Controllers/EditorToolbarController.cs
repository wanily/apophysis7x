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
			View.AddFinalIteratorButton.Click += OnAddIteratorClick;
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

			View.IteratorConvertToRegularButton.Click += OnConvertClick;
			View.IteratorConvertToFinalButton.Click += OnConvertClick;
		}
		protected override void DetachView()
		{
			//View.IteratorCanvas.Settings.SettingsChanged -= OnSettingsChanged;

			View.ResetAllIteratorsButton.Click -= OnResetAllIteratorsClick;

			View.AddIteratorButton.Click -= OnAddIteratorClick;
			View.AddFinalIteratorButton.Click -= OnAddIteratorClick;
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

			View.IteratorConvertToRegularButton.Click -= OnConvertClick;
			View.IteratorConvertToFinalButton.Click -= OnConvertClick;
		}

		private void OnResetAllIteratorsClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.ResetAll();
		}

		private void OnAddIteratorClick(object sender, EventArgs e)
		{
			if (ReferenceEquals(sender, View.AddIteratorButton))
			{
				View.IteratorCanvas.Commands.NewIterator();
			}
			else if (ReferenceEquals(sender, View.AddFinalIteratorButton))
			{
				View.IteratorCanvas.Commands.NewFinalIterator();
			}
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
			mParent.UndoController.Undo();
		}
		private void OnRedoClick(object sender, EventArgs e)
		{
			mParent.UndoController.Redo();
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
			float angle = 0;

			if (ReferenceEquals(sender, View.Rotate90CcwButton)) angle = Float.Pi / 2;
			if (ReferenceEquals(sender, View.Rotate90CwButton)) angle = -Float.Pi / 2;

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

		private void OnConvertClick(object sender, EventArgs e)
		{
			if (ReferenceEquals(sender, View.IteratorConvertToRegularButton))
			{
				View.IteratorCanvas.Commands.ConvertSelectedIterator(0);
			}
			else if (ReferenceEquals(sender, View.IteratorConvertToFinalButton))
			{
				View.IteratorCanvas.Commands.ConvertSelectedIterator(1);
			}
		}
		private void OnSettingsChanged(object sender, EventArgs e)
		{
			UpdateButtonStates();
		}

		public void UpdateButtonStates()
		{
			View.DuplicateIteratorButton.Enabled = View.IteratorCanvas.SelectedIterator != null;
			View.RemoveIteratorButton.Enabled = 
				View.IteratorCanvas.SelectedIterator != null && 
				mParent.Flame.Iterators.CanRemove(View.IteratorCanvas.SelectedIterator.GroupIndex);

			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;

			View.ToggleRulersButton.Checked = View.IteratorCanvas.ShowRuler;
			View.ToggleVariationPreviewButton.Checked = View.IteratorCanvas.Settings.ShowVariationPreview;
			View.TogglePostMatrixButton.Checked = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine;

			View.IteratorConvertToRegularButton.Enabled =
				View.IteratorCanvas.SelectedIterator != null &&
				View.IteratorCanvas.SelectedIterator.GroupIndex != 0;
			View.IteratorConvertToFinalButton.Enabled =
				View.IteratorCanvas.SelectedIterator != null &&
				View.IteratorCanvas.SelectedIterator.GroupIndex != 1 &&
				!View.IteratorCanvas.SelectedIterator.IsSingleInGroup;
		}
	}
}