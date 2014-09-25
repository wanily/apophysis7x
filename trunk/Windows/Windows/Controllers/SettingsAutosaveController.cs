using System;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Windows.Controls;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class SettingsAutosaveController : Controller<Forms.Settings>
	{
		private SettingsController mParent;

		public SettingsAutosaveController([NotNull] Forms.Settings view, [NotNull] SettingsController parent)
			: base(view)
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
			View.AutosaveThresholdComboBox.LostFocus += OnThresholdUpdated;
			View.AutosavePathButton.Click += OnAutosavePathClick;
		}
		protected override void DetachView()
		{
			View.AutosaveThresholdComboBox.LostFocus -= OnThresholdUpdated;
			View.AutosavePathButton.Click -= OnAutosavePathClick;
		}

		private void OnThresholdUpdated(object sender, EventArgs e)
		{
			int value;
			if (!int.TryParse(View.AutosaveThresholdComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
			{
				value = ApophysisSettings.Autosave.Threshold;
			}

			value = System.Math.Max(0, value);
			View.AutosaveThresholdComboBox.Text = value.ToString(InputController.Culture);
		}
		private void OnAutosavePathClick(object sender, EventArgs e)
		{
			using (var dialog = new FileDialogController<SaveFileDialog>(ControlResources.SelectAutosaveTarget, FileDialogController.BatchFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName();
				if (string.IsNullOrEmpty(result))
					return;

				View.AutosavePathTextBox.Text = result;
			}
		}

		public void Update()
		{
			View.AutosavePathTextBox.Text = Environment.ExpandEnvironmentVariables(ApophysisSettings.Autosave.TargetPath);
			View.AutosaveEnabledCheckBox.Checked = ApophysisSettings.Autosave.Enabled;
			View.AutosaveThresholdComboBox.Text = ApophysisSettings.Autosave.Threshold.ToString(InputController.Culture);
		}
		public void WriteSettings()
		{
			ApophysisSettings.Autosave.Enabled = View.AutosaveEnabledCheckBox.Checked;
			ApophysisSettings.Autosave.Threshold = int.Parse(View.AutosaveThresholdComboBox.Text, NumberStyles.Integer, InputController.Culture);
			ApophysisSettings.Autosave.TargetPath = View.AutosavePathTextBox.Text;
		}
	}
}