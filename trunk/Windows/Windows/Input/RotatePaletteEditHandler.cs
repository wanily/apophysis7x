using System;
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
			get { return 127; }
		}

		protected override Color[] Calculate(Color[] source, int value)
		{
			var temp = new Color[source.Length];

			if (value < 0)
			{
				value = 255 + value;
			}

			Array.Copy(source, value, temp, 0, 128);
			Array.Copy(source, 0, temp, value, 127);

			return temp;
		}
	}
}