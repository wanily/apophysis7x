using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Flame
	{
		private IteratorCollection mIterators;
		private static int mCounter;
		private int mIndex;
		private string mName;

		public Flame()
		{
			mIterators = new IteratorCollection(this);
			mIndex = ++mCounter;
		}

		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
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

		public Flame Copy()
		{
			var copy = new Flame();
			mCounter--;

			copy.mIndex = mIndex;
			copy.Name = mName;
			copy.mIterators = mIterators.Copy(copy);

			return copy;
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

			Iterators.ReadXml(element.Descendants(XName.Get("xform")).Concat(element.Descendants(XName.Get("finalxform"))));
		}

		public bool IsEqual([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			if (!mIterators.IsEqual(flame.mIterators))
				return false;

			if (!Equals(mName, flame.mName))
				return false;

			return true;
		}
	}
}