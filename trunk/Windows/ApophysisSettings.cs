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
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.EditorMoveDistance = value;
				mSettings.Save();
			}
		}
		public static double EditorRotateAngle
		{
			get { return mSettings.EditorRotateAngle; }
			set
			{
				if (value < double.Epsilon || value > 360)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.EditorRotateAngle = value;
				mSettings.Save();
			}
		}
		public static double EditorScaleRatio
		{
			get { return mSettings.EditorScaleRatio; }
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mSettings.EditorScaleRatio = value;
				mSettings.Save();
			}
		}

		public static bool EditorLockAxes
		{
			get { return mSettings.EditorLockAxes; }
			set
			{
				mSettings.EditorLockAxes = value;
				mSettings.Save();
			}
		}
	}
}
