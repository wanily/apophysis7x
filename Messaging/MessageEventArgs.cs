using System;

namespace Xyrus.Apophysis.Messaging
{
	[PublicAPI]
	public class MessageEventArgs : EventArgs
	{
		public string Message { get; private set; }

		public MessageEventArgs(string message)
		{
			Message = message;
		}
	}
}