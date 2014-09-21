using System;
using System.IO;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class AutosaveSettingsProvider : SettingsProvider<AutosaveSettings>
	{
		public string TargetPath
		{
			get { return Container.TargetPath; }
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
					throw new ArgumentNullException("value");

				if (!Directory.Exists(Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(value)) ?? string.Empty))
					throw new DirectoryNotFoundException();

				Container.TargetPath = value;
			}
		}
		public int Threshold
		{
			get { return Container.Threshold; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value");

				Container.Threshold = value;
			}
		}
	}
}