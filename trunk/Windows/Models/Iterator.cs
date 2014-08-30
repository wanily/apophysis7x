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
		private string mName;
		private double mWeight;

		public Iterator([NotNull] Flame hostingFlame)
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;

			mPreAffine = new AffineTransform();
			mPostAffine = new AffineTransform();

			mWeight = 0.5;
		}

		public int Index
		{
			get { return mFlame.Iterators.IndexOf(this); }
		}
		public string Name
		{
			get { return mName; }
			set
			{
				if (string.IsNullOrEmpty(value))
					mName = null;
				else if (string.IsNullOrEmpty(value.Trim()))
					mName = null;
				else mName = value;
			}
		}
		public double Weight
		{
			get { return mWeight; }
			set
			{
				if (value < double.Epsilon)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mWeight = value;
			}
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

		private double ParseFloat([NotNull] XAttribute attribute, double defaultValue = 0)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			double value;
			if (!double.TryParse(attribute.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
				value = defaultValue;

			return value;
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

			var nameAttribute = element.Attribute(XName.Get("name"));
			Name = nameAttribute == null ? null : nameAttribute.Value;

			var weightAttribute = element.Attribute(XName.Get("weight"));
			if (weightAttribute != null)
			{
				var weight = ParseFloat(weightAttribute, 0.5);
				if (weight < double.Epsilon)
				{
					throw new ApophysisException("Weight must not be less or equal to zero");
				}

				Weight = weight;
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