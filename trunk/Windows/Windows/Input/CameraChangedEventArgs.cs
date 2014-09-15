using System;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class CameraChangedEventArgs : EventArgs
	{
		[NotNull]
		public CameraInputOperation Operation { get; private set; }

		public CameraChangedEventArgs([NotNull] CameraInputOperation operation)
		{
			if (operation == null) throw new ArgumentNullException("operation");
			Operation = operation;
		}
	}
}