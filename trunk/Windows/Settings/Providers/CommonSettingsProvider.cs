using System;

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
	}
}