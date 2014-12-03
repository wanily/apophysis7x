using System;
using Xyrus.Apophysis.Windows;

namespace Xyrus.Apophysis.Settings.Providers
{
	public class EditorSettingsProvider : SettingsProvider<EditorSettings>
	{
		public float MoveDistance
		{
			get { return Container.MoveDistance; }
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.MoveDistance = value;
			}
		}
		public float RotateAngle
		{
			get { return Container.RotateAngle; }
			set
			{
				if (value < float.Epsilon || value > 360)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.RotateAngle = value;
			}
		}
		public float ScaleRatio
		{
			get { return Container.ScaleRatio; }
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.ScaleRatio = value;
			}
		}

		public bool LockAxes
		{
			get { return Container.LockAxes; }
			set
			{
				Container.LockAxes = value;
			}
		}
		public bool ShowVariationPreview
		{
			get { return Container.ShowVariationPreview; }
			set
			{
				Container.ShowVariationPreview = value;
			}
		}
		public bool ShowRulers
		{
			get { return Container.ShowRulers; }
			set
			{
				Container.ShowRulers = value;
			}
		}
		public bool AutoZoom
		{
			get { return Container.AutoZoom; }
			set
			{
				Container.AutoZoom = value;
			}
		}

		public float VariationPreviewRange
		{
			get { return Container.VariationPreviewRange; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.VariationPreviewRange = value;
			}
		}
		public float VariationPreviewDensity
		{
			get { return Container.VariationPreviewDensity; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(@"value");
				}

				Container.VariationPreviewDensity = value;
			}
		}
		public bool VariationPreviewApplyPostTransform
		{
			get { return Container.VariationPreviewApplyPostTransform; }
			set
			{
				Container.VariationPreviewApplyPostTransform = value;
			}
		}

		public CameraEditMode CameraEditMode
		{
			get { return (CameraEditMode)Container.CameraEditMode; }
			set { Container.CameraEditMode = (int)value; }
		}
		public bool CameraEditUseScale
		{
			get { return Container.CameraEditUseScale; }
			set { Container.CameraEditUseScale = value; }
		}
	}
}