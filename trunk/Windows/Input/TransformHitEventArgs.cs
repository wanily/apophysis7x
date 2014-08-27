using System;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformHitEventArgs : EventArgs
	{
		public TransformHitEventArgs([NotNull] TransformMouseOverOperation operation)
		{
			if (operation == null) throw new ArgumentNullException("operation");
			Operation = operation;
		}

		[NotNull]
		public TransformMouseOverOperation Operation
		{
			get;
			private set;
		}
	}
}