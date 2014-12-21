using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class MessagesController : Controller<Messages>, IMessagesController
	{
		private Collection<string> mUnknownAttributes;

		public MessagesController()
		{
			mUnknownAttributes = new Collection<string>();
		}

		protected override void AttachView()
		{
			MessageCenter.Message += OnMessage;
			MessageCenter.FlameParsing.Engaged += OnStartParsing;
			MessageCenter.FlameParsing.Released += OnEndParsing;
			MessageCenter.UnknownAttribute += OnUnknownAttribute;

			View.ClearMenuItem.Click += OnClearClick;

			Push(string.Join(Environment.NewLine, MessageCenter.Log.ToArray()));
		}
		protected override void DetachView()
		{
			MessageCenter.Message -= OnMessage;
			MessageCenter.FlameParsing.Engaged -= OnStartParsing;
			MessageCenter.FlameParsing.Released -= OnEndParsing; 
			MessageCenter.UnknownAttribute -= OnUnknownAttribute;

			View.ClearMenuItem.Click -= OnClearClick;
		}

		public void Push(string message)
		{
			if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(message.Trim()))
				return;

			var builder = new StringBuilder(View.Content.Text);
			if (!string.IsNullOrEmpty(View.Content.Text))
			{
				builder.AppendLine();
			}

			builder.Append(message);

			View.Content.Text = builder.ToString();
		}

		public void Summarize([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			var builder = new StringBuilder();
			var header = string.Format("Flame \"{0}\"", flame.CalculatedName);

			builder.AppendLine(header);
			builder.AppendLine(new string('=', header.Length + 3));

			builder.AppendFormat("Number of transforms: {0}\r\n", flame.Iterators.Count(x => x.GroupIndex == 0));
			builder.AppendFormat("Number of final transforms: {0}\r\n", flame.Iterators.Count(x => x.GroupIndex == 1));

			var externalVariations = flame.Iterators.SelectMany(x => x.Variations).Where(x => x is ExternalVariation && System.Math.Abs(x.Weight) > float.Epsilon).ToList();

			if (externalVariations.Count > 0)
			{
				builder.AppendLine("Used external variations:");
				foreach (var variation in externalVariations)
				{
					builder.AppendFormat(@" - {0}", variation.Name + "\r\n");
				}
			}
			else
			{
				builder.AppendLine("Used external variations: none");
			}

			builder.AppendLine();
			Push(builder.ToString());
			View.Show();
		}

		private void OnMessage(object sender, MessageEventArgs args)
		{
			Push(args.Message);
		}
		private void OnStartParsing(object sender, EventArgs e)
		{
			mUnknownAttributes.Clear();
		}
		private void OnUnknownAttribute(object sender, MessageEventArgs args)
		{
			mUnknownAttributes.Add(args.Message);
		}
		private void OnEndParsing(object sender, EventArgs e)
		{
			if (!ApophysisSettings.Common.ShowUnknownAttributesMessage || mUnknownAttributes.Count == 0)
			{
				return;
			}

			var distinctAttributes = mUnknownAttributes.Distinct().ToList();
			var attributes = distinctAttributes.Count > 10 ? distinctAttributes.Take(10) : distinctAttributes;

			var headline = "The following attributes could not be interpreted. Maybe you miss one or more plugins?" + Environment.NewLine;

			var body = string.Join(Environment.NewLine, attributes.Select(x => @"  - " + x).ToArray());
			var fullBody = string.Join(Environment.NewLine, distinctAttributes.Select(x => @"  - " + x).ToArray());

			Push(headline + fullBody);

			if (distinctAttributes.Count > 10)
			{
				body += Environment.NewLine + @"  - ...";
				body += Environment.NewLine + Environment.NewLine + Strings.Messages.GenericProblemListExceedsMaxSizeMessage;
			}

			MessageBox.Show(
				headline + Environment.NewLine + body,
				Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void OnClearClick(object sender, EventArgs e)
		{
			View.Content.Text = string.Empty;
		}
	}
}