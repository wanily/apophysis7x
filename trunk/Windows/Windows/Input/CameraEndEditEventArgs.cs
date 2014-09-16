using System;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class CameraEndEditEventArgs : EventArgs
	{
		[NotNull]
		public CameraData Data { get; private set; }

		public CameraEndEditEventArgs([NotNull] CameraData data)
		{
			if (data == null) throw new ArgumentNullException("data");
			Data = data;
		}
	}
}