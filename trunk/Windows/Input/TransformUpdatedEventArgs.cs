using System;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformUpdatedEventArgs : EventArgs
	{
		public TransformUpdatedEventArgs([NotNull] TransformInputOperation operation)
		{
			if (operation == null) throw new ArgumentNullException("operation");
			Operation = operation;
		}

		[NotNull]
		public TransformInputOperation Operation
		{
			get; 
			private set;
		}
	}
}