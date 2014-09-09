﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Xyrus.Apophysis.Properties;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Palette
	{
		private static int mCounter;
		private const int mDefaultLength = 256;
		private static Palette[] mFlam3Palettes;

		private Color[] mColors;
		private string mName;

		public Palette([NotNull] string name)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim())) throw new ArgumentNullException("name");

			mColors = new Color[mDefaultLength];
			mName = name;
		}
		public Palette([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			mColors = new Color[mDefaultLength];
			mName = flame.CalculatedName;
		}

		public static Palette[] Flam3Palettes
		{
			get { return mFlam3Palettes ?? (mFlam3Palettes = ReadCmap(Resources.Flam3ColorMaps).ToArray()); }
		}
		public static Palette GetRandomPalette([CanBeNull] Flame flame = null)
		{
			var rnd = new Random();
			var palette = Flam3Palettes[rnd.Next() % Flam3Palettes.Length];

			if (flame != null)
			{
				palette.mName = flame.CalculatedName;
			}

			return palette;
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

		public void ReadData(string data)
		{
			data = Regex.Replace(data ?? string.Empty, @"\s", "", RegexOptions.None);

			if (data.Length != mDefaultLength * 6) throw new FormatException(string.Format("Invalid palette format. Expected {0} bytes.", mDefaultLength * 6));
			if (!Regex.IsMatch(data, @"[a-f0-9]+", RegexOptions.IgnoreCase)) throw new FormatException("Invalid palette format. Expected hexadecimal characters.");

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

				mColors[j] = color;
			}
		}
		public static IEnumerable<Palette> ReadCmap(string data)
		{
			const string collectPalettes = @"([a-z0-9\-_]+)\s*{\s*((?:gradient|alpha):\s*([a-z0-9=\s\-_""]+)\s*)}";
			const string collectArrays = @"(alpha|gradient):\s*((?:(?:title|smooth|index|color)=[a-z0-9=\s\-_""]+(\s+|$))+)";
			const string collectIndexColorPairs = @"index=(\d+)\s+color=(\d+)";
			//const string collectSmooth = @"smooth=(yes|no)";

			var today = DateTime.Today;
			var palettes = Regex.Matches(data, collectPalettes, RegexOptions.IgnoreCase).OfType<Match>();

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

				var f = i / (double) steps;
				var c = (int)System.Math.Round(a + (b - a) * f);

				return c;
			});

			foreach (var palette in palettes)
			{
				var name = palette.Groups[1].Value;
				var paletteData = palette.Groups[2].Value;

				if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
				{
					name = "Apo7X-" + 
						today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
						today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
						today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "-" +
						(++mCounter).ToString(CultureInfo.InvariantCulture);
				}

				var arrays = Regex.Matches(paletteData, collectArrays, RegexOptions.IgnoreCase).OfType<Match>();
				var gradientArray = arrays.FirstOrDefault(x => x.Groups[1].Value.ToLower() == "gradient");

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

				Debug.Assert(array.All(x => x.A == 0xff), "One or more indices were not processed when loading CMAP \"" + name + "\". Please review the interpolation.");

				yield return new Palette(name) { mColors = array };
			}
		}

		public bool IsEqual([NotNull] Palette palette)
		{
			if (palette == null) throw new ArgumentNullException("palette");

			if (!Equals(Length, palette.Length))
				return false;

			for (int i = 0; i < Length; i++)
			{
				if (!Equals(this[i], palette[i]))
					return false;
			}

			return true;
		}
		public Palette Copy()
		{
			var copy = new Palette(mName);

			copy.mColors = new Color[Length];
			copy.mName = mName;

			for (int i = 0; i < Length; i++)
			{
				var c = this[i];
				copy[i] = Color.FromArgb(c.R, c.G, c.B);
			}

			return copy;
		}
	}
}
