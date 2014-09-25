using Xyrus.Apophysis.Messaging;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class RenderMessenger : RenderMessengerBase
	{
		public event MessageEventHandler Message;

		public override void SendMessage(string message)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(message.Trim()))
				return;

			if (Message != null)
			{
				Message(this, new MessageEventArgs(message));
			}
		}
	}
}