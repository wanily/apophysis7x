using System.Drawing;

namespace Xyrus.Apophysis.Windows.Input
{
	class ContrastPaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Contrast";
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

			var power = value > 0 ? value * 2 : value;

			for (int i = 0; i < source.Length; i++)
			{
				var red = (int)(source[i].R + (float)power / 100 * (source[i].R - 127));
				var green = (int)(source[i].G + (float)power / 100 * (source[i].G - 127));
				var blue = (int)(source[i].B + (float)power / 100 * (source[i].B - 127));

				if (red > 255) red = 255;
				if (red < 0) red = 0;
				if (green > 255) green = 255;
				if (green < 0) green = 0;
				if (blue > 255) blue = 255;
				if (blue < 0) blue = 0;

				var newColor = Color.FromArgb(source[i].A, red, green, blue);

				temp[i] = newColor;
			}

			return temp;
		}
	}
}