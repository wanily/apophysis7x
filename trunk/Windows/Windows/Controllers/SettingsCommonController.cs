using System;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Windows.Controls;

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
			View.PluginPathButton.Click += OnPluginPathClick;
			View.JpegQualityComboBox.LostFocus += OnJpegQualityUpdated;
		}
		protected override void DetachView()
		{
			View.PluginPathButton.Click -= OnPluginPathClick;
			View.JpegQualityComboBox.LostFocus -= OnJpegQualityUpdated;
		}

		private void OnJpegQualityUpdated(object sender, EventArgs e)
		{
			int value;
			if (!int.TryParse(View.JpegQualityComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
			{
				value = ApophysisSettings.Common.JpegQuality;
			}

			value = System.Math.Max(1, System.Math.Min(value, 120));
			View.JpegQualityComboBox.Text = value.ToString(InputController.Culture);
		}
		private void OnPluginPathClick(object sender, EventArgs e)
		{
			using (var dialog = new FolderBrowserDialog())
			{
				var path = View.PluginPathTextBox.Text;
				if (UtilityExtensions.CheckDirectory(ref path))
				{
					dialog.SelectedPath = path;
				}

				dialog.ShowNewFolderButton = true;
				dialog.Description = ControlResources.LocatePluginDirectoryHelpString;

				if (DialogResult.Cancel == dialog.ShowDialog())
					return;

				path = dialog.SelectedPath;
				if (!UtilityExtensions.CheckDirectory(ref path))
				{
					MessageBox.Show(Messages.DirectoryDoesntExistUiError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				View.PluginPathTextBox.Text = path;
			}
		}

		public void Update()
		{
			View.PluginPathTextBox.Text = Environment.ExpandEnvironmentVariables(ApophysisSettings.Common.PluginDirectoryName);
			View.JpegQualityComboBox.Text = ApophysisSettings.Common.JpegQuality.ToString(InputController.Culture);

			View.PngTransparencyComboBox.SelectedIndex = ApophysisSettings.Common.EnablePngTransparency ? 0 : 1;
			View.NamePrefixTextBox.Text = ApophysisSettings.Common.NamePrefix;

			View.WarnOnMissingPluginsCheckBox.Checked = ApophysisSettings.Common.ShowUnknownAttributesMessage;
			View.OldVariationStyleCheckBox.Checked = !ApophysisSettings.Common.VariationsIn15CStyle;
			View.ConfirmDeleteCheckBox.Checked = ApophysisSettings.Common.ShowDeleteConfirmation;
			View.ConfirmStopRendering.Checked = ApophysisSettings.Common.ShowCancelRenderConfirmation;
		}
		public void WriteSettings()
		{
			ApophysisSettings.Common.PluginDirectoryName = View.PluginPathTextBox.Text;
			ApophysisSettings.Common.JpegQuality = int.Parse(View.JpegQualityComboBox.Text, NumberStyles.Integer, InputController.Culture);

			ApophysisSettings.Common.EnablePngTransparency = View.PngTransparencyComboBox.SelectedIndex == 0;
			ApophysisSettings.Common.NamePrefix = View.NamePrefixTextBox.Text;

			ApophysisSettings.Common.ShowUnknownAttributesMessage = View.WarnOnMissingPluginsCheckBox.Checked;
			ApophysisSettings.Common.VariationsIn15CStyle = !View.OldVariationStyleCheckBox.Checked;
			ApophysisSettings.Common.ShowDeleteConfirmation = View.ConfirmDeleteCheckBox.Checked;
			ApophysisSettings.Common.ShowCancelRenderConfirmation = View.ConfirmStopRendering.Checked;
		}
	}
}