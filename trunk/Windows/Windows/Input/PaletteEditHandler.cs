using System;
using System.Drawing;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	abstract class PaletteEditHandler
	{
		private int mValue;
		private Color[] mStartColors;

		public event EventHandler ValueChanged;

		public void Reset()
		{
			Value = 0;
		}
		public int Value
		{
			get { return mValue; }
			set
			{
				if (Equals(value, mValue))
					return;

				mValue = value;
				OnValueChanged();
			}
		}

		public abstract int MinValue { get; }
		public abstract int MaxValue { get; }

		public bool ViewVisible { get; set; }

		public void Initialize([NotNull] Palette palette)
		{
			if (palette == null) throw new ArgumentNullException("palette");

			mStartColors = new Color[palette.Length];
			palette.CopyTo(mStartColors);
		}
		public void Calculate([NotNull] Palette palette)
		{
			if (palette == null) throw new ArgumentNullException(@"palette");

			var newColors = Calculate(mStartColors, mValue);
			palette.ReplaceColors(newColors);
		}

		[NotNull]
		protected abstract Color[] Calculate(Color[] source, int value);

		[NotNull]
		public abstract string GetDisplayName();

		protected void OnValueChanged()
		{
			OnValueChangedOverride();

			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}
		protected virtual void OnValueChangedOverride()
		{
			
		}

		public override string ToString()
		{
			return GetDisplayName();
		}
	}
}
