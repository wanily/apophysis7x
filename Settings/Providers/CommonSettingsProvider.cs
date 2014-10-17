using System;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Strings;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class CommonSettingsProvider : SettingsProvider<CommonSettings>
	{
		private const string mFlameExportVersionString = "Apophysis 7x";

		public string NamePrefix
		{
			get { return Container.NamePrefix; }
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
					throw new ArgumentNullException(@"value");

				Container.NamePrefix = value;
			}
		}
		public string PluginDirectoryName
		{
			get { return Container.PluginDirectoryName; }
			set
			{
				if (!UtilityExtensions.CheckDirectory(ref value))
					throw new ArgumentException(Messages.DirectoryDoesntExistError, @"value");
				Container.PluginDirectoryName = value;
			}
		}

		public bool ShowUnknownAttributesMessage
		{
			get { return Container.ShowUnknownAttributesMessage; }
			set { Container.ShowUnknownAttributesMessage = value; }
		}
		public bool VariationsIn15CStyle
		{
			get { return Container.VariationsIn15CStyle; }
			set { Container.VariationsIn15CStyle = value; }
		}

		public string FlameExportVersionString
		{
			get { return mFlameExportVersionString; }
		}

		public int JpegQuality
		{
			get { return Container.JpegQuality; }
			set
			{
				if (value < 1 || value > 120)
					throw new ArgumentOutOfRangeException(@"value");

				Container.JpegQuality = value;
			}
		}
		public bool EnablePngTransparency
		{
			get { return Container.EnablePngTransparency; }
			set { Container.EnablePngTransparency = value; }
		}

		public bool ShowDeleteConfirmation
		{
			get { return Container.ShowDeleteConfirmation; }
			set { Container.ShowDeleteConfirmation = value; }
		}
		public bool ShowCancelRenderConfirmation
		{
			get { return Container.ShowCancelRenderConfirmation; }
			set { Container.ShowCancelRenderConfirmation = value; }
		}
	}
}