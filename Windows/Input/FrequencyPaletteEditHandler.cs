using System.Drawing;

namespace Xyrus.Apophysis.Windows.Input
{
	class FrequencyPaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Frequency";
		}

		public override int MinValue
		{
			get { return 0; }
		}
		public override int MaxValue
		{
			get { return 9; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			value++;
			if (value == 1)
			{
				source.CopyTo(temp, 0);
				return temp;
			}

			var n = 256 / value;

			for (int j = 0; j < value; j++)
			{
				for (int i = 0; i < n; i++)
				{
					if (j * n + i < 256)
					{
						temp[j * n + i] = source[i * value];
					}
				}
			}

			return temp;
		}
	}
}