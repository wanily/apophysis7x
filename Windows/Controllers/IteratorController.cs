using System;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class IteratorController : Controller<Editor>, IIteratorController
	{
		private EditorController mParent;

		private Resolver<IIteratorColorController> mColorController;
		private Resolver<IIteratorPointEditController> mPointEditController;
		private Resolver<IIteratorVectorEditController> mVectorEditController;
		private Resolver<IIteratorVariationsController> mVariationsController;
		private Resolver<IIteratorVariablesController> mVariablesController;

		public IteratorController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			
			mParent = parent;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mColorController.Reset();
			mPointEditController.Reset();
			mVectorEditController.Reset();
			mVariablesController.Reset();
			mVariationsController.Reset();

			mParent = null;
		}

		protected override void AttachView()
		{
			View.IteratorWeightDragPanel.ValueChanged += OnWeightChanged;
			View.IteratorNameTextBox.TextChanged += OnNameChanged;

			View.PreviewDensityTrackBar.ValueChanged += OnDensityChanged;
			View.PreviewRangeTrackBar.ValueChanged += OnRangeChanged;
			View.ApplyPostTransformToVariationPreviewCheckBox.Click += OnApplyPostTransformClick;

			using (mParent.Initializer.Enter())
			{
				View.PreviewDensityTrackBar.Value = (int)View.IteratorCanvas.PreviewDensity;
				View.PreviewRangeTrackBar.Value = (int)View.IteratorCanvas.PreviewRange;
				View.ApplyPostTransformToVariationPreviewCheckBox.Checked = ApophysisSettings.Editor.VariationPreviewApplyPostTransform;
			}

			View.IteratorSelectionComboBox.SelectedIndexChanged += OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged += OnIteratorSelectedFromCanvas;
		}
		protected override void DetachView()
		{
			View.IteratorWeightDragPanel.ValueChanged -= OnWeightChanged;
			View.IteratorNameTextBox.TextChanged -= OnNameChanged;

			View.PreviewDensityTrackBar.ValueChanged -= OnDensityChanged;
			View.PreviewRangeTrackBar.ValueChanged -= OnRangeChanged;
			View.ApplyPostTransformToVariationPreviewCheckBox.Click -= OnApplyPostTransformClick;

			ApophysisSettings.Editor.VariationPreviewApplyPostTransform = View.ApplyPostTransformToVariationPreviewCheckBox.Checked;

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
			View.IteratorSelectionComboBox.Flame = mParent.Flame;
		}
		public void SelectIterator([CanBeNull] Models.Iterator iterator)
		{
			View.IteratorCanvas.SelectedIterator = iterator;

			

			using (mParent.Initializer.Enter())
			{
				View.IteratorSelectionComboBox.SelectedIndex = iterator == null ? -1 : iterator.Index;
				View.IteratorNameTextBox.Text = iterator == null ? null : iterator.Name;
				View.IteratorWeightDragPanel.Value = iterator == null ? 0.5f : iterator.Weight;
				View.IteratorColorDragPanel.Value = iterator == null ? 0.0f : iterator.Color;
				View.IteratorColorSpeedDragPanel.Value = iterator == null ? 0.0f : iterator.ColorSpeed;
				View.IteratorOpacityDragPanel.Value = iterator == null ? 1.0f : iterator.Opacity;
				View.IteratorDirectColorDragPanel.Value = iterator == null ? 1.0f : iterator.DirectColor;
				View.IteratorColorScrollBar.Value = iterator == null ? 0 : (int)(iterator.Color * 1000);

				View.IteratorWeightDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorColorSpeedDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorColorDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorColorScrollBar.Enabled = iterator == null || iterator.GroupIndex == 0;
				View.IteratorOpacityDragPanel.Enabled = iterator == null || iterator.GroupIndex == 0;
			}

			mParent.UpdateCoordinates();
			mParent.UpdateToolbar();
			mParent.UpdateVariations();
			mParent.UpdateVariables();
			mParent.UpdateColor();
		}

		public IIteratorColorController ColorControls
		{
			get { return mColorController.Object; }
		}
		public IIteratorPointEditController PointControls
		{
			get { return mPointEditController.Object; }
		}
		public IIteratorVectorEditController VectorControls
		{
			get { return mVectorEditController.Object; }
		}
		public IIteratorVariationsController VariationsList
		{
			get { return mVariationsController.Object; }
		}
		public IIteratorVariablesController VariablesList
		{
			get { return mVariablesController.Object; }
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
		private void OnApplyPostTransformClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.PreviewApplyPostTransform = View.ApplyPostTransformToVariationPreviewCheckBox.Checked;
		}
	}
}