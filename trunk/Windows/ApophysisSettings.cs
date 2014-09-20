using System;
using System.Data;
using System.Drawing;
using System.IO;
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

		public const string FlameExportVersionString = "Apophysis 7x";

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

		public static bool ShowUnknownAttributesMessage
		{
			get { return mSettings.ShowUnknownAttributesMessage; }
			set { mSettings.ShowUnknownAttributesMessage = value; }
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

		public static bool IsMainToolbarVisible
		{
			get { return mSettings.IsMainToolbarVisible; }
			set
			{
				mSettings.IsMainToolbarVisible = value;
			}
		}
		public static bool IsMainStatusbarVisible
		{
			get { return mSettings.IsMainStatusbarVisible; }
			set
			{
				mSettings.IsMainStatusbarVisible = value;
			}
		}
		public static bool IsBatchListVisible
		{
			get { return mSettings.IsBatchListVisible; }
			set
			{
				mSettings.IsBatchListVisible = value;
			}
		}

		public static int BatchListSize
		{
			get { return mSettings.BatchListSize; }
			set
			{
				if (value < 25)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.BatchListSize = value;
			}
		}
		public static int MainPreviewDensity
		{
			get { return mSettings.MainPreviewDensity; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.MainPreviewDensity = value;
			}
		}
		public static int MiniPreviewUpdateResolution
		{
			get { return mSettings.MiniPreviewUpdateResolution; }
			set
			{
				if (value < 0 || value > 1000)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.MiniPreviewUpdateResolution = value;
			}
		}
		public static bool MainPreviewShowGuidelines
		{
			get { return mSettings.MainPreviewShowGuidelines; }
			set
			{
				mSettings.MainPreviewShowGuidelines = value;
			}
		}
		public static bool MainPreviewShowTransparency
		{
			get { return mSettings.MainPreviewShowTransparency; }
			set
			{
				mSettings.MainPreviewShowTransparency = value;
			}
		}
		public static bool MainPreviewFitImage
		{
			get { return mSettings.MainPreviewFitImage; }
			set { mSettings.MainPreviewFitImage = value; }
		}

		public static CameraEditMode CameraEditMode
		{
			get { return (CameraEditMode)mSettings.CameraEditMode; }
			set { mSettings.CameraEditMode = (int)value; }
		}
		public static bool CameraEditUseScale
		{
			get { return mSettings.CameraEditUseScale; }
			set { mSettings.CameraEditUseScale = value; }
		}

		public static DensityLevel FlamePropertiesPreviewDensityLevel
		{
			get { return (DensityLevel)mSettings.FlamePropertiesPreviewDensityLevel; }
			set { mSettings.FlamePropertiesPreviewDensityLevel = (int)value; }
		}

		public static Size? SizePreset1
		{
			get { return Equals(mSettings.SizePreset1, default(Size)) ? (Size?)null : mSettings.SizePreset1; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				mSettings.SizePreset1 = value.GetValueOrDefault();
			}
		}
		public static Size? SizePreset2
		{
			get { return Equals(mSettings.SizePreset2, default(Size)) ? (Size?)null : mSettings.SizePreset2; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				mSettings.SizePreset2 = value.GetValueOrDefault();
			}
		}
		public static Size? SizePreset3
		{
			get { return Equals(mSettings.SizePreset3, default(Size)) ? (Size?)null : mSettings.SizePreset3; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				mSettings.SizePreset3 = value.GetValueOrDefault();
			}
		}

		public static bool MaintainCanvasAspectRatio
		{
			get { return mSettings.MaintainCanvasAspectRatio; }
			set { mSettings.MaintainCanvasAspectRatio = value; }
		}
		public static bool SyncMainWindowWithCanvasSize
		{
			get { return mSettings.SyncMainWindowWithCanvasSize; }
			set { mSettings.SyncMainWindowWithCanvasSize = value; }
		}

		public static string AutosavePath
		{
			get { return mSettings.AutosavePath; }
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
					throw new ArgumentNullException("value");

				if (!Directory.Exists(Path.GetDirectoryName(Environment.ExpandEnvironmentVariables(value)) ?? string.Empty))
					throw new DirectoryNotFoundException();

				mSettings.AutosavePath = value;
			}
		}
		public static int AutosaveThreshold
		{
			get { return mSettings.AutosaveThreshold; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value");

				mSettings.AutosaveThreshold = value;
			}
		}

		public static bool VariationsIn15CStyle
		{
			get { return mSettings.VariationsIn15CStyle; }
			set { mSettings.VariationsIn15CStyle = value; }
		}

		public static void Serialize()
		{
			mSettings.Save();
		}
	}
}
