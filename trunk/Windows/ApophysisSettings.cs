using System;
using Xyrus.Apophysis.Properties;
using Xyrus.Apophysis.Windows;

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

		public static double EditorVariationPreviewRange
		{
			get { return mSettings.EditorVariationPreviewRange; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorVariationPreviewRange = value;
			}
		}
		public static double EditorVariationPreviewDensity
		{
			get { return mSettings.EditorVariationPreviewDensity; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				mSettings.EditorVariationPreviewDensity = value;
			}
		}
		public static bool EditorVariationPreviewApplyPostTransform
		{
			get { return mSettings.EditorVariationPreviewApplyPostTransform; }
			set
			{
				mSettings.EditorVariationPreviewApplyPostTransform = value;
			}
		}

		public static DensityLevel EditorPreviewDensityLevel
		{
			get { return (DensityLevel) mSettings.EditorPreviewDensityLevel; }
			set { mSettings.EditorPreviewDensityLevel = (int) value; }
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
		public static string PluginDirectoryName
		{
			get { return mSettings.PluginDirectoryName; }
		}

		public static bool BatchListUsePreviews
		{
			get { return mSettings.BatchListUsePreviews; }
			set
			{
				mSettings.BatchListUsePreviews = value;
			}
		}
		public static int BatchListPreviewSize
		{
			get { return mSettings.BatchListPreviewSize; }
			set
			{
				if (value < 50 || value > 120)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.BatchListPreviewSize = value;
			}
		}
		public static double BatchListPreviewDensity
		{
			get { return mSettings.BatchListPreviewDensity; }
			set
			{
				if (value <= 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.BatchListPreviewDensity = value;
			}
		}

		public static double PreviewLowQualityDensity
		{
			get { return mSettings.PreviewLowQualityDensity; }
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				mSettings.PreviewLowQualityDensity = value;
			}
		}
		public static double PreviewMediumQualityDensity
		{
			get { return mSettings.PreviewMediumQualityDensity; }
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				mSettings.PreviewMediumQualityDensity = value;
			}
		}
		public static double PreviewHighQualityDensity
		{
			get { return mSettings.PreviewHighQualityDensity; }
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				mSettings.PreviewHighQualityDensity = value;
			}
		}

		public static void Serialize()
		{
			mSettings.Save();
		}
	}
}
