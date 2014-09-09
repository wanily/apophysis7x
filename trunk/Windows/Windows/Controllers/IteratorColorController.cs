using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
			View.IteratorPalettePictureBox.Paint += OnPalettePaint;
		}
		protected override void DetachView()
		{
			View.IteratorColorDragPanel.ValueChanged -= OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged -= OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged -= OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged -= OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged -= OnColorChanged;
			View.IteratorPalettePictureBox.Paint -= OnPalettePaint;
		}

		private void OnPalettePaint(object sender, PaintEventArgs e)
		{
			if (mParent == null || (View.IteratorCanvas.SelectedIterator != null && View.IteratorCanvas.SelectedIterator.GroupIndex > 0))
				return;

			var palette = mParent.Flame.Palette;
			var w = (float)View.IteratorPalettePictureBox.ClientSize.Width;
			var h = (float)View.IteratorPalettePictureBox.ClientSize.Height;

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			for (float pos = 0; pos < w; pos++)
			{
				var i = (int)System.Math.Round((palette.Length - 1) * pos / w);
				using (var brush = new SolidBrush(palette[i]))
				{
					e.Graphics.FillRectangle(brush, pos, 0.0f, w - pos, h);
				}
			}
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
			View.IteratorPalettePictureBox.Refresh();
			OnColorChanged(null, null);
		}
	}
}