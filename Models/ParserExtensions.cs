using System;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

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

		public static Vector2 ParseVector([NotNull] this XAttribute attribute, Vector2 defaultValue = default(Vector2))
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			var tokens = attribute.Value.Split(' ');
			if (tokens.Length < 2)
				return defaultValue;

			float x, y;
			if (!float.TryParse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x))
				x = defaultValue.X;
			if (!float.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
				y = defaultValue.Y;

			return new Vector2(x, y);
		}

		public static Color ParseColor([NotNull] this XAttribute attribute, Color defaultValue = default(Color))
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			var tokens = attribute.Value.Split(' ');
			if (tokens.Length < 3)
				return defaultValue;

			float x, y, z;
			if (!float.TryParse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x))
				x = defaultValue.R / 255.0f;
			if (!float.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
				y = defaultValue.G / 255.0f;
			if (!float.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out z))
				z = defaultValue.B / 255.0f;

			return Color.FromArgb(
				(int)(Float.Range(x, 0, 1) * 255),
				(int)(Float.Range(y, 0, 1) * 255),
				(int)(Float.Range(z, 0, 1) * 255));
		}

		public static float ParseFloat([NotNull] this XAttribute attribute, float defaultValue = 0)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			float value;
			if (!float.TryParse(attribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
				value = defaultValue;

			return value;
		}

		[NotNull]
		public static float[] ParseCoefficients([NotNull] this XAttribute attribute)
		{
			if (attribute == null) throw new ArgumentNullException("attribute");

			var strings = attribute.Value.Split(' ');
			if (strings.Length != 6)
			{
				throw new ApophysisException("Invalid value for attribute \"" + attribute.Name + "\": " + attribute.Value);
			}

			var values = new float[6];
			for (int i = 0; i < 6; i++)
			{
				float value;
				if (!float.TryParse(strings[i], NumberStyles.Float, CultureInfo.InvariantCulture, out value))
				{
					throw new ApophysisException("Invalid value for attribute \"" + attribute.Name + "\": " + attribute.Value);
				}

				values[i] = value;
			}

			return values;
		}
	}
}