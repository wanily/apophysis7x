using System;
using Xyrus.Apophysis.Properties;

namespace Xyrus.Apophysis
{
	public static class ApophysisSettings
	{
		private static readonly Settings mSettings;

		static ApophysisSettings()
		{
			mSettings = Settings.Default;
		}

		public static double EditorMoveDistance
		{
			get { return mSettings.EditorMoveDistance; }
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorMoveDistance = value;
			}
		}
		public static double EditorRotateAngle
		{
			get { return mSettings.EditorRotateAngle; }
			set
			{
				if (value < double.Epsilon || value > 360)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorRotateAngle = value;
			}
		}
		public static double EditorScaleRatio
		{
			get { return mSettings.EditorScaleRatio; }
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorScaleRatio = value;
			}
		}

		public static bool EditorLockAxes
		{
			get { return mSettings.EditorLockAxes; }
			set
			{
				mSettings.EditorLockAxes = value;
			}
		}
		public static bool EditorShowVariationPreview
		{
			get { return mSettings.EditorShowVariationPreview; }
			set
			{
				mSettings.EditorShowVariationPreview = value;
			}
		}
		public static bool EditorShowRulers
		{
			get { return mSettings.EditorShowRulers; }
			set
			{
				mSettings.EditorShowRulers = value;
			}
		}
		public static bool EditorAutoZoom
		{
			get { return mSettings.EditorAutoZoom; }
			set
			{
				mSettings.EditorAutoZoom = value;
			}
		}

		public static double EditorPreviewRange
		{
			get { return mSettings.EditorPreviewRange; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorPreviewRange = value;
			}
		}
		public static double EditorPreviewDensity
		{
			get { return mSettings.EditorPreviewDensity; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorPreviewDensity = value;
			}
		}

		public static string PluginDirectoryName
		{
			get { return mSettings.PluginDirectoryName; }
		}

		public static string NamePrefix
		{
			get { return mSettings.NamePrefix; }
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
					throw new ArgumentNullException(@"value");

				mSettings.NamePrefix = value;
			}
		}
		public static bool BatchListUsePreviews
		{
			get { return mSettings.BatchListUsePreviews; }
			set
			{
				mSettings.BatchListUsePreviews = value;
			}
		}

		public static void Serialize()
		{
			mSettings.Save();
		}
	}
}
