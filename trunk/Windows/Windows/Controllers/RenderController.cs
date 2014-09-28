using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;
using Messages = Xyrus.Apophysis.Strings.Messages;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class RenderController : DataInputController<Render>
	{
		private int mCounter;
		private NativeTimer mElapsedTimer;
		private IterationManagerBase mIterationManager;
		private Stack<Flame> mRenderStack;
		private Renderer mRenderer;

		private MainController mParent;
		private RenderMessenger mMessenger;

		private Flame mCurrentlyRenderingFlame;

		private bool mBatchMode;

		private bool mIsRendering;
		private bool mIsPaused;

		private Size? mPreset1;
		private Size? mPreset2;
		private Size? mPreset3;

		private Size mCurrentSize;
		private double mCurrentDensity;
		private double mCurrentFilterRadius;
		private int mCurrentOversample;
		private string mCurrentDestination;

		private int? mThreadCount;
		private TargetImageFileFormat mFormat;

		public RenderController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mIterationManager = new ThreadedIterationManager();
			mRenderStack = new Stack<Flame>();
			mElapsedTimer = new NativeTimer();

			mMessenger = new RenderMessenger();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mIterationManager != null)
				{
					mIterationManager.Dispose();
					mIterationManager = null;
				}

				if (mRenderer != null)
				{
					mRenderer.Dispose();
					mRenderer = null;
				}

				if (mRenderStack != null)
				{
					mRenderStack.Clear();
					mRenderStack = null;
				}
			}

			mParent = null;
			mElapsedTimer = null;
			mMessenger = null;
		}

		protected override void AttachView()
		{
			Register(View.DensityComboBox,
				xx => mCurrentDensity = xx,
				() => mCurrentDensity);
			Register(View.FilterRadiusDragPanel, 
				xx => mCurrentFilterRadius = xx,
				() => mCurrentFilterRadius);
			Register(View.OversampleTextBox,
				xx => mCurrentOversample = (int)xx,
				() => mCurrentOversample);

			View.MaintainAspectRatioCheckBox.Checked = ApophysisSettings.Render.MaintainCanvasAspectRatio;
			View.SaveIncompleteRendersCheckBox.Checked = ApophysisSettings.Render.AllowSaveIncompleteRender;
			View.SaveParametersCheckBox.Checked = ApophysisSettings.Render.SaveFlameAfterRender;

			View.WidthComboBox.TextChanged += OnWidthChanged;
			View.HeightComboBox.TextChanged += OnHeightChanged;
			View.WidthComboBox.SelectedIndexChanged += OnWidthChanged;
			View.HeightComboBox.SelectedIndexChanged += OnHeightChanged;
			View.WidthComboBox.LostFocus += OnWidthLeave;
			View.HeightComboBox.LostFocus += OnHeightLeave;

			View.Preset1SaveButton.Click += OnSavePresetClick;
			View.Preset1SelectButton.Click += OnSelectPresetClick;
			View.Preset2SaveButton.Click += OnSavePresetClick;
			View.Preset2SelectButton.Click += OnSelectPresetClick;
			View.Preset3SaveButton.Click += OnSavePresetClick;
			View.Preset3SelectButton.Click += OnSelectPresetClick;

			SetPresetButtonStates();

			View.StartButton.Click += OnStartClick;
			View.PauseButton.Click += OnPauseClick;
			View.CancelButton.Click += OnCancelClick;

			View.SelectFolderButton.Click += OnSelectFolderClick;
			View.FormatComboBox.SelectedIndexChanged += OnSelectFormat;
			View.GoToFolderButton.Click += OnGoToFolderClick;

			mCurrentSize = ApophysisSettings.Render.Size.GetValueOrDefault(new Size(1920, 1080));
			mCurrentDensity = ApophysisSettings.Render.Density;
			mCurrentFilterRadius = ApophysisSettings.Render.FilterRadius;
			mCurrentOversample = ApophysisSettings.Render.Oversample;
			mCurrentDestination = Environment.ExpandEnvironmentVariables(ApophysisSettings.Render.DestinationPath);
			mFormat = ApophysisSettings.Render.DestinationFormat;
			mThreadCount = ApophysisSettings.Render.ThreadCount;

			mIterationManager.Progress += OnProgress;
			mIterationManager.Finished += OnRenderExit;
			mMessenger.Message += OnMessage;
		}
		protected override void DetachView()
		{
			ApophysisSettings.Render.SizePreset1 = Preset1;
			ApophysisSettings.Render.SizePreset2 = Preset2;
			ApophysisSettings.Render.SizePreset3 = Preset3;

			ApophysisSettings.Render.MaintainCanvasAspectRatio = View.MaintainAspectRatioCheckBox.Checked;
			ApophysisSettings.Render.AllowSaveIncompleteRender = View.SaveIncompleteRendersCheckBox.Checked;
			ApophysisSettings.Render.SaveFlameAfterRender = View.SaveParametersCheckBox.Checked;

			View.WidthComboBox.TextChanged -= OnWidthChanged;
			View.HeightComboBox.TextChanged -= OnHeightChanged;
			View.WidthComboBox.SelectedIndexChanged -= OnWidthChanged;
			View.HeightComboBox.SelectedIndexChanged -= OnHeightChanged;
			View.WidthComboBox.LostFocus -= OnWidthLeave;
			View.HeightComboBox.LostFocus -= OnHeightLeave;

			View.Preset1SaveButton.Click -= OnSavePresetClick;
			View.Preset1SelectButton.Click -= OnSelectPresetClick;
			View.Preset2SaveButton.Click -= OnSavePresetClick;
			View.Preset2SelectButton.Click -= OnSelectPresetClick;
			View.Preset3SaveButton.Click -= OnSavePresetClick;
			View.Preset3SelectButton.Click -= OnSelectPresetClick;

			View.StartButton.Click -= OnStartClick;
			View.PauseButton.Click -= OnPauseClick;
			View.CancelButton.Click -= OnCancelClick;

			View.SelectFolderButton.Click -= OnSelectFolderClick;
			View.FormatComboBox.SelectedIndexChanged -= OnSelectFormat;
			View.GoToFolderButton.Click -= OnGoToFolderClick;

			ApophysisSettings.Render.Size = mCurrentSize;
			ApophysisSettings.Render.Density = mCurrentDensity;
			ApophysisSettings.Render.FilterRadius = mCurrentFilterRadius;
			ApophysisSettings.Render.Oversample = mCurrentOversample;
			ApophysisSettings.Render.DestinationPath = mCurrentDestination;
			ApophysisSettings.Render.DestinationFormat = mFormat;
			ApophysisSettings.Render.ThreadCount = mThreadCount;

			mIterationManager.Progress -= OnProgress;
			mIterationManager.Finished -= OnRenderExit;
			mMessenger.Message -= OnMessage;

			Cleanup();
		}

		private void OnSelectThreads(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			if (View.ThreadsComboBox.SelectedIndex == 0)
				mThreadCount = null;
			else mThreadCount = View.ThreadsComboBox.SelectedIndex;
		}
		private void OnSelectFormat(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			mFormat = (TargetImageFileFormat) View.FormatComboBox.SelectedIndex;

			if (!mBatchMode)
			{
				var name = Path.GetFileNameWithoutExtension(View.DestinationTextBox.Text) + '.' + GetExtension(mFormat);
				View.DestinationTextBox.Text = Path.Combine(mCurrentDestination, name);
			}
		}
		private void OnSelectFolderClick(object sender, EventArgs e)
		{
			if (mBatchMode)
			{
				using (var dialog = new FolderBrowserDialog())
				{
					dialog.SelectedPath = mCurrentDestination;
					dialog.ShowNewFolderButton = true;
					dialog.Description = Messages.BatchModeSelectPathHintText;

					if (DialogResult.Cancel == dialog.ShowDialog())
						return;

					mCurrentDestination = dialog.SelectedPath;

					using (mParent.Initializer.Enter())
					{
						View.DestinationTextBox.Text = mCurrentDestination;	
					}
				}
			}
			else
			{
				using (var dialog = new FileDialogController<SaveFileDialog>(Messages.SingleModeSelectPathHintText,
					FileDialogController.ImageFilesFilter,
					FileDialogController.AllFilesFilter))
				{
					var result = dialog.GetFileName();
					if (string.IsNullOrEmpty(result))
						return;

					mCurrentDestination = Path.GetDirectoryName(result);
					mFormat = GetFormat(Path.GetExtension(result));

					using (mParent.Initializer.Enter())
					{
						View.DestinationTextBox.Text = result;
						View.FormatComboBox.SelectedIndex = (int)mFormat;
					}
				}
			}
		}
		private void OnGoToFolderClick(object sender, EventArgs e)
		{
			new Process
			{
				StartInfo = new ProcessStartInfo(
					@"%windir%\explorer.exe", 
					mCurrentDestination)
			}.Start();
		}

		private void OnProgress(object sender, ProgressEventArgs args)
		{
			try
			{
				var progress = sender as ProgressProvider;
				if (progress == null)
					return;

				SetProgress(progress.IterationProgress);
				SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
				SetRemaining(progress.RemainingTime);
			}
			catch (ObjectDisposedException) { }
			
		}
		private void OnMessage(object sender, MessageEventArgs args)
		{
			View.Invoke(new Action(() =>
			{
				View.MessagesTextBox.Text += args.Message + Environment.NewLine;
				MessageCenter.SendMessage(args.Message);
				View.MessagesTextBox.SelectionStart = View.MessagesTextBox.Text.Length;
			}));
		}
		private void OnRenderExit(object sender, FinishedEventArgs e)
		{
			if (!e.Cancelled)
			{
				var bitmap = mRenderer.Histogram.CreateBitmap();
				SaveBitmap(bitmap);
			}

			View.Invoke(new Action(() => { View.MessagesTextBox.Text += Environment.NewLine; }));
		}

		private void OnStartClick(object sender, EventArgs e)
		{
			View.MessagesTextBox.Text = string.Empty;

			if (!mBatchMode)
			{
				var renderPath = View.DestinationTextBox.Text;
				if (File.Exists(renderPath))
				{
					var result = MessageBox.Show(
						string.Format(Messages.OverwriteRenderTargetConfirmMessage),
						Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.No)
						return;
				}
			}

			IsRendering = true;
			mCounter = 0;

			var savePath = Path.Combine(mCurrentDestination, Common.DefaultRenderBatchName + ".flame");
			if (File.Exists(savePath))
			{
				var backupName = Path.Combine(mCurrentDestination, Common.DefaultRenderBatchName + ".bak");
				File.Copy(savePath, backupName, true);
			}

			if (mBatchMode)
			{
				foreach (var flame in mParent.Flames.Reverse())
				{
					mRenderStack.Push(flame);
				}
			}
			else
			{
				mRenderStack.Push(mParent.BatchListController.GetSelectedFlame());
			}

			NextFlame();
		}
		private void OnPauseClick(object sender, EventArgs e)
		{
			if (!IsRendering)
				return;

			IsPaused = !IsPaused;

			if (IsPaused)
			{
				mIterationManager.Suspend();
			}
			else
			{
				mIterationManager.Resume();
			}
		}
		private void OnCancelClick(object sender, EventArgs e)
		{
			if (IsRendering)
			{
				if (ApophysisSettings.Common.ShowCancelRenderConfirmation)
				{
					var result = MessageBox.Show(
						string.Format(Messages.CancelRenderConfirmMessage),
						Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.No)
						return;
				}

				mIterationManager.Cancel();

				IsRendering = false;
				CurrentlyRenderingFlame = null;

				ResetStatus();
				mRenderStack.Clear();
			}
			else
			{
				View.Close();
			}
		}

		private void OnWidthChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			int value;

			if (!int.TryParse(View.WidthComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
				value = 0;

			if (value <= 0)
				return;

			if (View.MaintainAspectRatioCheckBox.Checked)
			{
				var aspect = (double)mCurrentSize.Height / mCurrentSize.Width;
				var size = new Size(value, (int)(aspect * value));

				mCurrentSize = size;

				using (mParent.Initializer)
				{
					View.HeightComboBox.Text = mCurrentSize.Height.ToString(InputController.Culture);
				}
			}
			else
			{
				mCurrentSize = new Size(value, mCurrentSize.Height);
			}
		}
		private void OnHeightChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			int value;

			if (!int.TryParse(View.HeightComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
				value = 0;

			if (value <= 0)
				return;

			if (View.MaintainAspectRatioCheckBox.Checked)
			{
				var aspect = (double)mCurrentSize.Width / mCurrentSize.Height;
				var size = new Size((int)(aspect * value), value);

				mCurrentSize = size;

				using (mParent.Initializer)
				{
					View.WidthComboBox.Text = mCurrentSize.Width.ToString(InputController.Culture);
				}
			}
			else
			{
				mCurrentSize = new Size(mCurrentSize.Width, value);
			}
		}
		private void OnWidthLeave(object sender, EventArgs e)
		{
			int value;

			if (!int.TryParse(View.WidthComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
				value = 0;

			if (value <= 0)
			{
				using (mParent.Initializer.Enter())
				{
					View.WidthComboBox.Text = mCurrentSize.Width.ToString(InputController.Culture);
				}
			}
		}
		private void OnHeightLeave(object sender, EventArgs e)
		{
			int value;

			if (!int.TryParse(View.HeightComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
				value = 0;

			if (value <= 0)
			{
				using (mParent.Initializer.Enter())
				{
					View.HeightComboBox.Text = mCurrentSize.Height.ToString(InputController.Culture);
				}
			}
		}

		private void OnSavePresetClick(object sender, EventArgs e)
		{
			if (ReferenceEquals(sender, View.Preset1SaveButton))
				Preset1 = mCurrentSize;

			if (ReferenceEquals(sender, View.Preset2SaveButton))
				Preset2 = mCurrentSize;

			if (ReferenceEquals(sender, View.Preset3SaveButton))
				Preset3 = mCurrentSize;

			SetPresetButtonStates();
		}
		private void OnSelectPresetClick(object sender, EventArgs e)
		{
			if (ReferenceEquals(sender, View.Preset1SelectButton) && Preset1.HasValue)
				mCurrentSize = Preset1.Value;

			if (ReferenceEquals(sender, View.Preset2SelectButton) && Preset2.HasValue)
				mCurrentSize = Preset2.Value;

			if (ReferenceEquals(sender, View.Preset3SelectButton) && Preset3.HasValue)
				mCurrentSize = Preset3.Value;

			UpdateSizeControls();
		}

		private void SetSingleMode(Flame flame)
		{
			Debug.Assert(flame != null);

			var header = string.Format(ControlResources.RenderSingleWindowTitleWithFlame, flame.CalculatedName);

			View.Text = header;
			CurrentlyRenderingFlame = null;
		}
		private void SetBatchMode(FlameCollection batch)
		{
			Debug.Assert(batch != null);

			var header = string.Format(ControlResources.RenderBatchWindowTitleWithFlames, batch.CalculatedName);

			View.Text = header;
			CurrentlyRenderingFlame = null;
		}

		private void UpdateMode()
		{
			if (mBatchMode)
			{
				SetBatchMode(mParent.Flames);
			}
			else
			{
				SetSingleMode(mParent.BatchListController.GetSelectedFlame());
			}
		}
		private void UpdateSizeControls()
		{
			View.WidthComboBox.Text = mCurrentSize.Width.ToString(InputController.Culture);
			View.HeightComboBox.Text = mCurrentSize.Height.ToString(InputController.Culture);
		}
		private void UpdatePathControls()
		{
			using (mParent.Initializer.Enter())
			{
				if (mBatchMode)
				{
					View.DestinationTextBox.Text = mCurrentDestination;
				}
				else
				{
					var flame = mParent.BatchListController.GetSelectedFlame();
					var name = (flame == null ? Common.DefaultRenderBatchName : flame.CalculatedName).CleanseFileName();

					name += '.' + GetExtension(mFormat);

					View.DestinationTextBox.Text = Path.Combine(mCurrentDestination, name);
				}

				View.FormatComboBox.SelectedIndex = (int) mFormat;
			}
		}

		private void NextFlame()
		{
			if (View == null)
				return;

			if (mRenderStack.Count == 0)
			{
				View.Invoke(new Action(() =>
				{
					IsRendering = false;
					CurrentlyRenderingFlame = null;
					ResetStatus();

					mCounter = 0;
				}));

				return;
			}

			mCounter++;
			mElapsedTimer.SetStartingTime();

			View.Invoke(new Action(() =>
			{
				CurrentlyRenderingFlame = mRenderStack.Pop();

				if (View.SaveParametersCheckBox.Checked)
				{
					var savePath = Path.Combine(mCurrentDestination, Common.DefaultRenderBatchName + ".flame");
					mParent.SaveFlame(CurrentlyRenderingFlame, savePath, batchName: Common.DefaultRenderBatchName);
				}
			}));

			mMessenger.SendMessage(string.Format(@"--- {0} ---", string.Format(Messages.RenderMessageHeader, CurrentlyRenderingFlame.CalculatedName)));
			mMessenger.SendMessage(string.Format(@"  {0}", string.Format(Messages.RenderSizeMessage, mCurrentSize.Width, mCurrentSize.Height)));
			mMessenger.SendMessage(string.Format(@"  {0}", string.Format(Messages.RenderDensityMessage, (int)mCurrentDensity)));
			mMessenger.SendMessage(string.Format(@"  {0}", string.Format(Messages.RenderFilterMessage, mCurrentOversample, mCurrentFilterRadius)));

			SetInfoString(mCurrentlyRenderingFlame.CalculatedName);

			var flame = mCurrentlyRenderingFlame;

			if (mRenderer != null)
			{
				mRenderer.Dispose();
			}

			mRenderer = new Renderer(
				flame, mCurrentSize, mCurrentOversample,
				mCurrentFilterRadius, mFormat == TargetImageFileFormat.Png && ApophysisSettings.Common.EnablePngTransparency);
			mRenderer.Messenger = mMessenger;
			mRenderer.Initialize();

			var threaded = mIterationManager as IThreaded;
			if (threaded != null)
			{
				threaded.SetThreadCount(mThreadCount);
			}

			mIterationManager.StartIterate(mRenderer.Histogram, mCurrentDensity);
		}
		private void SaveBitmap(Bitmap bitmap)
		{
			using (bitmap)
			{
				mMessenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderSavingImageMessage));

				var fileName = View.DestinationTextBox.Text;
				if (mBatchMode)
				{
					var flame = CurrentlyRenderingFlame;
					var name = (flame == null ? Common.DefaultRenderBatchName : flame.CalculatedName).CleanseFileName();

					name += '.' + GetExtension(mFormat);
					fileName = Path.Combine(fileName, name);
				}

				switch (mFormat)
				{
					case TargetImageFileFormat.Bitmap:
						bitmap.Save(fileName, ImageFormat.Bmp);
						break;
					case TargetImageFileFormat.Jpeg:
						var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
						var jpegQuality = Encoder.Quality;

						using (var jpegQualityParam = new EncoderParameter(jpegQuality, ApophysisSettings.Common.JpegQuality))
						using (var jpegParams = new EncoderParameters(1))
						{
							jpegParams.Param[0] = jpegQualityParam;
							bitmap.Save(fileName, jpegEncoder, jpegParams);
						}
						break;
					case TargetImageFileFormat.Png:
						bitmap.Save(fileName, ImageFormat.Png);
						break;
					case TargetImageFileFormat.Tiff:
						bitmap.Save(fileName, ImageFormat.Tiff);
						break;
				}
			}

			NextFlame();
		}
		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			return ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);
		}

		private void ResetStatus()
		{
			View.Invoke(new Action(() =>
			{
				View.ProgressBar.Value = 0;
				View.RemainingLabel.Text = null;
				View.ElapsedLabel.Text = null;
				View.InfoLabel.Text = null;

				UpdateSelection();
			}));
		}

		private void SetProgress(double progress)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.ProgressBar.Value = (int)(progress * 100)));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.ElapsedLabel.Text = string.Format("Elapsed: {0}", GetTimespanString(elapsed))));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.RemainingLabel.Text = string.Format("Remaining: {0}", remaining == null ? "calculating..." : GetTimespanString(remaining.Value))));
		}
		private void SetInfoString(string info)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.InfoLabel.Text = info));
		}

		private string GetTimespanString(TimeSpan time)
		{
			return string.Format("{0}:{1}:{2}",
				time.Hours.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Minutes.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Seconds.ToString("#0", InputController.Culture).PadLeft(2, '0'));
		}
		private string GetExtension(TargetImageFileFormat format)
		{
			switch (format)
			{
				case TargetImageFileFormat.Bitmap:
					return @"bmp";
				case TargetImageFileFormat.Jpeg:
					return @"jpg";
				case TargetImageFileFormat.Png:
					return @"png";
				case TargetImageFileFormat.Tiff:
					return @"tif";
				default:
					throw new ArgumentOutOfRangeException("format");
			}
		}
		private TargetImageFileFormat GetFormat(string extension)
		{
			switch (extension.TrimStart('.').ToLower())
			{
				case "bmp":
					return TargetImageFileFormat.Bitmap;
				case "jpg":
				case "jpeg":
					return TargetImageFileFormat.Jpeg;
				case "png":
					return TargetImageFileFormat.Png;
				case "tif":
				case "tiff":
					return TargetImageFileFormat.Tiff;
				default:
					return TargetImageFileFormat.Bitmap;
			}
		}

		private void SetPresetButtonText(Button button, int index, Size? preset)
		{
			button.Text = preset.HasValue
				? string.Format("{0} x {1}", preset.Value.Width, preset.Value.Height)
				: string.Format("Preset {0}", index);
		}
		private void SetPresetButtonStates()
		{
			View.Preset1SelectButton.Enabled = Preset1.HasValue;
			View.Preset2SelectButton.Enabled = Preset2.HasValue;
			View.Preset3SelectButton.Enabled = Preset3.HasValue;
		}

		public Size? Preset1
		{
			get { return mPreset1; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				mPreset1 = value;
				SetPresetButtonText(View.Preset1SelectButton, 1, value);
			}
		}
		public Size? Preset2
		{
			get { return mPreset2; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				mPreset2 = value;
				SetPresetButtonText(View.Preset2SelectButton, 2, value);
			}
		}
		public Size? Preset3
		{
			get { return mPreset3; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				mPreset3 = value;
				SetPresetButtonText(View.Preset3SelectButton, 3, value);
			}
		}

		public Flame CurrentlyRenderingFlame
		{
			get { return mCurrentlyRenderingFlame; }
			private set
			{
				mCurrentlyRenderingFlame = value;
			}
		}
		public bool BatchMode
		{
			get { return mBatchMode; }
			set
			{
				if (Equals(value, mBatchMode))
					return;

				mBatchMode = value;
				UpdateMode();
			}
		}

		public bool IsRendering
		{
			get { return mIsRendering; }
			private set
			{
				if (Equals(value, mIsRendering))
					return;

				var oldValue = mIsRendering;
				mIsRendering = value;

				View.StartButton.Enabled = !value;
				View.PauseButton.Enabled = value;
				View.CancelButton.Text = value ? ControlResources.Cancel : ControlResources.Close;

				View.SizeGroupBox.Enabled = !value;
				View.PropertiesGroupBox.Enabled = !value;
				View.SaveParametersCheckBox.Enabled = !value;
				View.DestinationLabel.Enabled = !value;
				View.DestinationTextBox.Enabled = !value;
				View.SelectFolderButton.Enabled = !value;
				View.GoToFolderButton.Enabled = !value;
				View.FormatComboBox.Enabled = !value;
				View.FormatLabel.Enabled = !value;
				View.ThreadsLabel.Enabled = !value;
				View.ThreadsComboBox.Enabled = !value;

				if (!oldValue && value)
				{
					//View.Tabs.SelectedTab = View.MessagesTab;
				}
				else if (!value && oldValue)
				{
					//
				}

				View.ProgressBar.Value = 0;
				IsPaused = false;
			}
		}
		public bool IsPaused
		{
			get { return mIsPaused; }
			private set
			{
				if (Equals(value, mIsPaused))
					return;

				mIsPaused = value;

				View.PauseButton.Text = value ? ControlResources.Resume : ControlResources.Pause;
			}
		}

		public override void Update()
		{
			UpdateMode();
			IsRendering = false;

			UpdateSizeControls();
			UpdatePathControls();

			using (mParent.Initializer.Enter())
			{
				View.FormatComboBox.SelectedIndex = (int)mFormat;
				View.ThreadsComboBox.SelectedIndex = ApophysisSettings.Render.ThreadCount.GetValueOrDefault();
			}

			base.Update();
		}
		public void UpdateSelection()
		{
			if (IsRendering)
				return;

			UpdateMode();
		}
	}
}