using System.Drawing;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	class SaturationPaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Saturation";
		}

		public override int MinValue
		{
			get { return -100; }
		}
		public override int MaxValue
		{
			get { return 100; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				var hue = source[i].GetHue();
				var sat = source[i].GetSaturation() + (value / 100f);
				var bri = source[i].GetBrightness();

				if (sat > 1)
					sat = 1;
				if (sat < 0)
					sat = 0;

				var newColor = UtilityExtensions.ColorFromAhsb(source[i].A, hue, sat, bri);

				temp[i] = newColor;
			}

			return temp;
		}
	}
}