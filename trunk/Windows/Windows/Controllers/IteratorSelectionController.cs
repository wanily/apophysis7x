using System;
using System.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorSelectionController : Controller<Editor>
	{
		private EditorController mParent;
		private bool mShowFinal;

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

			if (mShowFinal != mParent.Flame.Iterators.UseFinalIterator)
			{
				BuildIteratorComboBox();
				mShowFinal = mParent.Flame.Iterators.UseFinalIterator;
			}

			SelectIterator(iterator);
		}
		private void OnIteratorSelectedFromComboBox(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			var index = View.IteratorSelectionComboBox.SelectedIndex;
			Iterator iterator;

			if (index >= mParent.Flame.Iterators.Count)
			{
				iterator = mParent.Flame.Iterators.FinalIterator;
			}
			else if (index < 0)
			{
				iterator = null;
			}
			else
			{
				iterator = mParent.Flame.Iterators[index];
			}

			SelectIterator(iterator);
		}

		public void BuildIteratorComboBox()
		{
			View.IteratorSelectionComboBox.Items.Clear();
			View.IteratorSelectionComboBox.Items.AddRange(mParent
				.Flame
				.Iterators
				.Select(x => x.GetDisplayName())
				.OfType<object>()
				.ToArray());

			if (mParent.Flame.Iterators.UseFinalIterator)
			{
				View.IteratorSelectionComboBox.Items.Add("Final");
			}
		}
		public void SelectIterator([CanBeNull] Iterator iterator)
		{
			View.IteratorCanvas.SelectedIterator = iterator;

			using (mParent.Initializer.Enter())
			{
				var isFinal = iterator != null && ReferenceEquals(iterator, mParent.Flame.Iterators.FinalIterator);
				var index = iterator == null
					? -1
					: isFinal
						? mParent.Flame.Iterators.Count 
						: iterator.Index;

				View.IteratorSelectionComboBox.SelectedIndex = index;
				View.IteratorNameTextBox.Text = iterator == null ? null : isFinal ? "(Final)" : iterator.Name;
				View.IteratorWeightDragPanel.Value = iterator == null ? 0.5 : iterator.Weight;
				View.IteratorColorDragPanel.Value = iterator == null ? 0.0 : iterator.Color;
				View.IteratorColorSpeedDragPanel.Value = iterator == null ? 0.0 : iterator.ColorSpeed;
				View.IteratorOpacityDragPanel.Value = iterator == null ? 1.0 : iterator.Opacity;
				View.IteratorDirectColorDragPanel.Value = iterator == null ? 1.0 : iterator.DirectColor;
				View.IteratorColorScrollBar.Value = iterator == null ? 0 : (int)(iterator.Color * 1000);

				View.IteratorWeightDragPanel.Enabled = !isFinal;
				View.IteratorNameTextBox.Enabled = !isFinal;
				View.IteratorColorSpeedDragPanel.Enabled = !isFinal;
				View.IteratorOpacityDragPanel.Enabled = !isFinal;
				View.IteratorNameLabel.Enabled = !isFinal;
			}

			mParent.UpdateCoordinates();
			mParent.UpdateToolbar();
		}

		public void InitializeFlame([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			mShowFinal = flame.Iterators.UseFinalIterator;
		}
	}
}