using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
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
			View.PaletteSelectComboBox.SelectedIndexChanged += OnPaletteSelected;

			UpdateControls();
		}
		protected override void DetachView()
		{
			View.IteratorColorDragPanel.ValueChanged -= OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged -= OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged -= OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged -= OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged -= OnColorChanged;
			View.PaletteSelectComboBox.SelectedIndexChanged -= OnPaletteSelected;
		}

		private void OnPaletteSelected(object sender, EventArgs e)
		{
			var palette = View.PaletteSelectComboBox.SelectedItem as Palette;
			if (mParent.Initializer.IsBusy || palette == null)
				return;

			mParent.Flame.Palette = palette;
			UpdateControls();
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

			var color = (int)System.Math.Round(iterator.Color * (mParent.Flame.Palette.Length - 1));
			View.IteratorColorDragPanel.BackColor = iterator.GroupIndex > 0 ? SystemColors.Control : mParent.Flame.Palette[color];
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

		public void UpdateControls()
		{
			if (!mParent.Initializer.IsBusy && mParent.Flame != null)
			{
				using (mParent.Initializer.Enter())
				{
					var index = View.PaletteSelectComboBox.Items.IndexOf(mParent.Flame.Palette);
					if (index < 0)
					{
						View.PaletteSelectComboBox.Items.Add(mParent.Flame.Palette);
						index = View.PaletteSelectComboBox.Items.Count - 1;
					}

					View.PaletteSelectComboBox.SelectedIndex = index;
				}

				OnColorChanged(null, null);
			}
		}
	}
}