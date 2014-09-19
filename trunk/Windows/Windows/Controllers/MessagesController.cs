using System;
using System.Text;
using System.Linq;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class MessagesController : Controller<Messages>
	{
		protected override void AttachView()
		{
		}
		protected override void DetachView()
		{
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

			View.Content.Text = message;
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

			var externalVariations = flame.Iterators.SelectMany(x => x.Variations).Where(x => x is ExternalVariation && System.Math.Abs(x.Weight) > double.Epsilon).ToList();

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
	}
}