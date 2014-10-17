
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Xyrus.Apophysis.Windows;

namespace Xyrus.Apophysis.Messaging
{
	[PublicAPI]
	public static class MessageCenter
	{
		private static List<string> mLog = new List<string>();

		public static event MessageEventHandler Message;
		public static event MessageEventHandler UnknownAttribute;

		public static ReadOnlyCollection<String> Log
		{
			get { return new ReadOnlyCollection<String>(mLog); }
		}

		static MessageCenter()
		{
			FlameParsing = new Lock();
			SuspendMessaging = new Lock();
		}

		public static Lock FlameParsing { get; private set; }
		public static Lock SuspendMessaging { get; private set; }

		public static void SendUnknownAttribute(XName name)
		{
			if (SuspendMessaging.IsBusy)
				return;

			if (name == null)
				return;

			if (UnknownAttribute == null)
				return;

			UnknownAttribute(null, new MessageEventArgs(name.LocalName));
		}
		public static void SendMessage(string message)
		{
			if (SuspendMessaging.IsBusy)
				return;

			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(message.Trim()))
				return;

			mLog.Add(message);

			if (Message == null)
				return;

			Message(null, new MessageEventArgs(message));
		}
	}
}
