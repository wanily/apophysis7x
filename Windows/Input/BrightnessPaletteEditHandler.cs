using System.Drawing;

namespace Xyrus.Apophysis.Windows.Input
{
	class BrightnessPaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Brightness";
		}

		public override int MinValue
		{
			get { return -255; }
		}
		public override int MaxValue
		{
			get { return 255; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				var red = source[i].R + value;
				var green = source[i].G + value;
				var blue = source[i].B + value;
				
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