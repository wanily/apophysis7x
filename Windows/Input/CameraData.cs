using System;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class CameraData
	{
		private Vector2 mOrigin;

		[NotNull]
		public Vector2 Origin
		{
			get { return mOrigin; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mOrigin = value;
			}
		}

		public double Angle { get; set; }
		public double Zoom { get; set; }
		public double Scale { get; set; }
	}
}