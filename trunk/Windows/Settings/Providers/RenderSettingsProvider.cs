using System;
using System.Drawing;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class RenderSettingsProvider : SettingsProvider<RenderSettings>
	{
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

		public bool MaintainCanvasAspectRatio
		{
			get { return Container.MaintainCanvasAspectRatio; }
			set { Container.MaintainCanvasAspectRatio = value; }
		}
		public bool AllowSaveIncompleteRender
		{
			get { return Container.AllowSaveIncompleteRender; }
			set { Container.AllowSaveIncompleteRender = value; }
		}
		public bool SaveFlameAfterRender
		{
			get { return Container.SaveFlameAfterRender; }
			set { Container.SaveFlameAfterRender = value; }
		}

		public Size? Size
		{
			get { return Equals(Container.Size, default(Size)) ? (Size?)null : Container.Size; }
			set
			{
				if (value.HasValue)
				{
					if (value.Value.Width <= 0 || value.Value.Height <= 0)
						throw new ArgumentOutOfRangeException("value");
				}

				Container.Size = value.GetValueOrDefault();
			}
		}
		public double Density
		{
			get { return Container.Density; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.Density = value;
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

		public int? ThreadCount
		{
			get { return Container.ThreadCount == 0 ? (int?)null : Container.ThreadCount; }
			set
			{
				if (ThreadCount <= 0)
					throw new ArgumentOutOfRangeException(@"value");

				Container.ThreadCount = value.GetValueOrDefault();
			}
		}

		[NotNull]
		public string DestinationPath
		{
			get { return Container.DestinationPath; }
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
				{
					throw new ArgumentNullException(@"value");
				}

				Container.DestinationPath = value;
			}
		}
		public TargetImageFileFormat DestinationFormat
		{
			get { return (TargetImageFileFormat) Container.DestinationFormat; }
			set { Container.DestinationFormat = (int) value; }
		}
	}
}