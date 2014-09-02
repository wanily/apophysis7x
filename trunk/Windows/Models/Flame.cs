using System;
using System.Globalization;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Flame
	{
		private readonly IteratorCollection mIterators;
		private static int mCounter;
		private int mIndex;

		public Flame()
		{
			mIterators = new IteratorCollection(this);
			mIndex = ++mCounter;
		}

		public string Name { get; set; }
		public string CalculatedName
		{
			get
			{
				if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Name.Trim()))
				{
					var today = DateTime.Today;
					return "Apo7X-" + 
						today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
						today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
						today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "-" +
						mIndex.ToString(CultureInfo.InvariantCulture);
				}

				return Name;
			}
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

			var nameAttribute = element.Attribute(XName.Get("name"));
			Name = nameAttribute == null ? null : nameAttribute.Value;

			Iterators.ReadXml(element.Descendants(XName.Get("xform")));
		}
	}
}