using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class IteratorColorController : Controller<Editor>, IIteratorColorController
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
			View.PalettePicture.Paint += OnPalettePaint;
			View.Tabs.SelectedIndexChanged += OnTabSelected;
		}
		protected override void DetachView()
		{
			View.IteratorColorDragPanel.ValueChanged -= OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged -= OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged -= OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged -= OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged -= OnColorChanged;
			View.PalettePicture.Paint -= OnPalettePaint;
			View.Tabs.SelectedIndexChanged -= OnTabSelected;
		}

		private void RedrawPalette()
		{
			View.PalettePicture.Tag = mParent.Flame.Palette.Copy();
			View.PalettePicture.Refresh();
		}

		private void OnTabSelected(object sender, EventArgs e)
		{
			if (ReferenceEquals(View.Tabs.SelectedTab, View.ColorTab))
				RedrawPalette();
		}
		private void OnPalettePaint(object sender, PaintEventArgs e)
		{
			var w = (float)View.PalettePicture.ClientSize.Width;
			var h = View.PalettePicture.ClientSize.Height;

			var control = sender as Control;
			if (control == null)
				return;

			var palette = control.Tag as Palette;
			if (palette == null)
				return;

			for (float pos = 0; pos < w; pos++)
			{
				var i = (int)System.Math.Round((palette.Length - 1) * pos / w);

				using (var brush = new SolidBrush(palette[i]))
				{
					e.Graphics.FillRectangle(brush, pos, 0, w - pos, h);
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
				iterator.Color = View.IteratorColorScrollBar.Value / 1000.0f;
				using (mParent.Initializer.Enter())
				{
					View.IteratorColorDragPanel.Value = iterator.Color;
				}
			}

			var color = (int)System.Math.Round(iterator.Color * (mParent.Flame.Palette.Length - 1));
			View.IteratorColorDragPanel.BackColor = iterator.GroupIndex > 0 ? SystemColors.Control : mParent.Flame.Palette[color];

			RedrawPalette();
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

		public void Update()
		{
			RedrawPalette();
		}
	}
}