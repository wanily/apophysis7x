using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class FinishedEventArgs : EventArgs
	{
		public FinishedEventArgs(bool cancelled)
		{
			Cancelled = cancelled;
		}

		public bool Cancelled { get; private set; }
	}
}