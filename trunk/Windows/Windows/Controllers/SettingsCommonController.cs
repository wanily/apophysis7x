using System;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class SettingsCommonController : Controller<Forms.Settings>
	{
		private SettingsController mParent;

		public SettingsCommonController([NotNull] Forms.Settings view, [NotNull] SettingsController parent) : base(view)
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
		}
		protected override void DetachView()
		{
		}

		public void Update()
		{
			View.PluginPathTextBox.Text = Environment.ExpandEnvironmentVariables(ApophysisSettings.Common.PluginDirectoryName);
			View.JpegQualityComboBox.Text = ApophysisSettings.Common.JpegQuality.ToString(InputController.Culture);

			if (!ApophysisSettings.Preview.ThreadCount.HasValue)
				View.ThreadsComboBox.SelectedIndex = 0;
			else View.ThreadsComboBox.Text = ApophysisSettings.Preview.ThreadCount.GetValueOrDefault().ToString(InputController.Culture);

			View.PngTransparencyComboBox.SelectedIndex = ApophysisSettings.Common.EnablePngTransparency ? 0 : 1;
			View.NamePrefixTextBox.Text = ApophysisSettings.Common.NamePrefix;

			View.AutosavePathTextBox.Text = Environment.ExpandEnvironmentVariables(ApophysisSettings.Autosave.TargetPath);
			View.AutosaveEnabledCheckBox.Checked = ApophysisSettings.Autosave.Enabled;
			View.AutosaveThresholdComboBox.Text = ApophysisSettings.Autosave.Threshold.ToString(InputController.Culture);

			View.WarnOnMissingPluginsCheckBox.Checked = ApophysisSettings.Common.ShowUnknownAttributesMessage;
			View.CameraEditUseScaleCheckBox.Checked = ApophysisSettings.Editor.CameraEditUseScale;
			View.OldVariationStyleCheckBox.Checked = !ApophysisSettings.Common.VariationsIn15CStyle;
			View.ConfirmDeleteCheckBox.Checked = ApophysisSettings.Common.ShowDeleteConfirmation;
			View.ConfirmStopRendering.Checked = ApophysisSettings.Common.ShowCancelRenderConfirmation;
		}
		public void WriteSettings()
		{
			
		}
	}
}