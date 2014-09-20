using System.Drawing;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows.Input
{
	class HuePaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Hue";
		}

		public override int MinValue
		{
			get { return -180; }
		}
		public override int MaxValue
		{
			get { return 180; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				var hue = (360 + (int)source[i].GetHue() + value) % 360;
				var sat = source[i].GetSaturation();
				var bri = source[i].GetBrightness();

				var newColor = UtilityExtensions.ColorFromAhsb(source[i].A, hue, sat, bri);

				temp[i] = newColor;
			}

			return temp;
		}
	}
}