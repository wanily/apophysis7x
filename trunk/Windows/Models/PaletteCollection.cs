using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xyrus.Apophysis.Properties;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class PaletteCollection : ReadOnlyCollection<Palette>
	{
		private static PaletteCollection mFlam3Palettes;

		private EventHandler mContentChanged;
		private string mName;

		public PaletteCollection([NotNull] Flame fromFlame) : base(new List<Palette>())
		{
			if (fromFlame == null) throw new ArgumentNullException("fromFlame");
			Append(fromFlame.Palette);
		}
		public PaletteCollection([NotNull] IEnumerable<Palette> palettes) : base(new List<Palette>())
		{
			if (palettes == null) throw new ArgumentNullException("palettes");

			var array = palettes.ToArray();
			if (!array.Any())
			{
				throw new ArgumentException(Resources.EmptyPaletteCollectionError, @"palettes");
			}

			mName = null;

			foreach (var item in array)
			{
				Items.Add(item);
			}
		}

		[NotNull]
		public static PaletteCollection Flam3Palettes
		{
			get { return mFlam3Palettes ?? (mFlam3Palettes = new PaletteCollection(ReadUgr(Resources.Flam3ColorMaps).ToArray())); }
		}

		[NotNull]
		public static Palette GetRandomPalette([CanBeNull] Flame flame = null)
		{
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			var rnd = new Random(flame == null || flame.Palette == null ? (int)DateTime.Now.Ticks : (flame.Palette.GetHashCode() ^ (int)DateTime.Now.Ticks));
			var palette = Flam3Palettes[rnd.Next() % Flam3Palettes.Count];

			if (flame != null)
			{
				palette.Name = flame.CalculatedName;
			}

			return palette;
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
					return Items.First().CalculatedName;
				}

				return mName;
			}
		}

		public int Append([NotNull] Palette palette)
		{
			if (palette == null) throw new ArgumentNullException(@"palette");

			Items.Add(palette);
			RaiseContentChanged();

			return Count - 1;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= Count) return false;
			if (!CanRemove()) throw new ApophysisException("Can't remove last palette from collection");

			Items.RemoveAt(index);
			RaiseContentChanged();

			return true;
		}
		public bool Remove([NotNull] Palette palette)
		{
			if (palette == null) throw new ArgumentNullException("palette");
			if (!CanRemove()) throw new ApophysisException("Can't remove last palette from collection");

			if (!Contains(palette))
				return false;

			Items.Remove(palette);
			RaiseContentChanged();

			return true;
		}
		public bool CanRemove()
		{
			return Count > 1;
		}

		private void RaiseContentChanged()
		{
			if (mContentChanged == null)
				return;

			mContentChanged(this, new EventArgs());
		}
		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
		}

		[NotNull]
		public static IEnumerable<Palette> ReadUgr([NotNull] string data)
		{
			if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(data.Trim()))
				throw new ArgumentNullException(@"data");

			const string collectPalettes = @"([a-z0-9\-_]+)\s*{\s*((?:gradient|alpha):\s*([a-z0-9=\s\-_""]+)\s*)}";
			var palettes = Regex.Matches(data, collectPalettes, RegexOptions.IgnoreCase).OfType<Match>();

			return palettes.Select(palette => Palette.ReadCmap(palette.Groups[1].Value, palette.Groups[2].Value).Resize(256));
		}

		[NotNull]
		public static string WriteUgr([NotNull] IEnumerable<Palette> palettes)
		{
			if (palettes == null) throw new ArgumentNullException("palettes");

			var list = palettes.ToList();
			if (list.Count == 0)
				throw new ArgumentNullException("palettes");

			var builder = new StringBuilder();

			foreach (var palette in list)
			{
				var pal400 = palette.Resize(400);

				builder.AppendFormat(@"{0} {{" + Environment.NewLine, pal400.CalculatedName);
				builder.AppendLine(@"gradient:");
				builder.AppendFormat(@"  title=""{0}"" smooth=no" + Environment.NewLine, pal400.CalculatedName);

				for (int i = 0; i < pal400.Length; i++)
				{
					var col = pal400[i];

					var red = col.R & 0xff;
					var green = col.G & 0xff;
					var blue = col.B & 0xff;

					var value = (blue << 16) | (green << 8) | red;

					builder.AppendFormat(@"  index={0} color={1}" + Environment.NewLine, i, value);
				}

				builder.AppendLine(@"}" + Environment.NewLine);
			}

			return builder.ToString().TrimEnd();
		}
	}
}