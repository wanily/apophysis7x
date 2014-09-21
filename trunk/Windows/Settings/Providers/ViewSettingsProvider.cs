using System;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class ViewSettingsProvider : SettingsProvider<ViewSettings>
	{
		public bool BatchListUsePreviews
		{
			get { return Container.BatchListUsePreviews; }
			set
			{
				Container.BatchListUsePreviews = value;
			}
		}
		public int BatchListPreviewSize
		{
			get { return Container.BatchListPreviewSize; }
			set
			{
				if (value < 50 || value > 120)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				Container.BatchListPreviewSize = value;
			}
		}

		public bool IsMainToolbarVisible
		{
			get { return Container.IsMainToolbarVisible; }
			set
			{
				Container.IsMainToolbarVisible = value;
			}
		}
		public bool IsMainStatusbarVisible
		{
			get { return Container.IsMainStatusbarVisible; }
			set
			{
				Container.IsMainStatusbarVisible = value;
			}
		}
		public bool IsBatchListVisible
		{
			get { return Container.IsBatchListVisible; }
			set
			{
				Container.IsBatchListVisible = value;
			}
		}

		public int BatchListSize
		{
			get { return Container.BatchListSize; }
			set
			{
				if (value < 25)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				Container.BatchListSize = value;
			}
		}

		public bool ShowGuidelines
		{
			get { return Container.ShowGuidelines; }
			set
			{
				Container.ShowGuidelines = value;
			}
		}
		public bool ShowTransparency
		{
			get { return Container.ShowTransparency; }
			set
			{
				Container.ShowTransparency = value;
			}
		}
		public bool FitMainPreviewImage
		{
			get { return Container.FitMainPreviewImage; }
			set { Container.FitMainPreviewImage = value; }
		}

		public bool MaintainCanvasAspectRatio
		{
			get { return Container.MaintainCanvasAspectRatio; }
			set { Container.MaintainCanvasAspectRatio = value; }
		}
		public bool SyncMainWindowWithCanvasSize
		{
			get { return Container.SyncMainWindowWithCanvasSize; }
			set { Container.SyncMainWindowWithCanvasSize = value; }
		}
	}
}