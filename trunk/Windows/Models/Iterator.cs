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
		private double mColor;
		private double mColorSpeed;
		private double mOpacity;
		private double mDirectColor;

		public Iterator([NotNull] Flame hostingFlame)
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;
			Reset();
		}

		public void Reset()
		{
			mPreAffine = new AffineTransform();
			mPostAffine = new AffineTransform();

			mName = null;
			mWeight= 0.5;
			mColor = 0.0;
			mColorSpeed = 0.0;
			mOpacity = 1.0;
			mDirectColor = 1.0;
		}
		public Iterator Copy()
		{
			var copy = new Iterator(mFlame);

			copy.mName = mName;
			copy.mWeight = mWeight;
			copy.mColor = mColor;
			copy.mColorSpeed = mColorSpeed;
			copy.mOpacity = mOpacity;
			copy.mDirectColor = mDirectColor;

			copy.PreAffine.Origin.X = PreAffine.Origin.X;
			copy.PreAffine.Origin.Y = PreAffine.Origin.Y;
			copy.PreAffine.Matrix.X.X = PreAffine.Matrix.X.X;
			copy.PreAffine.Matrix.X.Y = PreAffine.Matrix.X.Y;
			copy.PreAffine.Matrix.Y.X = PreAffine.Matrix.Y.X;
			copy.PreAffine.Matrix.Y.Y = PreAffine.Matrix.Y.Y;

			copy.PostAffine.Origin.X = PostAffine.Origin.X;
			copy.PostAffine.Origin.Y = PostAffine.Origin.Y;
			copy.PostAffine.Matrix.X.X = PostAffine.Matrix.X.X;
			copy.PostAffine.Matrix.X.Y = PostAffine.Matrix.X.Y;
			copy.PostAffine.Matrix.Y.X = PostAffine.Matrix.Y.X;
			copy.PostAffine.Matrix.Y.Y = PostAffine.Matrix.Y.Y;

			return copy;
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
		public double Color
		{
			get { return mColor; }
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mColor = value;
			}
		}
		public double ColorSpeed
		{
			get { return mColorSpeed; }
			set
			{
				if (value < -1 || value > 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mColorSpeed = value;
			}
		}
		public double Opacity
		{
			get { return mOpacity; }
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mOpacity = value;
			}
		}
		public double DirectColor
		{
			get { return mDirectColor; }
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				mDirectColor = value;
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
					throw new ApophysisException("Weight must not be less or equal to 0");
				}

				Weight = weight;
			}

			var colorAttribute = element.Attribute(XName.Get("color"));
			if (colorAttribute != null)
			{
				var color = ParseFloat(colorAttribute);
				if (color < 0 || color > 1)
				{
					throw new ApophysisException("Color must be be in the range 0 - 1");
				}

				Color = color;
			}

			var colorSpeedAttribute = element.Attribute(XName.Get("symmetry"));
			if (colorSpeedAttribute != null)
			{
				var colorSpeed = ParseFloat(colorSpeedAttribute);
				if (colorSpeed < 0 || colorSpeed > 1)
				{
					throw new ApophysisException("Color must be be in the range -1 - 1");
				}

				ColorSpeed = colorSpeed;
			}

			var opacityAttribute = element.Attribute(XName.Get("opacity"));
			if (opacityAttribute != null)
			{
				var opacity = ParseFloat(opacityAttribute);
				if (opacity < 0 || opacity > 1)
				{
					throw new ApophysisException("Opacity must be be in the range 0 - 1");
				}

				Opacity = opacity;
			}

			var directColorAttribute = element.Attribute(XName.Get("var_color"));
			if (directColorAttribute != null)
			{
				var directColor = ParseFloat(directColorAttribute);
				if (directColor < 0 || directColor > 1)
				{
					throw new ApophysisException("DirectColor must be be in the range 0 - 1");
				}

				DirectColor = directColor;
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