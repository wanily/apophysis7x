using System.Drawing;

namespace Xyrus.Apophysis.Windows.Input
{
	class RotatePaletteEditHandler : PaletteEditHandler
	{
		public override string GetDisplayName()
		{
			return "Rotate";
		}

		public override int MinValue
		{
			get { return -128; }
		}
		public override int MaxValue
		{
			get { return 128; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				temp[i] = source[(256 + i - value) % 256];
			}

			return temp;
		}
	}
}