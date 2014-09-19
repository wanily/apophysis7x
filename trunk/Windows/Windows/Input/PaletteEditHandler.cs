using System;
using System.Drawing;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	abstract class PaletteEditHandler
	{
		private int mValue;
		private Palette mInputPalette;
		private Palette mOutputPalette;

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

		[NotNull]
		public Palette InputPalette
		{
			get { return mInputPalette; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				mInputPalette = value.Resize(256);
				mValue = 0;
				mOutputPalette = mInputPalette.Copy();
			}
		}

		[NotNull]
		public Palette OutputPalette
		{
			get { return mOutputPalette; }
			private set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				mOutputPalette = value;
			}
		}

		[NotNull]
		protected abstract Color[] Calculate(Color[] source, int value);

		[NotNull]
		public abstract string GetDisplayName();

		protected void OnValueChanged()
		{
			OnValueChangedOverride();

			var source = new Color[InputPalette.Length];
			var data = Calculate(source, mValue);

			OutputPalette = new Palette(InputPalette.CalculatedName, data);

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
