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
		private PaletteEditHandler mSelectedEditHandler;

		private double mCurrentEditValue;

		public FlamePropertiesPaletteController(FlameProperties view, [NotNull] FlamePropertiesController parent) : base(view, parent.Initializer)
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

			mSelectedEditHandler = null;
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

			View.PalettePicture.Paint += OnPalettePaint;
			View.PalettePresetPictureBox.Paint += OnPresetPaint;

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

			View.RotateHandlerMenuItem.Click += OnEditModeSelected;
			View.HueHandlerMenuItem.Click += OnEditModeSelected;
			View.SaturationHandlerMenuItem.Click += OnEditModeSelected;
			View.BrightnessHandlerMenuItem.Click += OnEditModeSelected;
			View.ContrastHandlerMenuItem.Click += OnEditModeSelected;
			View.BlurHandlerMenuItem.Click += OnEditModeSelected;
			View.FrequencyHandlerMenuItem.Click += OnEditModeSelected;

			CurrentHandler = mEditHandlers.First();
		}
		protected override void DetachView()
		{
			View.PalettePicture.Paint -= OnPalettePaint;
			View.PalettePresetPictureBox.Paint -= OnPresetPaint;

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

			View.RotateHandlerMenuItem.Click -= OnEditModeSelected;
			View.HueHandlerMenuItem.Click -= OnEditModeSelected;
			View.SaturationHandlerMenuItem.Click -= OnEditModeSelected;
			View.BrightnessHandlerMenuItem.Click -= OnEditModeSelected;
			View.ContrastHandlerMenuItem.Click -= OnEditModeSelected;
			View.BlurHandlerMenuItem.Click -= OnEditModeSelected;
			View.FrequencyHandlerMenuItem.Click -= OnEditModeSelected;

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
			DrawPalettePreset(mParent.Flame.Palette);
			UpdateViewValue();
			UpdateBounds();
			InitHandlers();
			RedrawPalette();

			base.Update();
		}

		[NotNull]
		internal PaletteEditHandler CurrentHandler
		{
			get { return mSelectedEditHandler; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mSelectedEditHandler = value;
				View.PaletteEditModeButton.Text = value.GetDisplayName();
			}
		}
		internal double CurrentEditValue
		{
			get { return mCurrentEditValue; }
			set
			{
				mCurrentEditValue = value;
				CurrentHandler.Value = (int)value;
			}
		}

		private void DrawPalettePreset([NotNull] Palette preset)
		{
			View.PalettePresetPictureBox.Tag = preset.Copy();
			View.PalettePresetPictureBox.Refresh();
		}
		private void UpdateViewValue()
		{
			View.PaletteEditScrollBar.Value = (int)(mCurrentEditValue = CurrentHandler.Value);
		}

		private void ResetAllHandlers()
		{
			foreach (var handler in mEditHandlers)
			{
				handler.Reset();
			}

			UpdateViewValue();
			UpdateBounds();
		}
		private void RedrawPalette()
		{
			View.PalettePicture.Refresh();
		}
		private void UpdateBounds()
		{
			var handler = CurrentHandler;

			View.PaletteEditScrollBar.Minimum = handler.MinValue;
			View.PaletteEditScrollBar.Maximum = handler.MaxValue;
		}
		private void InitHandlers()
		{
			using (mParent.Initializer.Enter())
			{
				foreach (var handler in mEditHandlers)
				{
					handler.Initialize(mParent.Flame.Palette);
					handler.Reset();
				}
			}

			UpdateViewValue();
			UpdateBounds();
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
		private void OnPresetPaint(object sender, PaintEventArgs e)
		{
			var w = (float)View.PalettePicture.ClientSize.Width;
			var h = View.PalettePicture.ClientSize.Height;

			var palette = View.PalettePresetPictureBox.Tag as Palette;
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

		private void OnPaletteSelectClick(object sender, EventArgs e)
		{
			//todo find palette with dialog

			InitHandlers();
			RedrawPalette();
			mParent.CommitValue();
		}
		private void OnEditModeSelected(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			PaletteEditHandler handler;

			if (ReferenceEquals(View.RotateHandlerMenuItem, sender)) handler = mEditHandlers[0];
			else if (ReferenceEquals(View.HueHandlerMenuItem, sender)) handler = mEditHandlers[1];
			else if (ReferenceEquals(View.SaturationHandlerMenuItem, sender)) handler = mEditHandlers[2];
			else if (ReferenceEquals(View.BrightnessHandlerMenuItem, sender)) handler = mEditHandlers[3];
			else if (ReferenceEquals(View.ContrastHandlerMenuItem, sender)) handler = mEditHandlers[4];
			else if (ReferenceEquals(View.BlurHandlerMenuItem, sender)) handler = mEditHandlers[5];
			else if (ReferenceEquals(View.FrequencyHandlerMenuItem, sender)) handler = mEditHandlers[6];
			else return;

			using (mParent.Initializer.Enter())
			{
				handler.Reset();
				handler.Initialize(mParent.Flame.Palette);
			}

			CurrentHandler = handler;

			UpdateViewValue();
			UpdateBounds();

			DrawPalettePreset(mParent.Flame.Palette);
			RedrawPalette();
		}
		private void OnEditHandlerUpdated(object sender, EventArgs e)
		{
			var handler = CurrentHandler;

			handler.Calculate(mParent.Flame.Palette);
			UpdateViewValue();
			RedrawPalette();
		}

		private void OnRandomPresetClick(object sender, EventArgs e)
		{
			mParent.Flame.Palette.Overwrite(PaletteCollection.GetRandomPalette(mParent.Flame));

			InitHandlers();
			mParent.CommitValue();
			Update();
		}
		private void OnResetClick(object sender, EventArgs e)
		{
			ResetAllHandlers();
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

					mParent.Flame.Palette.Overwrite(palette);

					InitHandlers();
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

			mParent.Flame.Palette.Overwrite(data);

			InitHandlers();
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

			mParent.Flame.Palette.Overwrite(data);

			InitHandlers();
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