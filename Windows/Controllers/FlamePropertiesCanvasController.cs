using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesCanvasController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;

		private Size? mPreset1;
		private Size? mPreset2;
		private Size? mPreset3;

		private Size mCurrentSize;

		public FlamePropertiesCanvasController(FlameProperties view, [NotNull] FlamePropertiesController parent) : base(view)
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
			Preset1 = ApophysisSettings.Preview.SizePreset1;
			Preset2 = ApophysisSettings.Preview.SizePreset2;
			Preset3 = ApophysisSettings.Preview.SizePreset3;

			View.MaintainAspectRatioCheckBox.Checked = ApophysisSettings.View.MaintainCanvasAspectRatio;
			View.ResizeMainWindowCheckBox.Checked = ApophysisSettings.View.SyncMainWindowWithCanvasSize;

			View.WidthComboBox.TextChanged += OnWidthChanged;
			View.HeightComboBox.TextChanged += OnHeightChanged;
			View.WidthComboBox.SelectedIndexChanged += OnWidthChanged;
			View.HeightComboBox.SelectedIndexChanged += OnHeightChanged;
			View.WidthComboBox.LostFocus += OnWidthLeave;
			View.HeightComboBox.LostFocus += OnHeightLeave;

			View.ApplyButton.Click += OnApplyClick;

			View.Preset1SaveButton.Click += OnSavePresetClick;
			View.Preset1SelectButton.Click += OnSelectPresetClick;
			View.Preset2SaveButton.Click += OnSavePresetClick;
			View.Preset2SelectButton.Click += OnSelectPresetClick;
			View.Preset3SaveButton.Click += OnSavePresetClick;
			View.Preset3SelectButton.Click += OnSelectPresetClick;

			SetPresetButtonStates();
		}
		protected override void DetachView()
		{
			ApophysisSettings.Preview.SizePreset1 = Preset1;
			ApophysisSettings.Preview.SizePreset2 = Preset2;
			ApophysisSettings.Preview.SizePreset3 = Preset3;

			ApophysisSettings.View.MaintainCanvasAspectRatio = View.MaintainAspectRatioCheckBox.Checked;
			ApophysisSettings.View.SyncMainWindowWithCanvasSize = View.ResizeMainWindowCheckBox.Checked;

			View.WidthComboBox.TextChanged -= OnWidthChanged;
			View.HeightComboBox.TextChanged -= OnHeightChanged;
			View.WidthComboBox.SelectedIndexChanged -= OnWidthChanged;
			View.HeightComboBox.SelectedIndexChanged -= OnHeightChanged;
			View.WidthComboBox.LostFocus -= OnWidthLeave;
			View.HeightComboBox.LostFocus -= OnHeightLeave;

			View.ApplyButton.Click -= OnApplyClick;

			View.Preset1SaveButton.Click -= OnSavePresetClick;
			View.Preset1SelectButton.Click -= OnSelectPresetClick;
			View.Preset2SaveButton.Click -= OnSavePresetClick;
			View.Preset2SelectButton.Click -= OnSelectPresetClick;
			View.Preset3SaveButton.Click -= OnSavePresetClick;
			View.Preset3SelectButton.Click -= OnSelectPresetClick;
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

		public void Update()
		{
			if (mParent.Flame == null)
				return;

			View.WidthComboBox.Text = mParent.Flame.CanvasSize.Width.ToString(InputController.Culture);
			View.HeightComboBox.Text = mParent.Flame.CanvasSize.Height.ToString(InputController.Culture);

			mCurrentSize = mParent.Flame.CanvasSize;
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
				var aspect = (float)mCurrentSize.Height / mCurrentSize.Width;
				var size = new Size(value, (int) (aspect*value));

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
				var aspect = (float)mCurrentSize.Width / mCurrentSize.Height;
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

		private void OnApplyClick(object sender, EventArgs e)
		{
			if (mParent.Flame == null)
				return;

			mParent.Flame.CanvasSize = mCurrentSize;
			mParent.ApplyCanvas(View.ResizeMainWindowCheckBox.Checked);
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
			if (mParent.Flame == null)
				return;

			if (ReferenceEquals(sender, View.Preset1SelectButton) && Preset1.HasValue)
				mParent.Flame.CanvasSize = Preset1.Value;

			if (ReferenceEquals(sender, View.Preset2SelectButton) && Preset2.HasValue)
				mParent.Flame.CanvasSize = Preset2.Value;

			if (ReferenceEquals(sender, View.Preset3SelectButton) && Preset3.HasValue)
				mParent.Flame.CanvasSize = Preset3.Value;

			mParent.ApplyCanvas(View.ResizeMainWindowCheckBox.Checked);
			Update();
		}
	}
}