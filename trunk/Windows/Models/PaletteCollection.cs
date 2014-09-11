using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
				throw new ArgumentException("Source enumeration of palettes can't be empty", @"palettes");
			}

			mName = null;

			foreach (var item in array)
			{
				Items.Add(item);
			}
		}

		public static PaletteCollection Flam3Palettes
		{
			get { return mFlam3Palettes ?? (mFlam3Palettes = new PaletteCollection(ReadUgr(Resources.Flam3ColorMaps).ToArray())); }
		}
		public static Palette GetRandomPalette([CanBeNull] Flame flame = null)
		{
			var rnd = new Random();
			var palette = Flam3Palettes[rnd.Next() % Flam3Palettes.Count];

			if (flame != null)
			{
				palette.Name = flame.CalculatedName;
			}

			return palette;
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
					return Items.First().CalculatedName;
				}

				return Name;
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

		internal static IEnumerable<Palette> ReadUgr(string data)
		{
			const string collectPalettes = @"([a-z0-9\-_]+)\s*{\s*((?:gradient|alpha):\s*([a-z0-9=\s\-_""]+)\s*)}";
			var palettes = Regex.Matches(data, collectPalettes, RegexOptions.IgnoreCase).OfType<Match>();

			return palettes.Select(palette => Palette.ReadCmap(palette.Groups[1].Value, palette.Groups[2].Value));
		}
	}
}