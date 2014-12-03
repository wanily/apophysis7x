using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class BitmapReadyEventArgs : EventArgs
	{
		public BitmapReadyEventArgs(float nextIssue)
		{
			NextIssue = nextIssue;
		}

		public float NextIssue { get; private set; }
	}
}