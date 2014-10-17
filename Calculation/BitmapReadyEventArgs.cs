using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class BitmapReadyEventArgs : EventArgs
	{
		public BitmapReadyEventArgs(double nextIssue)
		{
			NextIssue = nextIssue;
		}

		public double NextIssue { get; private set; }
	}
}