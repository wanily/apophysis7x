using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesPaletteController : DataInputController<FlameProperties>
	{
		private FlamePropertiesController mParent;
		private List<PaletteEditHandler> mEditHandlers;
		private PaletteEditHandler mCurrentEditHandler;

		private double mCurrentEditValue;

		public FlamePropertiesPaletteController(FlameProperties view, [NotNull] FlamePropertiesController parent)
			: base(view, parent.Initializer)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mEditHandlers = new List<PaletteEditHandler>
			{
				new RotatePaletteEditHandler(),
				new HuePaletteEditHandler(),
				new SaturationPaletteEditHandler(),
				new BrightnessPaletteEditHandler(),
				new ContrastPaletteEditHandler(),
				new BlurPaletteEditHandler(),
				new FrequencyPaletteEditHandler()
			};

			foreach (var editHandler in mEditHandlers)
			{
				editHandler.ValueChanged += OnEditHandlerUpdated;
			}
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mEditHandlers != null)
				{
					foreach (var editHandler in mEditHandlers)
					{
						editHandler.ValueChanged -= OnEditHandlerUpdated;
					}

					mEditHandlers.Clear();
					mEditHandlers = null;
				}
			}

			mParent = null;
		}

		protected override void AttachView()
		{
			Register(View.PaletteEditTextBox,
				xx => CurrentEditValue = xx,
				() => CurrentEditValue);
			Register(View.PaletteEditScrollBar,
				xx => CurrentEditValue = xx,
				() => CurrentEditValue);

			View.PaletteEditModeComboBox.SelectedIndexChanged += OnEditModeSelected;
			View.PaletteSelectComboBox.SelectedIndexChanged += OnPaletteSelected;
			View.PalettePicture.Paint += OnPalettePaint;

			View.RandomPresetButton.Click += OnRandomPresetClick;
			View.PaletteResetButton.Click += OnResetClick;

			View.CopyPaletteButton.Click += OnCopyClick;
			View.PastePaletteButton.Click += OnPasteClick;
			View.PaletteBrowserButton.Click += OnPaletteBrowserClick;
			View.PaletteFromImageButton.Click += OnPaletteFromImageClick;

			View.RandomizePaletteMenuItem.Click += OnRandomPresetClick;
			View.InvertPaletteMenuItem.Click += OnInvertClick;
			View.ReversePaletteMenuItem.Click += OnReverseClick;
			View.PaletteFromImageMenuItem.Click += OnPaletteFromImageClick;
			View.PaletteBrowserMenuItem.Click += OnPaletteBrowserClick;
			View.SavePaletteMenuItem.Click += OnSaveClick;
			View.CopyPaletteMenuItem.Click += OnCopyClick;
			View.PastePaletteMenuItem.Click += OnPasteClick;

			using (mParent.Initializer.Enter())
			{
				View.PaletteSelectComboBox.Items.Clear();
				View.PaletteSelectComboBox.Items.AddRange(PaletteCollection.Flam3Palettes.OfType<object>().ToArray());

				View.PaletteEditModeComboBox.Items.AddRange(mEditHandlers.OfType<object>().ToArray());
				View.PaletteEditModeComboBox.SelectedIndex = 0;

				mCurrentEditHandler = mEditHandlers.First();
				mCurrentEditHandler.Reset();

				View.PaletteEditScrollBar.Value = (int)(mCurrentEditValue = mCurrentEditHandler.Value);
				View.PaletteEditScrollBar.Minimum = mCurrentEditHandler.MinValue;
				View.PaletteEditScrollBar.Maximum = mCurrentEditHandler.MaxValue;
			}
		}
		protected override void DetachView()
		{
			View.PaletteEditModeComboBox.SelectedIndexChanged -= OnEditModeSelected;
			View.PaletteSelectComboBox.SelectedIndexChanged -= OnPaletteSelected;
			View.PalettePicture.Paint -= OnPalettePaint;

			View.RandomPresetButton.Click -= OnRandomPresetClick;
			View.PaletteResetButton.Click -= OnResetClick;

			View.CopyPaletteButton.Click -= OnCopyClick;
			View.PastePaletteButton.Click -= OnPasteClick;
			View.PaletteBrowserButton.Click -= OnPaletteBrowserClick;
			View.PaletteFromImageButton.Click -= OnPaletteFromImageClick;

			View.RandomizePaletteMenuItem.Click -= OnRandomPresetClick;
			View.InvertPaletteMenuItem.Click -= OnInvertClick;
			View.ReversePaletteMenuItem.Click -= OnReverseClick;
			View.PaletteFromImageMenuItem.Click -= OnPaletteFromImageClick;
			View.PaletteBrowserMenuItem.Click -= OnPaletteBrowserClick;
			View.SavePaletteMenuItem.Click -= OnSaveClick;
			View.CopyPaletteMenuItem.Click -= OnCopyClick;
			View.PastePaletteMenuItem.Click -= OnPasteClick;

			Cleanup();
		}

		protected override void OnValueCommittedOverride(object control)
		{
			mParent.CommitValue();
		}
		protected override void OnValueChangedOverride(object control)
		{
			mParent.PreviewController.DelayedUpdatePreview();
		}

		public override void Update()
		{
			if (!mParent.Initializer.IsBusy)
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
			}

			foreach (var handler in mEditHandlers)
			{
				handler.InputPalette = mParent.Flame.Palette;
				handler.Reset();
			}
			
			base.Update();
			View.PalettePicture.Refresh();
		}

		private double CurrentEditValue
		{
			get { return mCurrentEditValue; }
			set
			{
				mCurrentEditValue = value;
				mCurrentEditHandler.Value = (int)value;
			}
		}

		private void OnPalettePaint(object sender, PaintEventArgs e)
		{
			var w = (float)View.PalettePicture.ClientSize.Width;
			var h = View.PalettePicture.ClientSize.Height;

			var palette = mParent.Flame.Palette;

			for (float pos = 0; pos < w; pos++)
			{
				var i = (int)System.Math.Round((palette.Length - 1) * pos / w);

				using (var brush = new SolidBrush(palette[i]))
				{
					e.Graphics.FillRectangle(brush, pos, 0, w - pos, h);
				}
			}
		}
		private void OnPaletteSelected(object sender, EventArgs e)
		{
			var palette = View.PaletteSelectComboBox.SelectedItem as Palette;
			if (mParent.Initializer.IsBusy || palette == null)
				return;

			mParent.Flame.Palette = palette;

			mCurrentEditHandler.InputPalette = palette;
			mCurrentEditHandler.Reset();

			View.PalettePicture.Refresh();
			mParent.CommitValue();
		}
		private void OnEditModeSelected(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			var prevEditHandler = mCurrentEditHandler;

			mCurrentEditHandler = View.PaletteEditModeComboBox.SelectedItem as PaletteEditHandler;
			if (mCurrentEditHandler == null)
			{
				mCurrentEditHandler = mEditHandlers.First();
				using (mParent.Initializer.Enter())
				{
					View.PaletteEditModeComboBox.SelectedIndex = 0;
				}
			}

			mCurrentEditHandler.InputPalette = prevEditHandler.OutputPalette;
			mCurrentEditHandler.Reset();

			View.PaletteEditScrollBar.Value = (int)(mCurrentEditValue = mCurrentEditHandler.Value);
			View.PaletteEditScrollBar.Minimum = mCurrentEditHandler.MinValue;
			View.PaletteEditScrollBar.Maximum = mCurrentEditHandler.MaxValue;
		}
		private void OnEditHandlerUpdated(object sender, EventArgs e)
		{
			mParent.Flame.Palette = mCurrentEditHandler.OutputPalette;
			View.PalettePicture.Refresh();
		}

		private void OnRandomPresetClick(object sender, EventArgs e)
		{
			mParent.Flame.Palette = PaletteCollection.GetRandomPalette(mParent.Flame);

			mCurrentEditHandler.Reset();
			mCurrentEditValue = mCurrentEditHandler.Value;

			mParent.CommitValue();
			Update();
		}
		private void OnResetClick(object sender, EventArgs e)
		{
			mCurrentEditHandler.Reset();
			mCurrentEditValue = mCurrentEditHandler.Value;

			mParent.CommitValue();
			Update();
		}
		
		private void OnCopyClick(object sender, EventArgs e)
		{
			var ugr = PaletteCollection.WriteUgr(new[] {mParent.Flame.Palette});
			Clipboard.SetText(ugr);
		}
		private void OnPasteClick(object sender, EventArgs e)
		{
			var clipboard = Clipboard.GetText();
			if (!string.IsNullOrEmpty(clipboard))
			{
				try
				{
					var result = PaletteCollection.ReadUgr(clipboard);
					var palette = result.FirstOrDefault();

					if (palette == null)
						throw new ApophysisException("No gradients found in clipboard");

					mParent.Flame.Palette = palette;

					mCurrentEditHandler.Reset();
					mCurrentEditValue = mCurrentEditHandler.Value;

					mParent.CommitValue();
					Update();
				}
				catch (ApophysisException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void OnPaletteBrowserClick(object sender, EventArgs e)
		{
			ApophysisApplication.MainWindow.MainMenuController.OnBrowsePalettesClick(sender, e);
		}
		private void OnPaletteFromImageClick(object sender, EventArgs e)
		{
			ApophysisApplication.MainWindow.MainMenuController.OnPaletteFromImageClick(sender, e);
		}

		private void OnInvertClick(object sender, EventArgs e)
		{
			var data = new Color[mParent.Flame.Palette.Length];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = mParent.Flame.Palette[i].Invert();
			}

			mParent.Flame.Palette = new Palette(mParent.Flame.Palette.CalculatedName, data);

			mCurrentEditHandler.Reset();
			mCurrentEditValue = mCurrentEditHandler.Value;

			mParent.CommitValue();
			Update();
		}
		private void OnReverseClick(object sender, EventArgs e)
		{
			var data = new Color[mParent.Flame.Palette.Length];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = mParent.Flame.Palette[mParent.Flame.Palette.Length - i - 1];
			}

			mParent.Flame.Palette = new Palette(mParent.Flame.Palette.CalculatedName, data);

			mCurrentEditHandler.Reset();
			mCurrentEditValue = mCurrentEditHandler.Value;

			mParent.CommitValue();
			Update();
		}

		private void OnSaveClick(object sender, EventArgs e)
		{
			using (var dialog = new FileDialogController<SaveFileDialog>("Save gradient...", FileDialogController.PaletteFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName(false);
				if (string.IsNullOrEmpty(result))
					return;

				var ugr = PaletteCollection.WriteUgr(new[] { mParent.Flame.Palette });
				File.AppendAllText(result, ugr + Environment.NewLine);
			}
		}
	}
}