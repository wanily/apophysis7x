using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Models
{
	static class ParserExtensions
	{
		public static Size ParseSize([NotNull] this XAttribute attribute, Size defaultValue = default(Size))
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			var tokens = attribute.Value.Split(' ');
			if (tokens.Length < 2)
				return defaultValue;

			int x, y;
			if (!int.TryParse(tokens[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out x))
				x = defaultValue.Width;
			if (!int.TryParse(tokens[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out y))
				y = defaultValue.Height;

			return new Size(x, y);
		}
		public static Vector2 ParseVector([NotNull] this XAttribute attribute, Vector2 defaultValue = null)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			defaultValue = defaultValue ?? new Vector2();

			var tokens = attribute.Value.Split(' ');
			if (tokens.Length < 2)
				return defaultValue;

			double x, y;
			if (!double.TryParse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x))
				x = defaultValue.X;
			if (!double.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
				y = defaultValue.Y;

			return new Vector2(x, y);
		}
		public static double ParseFloat([NotNull] this XAttribute attribute, double defaultValue = 0)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			double value;
			if (!double.TryParse(attribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
				value = defaultValue;

			return value;
		}
		public static double[] ParseCoefficients([NotNull] this XAttribute attribute)
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
	}
}