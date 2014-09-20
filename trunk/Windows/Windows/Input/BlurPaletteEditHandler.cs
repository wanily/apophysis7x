using System.Drawing;

namespace Xyrus.Apophysis.Windows.Input
{
	class BlurPaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Blur";
		}

		public override int MinValue
		{
			get { return 0; }
		}
		public override int MaxValue
		{
			get { return 127; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			if (value == 0)
			{
				source.CopyTo(temp, 0);
				return temp;
			}

			for (int i = 0; i < source.Length; i++)
			{
				var n = -1;
				var r = 0;
				var g = 0;
				var b = 0;

				for (int j = i - value; j < i + value; j++)
				{
					n++;
					var k = (256 + j)%256;
					if (k != i)
					{
						r += source[k].R;
						g += source[k].G;
						b += source[k].B;
					}
				}

				if (n > 0)
				{
					temp[i] = Color.FromArgb(source[i].A,
						System.Math.Max(0, System.Math.Min(r/n, 255)),
						System.Math.Max(0, System.Math.Min(g/n, 255)),
						System.Math.Max(0, System.Math.Min(b/n, 255)));
				}
			}

			return temp;
		}
	}
}