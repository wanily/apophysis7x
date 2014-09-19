
using System.Xml.Linq;
using Xyrus.Apophysis.Windows;

namespace Xyrus.Apophysis.Messaging
{
	[PublicAPI]
	public static class MessageCenter
	{
		public static event MessageEventHandler Message;
		public static event MessageEventHandler UnknownAttribute;

		static MessageCenter()
		{
			FlameParsing = new Lock();
		}

		public static Lock FlameParsing { get; private set; }

		public static void SendUnknownAttribute(XName name)
		{
			if (name == null)
				return;

			if (UnknownAttribute == null)
				return;

			UnknownAttribute(null, new MessageEventArgs(name.LocalName));
		}
		public static void SendMessage(string message)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(message.Trim()))
				return;

			if (Message == null)
				return;

			Message(null, new MessageEventArgs(message));
		}
	}
}
