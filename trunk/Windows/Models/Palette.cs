using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Xyrus.Apophysis.Strings;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Palette
	{
		private static int mCounter;
		private readonly int mIndex;

		private const int mDefaultLength = 256;

		private Color[] mColors;
		private string mName;

		private Palette()
		{
			mIndex = ++mCounter;
		}

		public Palette([NotNull] string name, [NotNull] Color[] colors) : this()
		{
			if (colors == null) throw new ArgumentNullException("colors");
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim())) throw new ArgumentNullException(@"name");
			if (colors.Length < 1) throw new ArgumentOutOfRangeException("colors");

			mColors = colors;
			mName = name;
		}

		internal Palette([NotNull] string name) : this()
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim())) throw new ArgumentNullException(@"name");

			mColors = new Color[mDefaultLength];
			mName = name;
		}
		internal Palette([NotNull] Flame flame) : this()
		{
			if (flame == null) throw new ArgumentNullException(@"flame");

			mColors = new Color[mDefaultLength];
			mName = flame.CalculatedName;
		}

		[CanBeNull]
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}

		[NotNull]
		public string CalculatedName
		{
			get
			{
				if (string.IsNullOrEmpty(mName) || string.IsNullOrEmpty(mName.Trim()))
				{
					var today = DateTime.Today;
					return ApophysisSettings.Common.NamePrefix + @"-" +
						today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
						today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
						today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + @"-" +
						mIndex.ToString(CultureInfo.InvariantCulture);
				}

				return mName;
			}
		}

		public void CopyTo(Array array)
		{
			mColors.CopyTo(array, 0);
		}

		public Color this[int index]
		{
			get { return mColors[index]; }
			set { mColors[index] = value; }
		}
		public int Length
		{
			get { return mColors.Length; }
		}

		public void ReadCondensedHexData(string data)
		{
			data = Regex.Replace(data ?? string.Empty, @"\s", "", RegexOptions.None);

			if (data.Length != mDefaultLength * 6) throw new FormatException(string.Format("Invalid palette format. Expected {0} bytes.", mDefaultLength * 6));
			if (!Regex.IsMatch(data, @"[a-f0-9]+", RegexOptions.IgnoreCase)) throw new FormatException("Invalid palette format. Expected hexadecimal characters.");

			var colors = new Color[mDefaultLength];
			for (int i = 0, j = 0; i < data.Length; i += 6, j++)
			{
				var str = data.Substring(i, 6);
				var bytes = new[] {str.Substring(0, 2), str.Substring(2, 2), str.Substring(4, 2)};

				var rgb = new[]
				{
					int.Parse(bytes[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
					int.Parse(bytes[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
					int.Parse(bytes[2], NumberStyles.HexNumber, CultureInfo.InvariantCulture)
				};

				var color = Color.FromArgb(rgb[0], rgb[1], rgb[2]);

				colors[j] = color;
			}

			mColors = colors;
		}
		public void WriteXml([NotNull] out XElement element)
		{
			element = new XElement(XName.Get("palette"));

			var builder = new StringBuilder();
			var line = new StringBuilder(@"      ");

			builder.AppendLine();

			var colors256 = GetResizedCmap(256);

			foreach (var color in colors256)
			{
				line.Append(color.R.ToString("X2"));
				line.Append(color.G.ToString("X2"));
				line.Append(color.B.ToString("X2"));

				if (line.ToString().TrimStart().Length >= 48)
				{
					builder.AppendLine(line.ToString());
					line = new StringBuilder(@"      ");
				}
			}

			if (line.Length > 0)
			{
				builder.Append(line);
			}

			var hexData = builder.ToString().TrimEnd() + "\r\n  ";

			element.Add(new XAttribute(XName.Get("count"), colors256.Length.Serialize()));
			element.Add(new XAttribute(XName.Get("format"), "RGB"));

			element.SetValue(hexData);
		}

		[NotNull]
		internal static Palette ReadCmap(string name, string data)
		{
			const string collectArrays = @"(alpha|gradient):\s*((?:(?:title|smooth|index|color)=[a-z0-9=\s\-_""]+(\s+|$))+)";
			const string collectIndexColorPairs = @"index=(\d+)\s+color=(\d+)";
			//const string collectSmooth = @"smooth=(yes|no)";

			var today = DateTime.Today;
			var tryParseInt = new Func<string, int, int>((str, def) =>
			{
				int iout;
				if (!int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out iout))
				{
					iout = def;
				}
				return iout;
			});
			var lerpInt = new Func<int, int, int, int, int>((a, b, i, steps) =>
			{
				if (steps == 0)
					return a;

				var f = i / (double)steps;
				var c = (int)System.Math.Round(a + (b - a) * f);

				return c;
			});

			var paletteData = data;

			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
			{
				name = ApophysisSettings.Common.NamePrefix + @"-" +
					today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
					today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
					today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + @"-" +
					(++mCounter).ToString(CultureInfo.InvariantCulture);
			}

			var arrays = Regex.Matches(paletteData, collectArrays, RegexOptions.IgnoreCase).OfType<Match>();
			var gradientArray = arrays.FirstOrDefault(x => x.Groups[1].Value.ToLower() == @"gradient");

			if (gradientArray == null)
			{
				throw new ApophysisException(string.Format("Palette \"{0}\" is doesn't have gradient", name));
			}

			var gradientData = gradientArray.Groups[2].Value;

			//var smooth = Regex.Match(gradientData, collectSmooth, RegexOptions.IgnoreCase);
			//var isSmooth = smooth.Success && smooth.Groups[1].Value.ToLower() == "yes";

			var pairs = Regex
				.Matches(gradientData, collectIndexColorPairs, RegexOptions.IgnoreCase)
				.OfType<Match>()
				.Select(x => new
				{
					index = tryParseInt(x.Groups[1].Value, -1),
					color = tryParseInt(x.Groups[2].Value, -1)
				})
				.OrderBy(x => x.index)
				.ToArray();

			if (pairs.Length == 0)
			{
				throw new ApophysisException(string.Format("Gradient in palette \"{0}\" doesn't have colors", name));
			}

			var array = new Color[pairs.Last().index + 1];
			var lastIndex = -1;

			foreach (var pair in pairs)
			{
				if (pair.color < 0 || pair.index < 0)
				{
					throw new ApophysisException(string.Format("Gradient in palette \"{0}\" has invalid colors", name));
				}

				var red = pair.color & 0xff;
				var green = (pair.color & 0xff00) >> 8;
				var blue = (pair.color & 0xff0000) >> 16;

				var steps = /*isSmooth ?*/ pair.index - lastIndex /*: 0*/;
				for (int i = lastIndex + 1; i < pair.index; i++)
				{
					var r = lerpInt(array[i].R, red, i - lastIndex, steps);
					var g = lerpInt(array[i].G, green, i - lastIndex, steps);
					var b = lerpInt(array[i].B, blue, i - lastIndex, steps);

					array[i] = Color.FromArgb(r, g, b);
				}

				array[pair.index] = Color.FromArgb(red, green, blue);
				lastIndex = pair.index;
			}

			Debug.Assert(array.All(x => x.A == 0xff), @"One or more indices were not processed when loading CMAP """ + name + @""". Please review the interpolation.");

			return new Palette(name) { mColors = array };
		}

		[NotNull]
		internal void ReplaceColors([NotNull] Color[] newColors)
		{
			if (newColors == null) throw new ArgumentNullException("newColors");
			if (newColors.Length != mColors.Length) 
				throw new ArgumentException(Messages.MismatchingPaletteArraysError, "newColors");

			mColors = newColors;
		}

		private static Color Mix(Color operand1, Color operand2)
		{
			double r1 = operand1.R, r2 = operand2.R;
			double g1 = operand1.G, g2 = operand2.G;
			double b1 = operand1.B, b2 = operand2.B;

			return Color.FromArgb(
				(int)System.Math.Max(0, System.Math.Min(System.Math.Round(0.5 * (r1 + r2), 0), 255.0)),
				(int)System.Math.Max(0, System.Math.Min(System.Math.Round(0.5 * (g1 + g2), 0), 255.0)),
				(int)System.Math.Max(0, System.Math.Min(System.Math.Round(0.5 * (b1 + b2), 0), 255.0)));
		}
		private void Spread(int index, ref Color[] array, Color value)
		{
			while (index < 0) index += array.Length;
			if (index < array.Length)
			{
				array[index] = value;
			}
			else
			{
				var newColors = new Color[index + 1];

				if (array.Length > 0)
				{
					var lastColor = array[array.Length - 1];
					
					array.CopyTo(newColors, 0);

					double r1 = lastColor.R,
						   g1 = lastColor.G,
						   b1 = lastColor.B;
					double r2 = value.R,
						   g2 = value.G,
						   b2 = value.B;

					for (int i = array.Length; i < index; i++)
					{
						double p = (i - array.Length) / (double)(index - array.Length);
						newColors[i] = Color.FromArgb(
							(int)System.Math.Max(0, System.Math.Min(System.Math.Round(r1 + (r2 - r1) * p, 0), 255.0)),
							(int)System.Math.Max(0, System.Math.Min(System.Math.Round(g1 + (g2 - g1) * p, 0), 255.0)),
							(int)System.Math.Max(0, System.Math.Min(System.Math.Round(b1 + (b2 - b1) * p, 0), 255.0)));
					}

					newColors[index] = value;
					array = newColors;
				}
				else
				{
					array = new Color[index + 1];
					for (int i = 0; i <= index; i++)
					{
						array[i] = value;
					}
				}
			}
		}

		private Color[] GetResizedCmap(int count)
		{
			var oldLength = mColors.Length;
			var newLength = count;

			if (newLength <= 1) throw new ArgumentOutOfRangeException("count", Messages.TooSmallPaletteError);
			if (oldLength == newLength)
				return mColors.ToArray();

			var colors = new Color[1];

			double r = (double)newLength / oldLength;
			if (r < 1)
			{
				var preI2 = 0;

				for (int i = 0; i < oldLength; i++)
				{
					var i2 = (int)System.Math.Max(0, System.Math.Min(System.Math.Round((r * i), 0), newLength - 1));

					if (preI2 == i2)
					{
						Spread(i2, ref colors, Mix(colors[i2], mColors[i]));
					}
					else
					{
						Spread(i2, ref colors, this[i]);
					}
					preI2 = i2;
				}
			}
			else
			{
				for (int i = 0; i < oldLength; i++)
				{
					double p = (double)i / (oldLength - 1);
					var i2 = (int)System.Math.Max(0, System.Math.Min(System.Math.Round(p * (newLength - 1), 0), newLength - 1));

					Spread(i2, ref colors, this[i]);
				}
			}

			var copy = new Color[newLength];
			Array.Copy(colors, 0, copy, 0, newLength);

			return copy;
		}

		public bool IsEqual([NotNull] Palette palette)
		{
			if (palette == null) throw new ArgumentNullException(@"palette");

			if (!Equals(Length, palette.Length))
				return false;

			for (int i = 0; i < Length; i++)
			{
				if (!Equals(this[i], palette[i]))
					return false;
			}

			return true;
		}

		[NotNull]
		public Palette Copy()
		{
			var copy = new Palette(mName)
			{
				mColors = new Color[Length], 
				mName = mName
			};

			for (int i = 0; i < Length; i++)
			{
				var c = this[i];
				copy[i] = Color.FromArgb(c.R, c.G, c.B);
			}

			return copy;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				// ReSharper disable NonReadonlyFieldInGetHashCode
				return ((mColors != null ? mColors.GetHashCode() : 0) * 397) ^ (mName != null ? mName.GetHashCode() : 0);
				// ReSharper restore NonReadonlyFieldInGetHashCode
			}
		}
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			
			return IsEqual((Palette)obj);
		}

		[NotNull]
		public Palette Resize(int length)
		{
			var colors = GetResizedCmap(length);
			return new Palette(CalculatedName, colors);
		}
	}
}
