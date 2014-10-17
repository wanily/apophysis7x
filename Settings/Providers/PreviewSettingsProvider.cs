using System;
using System.Drawing;
using Xyrus.Apophysis.Windows;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class PreviewSettingsProvider : SettingsProvider<PreviewSettings>
	{
		public DensityLevel EditorPreviewDensityLevel
		{
			get { return (DensityLevel)Container.EditorPreviewDensityLevel; }
			set { Container.EditorPreviewDensityLevel = (int)value; }
		}
		public DensityLevel FlamePropertiesPreviewDensityLevel
		{
			get { return (DensityLevel)Container.FlamePropertiesPreviewDensityLevel; }
			set { Container.FlamePropertiesPreviewDensityLevel = (int)value; }
		}

		public int MiniPreviewUpdateResolution
		{
			get { return Container.MiniPreviewUpdateResolution; }
			set
			{
				if (value < 0 || value > 1000)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				Container.MiniPreviewUpdateResolution = value;
			}
		}

		public double MainPreviewDensity
		{
			get { return Container.MainPreviewDensity; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				Container.MainPreviewDensity = (int)value;
			}
		}
		public double BatchListPreviewDensity
		{
			get { return Container.BatchListPreviewDensity; }
			set
			{
				if (value <= 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				Container.BatchListPreviewDensity = value;
			}
		}

		public double LowQualityDensity
		{
			get { return Container.LowQualityDensity; }
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				Container.LowQualityDensity = value;
			}
		}
		public double MediumQualityDensity
		{
			get { return Container.MediumQualityDensity; }
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				Container.MediumQualityDensity = value;
			}
		}
		public double HighQualityDensity
		{
			get { return Container.HighQualityDensity; }
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("value");
				Container.HighQualityDensity = value;
			}
		}

		public Size? SizePreset1
		{
			get { return Equals(Container.SizePreset1, default(Size)) ? (Size?)null : Container.SizePreset1; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				Container.SizePreset1 = value.GetValueOrDefault();
			}
		}
		public Size? SizePreset2
		{
			get { return Equals(Container.SizePreset2, default(Size)) ? (Size?)null : Container.SizePreset2; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				Container.SizePreset2 = value.GetValueOrDefault();
			}
		}
		public Size? SizePreset3
		{
			get { return Equals(Container.SizePreset3, default(Size)) ? (Size?)null : Container.SizePreset3; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				Container.SizePreset3 = value.GetValueOrDefault();
			}
		}

		public int? ThreadCount
		{
			get { return Container.ThreadCount == 0 ? (int?) null : Container.ThreadCount; }
			set
			{
				if (ThreadCount <= 0)
					throw new ArgumentOutOfRangeException(@"value");

				Container.ThreadCount = value.GetValueOrDefault();
			}
		}

		public double FilterRadius
		{
			get { return Container.FilterRadius; }
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.FilterRadius = value;
			}
		}
		public int Oversample
		{
			get { return Container.Oversample; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.Oversample = value;
			}
		}
	}
}