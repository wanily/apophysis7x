using System;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Flame
	{
		private readonly IteratorCollection mIterators;

		public Flame()
		{
			mIterators = new IteratorCollection(this);
		}

		public IteratorCollection Iterators
		{
			get { return mIterators; }
		}

		public void ReadXml([NotNull] XElement element)
		{
			if (element == null) throw new ArgumentNullException("element");

			if ("flame" != element.Name.ToString().ToLower())
			{
				throw new ApophysisException("Expected XML node \"flame\" but received \"" + element.Name + "\"");
			}

			Iterators.ReadXml(element.Descendants(XName.Get("xform")));
		}
	}
}