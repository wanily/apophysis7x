using System;
using System.Globalization;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Iterator
	{
		private readonly Flame mFlame;

		private AffineTransform mPreAffine;
		private AffineTransform mPostAffine;

		public Iterator([NotNull] Flame hostingFlame)
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;
			mPreAffine = new AffineTransform();
			mPostAffine = new AffineTransform();
		}

		public int Index
		{
			get { return mFlame.Iterators.IndexOf(this); }
		}

		public AffineTransform PreAffine
		{
			get { return mPreAffine; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mPreAffine = value;
			}
		}
		public AffineTransform PostAffine
		{
			get { return mPostAffine; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mPostAffine = value;
			}
		}

		private double[] ParseCoefficients([NotNull] XAttribute attribute)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			var strings = attribute.Value.Split(' ');
			if (strings.Length != 6)
			{
				throw new ApophysisException("Invalid value for attribute \"" + attribute.Name + "\": " + attribute.Value);
			}

			var values = new double[6];
			for (int i = 0; i < 6; i++)
			{
				double value;
				if (!double.TryParse(strings[i], NumberStyles.Float, CultureInfo.InvariantCulture, out value))
				{
					throw new ApophysisException("Invalid value for attribute \"" + attribute.Name + "\": " + attribute.Value);
				}

				values[i] = value;
			}

			return values;
		}

		public void ReadXml([NotNull] XElement element)
		{
			if (element == null) throw new ArgumentNullException("element");

			if ("xform" != element.Name.ToString().ToLower())
			{
				throw new ApophysisException("Expected XML node \"xform\" but received \"" + element.Name + "\"");
			}

			var coefsAttribute = element.Attribute(XName.Get("coefs"));
			if (coefsAttribute != null)
			{
				var vector = ParseCoefficients(coefsAttribute);

				PreAffine.Matrix.X.X = vector[0];
				PreAffine.Matrix.X.Y = -vector[1];
				PreAffine.Matrix.Y.X = -vector[2];
				PreAffine.Matrix.Y.Y = vector[3];
				PreAffine.Origin.X = vector[4];
				PreAffine.Origin.Y = -vector[5];
			}

			var postAttribute = element.Attribute(XName.Get("post"));
			if (postAttribute != null)
			{
				var vector = ParseCoefficients(postAttribute);

				PostAffine.Matrix.X.X = vector[0];
				PostAffine.Matrix.X.Y = -vector[1];
				PostAffine.Matrix.Y.X = -vector[2];
				PostAffine.Matrix.Y.Y = vector[3];
				PostAffine.Origin.X = vector[4];
				PostAffine.Origin.Y = -vector[5];
			}
		}
	}
}